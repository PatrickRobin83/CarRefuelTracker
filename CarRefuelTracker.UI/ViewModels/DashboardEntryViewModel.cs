/*
*----------------------------------------------------------------------------------
*          Filename:	DashboardEntryViewModel.cs
*          Date:        2020.06.12 17:52:07
*          All rights reserved
*
*----------------------------------------------------------------------------------
* @author Patrick Robin <support@rietrob.de>
*/

using System.Collections.ObjectModel;
using Caliburn.Micro;
using CarRefuelTracker.UI.Models;

namespace CarRefuelTracker.UI.ViewModels

{
    public class DashboardEntryViewModel : Screen
    {

        #region Fields

        private CarModel carModel;
        private ObservableCollection<EntryModel> carEntrys;
        private EntryModel selectedEntryModel;

        #endregion

        #region Properties

        public CarModel CarModel
        {
            get { return carModel; }
            set
            {
                carModel = value;
                NotifyOfPropertyChange(()=>CarModel);
            }
        }
        public ObservableCollection<EntryModel> CarEntrys
        {
            get { return carEntrys; }
            set
            {
                carEntrys = value;
                NotifyOfPropertyChange(() => CarEntrys);
            }
        }

        public EntryModel SelectedEntryModel
        {
            get { return selectedEntryModel; }
            set
            {
                selectedEntryModel = value;
                NotifyOfPropertyChange(() => SelectedEntryModel);
            }
        }

        #endregion

        #region Constructor

        public DashboardEntryViewModel(CarModel selectedCarModel)
        {
            if (selectedCarModel != null)
            {
                CarModel = selectedCarModel;
                CarEntrys = selectedCarModel.Entries;
            }
        }

        #endregion

        #region Methods

        #endregion

        #region EventHandler

        #endregion

    }
}