/*
*----------------------------------------------------------------------------------
*          Filename:	AddBrandViewModel.cs
*          Date:        2020.06.13 17:18:07
*          All rights reserved
*
*----------------------------------------------------------------------------------
* @author Patrick Robin <support@rietrob.de>
*/

using Caliburn.Micro;
using CarRefuelTracker.UI.DataAccess;
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
            TryClose();
        }

        public void AddBrand()
        {
            Brand = new BrandModel();
            Brand.BrandName = BrandName;
            Brand = SqliteDataAccess.AddBrand(Brand);
            EventAggregationProvider.EventAggregator.PublishOnUIThread(Brand);
            TryClose();

        }
        #endregion

        #region EventHandler

        #endregion


    }
}