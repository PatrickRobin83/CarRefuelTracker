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
using System.Data.SQLite;
using System.Linq;
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
                // ToDo: Implement the DeleteCar Query
                throw new NotImplementedException("Not implemented Yet");
            }
        }
        public static List<BrandModel> LoadAllBrands()
        {
            List<BrandModel> allBrands = new List<BrandModel>();
            using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
            {
                allBrands = cnn.Query<BrandModel>("SELECT * FROM Brand").ToList();
                return allBrands;
            }
        }
        public static List<ModelTypeModel> ModelsFromBrands(int brandId)
        {
            List<ModelTypeModel> carModels = new List<ModelTypeModel>();
            using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
            {
                carModels = cnn.Query<ModelTypeModel>($"SELECT * FROM Model WHERE {brandId} = brandId").ToList();

                return carModels;
            }
        }
        #endregion

        #region Brand Operations

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

        #endregion

        #region Model Operations

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

        #endregion

        #region Entry Operations
        //ToDo: implement all Methods to update, insert, and delete entries
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