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
            NotifyOfPropertyChange(() => BrandModel);
        }

        #endregion

        #region Methods

        public void Cancel()
        {
            TryClose();
        }

        public void AddModelType()
        {
            ModelTypeModel = new ModelTypeModel();
            ModelTypeModel.ModelName = ModelTypeModelName;
            ModelTypeModel = SqliteDataAccess.AddModel(ModelTypeModel, BrandModel);
            EventAggregationProvider.EventAggregator.PublishOnUIThread(ModelTypeModel);
            TryClose();
        }

        #endregion

        #region EventHandler

        #endregion


    }
}