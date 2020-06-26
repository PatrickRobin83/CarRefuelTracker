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
    /// <summary>
    /// ViewModel for AddFuelTypeView
    /// </summary>
    public class AddFuelTypeViewModel : Screen
    {

        #region Fields

        private FuelTypeModel fuelTypeModel;
        private string fuelTypeName;
        #endregion

        #region Properties
        /// <summary>
        /// FuelTypeModel that will saved in database or shown in the textbox
        /// </summary>
        public FuelTypeModel FuelTypeModel
        {
            get { return fuelTypeModel; }
            set
            {
                fuelTypeModel = value;
                NotifyOfPropertyChange(() => FuelTypeModel);
            }
        }
        /// <summary>
        /// FuelTypeName string that represents the text from the Textbox
        /// </summary>
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

        /// <summary>
        /// This method is called by pressing the Cancel Button and closes the AddFuelTypeView. Logs the Event in the LogFile
        /// </summary>
        public void Cancel()
        {
            LogHelper.WriteToLog("Cancel Button clicked, no fuelType created", LogState.Debug);
            TryClose();
        }

        /// <summary>
        /// Assign the values from the TextBox to the FuelTypeModel Properties. Save the FuelTypeModel in the Database.
        /// Logs the Event in the LogFileName. Closes the Add FuelTypeView
        /// </summary>
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