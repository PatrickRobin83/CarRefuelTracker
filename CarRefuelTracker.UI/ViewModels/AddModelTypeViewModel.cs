/*
*----------------------------------------------------------------------------------
*          Filename:	AddModelTypeViewModel.cs
*          Date:        2020.06.13 22:24:48
*          All rights reserved
*
*----------------------------------------------------------------------------------
* @author Patrick Robin <support@rietrob.de>
*/

using System;
using Caliburn.Micro;
using CarRefuelTracker.UI.DataAccess;
using CarRefuelTracker.UI.Enums;
using CarRefuelTracker.UI.Helper;
using CarRefuelTracker.UI.Models;

namespace CarRefuelTracker.UI.ViewModels

{
    /// <summary>
    /// AddModelTypeViewModel for representing the AddModelTypView
    /// </summary>
    public class AddModelTypeViewModel : Screen
    {

        #region Fields

        private BrandModel brandModel;
        private string modelTypeModelName;
        private ModelTypeModel modelTypeModel;
        #endregion

        #region Properties

        /// <summary>
        /// BrandModel to aasign the ModelType to
        /// </summary>
        public BrandModel BrandModel
        {
            get
            {
                return brandModel;
            }
            set
            {
                brandModel = value;
                NotifyOfPropertyChange(() => BrandModel);

            }
        }
        /// <summary>
        /// ModelType Name string from the TextBox
        /// </summary>
        public string ModelTypeModelName
        {
            get
            {
                return modelTypeModelName;
            }
            set
            {
                modelTypeModelName = value;
                NotifyOfPropertyChange(() => ModelTypeModelName);
            }
        }
        /// <summary>
        /// ModelTypeModel thats provide the data to store in database
        /// </summary>
        public ModelTypeModel ModelTypeModel
        {
            get
            {
                return modelTypeModel;
            }
            set
            {
                modelTypeModel = value;
                NotifyOfPropertyChange(() => ModelTypeModel);
            }
        }

        #endregion

        #region Constructor
        /// <summary>
        /// Default Constructor with one parameter to inject.
        /// </summary>
        /// <param name="brandModel">BrandModel is needed for the BrandModel.Id to aasign the ModelTypeModel to</param>
        public AddModelTypeViewModel(BrandModel brandModel)
        {
            BrandModel = brandModel;
            
        }

        #endregion

        #region Methods
        /// <summary>
        /// This Method executes by using the Cancel Button. This Methods writes a LogMessage into the LogFile and closes the Add ModelTapeModel
        /// </summary>
        public void Cancel()
        {
            LogHelper.WriteToLog("Cancel Button clicked, no modelType created", LogState.Debug);
            TryClose();
        }
        /// <summary>
        /// This Methods executes after AddModel Button triggers. It writes a logmessage and adds the ModelType to the Database.
        /// After adding the ModelType to database closes the AddModelView
        /// </summary>
        public void AddModelType()
        {
            LogHelper.WriteToLog("ModelType creation started", LogState.Debug);
            ModelTypeModel = new ModelTypeModel();
            ModelTypeModel.ModelName = ModelTypeModelName;
            ModelTypeModel = SqliteDataAccess.AddModel(ModelTypeModel, BrandModel);
            EventAggregationProvider.EventAggregator.PublishOnUIThread(ModelTypeModel);
            if (ModelTypeModel != null && ModelTypeModel.ModelName != "")
            {
                LogHelper.WriteToLog("ModelType created", LogState.Info);
            }
            TryClose();
        }

        #endregion

        #region EventHandler

        #endregion


    }
}