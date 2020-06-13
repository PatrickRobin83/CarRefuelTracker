/*
*----------------------------------------------------------------------------------
*          Filename:	EntryModel.cs
*          Date:        2020.06.12 17:34:43
*          All rights reserved
*
*----------------------------------------------------------------------------------
* @author Patrick Robin <support@rietrob.de>
*/

namespace CarRefuelTracker.UI.Models

{
    public class EntryModel
    {

        #region Fields

        #endregion

        #region Properties
        public int Id { get; set; }

        public string Entrydate { get; set; }

        public double PricePerLiter { get; set; }

        public double AmountOffuel { get; set; }

        public double DrivenDistance { get; set; }

        public double Totalamount { get; set; }

        public double CostPerHundredKilometer { get; set; }

        public double ConsumptationPerHundredKilometer { get; set; }

        #endregion

        #region Constructor

        public EntryModel()
        {
            //   CalculateTotalCosts(PricePerLiter, AmountOffuel);
            //   CalculateConsumptionPerHundredKilometer(AmountOffuel, DrivenDistance);
        }

        #endregion

        #region Methods

        private double CalculateTotalCosts(double pricePerLiter, double amountOfRefuel)
        {
            this.PricePerLiter = pricePerLiter;
            this.AmountOffuel = amountOfRefuel;

            Totalamount = pricePerLiter * amountOfRefuel;


            return Totalamount;
        }

        private double CalculateConsumptionPerHundredKilometer(double amountOfFuel, double drivenDistance)
        {
            ConsumptationPerHundredKilometer = (amountOfFuel / drivenDistance) * 100;

            return ConsumptationPerHundredKilometer;
        }

        #endregion

        #region EventHandler

        #endregion


    }
}