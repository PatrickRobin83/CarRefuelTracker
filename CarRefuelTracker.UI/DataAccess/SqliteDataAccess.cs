/*
*----------------------------------------------------------------------------------
*          Filename:	SqliteDataAccess.cs
*          Date:        2020.06.12 17:40:49
*          All rights reserved
*
*----------------------------------------------------------------------------------
* @author Patrick Robin <support@rietrob.de>
*/

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using CarRefuelTracker.UI.Enums;
using CarRefuelTracker.UI.Helper;
using CarRefuelTracker.UI.Models;
using Dapper;

namespace CarRefuelTracker.UI.DataAccess

{
    /// <summary>
    /// DataAccess to the database
    /// </summary>
    public static class SqliteDataAccess
    {

        #region Fields

        #endregion

        #region Properties

        #endregion

        #region Constructor

        #endregion

        #region Methods

        #region Car Operations
        /// <summary>
        /// Gets all CarModels from the Database and return them in a List
        /// </summary>
        /// <returns>List of CarModels</returns>
        public static List<CarModel> LoadCars()
        {
            List<CarModel> allCarModels = new List<CarModel>();
            using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
            {
                try
                {
                    allCarModels = cnn.Query<CarModel>("SELECT * FROM Car WHERE isActive = 1").ToList();

                    foreach (CarModel carModel in allCarModels)
                    {
                        List<EntryModel> allEntriesForCar = new List<EntryModel>();
                        carModel.Brand = cnn.QueryFirst<BrandModel>($"SELECT * FROM Brand WHERE {carModel.BrandId} = id");
                        carModel.ModelType = cnn.QueryFirst<ModelTypeModel>($"SELECT * FROM Model WHERE {carModel.ModelId} = id");
                        carModel.FuelType = cnn.QueryFirst<FuelTypeModel>($"SELECT * from TypeOfFuel WHERE {carModel.TypeoffuelId} = id");
                        allEntriesForCar = cnn.Query<EntryModel>($"SELECT * FROM `Entry` WHERE CarId = {carModel.Id}  ORDER BY Date(Replace(entrydate, '.', '-'));").ToList();
                        carModel.Entries = new ObservableCollection<EntryModel>(allEntriesForCar);
                    }
                    return allCarModels;
                }
                catch (SQLiteException ex)
                {
                    LogHelper.WriteToLog("LoadCars Error", LogState.Error);
                    return new List<CarModel>();
                }
            }
            
        }
        /// <summary>
        /// Save a CarModel in the DataBase
        /// </summary>
        /// <param name="carToSave">CarModel to save in the database</param>
        /// <returns>The CarModel with the Id from the database</returns>
        public static CarModel SaveCar(CarModel carToSave)
        {
            try
            {
                using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
                {
                    carToSave = cnn.Query<CarModel>(@"INSERT INTO Car(brandid, modelid, typeoffuelid,isActive) 
                                                   VALUES(@BrandId, @ModelId, @TypeOfFuelId, @IsActive); 
                                                   SELECT last_insert_rowid()",carToSave).First();
                }
                return carToSave;
            }
            catch (SQLiteException ex)
            {
                LogHelper.WriteToLog("SaveCar Error", LogState.Error);
                return new CarModel();
            }

            
        }
        /// <summary>
        /// Updates the given CarModel in the database
        /// </summary>
        /// <param name="carToUpdate">CarModel to update</param>
        public static void UpdateCar(CarModel carToUpdate)
        {
            try
            {
                using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
                {
                    cnn.Query(@"UPDATE Car SET brandid = @BrandId, modelid = @ModelId, typeoffuelid = @TypeOfFuelId, 
                              isActive = @isActive WHERE id = @Id",carToUpdate);
                }
            }
            catch (SQLiteException ex)
            {
                LogHelper.WriteToLog("UpdateCar Error", LogState.Error);
            }
        }
        /// <summary>
        /// Delete the given CarModel from the database
        /// </summary>
        /// <param name="carToDelete"></param>
        public static void DeleteCar(CarModel carToDelete)
        {
            try
            {
                using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
                {
                    cnn.Query($"UPDATE Car SET isActive = 0");
                }
            }
            catch (SQLiteException ex)
            {
                LogHelper.WriteToLog("DeleteCar Error", LogState.Error);
            }
        }
        #endregion

        #region Brand Operations
        /// <summary>
        /// Loads all Brands from Database and returns them in a List
        /// </summary>
        /// <returns>List of BrandModel</returns>
        public static List<BrandModel> LoadAllBrands()
        {
            List<BrandModel> allBrands = new List<BrandModel>();
            try
            {
                using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
                {
                    allBrands = cnn.Query<BrandModel>("SELECT * FROM Brand").ToList();
                    return allBrands;
                }
            }
            catch (SQLiteException ex)
            {
                LogHelper.WriteToLog("LoadAllBrand Error", LogState.Error);
                return new List<BrandModel>();
            }
        }
        /// <summary>
        /// Add the given BrandModel to the database
        /// </summary>
        /// <param name="brand">The BrandModel to save</param>
        /// <returns>BrandModel with the Id from Database</returns>
        public static BrandModel AddBrand(BrandModel brand)
        {
            try
            {
                using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
                {

                    int isBrandInDatabase = cnn.Query<int>($"SELECT id FROM Brand WHERE brandname = '{brand.BrandName}'").Count();

                    if (isBrandInDatabase == 0)
                    {
                        brand.Id = cnn.Query<int>(@"INSERT INTO Brand (brandname) VALUES (@BrandName); SELECT last_insert_rowid()", brand).First();
                    }
                    return brand;
                }
            }
            catch (SQLiteException ex)
            {
                LogHelper.WriteToLog("AddBrand Error", LogState.Error);
                return new BrandModel();
            }
        }
        /// <summary>
        /// Deletes the given BrandModel from the Database
        /// </summary>
        /// <param name="brand">BrandModel to delete from Database</param>
        public static void RemoveBrandFromDataBase(BrandModel brand)
        {
            try
            {
                using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
                {
                    cnn.Query($"DELETE FROM Brand WHERE id = {brand.Id}");
                    List<ModelTypeModel> modelTypes = cnn.Query<ModelTypeModel>($"SELECT * FROM Model WHERE brandId = {brand.Id} ").ToList();
                    List<CarModel> carModels = cnn.Query<CarModel>($"SELECT * FROM Car WHERE brandId = {brand.Id}").ToList();

                    if (carModels != null && carModels.Count > 0)
                    {
                        foreach (CarModel carModel in carModels)
                        {
                            cnn.Query($"DELETE FROM Car WHERE brandId = {carModel.Id}");
                        }
                    }

                    if (modelTypes != null && modelTypes.Count > 0)
                    {
                        foreach (ModelTypeModel modelType in modelTypes)
                        {
                            cnn.Query($"DELETE FROM Model WHERE id = {modelType.Id}");
                        }
                    }
                }
            }
            catch (SQLiteException ex)
            {
                LogHelper.WriteToLog("RemoveBrandFromDatabase Error", LogState.Error);
            }
        }

        #endregion

        #region Model Operations
        /// <summary>
        /// Loads all Models for the given Brand and returns them in a list
        /// </summary>
        /// <param name="brandId">Brand to get the Models from</param>
        /// <returns>List of ModelTypeModel</returns>
        public static List<ModelTypeModel> ModelsFromBrands(int brandId)
        {
            List<ModelTypeModel> carModels = new List<ModelTypeModel>();
            try
            {
                using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
                {
                    carModels = cnn.Query<ModelTypeModel>($"SELECT * FROM Model WHERE {brandId} = brandId").ToList();

                    return carModels;
                }
            }
            catch (SQLiteException ex)
            {
                LogHelper.WriteToLog("ModelsFromBrand Error", LogState.Error);
                return new List<ModelTypeModel>();
            }
        }
        /// <summary>
        /// Adds the given ModelTypeModel to the Database
        /// </summary>
        /// <param name="modeltype">ModelTypeModel to add to the Database</param>
        /// <param name="brandmodel">BrandModel the ModelType is from</param>
        /// <returns>The ModelTypeModel with Id from the Database</returns>
        public static ModelTypeModel AddModel(ModelTypeModel modeltype, BrandModel brandmodel)
        {
            try
            {
                using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
                {
                    int modelInDatabase = cnn.Query<int>($"SELECT id FROM Model WHERE modelname = '{modeltype.ModelName}'").Count();

                    if (modelInDatabase == 0)
                    {
                        modeltype.Id = cnn.Query<int>($"INSERT INTO Model (modelname,  brandId) VALUES ('{modeltype.ModelName}', {brandmodel.Id}); SELECT last_insert_rowid()",modeltype).First();
                    }
                    return modeltype;
                }
            }
            catch (SQLiteException ex)
            {
                LogHelper.WriteToLog("AddModel Error", LogState.Error);
                return new ModelTypeModel();
            }
        }
        /// <summary>
        /// Removes the given ModelTypeModel from the Database
        /// </summary>
        /// <param name="model">ModelTypeModel do remove</param>
        public static void RemoveModelTypeFromDatabase(ModelTypeModel model)
        {
            try
            {
                using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
                {
                    cnn.Query($"DELETE FROM Model WHERE id = {model.Id}");
                    cnn.Query($"DELETE FROM Car WHERE modelId = {model.Id}");
                }
            }
            catch (SQLiteException ex)
            {
                LogHelper.WriteToLog("RemoveModelTypeFromDatabase", LogState.Error);
            }
        }

        #endregion

        #region FuelType Operations

        /// <summary>
        /// Loads all FuelTypeModels from Database an return them in a list
        /// </summary>
        /// <returns>List of FuelTypeModel</returns>
        public static List<FuelTypeModel> LoadAllFuelTypes()
        {
            List<FuelTypeModel> fuelTypesList = new List<FuelTypeModel>();

            try
            {
                using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
                {
                    fuelTypesList = cnn.Query<FuelTypeModel>($"SELECT * FROM TypeOfFuel").ToList();

                    return fuelTypesList;
                }
            }
            catch (SQLiteException ex)
            {
                LogHelper.WriteToLog("LoadAllFuelTypes Error", LogState.Error);
                return new List<FuelTypeModel>();
            }
        }
        /// <summary>
        /// Adds a FuelTypeModel to the Database
        /// </summary>
        /// <param name="fuelType">FuelTypeModel to add</param>
        /// <returns>FuelTypeModel with Id from Database</returns>
        public static FuelTypeModel AddFuelType(FuelTypeModel fuelType)
        {
            try
            {
                using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
                {
                    int fueltTypeInDatabase =
                        cnn.Query<int>($"SELECT id FROM TypeOfFuel WHERE TypeOfFuel = '{fuelType.TypeOfFuel}'").Count();
                    if (fueltTypeInDatabase == 0)
                    {
                        fuelType.Id =
                            cnn.Query<int>($"INSERT INTO TypeOfFuel (TypeOfFuel) VALUES('{fuelType.TypeOfFuel}'); " +
                                           $"SELECT last_insert_rowid()", fuelType).First();
                    }

                    return fuelType;
                }
            }
            catch (SQLiteException ex)
            {
                LogHelper.WriteToLog("AddFuelTypeError", LogState.Error);
                return new FuelTypeModel();
            }
        }
        /// <summary>
        /// Removes the given FuelTypeModel from the Database and also all cars with the deleted FuelType
        /// </summary>
        /// <param name="fuelTypeModel">FuelTypeModel to delete</param>
        public static void RemoveFuelTypeFromDatabase(FuelTypeModel fuelTypeModel)
        {
            try
            {
                using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
                {
                    cnn.Query($"DELETE FROM TypeOfFuel WHERE id = {fuelTypeModel.Id}");
                    cnn.Query($"DELETE FROM Car WHERE typeoffuelid = {fuelTypeModel.Id}");
                }
            }
            catch (SQLiteException ex)
            {
                LogHelper.WriteToLog("RemoveFuelTypeFromDatabase Error", LogState.Error);
            }
        }

        #endregion

        #region Entry Operations
        /// <summary>
        /// Loads all Entrys from the given car id
        /// </summary>
        /// <param name="carId">CarId where the entrys from</param>
        /// <returns>List of EntryModel for the given Car</returns>
        public static List<EntryModel> LoadEntrysForCar(int carId)
        {
            List<EntryModel> entryModels = new List<EntryModel>();
            try
            {
                using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
                {
                    entryModels = cnn.Query<EntryModel>($"SELECT * FROM `Entry` WHERE CarId = {carId}  ORDER BY Date(Replace(entrydate, '.', '-'));").ToList();

                    return entryModels;
                }
            }
            catch (SQLiteException ex)
            {
                LogHelper.WriteToLog("LoadEntrysForCar Error", LogState.Error);
                return new List<EntryModel>();
            }
        }
        /// <summary>
        /// Saves an EntryModel in the Database
        /// </summary>
        /// <param name="entryToSave">EntryModel to Save</param>
        /// <returns>EntryModel with Id from Database</returns>
        public static EntryModel SaveEntryInDatabase(EntryModel entryToSave)
        {
            try
            {
                using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
                {
                    entryToSave = cnn.Query<EntryModel>(@"INSERT INTO Entry(carId, entrydate, priceperliter, amountoffuel, drivendistance, totalamount, 
                                                     costperhundredkilometer, consumptationperhundredkilometer) 
                                                     VALUES(@CarId, @EntryDate, @PricePerLiter, 
                                                     @AmountOfFuel, @DrivenDistance, @TotalAmount, @CostPerHundredKilometer, 
                                                     @ConsumptationPerHundredKilometer); SELECT last_insert_rowid()", entryToSave).First();
                }
                return entryToSave;
            }
            catch (SQLiteException ex)
            {
                LogHelper.WriteToLog("SaveEntryInDatabase Error", LogState.Error);
                return new EntryModel();
            }
        }
        /// <summary>
        /// Updates the given EntryModel in the Database
        /// </summary>
        /// <param name="entryModelToUpdate">Entry to update</param>
        public static void UpdateEntryInDatabase(EntryModel entryModelToUpdate)
        {
            try
            {
                using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
                {
                    cnn.Query(@"UPDATE Entry SET carid = @CarId, entrydate = @EntryDate, priceperliter = @PricePerLiter, 
                               amountoffuel = @AmountOfFuel, drivendistance = @DrivenDistance, totalamount = @TotalAmount, 
                               costperhundredkilometer = @CostPerHundredKilometer, 
                               consumptationperhundredkilometer = @ConsumptationPerHundredKilometer WHERE id = @Id", entryModelToUpdate);
                }
            }
            catch (SQLiteException ex)
            {
                LogHelper.WriteToLog("UpdateEntryInDatabase Error", LogState.Error);
            }
        }
        /// <summary>
        /// Deletes the given EntryModel from Database
        /// </summary>
        /// <param name="entryToDelete">EntryModel to remove</param>
        public static void DeleteEntryFromDatabase(EntryModel entryToDelete)
        {
            try
            {
                using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
                {
                    cnn.Query($"DELETE FROM Entry WHERE id = {entryToDelete.Id}");
                }
            }
            catch (SQLiteException ex)
            {
                LogHelper.WriteToLog("DeleteEntryFromDatabase Error", LogState.Error);
            }
        }

        #endregion

        #region private Methods
        /// <summary>
        /// Reads the Connection String from the App.config
        /// </summary>
        /// <param name="id"></param>
        /// <returns>The Connection String from App.config</returns>
        private static string LoadConnectionString(string id = "Default")
        {
            return ConfigurationManager.ConnectionStrings[id].ConnectionString;
        }

        #endregion

        #endregion

        #region EventHandler

        #endregion
    }
}