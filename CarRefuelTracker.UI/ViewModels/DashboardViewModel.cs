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
using System.Windows;
using Caliburn.Micro;
using CarRefuelTracker.UI.DataAccess;
using CarRefuelTracker.UI.Models;

namespace CarRefuelTracker.UI.ViewModels
{
    public class DashboardViewModel : Conductor<object>.Collection.OneActive, IHandle<EntryModel>, IHandle<CarModel>
    {
        

        #region Fields

        private IWindowManager windowManager;
        private DashboardEntryViewModel selectedEntryViewModel;
        private CarDetailsViewModel carDetailsViewModel;
        private CarModel selectedCarModel;
        private ObservableCollection<CarModel> availableCars;
        private bool selectedEntryViewModelIsVisible = false;
        private bool selectedCreateCarViewModelIsVisible = false;


        #endregion

        #region Properties

        public DashboardEntryViewModel SelectedEntryViewModel
        {
            get { return selectedEntryViewModel; }
            set
            {
                selectedEntryViewModel = value;
                NotifyOfPropertyChange(() => SelectedEntryViewModel);
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
                ShowSelectedEntryViewModel();
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
        }

        #endregion

        #region Methods

        public void ShowSelectedEntryViewModel()
        {
            SelectedEntryViewModel = new DashboardEntryViewModel(SelectedCarModel);
        }

        public void EditCar()
        {
            CarDetailsViewModel = new CarDetailsViewModel(SelectedCarModel);
            SelectedCreateCarViewModelIsVisible = true;
        }

        public void DeleteCar()
        {
            SqliteDataAccess.DeleteCar(SelectedCarModel);
        }

        public void CreateNewCar()
        {
            SelectedCarModel = null;
            CarDetailsViewModel = new CarDetailsViewModel();
            SelectedCreateCarViewModelIsVisible = true;
        }
        public void Exit()
        {
            Environment.Exit(0);
        }


        #region Implementation of IHandle<EntryModel>

        public void Handle(EntryModel entryModel)
        {
            throw new NotImplementedException();
        }

        #endregion

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