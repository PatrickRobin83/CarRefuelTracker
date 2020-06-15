/*
*----------------------------------------------------------------------------------
*          Filename:	DashboardEntryViewModel.cs
*          Date:        2020.06.12 17:52:07
*          All rights reserved
*
*----------------------------------------------------------------------------------
* @author Patrick Robin <support@rietrob.de>
*/

using System;
using System.Collections.ObjectModel;
using System.Dynamic;
using System.Windows;
using Caliburn.Micro;
using CarRefuelTracker.UI.DataAccess;
using CarRefuelTracker.UI.Models;

namespace CarRefuelTracker.UI.ViewModels

{
    public class DashboardEntryViewModel : Conductor<object>.Collection.OneActive, IHandle<EntryModel>
    {

        #region Fields

        private CarModel carModel;
        private int id;
        private ObservableCollection<EntryModel> carEntrys;
        private EntryModel selectedEntryModel;

        #endregion

        #region Properties
        public CarModel CarModel
        {
            get { return carModel; }
            set
            {
                carModel = value;
                NotifyOfPropertyChange(()=>CarModel);
            }
        }
        public ObservableCollection<EntryModel> CarEntrys
        {
            get { return carEntrys; }
            set
            {
                carEntrys = value;
                NotifyOfPropertyChange(() => CarEntrys);
            }
        }
        public EntryModel SelectedEntryModel
        {
            get { return selectedEntryModel; }
            set
            {
                selectedEntryModel = value;
                NotifyOfPropertyChange(() => SelectedEntryModel);
            }
        }

        #endregion

        #region Constructor

        public DashboardEntryViewModel(CarModel selectedCarModel)
        {
            if (selectedCarModel != null)
            {
                CarModel = selectedCarModel;
                CarEntrys = selectedCarModel.Entries;
                EventAggregationProvider.EventAggregator.Subscribe(this);
            }
        }

        #endregion

        #region Methods

        public void AddEntry()
        {
            var addEntryDialog = new EntryDetailsViewModel(CarModel);
            var wm = new WindowManager();
            dynamic settings = new ExpandoObject();
            settings.Windowstyle = WindowStyle.None;
            settings.Title = "Eintrag hinzufügen";
            settings.ResizeMode = ResizeMode.NoResize;
            settings.ShowInTaskbar = false;
            wm.ShowDialog(addEntryDialog, null, settings);
        }

        public void EditEntry()
        {
            var editEntryDialog = new EntryDetailsViewModel(SelectedEntryModel);
            var wm = new WindowManager();
            dynamic settings = new ExpandoObject();
            settings.Windowstyle = WindowStyle.None;
            settings.Title = "Eintrag editieren";
            settings.ResizeMode = ResizeMode.NoResize;
            settings.ShowInTaskbar = false;
            wm.ShowDialog(editEntryDialog, null, settings);
            NotifyOfPropertyChange(() => CarEntrys);
            NotifyOfPropertyChange(() => CarModel.Entries);
        }

        public void DeleteEntry()
        {
            SqliteDataAccess.DeleteEntryFromDatabase(SelectedEntryModel);
            CarModel.Entries = new ObservableCollection<EntryModel>(SqliteDataAccess.LoadEntrysForCar(CarModel.Id));
            //NotifyOfPropertyChange(() => CarEntrys);
            //NotifyOfPropertyChange(() => CarModel.Entries);
        }
        #endregion

        #region EventHandler

        #endregion

        public void Handle(EntryModel entryModel)
        {
            CarEntrys = new ObservableCollection<EntryModel>(SqliteDataAccess.LoadEntrysForCar(CarModel.Id));
           // CarModel.Entries = new ObservableCollection<EntryModel>(SqliteDataAccess.LoadEntrysForCar(CarModel.Id));
             NotifyOfPropertyChange(() => CarEntrys); 
            //NotifyOfPropertyChange(() => CarModel.Entries);

        }
    }
}