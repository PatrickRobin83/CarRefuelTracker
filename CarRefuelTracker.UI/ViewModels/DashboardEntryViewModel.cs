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
using System.Globalization;
using System.Threading;
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
        private string averagePricePerLiter;
        private string averageFuelAmount;
        private string averageRefuelCosts;
        private string averageDrivenDistance;
        private string averageConsumption;
        private string averageCosts;
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
        public string AveragePricePerLiter
        {
            get { return averagePricePerLiter; }
            set
            {
                averagePricePerLiter = value; 
                NotifyOfPropertyChange(() => AveragePricePerLiter);
            }
        }
        public string AverageFuelAmount
        {
            get { return averageFuelAmount; }
            set
            {
                averageFuelAmount = value; 
                NotifyOfPropertyChange(() => AverageFuelAmount);
            }
        }
        public string AverageRefuelCosts
        {
            get { return averageRefuelCosts; }
            set
            {
                averageRefuelCosts = value; 
                NotifyOfPropertyChange(() => AverageRefuelCosts);
            }
        }
        public string AverageDrivenDistance
        {
            get { return averageDrivenDistance; }
            set
            {
                averageDrivenDistance = value; 
                NotifyOfPropertyChange(() => AverageDrivenDistance);
            }
        }
        public string AverageConsumption
        {
            get { return averageConsumption; }
            set
            {
                averageConsumption = value; 
                NotifyOfPropertyChange(() => AverageConsumption);
            }
        }
        public string AverageCosts
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
            #region double.IsNan Validation

            if (!double.IsNaN(Math.Round(tmpPricePerLiter / CarEntrys.Count, 3)))
            {
                AveragePricePerLiter = Convert.ToString(Math.Round(tmpPricePerLiter / CarEntrys.Count, 3));

            }
            else
            {
                AveragePricePerLiter = "0";
            }
            if(!double.IsNaN(Math.Round(tmpFuelAmount, 2)))
            {
                AverageFuelAmount = Convert.ToString(Math.Round(tmpFuelAmount, 2));
            }
            else
            {
                AverageFuelAmount = "0";
            }

            if (!double.IsNaN(Math.Round(tmpRefuelCosts, 2)))
            {
                AverageRefuelCosts = Convert.ToString(Math.Round(tmpRefuelCosts, 2));
            }
            else
            {
                AverageRefuelCosts = "0";
            }

            if (!double.IsNaN(Math.Round(tmpDrivenDistance, 2)))
            {
                AverageDrivenDistance = Convert.ToString(Math.Round(tmpDrivenDistance, 2));
            }
            else
            {
                AverageDrivenDistance = "0";
            }

            if (!double.IsNaN(Math.Round(tmpConsumption / CarEntrys.Count, 2)))
            {
                AverageConsumption = Convert.ToString(Math.Round(tmpConsumption / CarEntrys.Count, 2));
            }
            else
            {
                AverageConsumption = "0";
            }
            if (!double.IsNaN(Math.Round(tmpCosts / CarEntrys.Count, 2)))
            {
                AverageCosts = Convert.ToString(Math.Round(tmpCosts / CarEntrys.Count, 2));
            }
            else
            {
                AverageCosts = "0";
            }

            #endregion
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