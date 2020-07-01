/*
*----------------------------------------------------------------------------------
*          Filename:	CarDetailsViewModel.cs
*          Date:        2020.06.12 18:07:48
*          All rights reserved
*
*----------------------------------------------------------------------------------
* @author Patrick Robin <support@rietrob.de>
*/

using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Dynamic;
using System.Linq;
using System.Windows;
using Caliburn.Micro;
using CarRefuelTracker.UI.DataAccess;
using CarRefuelTracker.UI.Enums;
using CarRefuelTracker.UI.Helper;
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
        /// <summary>
        /// The CarModel that holds the data from the TextBoxes
        /// </summary>
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
        /// <summary>
        /// Unique Id from Database
        /// </summary>
        public int Id
        {
            get { return id; }
            set
            {
                id = value;
                NotifyOfPropertyChange(nameof(Id));
            }
        }
        /// <summary>
        /// Indicator for is the CarModel an active one
        /// </summary>
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
        /// <summary>
        /// The BrandModel which is selected in the associated ComboBox
        /// </summary>
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
        /// <summary>
        /// The ModelTypeModel which is selected in the associated ComboBox
        /// </summary>
        public ModelTypeModel SelectedModelType
        {
            get { return selectedModelType; }
            set
            {
                selectedModelType = value;
                NotifyOfPropertyChange(nameof(SelectedModelType));
            }
        }
        /// <summary>
        /// The FuelTypeModel which is selected in the associated ComboBox
        /// </summary>
        public FuelTypeModel SelectedFuelType
        {
            get { return selectedFuelType; }
            set
            {
                selectedFuelType = value;
                NotifyOfPropertyChange(nameof(SelectedFuelType));
            }
        }
        /// <summary>
        /// List of all EntryModels for the selected CarModel
        /// </summary>
        public ObservableCollection<EntryModel> Entries
        {
            get { return entries; }
            set
            {
                entries = value;
                NotifyOfPropertyChange(nameof(Entries));
            }
        }
        /// <summary>
        /// List of all available Brands in Database. Datasource for the associated ComboBox
        /// </summary>
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
        /// <summary>
        /// List of all available ModelTypeModels in the Database. Datasource for the associated ComboBox
        /// </summary>
        public ObservableCollection<ModelTypeModel> AvailableCarModels
        {
            get { return availableCarModels; }
            set
            {
                availableCarModels = value;
                NotifyOfPropertyChange(nameof(AvailableCarModels));
            }
        }
        /// <summary>
        /// List of all available FuelTypes in the Database. Datasource for the associated ComboBox
        /// </summary>
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
        /// <summary>
        /// Constructor without a given CarModel. Set all Values to Default 
        /// </summary>
        public CarDetailsViewModel()
        {
            InitializeCarDetailsView();
        }
        /// <summary>
        /// Initializes the CarViewModel with the given CarModel 
        /// </summary>
        /// <param name="carModel">Data for the UserControls in the View</param>
        public CarDetailsViewModel(CarModel carModel)
        {
            CarModel = carModel;
            DataToControls();
            EventAggregationProvider.EventAggregator.Subscribe(this);
        }
        #endregion

        #region Methods
        /// <summary>
        /// Assigns all values to default after the default Constructor is called.
        /// </summary>
        private void InitializeCarDetailsView()
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
        /// <summary>
        /// Initializes the the Details with the data from the given CarModel ou of the Constructor
        /// </summary>
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
            LogHelper.WriteToLog("Car loaded", LogState.Debug);
        }
        /// <summary>
        /// Is executed by trigger the SaveCarButton
        /// Writes the event into the LogFile, assign the values to the Properties of the Model.
        /// Save the Model to Database and write the event into the logFile
        /// </summary>
        public void SaveCar()
        {
            LogHelper.WriteToLog("Saving Car", LogState.Debug);
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
            LogHelper.WriteToLog("Car saved", LogState.Debug);
            TryClose();
        }
        /// <summary>
        /// Is executed by trigger the AddBrand Button. Shows the Dialogwindow to add a Brand to the Database
        /// </summary>
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
            LogHelper.WriteToLog("AddBrandView opened", LogState.Debug);
        }
        /// <summary>
        ///  Is executed by trigger the RemoveBrand Button. Deletes the Brand from Database.
        /// </summary>
        public void RemoveBrand()
        {
            SqliteDataAccess.RemoveBrandFromDataBase(SelectedBrand);
            AvailableBrands = new ObservableCollection<BrandModel>(SqliteDataAccess.LoadAllBrands());
            if (AvailableBrands.Count > 0)
            {
                SelectedBrand = AvailableBrands.First();
            }
            LogHelper.WriteToLog("Brand deleted", LogState.Debug);
        }
        /// <summary>
        ///  Is executed by trigger the AddModel Button. Shows the Dialogwindow to add a Model to the Database
        /// </summary>
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
            LogHelper.WriteToLog("AddModelType View opened", LogState.Debug);
        }
        /// <summary>
        ///  Is executed by trigger the RemoveBrand Button. Removes the Brand from Database
        /// </summary>
        public void RemoveModelType()
        {
            SqliteDataAccess.RemoveModelTypeFromDatabase(SelectedModelType);
            SelectedBrand = AvailableBrands.First();
            AvailableCarModels = new ObservableCollection<ModelTypeModel>(SqliteDataAccess.ModelsFromBrands(SelectedBrand.Id));
            LogHelper.WriteToLog("ModelType deleted", LogState.Debug);
        }
        /// <summary>
        ///  Is executed by trigger the AddFuelType Button. Shows the Dialogwindow to add a FuelType to the Database
        /// </summary>
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
            LogHelper.WriteToLog("FuelTypeView opened", LogState.Debug);
        }
        /// <summary>
        ///  Is executed by trigger the RemoveFuelType Button. Deletes the FuelType from Database
        /// </summary>
        public void RemoveFuelType()
        {
            SqliteDataAccess.RemoveFuelTypeFromDatabase(SelectedFuelType);
            AvailableFuelTypes = new ObservableCollection<FuelTypeModel>(SqliteDataAccess.LoadAllFuelTypes());
            if (SelectedFuelType != null && AvailableFuelTypes.Count > 0)
            {
                SelectedFuelType = AvailableFuelTypes.First();
            }
            LogHelper.WriteToLog("FuelType deleted", LogState.Debug);
        }
        /// <summary>
        /// Is executed by trigger the CancelButton. Closes the CarDetailsView
        /// </summary>
        public void CancelCreateCar()
        {
            EventAggregationProvider.EventAggregator.PublishOnUIThread(new CarModel());
            LogHelper.WriteToLog("CreateCarView closed, no car created", LogState.Debug);
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