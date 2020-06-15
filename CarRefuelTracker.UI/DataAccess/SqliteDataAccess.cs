/*
*----------------------------------------------------------------------------------
*          Filename:	SqliteDataAccess.cs
*          Date:        2020.06.12 17:40:49
*          All rights reserved
*
*----------------------------------------------------------------------------------
* @author Patrick Robin <support@rietrob.de>
*/

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Data;
using System.Data.Entity.Infrastructure.Design;
using System.Data.SQLite;
using System.Linq;
using Caliburn.Micro;
using CarRefuelTracker.UI.Models;
using Dapper;

namespace CarRefuelTracker.UI.DataAccess

{
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
        public static List<CarModel> LoadCars()
        {
            List<CarModel> allCarModels = new List<CarModel>();
            using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
            {
                allCarModels = cnn.Query<CarModel>("SELECT * FROM Car WHERE isActive = 1").ToList();

                foreach (CarModel carModel in allCarModels)
                {
                    List<EntryModel> allEntriesForCar = new List<EntryModel>();
                    carModel.Brand = cnn.QueryFirst<BrandModel>($"SELECT * FROM Brand WHERE {carModel.BrandId} = id");
                    carModel.ModelType = cnn.QueryFirst<ModelTypeModel>($"SELECT * FROM Model WHERE {carModel.ModelId} = id");
                    carModel.FuelType = cnn.QueryFirst<FuelTypeModel>($"SELECT * from TypeOfFuel WHERE {carModel.TypeoffuelId} = id");
                    allEntriesForCar = cnn.Query<EntryModel>($"SELECT * FROM Entry WHERE {carModel.Id} = carId").ToList();
                    carModel.Entries = new ObservableCollection<EntryModel>(allEntriesForCar);
                }
            }
            return allCarModels;
        }
        public static CarModel SaveCar(CarModel carToSave)
        {
            using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
            {
                carToSave = cnn.Query<CarModel>(@"INSERT INTO Car(brandid, modelid, typeoffuelid, cartaxperyear, 
                                                   carinsurance, isActive) 
                                                   VALUES(@BrandId, @ModelId, @TypeOfFuelId, @CarTaxPerYear, 
                                                   @CarInsurance, @IsActive); SELECT last_insert_rowid()",carToSave).First();
            }

            return carToSave;
        }
        public static void UpdateCar(CarModel carToUpdate)
        {
            using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
            {
                cnn.Query(@"UPDATE Car SET brandid = @BrandId, modelid = @ModelId, typeoffuelid = @TypeOfFuelId, 
                              cartaxperyear = @CarTaxPerYear, carinsurance = @CarInsurance, isActive = @isActive 
                              WHERE id = @Id",carToUpdate);
            }
        }
        public static void DeleteCar(CarModel carToDelete)
        {
            using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
            {
                cnn.Query($"UPDATE Car SET isActive = 0");
            }
        }
        #endregion

        #region Brand Operations
        public static List<BrandModel> LoadAllBrands()
        {
            List<BrandModel> allBrands = new List<BrandModel>();
            using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
            {
                allBrands = cnn.Query<BrandModel>("SELECT * FROM Brand").ToList();
                return allBrands;
            }
        }
        public static BrandModel AddBrand(BrandModel brand)
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
        public static void RemoveBrandFromDataBase(BrandModel brand)
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

        #endregion

        #region Model Operations
        public static List<ModelTypeModel> ModelsFromBrands(int brandId)
        {
            List<ModelTypeModel> carModels = new List<ModelTypeModel>();
            using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
            {
                carModels = cnn.Query<ModelTypeModel>($"SELECT * FROM Model WHERE {brandId} = brandId").ToList();

                return carModels;
            }
        }
        public static ModelTypeModel AddModel(ModelTypeModel modeltype, BrandModel brandmodel)
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
        public static void RemoveModelTypeFromDatabase(ModelTypeModel model)
        {
            using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
            {
                cnn.Query($"DELETE FROM Model WHERE id = {model.Id}");
                cnn.Query($"DELETE FROM Car WHERE modelId = {model.Id}");
            }
        }

        #endregion

        #region FuelType Operations

        public static List<FuelTypeModel> LoadAllFuelTypes()
        {
            List<FuelTypeModel> fuelTypesList = new List<FuelTypeModel>();

            using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
            {
                fuelTypesList = cnn.Query<FuelTypeModel>($"SELECT * FROM TypeOfFuel").ToList();

                return fuelTypesList;
            }
        }

        public static FuelTypeModel AddFuelType(FuelTypeModel fuelType)
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

        public static void RemoveFuelTypeFromDatabase(FuelTypeModel fuelTypeModel)
        {
            using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
            {
                cnn.Query($"DELETE FROM TypeOfFuel WHERE id = {fuelTypeModel.Id}");
                cnn.Query($"DELETE FROM Car WHERE typeoffuelid = {fuelTypeModel.Id}");
            }
        }

        #endregion

        #region Entry Operations

        public static List<EntryModel> LoadEntrysForCar(int carId)
        {
            List<EntryModel> entryModels = new List<EntryModel>();
            using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
            {
                entryModels = cnn.Query<EntryModel>($"SELECT * FROM Entry WHERE CarId = '{carId}'").ToList();

                return entryModels;
            }
        }

        public static EntryModel SaveEntryInDatabase(EntryModel entryToSave)
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

        public static void UpdateEntryInDatabase(EntryModel entryModelToUpdate)
        {
            try
            {
                using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
                {
                    cnn.Query(@"UPDATE Entry SET id = @CarId, entrydate = @EntryDate, priceperliter = @PricePerLiter, 
                               amountoffuel = @AmountOfFuel, drivendistance = @DrivenDistance, totalamount = @TotalAmount, 
                               costperhundredkilometer = @CostPerHundredKilometer, 
                               consumptationperhundredkilometer = @ConsumptationPerHundredKilometer", entryModelToUpdate);
                }
            }
            catch (SQLiteException e)
            {
                Console.WriteLine(e);
            }
        }

        public static void DeleteEntryFromDatabase(EntryModel entryToDelete)
        {
            using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
            {
                cnn.Query($"DELETE FROM Entry WHERE id = {entryToDelete.Id}");
            }
        }

        #endregion

        #region private Methods

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