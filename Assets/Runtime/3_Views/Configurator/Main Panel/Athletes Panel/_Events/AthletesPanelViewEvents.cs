// Custom Dependencies
using static YannickSCF.GeneralApp.CommonEventsDelegates;

namespace YannickSCF.LSTournaments.Common.Views.MainPanel.AthletesPanel.Events {
    public static class AthletesPanelViewEvents {

        // ------------------------- Specific Delegates -------------------------

        public delegate void ParticipantDataEvent(AthleteInfoType infoType, string dataUpdated, int participantIndex);
        public delegate void ParticipantInfoCheckboxEvent(AthleteInfoType checkboxInfo, bool isChecked);

        // ------------------------------- Events -------------------------------

        #region --------------- Participants panel events ---------------
        public static event SimpleEventDelegate OnLoadParticipantsFromFile;
        public static void ThrowOnLoadParticipantsFromFile() {
            OnLoadParticipantsFromFile?.Invoke();
        }

        public static event SimpleEventDelegate OnParticipantAdded;
        public static void ThrowOnParticipantAdded() {
            OnParticipantAdded?.Invoke();
        }

        public static event SimpleEventDelegate OnParticipantRemoved;
        public static void ThrowOnParticipantRemoved() {
            OnParticipantRemoved?.Invoke();
        }

        public static event ParticipantDataEvent OnParticipantDataUpdated;
        public static void ThrowOnParticipantDataUpdated(
            AthleteInfoType infoType, string dataUpdated, int participantIndex) {
            OnParticipantDataUpdated?.Invoke(infoType, dataUpdated, participantIndex);
        }
        #endregion
    }
}
