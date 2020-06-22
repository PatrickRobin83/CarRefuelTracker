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
        #region Fields

        #endregion

        #region Properties

        public int Id { get; set; }

        public int CarId { get; set; }

        public string EntryDate { get; set; }

        public string PricePerLiter { get; set; }

        public string AmountOffuel { get; set; }

        public string DrivenDistance { get; set; }

        public string TotalAmount { get; set; }

        public string CostPerHundredKilometer { get; set; }

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