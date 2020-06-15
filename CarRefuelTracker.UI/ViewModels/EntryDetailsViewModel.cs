/*
 * -----------------------------------------------------------------------------
 *	 
 *   Filename		:   EntryDetailsViewModel.cs
 *   Date			:   2020-06-15 10:37:17
 *   All rights reserved
 * 
 * -----------------------------------------------------------------------------
 * @author     Patrick Robin <support@rietrob.de>
 * @Version      1.0.0
 */

using System;
using Caliburn.Micro;
using CarRefuelTracker.UI.DataAccess;
using CarRefuelTracker.UI.Models;

namespace CarRefuelTracker.UI.ViewModels
{
    public class EntryDetailsViewModel : Screen
    {


        #region Fields
        private EntryModel selectedEntryModel;
        private int id;
        private int carId;
        private DateTime entryDate;
        private string pricePerLiter;
        private string amountOfFuel;
        private string drivenDistance;
        private string totalAmount;
        private string costPerHundredKilometer;
        private string consumptationOfHundredKilometer;

        private DateTime pickedDate;

        #endregion

        #region Properties

        public EntryModel SelectedEntryModel
        {
            get
            {
                return selectedEntryModel;

            }
            set
            {
                selectedEntryModel = value;
                NotifyOfPropertyChange(() => SelectedEntryModel);
            }
        }
        public int Id
        {
            get
            {
                return id;
            }
            set
            {
                id = value;
                NotifyOfPropertyChange(() => Id);
            }
        }
        public int CarId
        {
            get
            {
                return carId;
            }
            set
            {
                carId = value;
                NotifyOfPropertyChange(() => CarId);
            }
        }
        public DateTime EntryDate
        {
            get
            {
                return entryDate; 

            }
            set
            {
                entryDate = value; 
                NotifyOfPropertyChange(() => EntryDate);
            }
        }
        public string PricePerLiter
        {
            get { return pricePerLiter; }
            set
            {
                pricePerLiter = value; 
                NotifyOfPropertyChange(() => PricePerLiter);
            }
        }
        public string AmountOfFuel
        {
            get { return amountOfFuel; }
            set
            {
                amountOfFuel = value; 
                NotifyOfPropertyChange(() => AmountOfFuel);
            }
        }
        public string DrivenDistance
        {
            get { return drivenDistance; }
            set
            {
                drivenDistance = value; 
                NotifyOfPropertyChange(() => DrivenDistance);
            }
        }
        public string TotalAmount
        {
            get { return totalAmount; }
            set
            {
                totalAmount = value; 
                NotifyOfPropertyChange(() => TotalAmount);
            }
        }
        public string CostPerHundredKilometer
        {
            get { return costPerHundredKilometer; }
            set
            {
                costPerHundredKilometer = value; 
                NotifyOfPropertyChange(() => CostPerHundredKilometer);
            }
        }
        public string ConsumptationOfHundredKilometer
        {
            get { return consumptationOfHundredKilometer; }
            set
            {
                consumptationOfHundredKilometer = value; 
                NotifyOfPropertyChange(() => ConsumptationOfHundredKilometer);
            }
        }

        public DateTime PickedDate
        {
            get
            {
                return pickedDate;
            }
            set
            {
                pickedDate = value;
                NotifyOfPropertyChange(() => PickedDate);
            }
        }

        #endregion

        #region Constructor

        public EntryDetailsViewModel(CarModel carModel)
        {
            EntryDate = DateTime.Today;
            CarId = carModel.Id;
        }

        public EntryDetailsViewModel(EntryModel entryModel)
        {
            if (entryModel != null)
            {
                SelectedEntryModel = entryModel;
            }

            EntryDate.ToShortDateString();
            EntryDate = Convert.ToDateTime(SelectedEntryModel.EntryDate);
            Id = SelectedEntryModel.Id;
            CarId = SelectedEntryModel.CarId;
            PricePerLiter = SelectedEntryModel.PricePerLiter;
            AmountOfFuel = SelectedEntryModel.AmountOffuel;
            DrivenDistance = SelectedEntryModel.DrivenDistance;
            TotalAmount = SelectedEntryModel.TotalAmount;
            CostPerHundredKilometer = SelectedEntryModel.CostPerHundredKilometer;
            ConsumptationOfHundredKilometer = SelectedEntryModel.ConsumptationPerHundredKilometer;
        }
        #endregion

        #region Methods

        public void SaveEntry()
        {
            EntryModel entryModelToSave = new EntryModel();

            entryModelToSave.Id = Id;
            entryModelToSave.CarId = CarId;
            entryModelToSave.EntryDate = EntryDate.ToShortDateString();
            entryModelToSave.PricePerLiter = PricePerLiter;
            entryModelToSave.AmountOffuel = AmountOfFuel;
            entryModelToSave.DrivenDistance = DrivenDistance;
            entryModelToSave.TotalAmount = CalculateTotalAmount();
            entryModelToSave.ConsumptationPerHundredKilometer = CalculateConsumptationOfHundredKilometer();
            entryModelToSave.CostPerHundredKilometer = CalculateCostsPerHundredKilometer();
            
            if (SelectedEntryModel != null && SelectedEntryModel.Id > 0)
            {
               SqliteDataAccess.UpdateEntryInDatabase(entryModelToSave);
            }
            else
            {
                SqliteDataAccess.SaveEntryInDatabase(entryModelToSave);
                SelectedEntryModel = entryModelToSave;
            }
            EventAggregationProvider.EventAggregator.PublishOnUIThread(SelectedEntryModel);
            TryClose();
        }

        public void CancelEntryDetailsView()
        {
            EventAggregationProvider.EventAggregator.PublishOnUIThread(new EntryModel());
            TryClose();
        }

        #endregion

        #region private Methods
        private string CalculateTotalAmount()
        {
            double result; 

            result = Convert.ToDouble(PricePerLiter) * Convert.ToDouble(AmountOfFuel);

            result = Math.Round(result, 2);

            TotalAmount = result.ToString();

            return TotalAmount;
        }
        private string CalculateConsumptationOfHundredKilometer()
        {
            double result;
            
            result = (Convert.ToDouble(AmountOfFuel) / Convert.ToDouble(DrivenDistance)) * 100;

            result = Math.Round(result, 2);

            ConsumptationOfHundredKilometer = result.ToString();

            return ConsumptationOfHundredKilometer;
        }
        private string CalculateCostsPerHundredKilometer()
        {
            double result;

            result = Convert.ToDouble(ConsumptationOfHundredKilometer) * Convert.ToDouble(PricePerLiter);

            result = Math.Round(result, 2);

            CostPerHundredKilometer = result.ToString();

            return CostPerHundredKilometer;
        }

        #endregion
    }
}