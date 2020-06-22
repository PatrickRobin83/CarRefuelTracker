/*
*----------------------------------------------------------------------------------
*          Filename:	AddFuelTypeViewModel.cs
*          Date:        2020.06.13 23:31:12
*          All rights reserved
*
*----------------------------------------------------------------------------------
* @author Patrick Robin <support@rietrob.de>
*/

using Caliburn.Micro;
using CarRefuelTracker.UI.DataAccess;
using CarRefuelTracker.UI.Enums;
using CarRefuelTracker.UI.Helper;
using CarRefuelTracker.UI.Models;

namespace CarRefuelTracker.UI.ViewModels

{
    public class AddFuelTypeViewModel : Screen
    {

        #region Fields

        private FuelTypeModel fuelTypeModel;
        private string fuelTypeName;
        #endregion

        #region Properties
        public FuelTypeModel FuelTypeModel
        {
            get { return fuelTypeModel; }
            set
            {
                fuelTypeModel = value;
                NotifyOfPropertyChange(() => FuelTypeModel);
            }
        }
        public string FuelTypeName
        {
            get { return fuelTypeName; }
            set
            {
                fuelTypeName = value; 
                NotifyOfPropertyChange(() => FuelTypeName);
            }
        }
        #endregion

        #region Constructor

        #endregion

        #region Methods

        public void Cancel()
        {
            LogHelper.WriteToLog("Cancel Button clicked, no fuelType created", LogState.Debug);
            TryClose();
        }

        public void AddFuelType()
        {
            LogHelper.WriteToLog("FuelType creation started", LogState.Debug);
            FuelTypeModel = new FuelTypeModel();
            FuelTypeModel.TypeOfFuel = FuelTypeName;
            FuelTypeModel = SqliteDataAccess.AddFuelType(FuelTypeModel);
            EventAggregationProvider.EventAggregator.PublishOnUIThread(FuelTypeModel);
            if (FuelTypeModel.TypeOfFuel != null && FuelTypeModel.TypeOfFuel != "")
            {
                LogHelper.WriteToLog("Brand created", LogState.Info);
            }
            TryClose();
        }

        #endregion

        #region EventHandler

        #endregion


    }
}