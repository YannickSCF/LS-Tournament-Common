// Custom Dependencies
using static YannickSCF.GeneralApp.CommonEventsDelegates;

namespace YannickSCF.LSTournaments.Common.Views.MainPanel.AthletesPanel.Events {
    public static class AthletesPanelViewEvents {

        // ------------------------- Specific Delegates -------------------------

        public delegate void AthleteDataEvent(AthleteInfoType infoType, string dataUpdated, int AthleteIndex);
        public delegate void AthleteInfoCheckboxEvent(AthleteInfoType checkboxInfo, bool isChecked);

        // ------------------------------- Events -------------------------------

        #region --------------- Athletes panel events ---------------
        public static event SimpleEventDelegate OnLoadAthletesFromFile;
        public static void ThrowOnLoadAthletesFromFile() {
            OnLoadAthletesFromFile?.Invoke();
        }

        public static event SimpleEventDelegate OnAthleteAdded;
        public static void ThrowOnAthleteAdded() {
            OnAthleteAdded?.Invoke();
        }

        public static event SimpleEventDelegate OnAthleteRemoved;
        public static void ThrowOnAthleteRemoved() {
            OnAthleteRemoved?.Invoke();
        }

        public static event AthleteDataEvent OnAthleteDataUpdated;
        public static void ThrowOnAthleteDataUpdated(
            AthleteInfoType infoType, string dataUpdated, int AthleteIndex) {
            OnAthleteDataUpdated?.Invoke(infoType, dataUpdated, AthleteIndex);
        }

        public static event AthleteInfoCheckboxEvent OnAthleteInfoCheckboxToggle;
        public static void ThrowOnAthleteInfoCheckboxToggle(AthleteInfoType checkboxInfo, bool isChecked) {
            OnAthleteInfoCheckboxToggle?.Invoke(checkboxInfo, isChecked);
        }
        #endregion
    }
}
