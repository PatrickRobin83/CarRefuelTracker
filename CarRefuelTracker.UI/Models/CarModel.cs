/*
*----------------------------------------------------------------------------------
*          Filename:	CarModel.cs
*          Date:        2020.06.12 17:33:54
*          All rights reserved
*
*----------------------------------------------------------------------------------
* @author Patrick Robin <support@rietrob.de>
*/

using System.Collections.ObjectModel;

namespace CarRefuelTracker.UI.Models

{
    public class CarModel
    {

        #region Fields
        #endregion

        #region Properties

        public int Id { get; set; }
        public bool IsActive { get; set; }
        public int BrandId { get; set; }
        public int ModelId { get; set; }
        public int TypeoffuelId { get; set; }
        public ModelTypeModel ModelType { get; set; }
        public FuelTypeModel FuelType { get; set; }
        public BrandModel Brand { get; set; }
        public double CarTaxPerYear { get; set; }
        public double CarInsurance { get; set; }
        public ObservableCollection<EntryModel> Entries { get; set; }

        #endregion

        #region Constructor

        public CarModel()
        {
            Brand = new BrandModel();
            ModelType = new ModelTypeModel();
            FuelType = new FuelTypeModel();
            Brand.Id = BrandId;
            ModelType.Id = ModelId;
            FuelType.Id = TypeoffuelId;
        }

        #endregion

        #region Methods

        #endregion

        #region EventHandler

        #endregion


    }
}