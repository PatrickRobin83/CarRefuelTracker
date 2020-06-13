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

        public string ModelName { get; set; }
        public int Id { get; set; }

        #endregion

        #region Constructor

        #endregion

        #region Methods

        public override string ToString()
        {
            return ModelName;
        }

        #endregion

        #region EventHandler

        #endregion


    }
}