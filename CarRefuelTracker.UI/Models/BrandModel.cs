/*
 * -----------------------------------------------------------------------------
 *	 
 *   Filename		:   BrandModel.cs
 *   Date			:   2020-06-05 11:49:53
 *   All rights reserved
 * 
 * -----------------------------------------------------------------------------
 * @author     Patrick Robin <support@rietrob.de>
 * @Version      1.0.0
 */

namespace CarRefuelTracker.UI.Models
{
    public class BrandModel
    {


        #region Fields
        #endregion

        #region Properties

        public int Id { get; set; }
        public string BrandName { get; set; }

        #endregion

        #region Constructor

        #endregion

        #region Methods

        public override string ToString()
        {
            return BrandName;
        }
        #endregion

    }
}
