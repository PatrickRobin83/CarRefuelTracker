/*
*----------------------------------------------------------------------------------
*          Filename:	FuelTypeModel.cs
*          Date:        2020.06.12 17:37:48
*          All rights reserved
*
*----------------------------------------------------------------------------------
* @author Patrick Robin <support@rietrob.de>
*/

namespace CarRefuelTracker.UI.Models

{
    public class FuelTypeModel
    {
        #region Fields

        #endregion

        #region Properties
        /// <summary>
        /// Unique identifier
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Name of the FuelType
        /// </summary>
        public string TypeOfFuel { get; set; }

        #endregion

        #region Constructor

        #endregion

        #region Methods

        public override string ToString()
        {
            return TypeOfFuel;
        }

        #endregion

    }
}