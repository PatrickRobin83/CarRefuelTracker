/*
 * -----------------------------------------------------------------------------
 *	 
 *   Filename		:   Bootstrapper.cs
 *   Date			:   2020-06-12 11:45:37
 *   All rights reserved
 * 
 * -----------------------------------------------------------------------------
 * @author     Patrick Robin <support@rietrob.de>
 * @Version      1.0.0
 */

using System.Windows;
using Caliburn.Micro;
using CarRefuelTracker.UI.ViewModels;

namespace CarRefuelTracker.UI
{
    public class Bootstrapper : BootstrapperBase
    {
        

        #region Fields

        #endregion

        #region Properties

        #endregion

        #region Constructor

        public Bootstrapper()
        {
            Initialize();
        }

        #endregion

        #region Methods

        protected override void OnStartup(object sender, StartupEventArgs e)
        {
            DisplayRootViewFor<DashboardViewModel>();
        }

        #endregion

    }
}