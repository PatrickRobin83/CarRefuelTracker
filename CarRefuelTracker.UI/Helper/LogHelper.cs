/*
 * -----------------------------------------------------------------------------
 *	 
 *   Filename		:   LogHelper.cs
 *   Date			:   2020-06-22 13:19:59
 *   All rights reserved
 * 
 * -----------------------------------------------------------------------------
 * @author     Patrick Robin <support@rietrob.de>
 * @Version      1.0.0
 */

using System.Runtime.CompilerServices;

namespace CarRefuelTracker.UI.Helper
{
    public class LogHelper
    {
        

        #region Fields

        #endregion

        #region Properties

        #endregion

        #region Constructor

        #endregion

        #region Methods

        public static log4net.ILog GetLogger([CallerFilePath] string filename = "")
        {
            return log4net.LogManager.GetLogger(filename);
        }

        #endregion

    }
}