/**
 * Author:      Yannick Santa Cruz Feuillias
 * Created:     28/09/2023
 **/

// Dependencies
using System;
using System.Collections.Generic;
using UnityEngine;
// Custom dependencies
using static YannickSCF.GeneralApp.CommonEventsDelegates;

namespace YannickSCF.LSTournaments.Common.Views.MainPanel.AthletesDataPanel.Events {
    public static class AthletesPanelViewEvents {

        // ------------------------- Specific Delegates -------------------------

        public delegate void AthleteInfoCheckboxEvent(AthleteInfoType checkboxInfo, bool isChecked);

        public delegate void AthleteDataStringEvent(AthleteInfoType infoType, string updatedData, int athleteIndex);
        public delegate void AthleteDataDateTimeEvent(AthleteInfoType infoType, DateTime updatedData, int athleteIndex);
        public delegate void AthleteDataRankEvent(RankType updatedData, int athleteIndex);
        public delegate void AthleteDataStylesEvent(List<StyleType> updatedData, int athleteIndex);
        public delegate void AthleteDataColorEvent(Color updatedData, int athleteIndex);

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

        public static event AthleteInfoCheckboxEvent OnAthleteInfoCheckboxToggle;
        public static void ThrowOnAthleteInfoCheckboxToggle(AthleteInfoType checkboxInfo, bool isChecked) {
            OnAthleteInfoCheckboxToggle?.Invoke(checkboxInfo, isChecked);
        }

        public static event AthleteDataStringEvent OnAthleteDataStringUpdated;
        public static void ThrowOnAthleteDataStringUpdated(AthleteInfoType infoType, string updatedData, int athleteIndex) {
            OnAthleteDataStringUpdated?.Invoke(infoType, updatedData, athleteIndex);
        }

        public static event AthleteDataDateTimeEvent OnAthleteDataDateTimeUpdated;
        public static void ThrowOnAthleteDataDateTimeUpdated(AthleteInfoType infoType, DateTime updatedData, int athleteIndex) {
            OnAthleteDataDateTimeUpdated?.Invoke(infoType, updatedData, athleteIndex);
        }

        public static event AthleteDataRankEvent OnAthleteDataRankUpdated;
        public static void ThrowOnAthleteDataRankUpdated(RankType updatedData, int athleteIndex) {
            OnAthleteDataRankUpdated?.Invoke(updatedData, athleteIndex);
        }

        public static event AthleteDataStylesEvent OnAthleteDataStylesUpdated;
        public static void ThrowOnAthleteDataStylesUpdated(List<StyleType> updatedData, int athleteIndex) {
            OnAthleteDataStylesUpdated?.Invoke(updatedData, athleteIndex);
        }

        public static event AthleteDataColorEvent OnAthleteDataColorUpdated;
        public static void ThrowOnAthleteDataColorUpdated(Color updatedData, int athleteIndex) {
            OnAthleteDataColorUpdated?.Invoke(updatedData, athleteIndex);
        }
        #endregion
    }
}
