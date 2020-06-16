/*
*----------------------------------------------------------------------------------
*          Filename:	CarDetailsViewModel.cs
*          Date:        2020.06.12 18:07:48
*          All rights reserved
*
*----------------------------------------------------------------------------------
* @author Patrick Robin <support@rietrob.de>
*/

using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Dynamic;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Caliburn.Micro;
using CarRefuelTracker.UI.DataAccess;
using CarRefuelTracker.UI.Models;

namespace CarRefuelTracker.UI.ViewModels

{
    public class CarDetailsViewModel : Conductor<object>.Collection.OneActive, 
                                      IHandle<BrandModel>, IHandle<ModelTypeModel>, IHandle<FuelTypeModel>
    {

        #region Fields

        private CarModel carModel;
        private int id;
        private bool isActive;
        private BrandModel selectedBrand;
        private ModelTypeModel selectedModelType;
        private FuelTypeModel selectedFuelType;
        private string carTaxPerYear;
        private string carInsurance;
        private ObservableCollection<EntryModel> entries;
        private ObservableCollection<BrandModel> availableBrands;
        private ObservableCollection<ModelTypeModel> availableCarModels;
        private ObservableCollection<FuelTypeModel> availableFuelTypes;
        private IDataErrorInfo dataErrorInfoImplementation;

        #endregion

        #region Properties
        public CarModel CarModel
        {
            get
            {
                return carModel;
            }
            set
            {
                carModel = value;
                NotifyOfPropertyChange(nameof(CarModel));
            }
        }
        public int Id
        {
            get { return id; }
            set
            {
                id = value;
                NotifyOfPropertyChange(nameof(Id));
            }
        }
        public bool IsActive
        {
            get
            {
                return isActive;
            }
            set
            {
                isActive = value;
                NotifyOfPropertyChange(nameof(IsActive));
            }
        }
        public BrandModel SelectedBrand
        {
            get
            {
                return selectedBrand;
            }
            set
            {
                selectedBrand = value;
                if (SelectedBrand != null && SelectedBrand.Id != 0)
                {
                    AvailableCarModels = new ObservableCollection<ModelTypeModel>(SqliteDataAccess.ModelsFromBrands(SelectedBrand.Id));
                }
                NotifyOfPropertyChange(() => SelectedBrand);
                NotifyOfPropertyChange(() => AvailableCarModels);
            }
        }
        public ModelTypeModel SelectedModelType
        {
            get { return selectedModelType; }
            set
            {
                selectedModelType = value;
                NotifyOfPropertyChange(nameof(SelectedModelType));
            }
        }
        public FuelTypeModel SelectedFuelType
        {
            get { return selectedFuelType; }
            set
            {
                selectedFuelType = value;
                NotifyOfPropertyChange(nameof(SelectedFuelType));
            }
        }
        public ObservableCollection<EntryModel> Entries
        {
            get { return entries; }
            set
            {
                entries = value;
                NotifyOfPropertyChange(nameof(Entries));
            }
        }
        public ObservableCollection<BrandModel> AvailableBrands
        {
            get { return availableBrands; }
            set
            {
                availableBrands = value;
                NotifyOfPropertyChange(nameof(AvailableBrands));
                NotifyOfPropertyChange(() => AvailableCarModels);
            }
        }
        public ObservableCollection<ModelTypeModel> AvailableCarModels
        {
            get { return availableCarModels; }
            set
            {
                availableCarModels = value;
                NotifyOfPropertyChange(nameof(AvailableCarModels));
            }
        }
        public ObservableCollection<FuelTypeModel> AvailableFuelTypes
        {
            get { return availableFuelTypes; }
            set
            {
                availableFuelTypes = value;
                NotifyOfPropertyChange(nameof(AvailableFuelTypes));
            }
        }

        #endregion

        #region Constructor
        public CarDetailsViewModel()
        {
            carModel = new CarModel();
            AvailableFuelTypes = new ObservableCollection<FuelTypeModel>(SqliteDataAccess.LoadAllFuelTypes());
            AvailableBrands = new ObservableCollection<BrandModel>(SqliteDataAccess.LoadAllBrands());

            if (AvailableBrands != null && AvailableBrands.Count > 0)
            {
                SelectedBrand = AvailableBrands.First();
            }

            if (AvailableCarModels != null && AvailableCarModels.Count > 0)
            {
                SelectedModelType = AvailableCarModels.First();
            }

            if (AvailableFuelTypes != null && AvailableFuelTypes.Count > 0)
            {
                SelectedFuelType = AvailableFuelTypes.First();
            }
            
            IsActive = true;
            EventAggregationProvider.EventAggregator.Subscribe(this);
        }
        public CarDetailsViewModel(CarModel carModel)
        {
            CarModel = carModel;
            DataToControls();
            EventAggregationProvider.EventAggregator.Subscribe(this);
        }
        #endregion

        #region Methods

        private void DataToControls()
        {
            AvailableBrands = new ObservableCollection<BrandModel>(SqliteDataAccess.LoadAllBrands());
            AvailableFuelTypes = new ObservableCollection<FuelTypeModel>(SqliteDataAccess.LoadAllFuelTypes());
                Id = CarModel.Id;
                IsActive = CarModel.IsActive;
                SelectedBrand = CarModel.Brand;
                AvailableCarModels = new ObservableCollection<ModelTypeModel>(SqliteDataAccess.ModelsFromBrands(SelectedBrand.Id));
                SelectedModelType = CarModel.ModelType;
                SelectedFuelType = CarModel.FuelType;
                Entries = CarModel.Entries;
                NotifyOfPropertyChange(() => SelectedBrand);
                NotifyOfPropertyChange(() => SelectedModelType);
                NotifyOfPropertyChange(() => SelectedFuelType);
                NotifyOfPropertyChange(() => AvailableBrands);
                NotifyOfPropertyChange(() => AvailableCarModels);
                NotifyOfPropertyChange(() => AvailableFuelTypes);

        }
        public void SaveCar()
        {
            CarModel carToSave = new CarModel();
            carToSave.Id = Id;
            carToSave.IsActive = IsActive;
            carToSave.Brand = SelectedBrand;
            carToSave.BrandId = SelectedBrand.Id;
            carToSave.ModelType = SelectedModelType;
            carToSave.ModelId = SelectedModelType.Id;
            carToSave.FuelType = SelectedFuelType;
            carToSave.TypeoffuelId = SelectedFuelType.Id;
            carToSave.Entries = Entries;

            if (carToSave.Id > 0)
            {
                SqliteDataAccess.UpdateCar(carToSave);
            }
            else
            {
                SqliteDataAccess.SaveCar(carToSave);
            }
            EventAggregationProvider.EventAggregator.PublishOnUIThread(carToSave);
            TryClose();
        }
        public void AddBrand()
        {
            var addBrandDialog = new AddBrandViewModel();
            var vm = new WindowManager();
            dynamic settings = new ExpandoObject();
            settings.Windowstyle = WindowStyle.None;
            settings.Title = "Marke hinzufügen";
            settings.ResizeMode = ResizeMode.NoResize;
            settings.ShowInTaskbar = false;
            vm.ShowDialog(addBrandDialog, null, settings);
        }

        public void RemoveBrand()
        {
            SqliteDataAccess.RemoveBrandFromDataBase(SelectedBrand);
            AvailableBrands = new ObservableCollection<BrandModel>(SqliteDataAccess.LoadAllBrands());
            if (AvailableBrands.Count > 0)
            {
                SelectedBrand = AvailableBrands.First();
            }
        }

        public void AddModelType()
        {
            var addModelType = new AddModelTypeViewModel(SelectedBrand);
            var vm = new WindowManager();
            dynamic settings = new ExpandoObject();
            settings.Windowstyle = WindowStyle.None;
            settings.Title = "Modell hinzufügen";
            settings.ResizeMode = ResizeMode.NoResize;
            settings.ShowInTaskbar = false;
            vm.ShowDialog(addModelType, null,settings);
        }

        public void RemoveModelType()
        {
            SqliteDataAccess.RemoveModelTypeFromDatabase(SelectedModelType);
            SelectedBrand = AvailableBrands.First();
            AvailableCarModels = new ObservableCollection<ModelTypeModel>(SqliteDataAccess.ModelsFromBrands(SelectedBrand.Id));
        }

        public void AddFuelType()
        {
            var addFuelTypeDialog = new AddFuelTypeViewModel();
            var vm = new WindowManager();
            dynamic settings = new ExpandoObject();
            settings.Windowstyle = WindowStyle.None;
            settings.Title = "Kraftstoffart hinzufügen";
            settings.ResizeMode = ResizeMode.NoResize;
            settings.ShowInTaskbar = false;
            vm.ShowDialog(addFuelTypeDialog, null, settings);
        }

        public void RemoveFuelType()
        {
            SqliteDataAccess.RemoveFuelTypeFromDatabase(SelectedFuelType);
            AvailableFuelTypes = new ObservableCollection<FuelTypeModel>(SqliteDataAccess.LoadAllFuelTypes());
            if (SelectedFuelType != null && AvailableFuelTypes.Count > 0)
            {
                SelectedFuelType = AvailableFuelTypes.First();
            }
        }

        public void CancelCreateCar()
        {
            EventAggregationProvider.EventAggregator.PublishOnUIThread(new CarModel());
            TryClose();
        }


        #endregion

        #region EventHandler

        #region Implementation of IHandle<BrandModel>

        public void Handle(BrandModel brandModel)
        {
            AvailableBrands = new ObservableCollection<BrandModel>(SqliteDataAccess.LoadAllBrands());
            SelectedBrand = brandModel;
            NotifyOfPropertyChange(() => AvailableBrands);
            NotifyOfPropertyChange(() => SelectedBrand);
        }

        #endregion

        #region Implementation of IHandle<ModelTypeModel>

        public void Handle(ModelTypeModel modelTypeModel)
        {
            AvailableCarModels = new ObservableCollection<ModelTypeModel>(SqliteDataAccess.ModelsFromBrands(SelectedBrand.Id));
            SelectedModelType = modelTypeModel;
            NotifyOfPropertyChange(() => AvailableCarModels);
            NotifyOfPropertyChange(() => SelectedModelType);
        }

        #endregion

        #region Implementation of IHandle<FuelTypeModel>

        public void Handle(FuelTypeModel fuelTypeModel)
        {
            AvailableFuelTypes = new ObservableCollection<FuelTypeModel>(SqliteDataAccess.LoadAllFuelTypes());
            SelectedFuelType = fuelTypeModel;
            NotifyOfPropertyChange(() => AvailableFuelTypes);
            NotifyOfPropertyChange(() => SelectedFuelType);
        }

        #endregion

        #endregion

        
    }
}