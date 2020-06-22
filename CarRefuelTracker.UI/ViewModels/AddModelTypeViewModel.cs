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
    public class AddModelTypeViewModel : Screen
    {

        #region Fields

        private BrandModel brandModel;
        private string modelTypeModelName;
        private ModelTypeModel modelTypeModel;
        #endregion

        #region Properties

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

        public AddModelTypeViewModel(BrandModel brandModel)
        {
            BrandModel = brandModel;
            
        }

        #endregion

        #region Methods

        public void Cancel()
        {
            LogHelper.WriteToLog("Cancel Button clicked, no modelType created", LogState.Debug);
            TryClose();
        }

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