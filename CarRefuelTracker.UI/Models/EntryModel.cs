/*
*----------------------------------------------------------------------------------
*          Filename:	EntryModel.cs
*          Date:        2020.06.12 17:34:43
*          All rights reserved
*
*----------------------------------------------------------------------------------
* @author Patrick Robin <support@rietrob.de>
*/

using System;

namespace CarRefuelTracker.UI.Models

{
    public class EntryModel
    {
        private int id;
        private int carId;
        private string entryDate;
        private string pricePerLiter;
        private string amountOffuel;
        private string drivenDistance;
        private string totalAmount;
        private string costPerHundredKilometer;
        private string consumptationPerHundredKilometer;

        #region Fields
        
        #endregion

        #region Properties

        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        public int CarId
        {
            get { return carId; }
            set { carId = value; }
        }

        public string EntryDate
        {
            get { return entryDate; }
            set { entryDate = value; }
        }

        public string PricePerLiter
        {
            get { return pricePerLiter; }
            set { pricePerLiter = value; }
        }

        public string AmountOffuel
        {
            get { return amountOffuel; }
            set { amountOffuel = value; }
        }

        public string DrivenDistance
        {
            get { return drivenDistance; }
            set { drivenDistance = value; }
        }

        public string TotalAmount
        {
            get { return totalAmount; }
            set { totalAmount = value; }
        }

        public string CostPerHundredKilometer
        {
            get { return costPerHundredKilometer; }
            set { costPerHundredKilometer = value; }
        }

        public string ConsumptationPerHundredKilometer
        {
            get { return consumptationPerHundredKilometer; }
            set { consumptationPerHundredKilometer = value; }
        }

        #endregion

        #region Constructor

        #endregion

        #region Methods


        #endregion

        #region EventHandler

        #endregion


    }
}