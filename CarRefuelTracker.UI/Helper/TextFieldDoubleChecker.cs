/*
*----------------------------------------------------------------------------------
*          Filename:	TextFieldDoubleChecker.cs
*          Date:        2020.06.20 00:31:40
*          All rights reserved
*
*----------------------------------------------------------------------------------
* @author Patrick Robin <support@rietrob.de>
*/

using System.Runtime.InteropServices;
using CarRefuelTracker.UI.Enums;

namespace CarRefuelTracker.UI.Helper

{
    public static class TextFieldDoubleChecker
    {

        #region Fields

        

        #endregion

        #region Properties

        #endregion

        #region Constructor

        #endregion

        #region Methods
        /// <summary>
        /// checks if the given string is an valid value
        /// </summary>
        /// <param name="inputToCheck">string to try to convert</param>
        /// <returns>Bool if the given string is convertable to an double</returns>
        public static bool CheckIsInputDouble(string inputToCheck)
        {
            bool isInputBool = false;
            double result = 0;

            if (inputToCheck != "")
            {
                return double.TryParse(inputToCheck, out result);
            }
            else
            {
                return true;
            }
        }

        #endregion

        #region EventHandler

        #endregion


    }
}