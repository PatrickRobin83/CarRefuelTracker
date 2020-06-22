/*
*----------------------------------------------------------------------------------
*          Filename:	AddBrandViewModel.cs
*          Date:        2020.06.13 17:18:07
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
    public class AddBrandViewModel : Screen
    {

        #region Fields

        private BrandModel brand;
        private string brandName;

        #endregion

        #region Properties

        public string BrandName
        {
            get { return brandName;}
            set
            {
                brandName = value;
                NotifyOfPropertyChange(() => BrandName);
            }
        }

        public BrandModel Brand
        {
            get
            {
                return brand;
            }
            set
            {
                brand = value;
                NotifyOfPropertyChange(() => Brand);
            }
        }

        #endregion

        #region Constructor

        #endregion

        #region Methods

        public void Cancel()
        {
            LogHelper.WriteToLog("Cancel Button clicked, no Brand created", LogState.Debug);
            TryClose();
        }

        public void AddBrand()
        {
            LogHelper.WriteToLog("Brand creation started", LogState.Debug);
            Brand = new BrandModel();
            Brand.BrandName = BrandName;
            Brand = SqliteDataAccess.AddBrand(Brand);
            EventAggregationProvider.EventAggregator.PublishOnUIThread(Brand);
            if (Brand != null && Brand.BrandName != "")
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