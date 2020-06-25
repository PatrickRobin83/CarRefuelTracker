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
    /// <summary>
    /// This class holds the data for a Brand. It's an unique int Id and a string Brandname
    /// </summary>
    public class BrandModel
    {


        #region Fields
        #endregion

        #region Properties
        /// <summary>
        /// represents an int unique identifier. Its is also the primary Key in the sqlite database
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// represents the name of the brand (like Mercedes or Opel)
        /// </summary>
        public string BrandName { get; set; }

        #endregion

        #region Constructor

        #endregion

        #region Methods

        /// <summary>
        /// overrides the ToString() Method from object. If BrandModel.ToString() is called the value of the BrandName will be returned
        /// </summary>
        /// <returns>string BrandName</returns>
        public override string ToString()
        {
            return BrandName;
        }
        #endregion

    }
}
