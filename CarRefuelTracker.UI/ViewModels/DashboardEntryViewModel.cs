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
        private double averagePricePerLiter;
        private double averageFuelAmount;
        private double averageRefuelCosts;
        private double averageDrivenDistance;
        private double averageConsumption;
        private double averageCosts;
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
                NotifyOfPropertyChange(() => CanDeleteEntry);
                NotifyOfPropertyChange(() => CanEditEntry);
            }
        }
        public bool CanEditEntry
        {
            get
            {
                bool result = false;

                if (SelectedEntryModel != null && SelectedEntryModel.Id >= 0)
                {
                    result = true;
                }
                return result;
            }
        }
        public bool CanDeleteEntry
        {
            get
            {
                bool result = false;

                if (SelectedEntryModel != null && SelectedEntryModel.Id >= 0)
                {
                    result = true;
                }
                return result;
            }
        }
        public double AveragePricePerLiter
        {
            get { return averagePricePerLiter; }
            set
            {
                averagePricePerLiter = value; 
                NotifyOfPropertyChange(() => AveragePricePerLiter);
            }
        }
        public double AverageFuelAmount
        {
            get { return averageFuelAmount; }
            set
            {
                averageFuelAmount = value; 
                NotifyOfPropertyChange(() => AverageFuelAmount);
            }
        }
        public double AverageRefuelCosts
        {
            get { return averageRefuelCosts; }
            set
            {
                averageRefuelCosts = value; 
                NotifyOfPropertyChange(() => AverageRefuelCosts);
            }
        }
        public double AverageDrivenDistance
        {
            get { return averageDrivenDistance; }
            set
            {
                averageDrivenDistance = value; 
                NotifyOfPropertyChange(() => AverageDrivenDistance);
            }
        }
        public double AverageConsumption
        {
            get { return averageConsumption; }
            set
            {
                averageConsumption = value; 
                NotifyOfPropertyChange(() => AverageConsumption);
            }
        }
        public double AverageCosts
        {
            get { return averageCosts; }
            set
            {
                averageCosts = value; 
                NotifyOfPropertyChange(() => AverageCosts);
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
                CalculateAverages();
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
        }

        public void DeleteEntry()
        {
            SqliteDataAccess.DeleteEntryFromDatabase(SelectedEntryModel);
            CarEntrys = new ObservableCollection<EntryModel>(SqliteDataAccess.LoadEntrysForCar(CarModel.Id));
            CarModel.Entries = CarEntrys;
            CalculateAverages();
        }

        public void CalculateAverages()
        {
            double tmpPricePerLiter = 0;
            double tmpFuelAmount = 0;
            double tmpRefuelCosts = 0;
            double tmpDrivenDistance = 0;
            double tmpConsumption = 0;
            double tmpCosts = 0;

            foreach (EntryModel entryModel in CarEntrys)
            {
                tmpPricePerLiter += Convert.ToDouble(entryModel.PricePerLiter);
                tmpFuelAmount += Convert.ToDouble(entryModel.AmountOffuel);
                tmpRefuelCosts += Convert.ToDouble(entryModel.TotalAmount);
                tmpDrivenDistance += Convert.ToDouble(entryModel.DrivenDistance);
                tmpConsumption += Convert.ToDouble(entryModel.ConsumptationPerHundredKilometer);
                tmpCosts += Convert.ToDouble(entryModel.CostPerHundredKilometer);
            }
            AveragePricePerLiter = Math.Round(tmpPricePerLiter / CarEntrys.Count,3);
            AverageFuelAmount = Math.Round(tmpFuelAmount,2);
            AverageRefuelCosts = Math.Round(tmpRefuelCosts,2);
            AverageDrivenDistance = Math.Round(tmpDrivenDistance,2);
            AverageConsumption = Math.Round(tmpConsumption / CarEntrys.Count,2);
            AverageCosts = Math.Round(tmpCosts / CarEntrys.Count,2);
        }
        #endregion

        #region EventHandler

        #endregion

        public void Handle(EntryModel entryModel)
        {
            CarEntrys = new ObservableCollection<EntryModel>(SqliteDataAccess.LoadEntrysForCar(CarModel.Id));
            CarModel.Entries = CarEntrys;
            CalculateAverages();
        }
    }
}