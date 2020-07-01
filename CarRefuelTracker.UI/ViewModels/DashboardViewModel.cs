/*
 * -----------------------------------------------------------------------------
 *	 
 *   Filename		:   DashboardViewModel.cs
 *   Date			:   2020-06-12 11:49:30
 *   All rights reserved
 * 
 * -----------------------------------------------------------------------------
 * @author     Patrick Robin <support@rietrob.de>
 * @Version      1.0.0
 */

using System;
using System.Collections.ObjectModel;
using Caliburn.Micro;
using CarRefuelTracker.UI.DataAccess;
using CarRefuelTracker.UI.Enums;
using CarRefuelTracker.UI.Helper;
using CarRefuelTracker.UI.Models;

[assembly: log4net.Config.XmlConfigurator(Watch = true)]

namespace CarRefuelTracker.UI.ViewModels
{
    public class DashboardViewModel : Conductor<object>.Collection.OneActive, IHandle<CarModel>
    {
        #region Fields

        private IWindowManager windowManager;
        private DashboardEntryViewModel selectedCarEntryViewModel;
        private CarDetailsViewModel carDetailsViewModel;
        private CarModel selectedCarModel;
        private ObservableCollection<CarModel> availableCars;
        private bool selectedEntryViewModelIsVisible = false;
        private bool selectedCreateCarViewModelIsVisible = false;
        #endregion

        #region Properties

        public DashboardEntryViewModel SelectedCarEntryViewModel
        {
            get { return selectedCarEntryViewModel; }
            set
            {
                selectedCarEntryViewModel = value;
                NotifyOfPropertyChange(() => SelectedCarEntryViewModel);
            }
        }
        public CarDetailsViewModel CarDetailsViewModel
        {
            get { return carDetailsViewModel; }
            set
            {
                carDetailsViewModel = value;
                NotifyOfPropertyChange(() => CarDetailsViewModel);
            }
        }
        public CarModel SelectedCarModel
        {
            get
            {
                return selectedCarModel;
            }
            set
            {
                selectedCarModel = value;
                NotifyOfPropertyChange(() => SelectedCarModel);
                NotifyOfPropertyChange(() => CanEditCar);
                NotifyOfPropertyChange(() => CanDeleteCar);
                ShowEntriesForSelectedCar();
                SelectedEntryViewModelIsVisible = SelectedCarModel != null;
            }
        }
        public ObservableCollection<CarModel> AvailableCars
        {
            get
            {
                return availableCars;
            }
            set
            {
                availableCars = value;
                NotifyOfPropertyChange(nameof(AvailableCars));
            }
        }
        public bool SelectedEntryViewModelIsVisible
        {
            get
            {
                return selectedEntryViewModelIsVisible;
            }
            set
            {
                selectedEntryViewModelIsVisible = value;
                NotifyOfPropertyChange(() => SelectedEntryViewModelIsVisible);
            }
        }
        public bool SelectedCreateCarViewModelIsVisible
        {
            get
            {
                return selectedCreateCarViewModelIsVisible;
            }
            set
            {
                selectedCreateCarViewModelIsVisible = value;
                NotifyOfPropertyChange(() => SelectedCreateCarViewModelIsVisible);
                NotifyOfPropertyChange(() => CanCreateNewCar);
            }
        }
        public bool CanEditCar
        {
            get
            {
                bool result = false;

                if (SelectedCarModel != null)
                {
                    result = true;
                }

                return result;
            }
        }
        public bool CanDeleteCar
        {
            get
            {
                bool result = false;

                if (SelectedCarModel != null)
                {
                    result = true;
                }

                return result;
            }
        }
        public bool CanCreateNewCar
        {
            get
            {
                if (SelectedCreateCarViewModelIsVisible)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }

        #endregion

        #region Constructor

        public DashboardViewModel()
        {
            windowManager = new WindowManager();
            AvailableCars = new ObservableCollection<CarModel>(SqliteDataAccess.LoadCars());
            EventAggregationProvider.EventAggregator.Subscribe(this);
            LogHelper.WriteToLog("Application started",LogState.Info);
        }

        #endregion

        #region Methods

        public void ShowEntriesForSelectedCar()
        {
            SelectedCarEntryViewModel = new DashboardEntryViewModel(SelectedCarModel);
        }
        /// <summary>
        /// Opens the CarDetailsView with the values from the selected car filled in the form
        /// </summary>
        public void EditCar()
        {
            CarDetailsViewModel = new CarDetailsViewModel(SelectedCarModel);
            SelectedCreateCarViewModelIsVisible = true;
            LogHelper.WriteToLog("EditCar Button clicked",LogState.Debug);
        }
        /// <summary>
        /// Deletes the selected car from Database
        /// </summary>
        public void DeleteCar()
        {
            SqliteDataAccess.DeleteCar(SelectedCarModel);
            NotifyOfPropertyChange(() => AvailableCars);
            LogHelper.WriteToLog($"Car {SelectedCarModel.Brand} {SelectedCarModel.ModelType} was deleted", LogState.Info);
        }
        /// <summary>
        /// Opens the CarDetailsView with an empty  form
        /// </summary>
        public void CreateNewCar()
        {
            SelectedCarModel = null;
            CarDetailsViewModel = new CarDetailsViewModel();
            SelectedCreateCarViewModelIsVisible = true;
            LogHelper.WriteToLog("CreateNewCarButton clicked", LogState.Debug);
        }
        /// <summary>
        /// Close the Application and writes the Event into the log file
        /// </summary>
        public void Exit()
        {
            LogHelper.WriteToLog("Application Exited", LogState.Info);
            Environment.Exit(0);
        }

        #region Implementation of IHandle<CarModel>
        public void Handle(CarModel carModel)
        {
            if (carModel != null)
            {
                AvailableCars = new ObservableCollection<CarModel>(SqliteDataAccess.LoadCars());
                NotifyOfPropertyChange(() => AvailableCars);
            }
            AvailableCars = new ObservableCollection<CarModel>(SqliteDataAccess.LoadCars());
            SelectedCreateCarViewModelIsVisible = false;
        }

        #endregion

        #endregion
    }
}