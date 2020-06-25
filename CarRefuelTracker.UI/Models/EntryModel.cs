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
    /// <summary>
    /// Represents a Entry for a Car.
    /// </summary>
    public class EntryModel
    {
        #region Fields

        #endregion

        #region Properties

        /// <summary>
        /// Unique Identifier
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Id of the CarModel
        /// </summary>
        public int CarId { get; set; }
        /// <summary>
        /// Date of Refueling
        /// </summary>
        public string EntryDate { get; set; }
        /// <summary>
        /// Price for one liter fuel
        /// </summary>
        public string PricePerLiter { get; set; }

        /// <summary>
        /// Total Amount of liters of refueling
        /// </summary>
        public string AmountOffuel { get; set; }
        /// <summary>
        /// Driven Distance after the last refueling
        /// </summary>
        public string DrivenDistance { get; set; }
        /// <summary>
        /// Total Costs for the refueling
        /// </summary>
        public string TotalAmount { get; set; }
        /// <summary>
        /// The Cost for hundred Kilometer (Calculated)
        /// </summary>
        public string CostPerHundredKilometer { get; set; }
        /// <summary>
        /// Consumption per hundred Kilometer (Calculated)
        /// </summary>
        public string ConsumptationPerHundredKilometer { get; set; }

        #endregion

        #region Constructor

        #endregion

        #region Methods


        #endregion

        #region EventHandler

        #endregion


    }
}