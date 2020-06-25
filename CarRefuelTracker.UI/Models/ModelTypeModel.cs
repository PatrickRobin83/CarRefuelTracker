/*
*----------------------------------------------------------------------------------
*          Filename:	ModelTypeModel.cs
*          Date:        2020.06.12 17:38:33
*          All rights reserved
*
*----------------------------------------------------------------------------------
* @author Patrick Robin <support@rietrob.de>
*/

namespace CarRefuelTracker.UI.Models

{
    public class ModelTypeModel
    {

        #region Fields

        #endregion

        #region Properties
        /// <summary>
        /// Name of the Car Model like "Corolla" from Toyota
        /// </summary>
        public string ModelName { get; set; }
        /// <summary>
        /// Unique identifier 
        /// </summary>
        public int Id { get; set; }

        #endregion

        #region Constructor

        #endregion

        #region Methods
        /// <summary>
        /// Overrides the ToString() Method from object and returns the ModelName
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return ModelName;
        }

        #endregion

        #region EventHandler

        #endregion


    }
}