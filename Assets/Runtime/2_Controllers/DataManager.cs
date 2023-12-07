/**
 * Author:      Yannick Santa Cruz Feuillias
 * Created:     13/11/2023
 **/

// Dependencies
using UnityEngine;
// Custom dependencies
using YannickSCF.GeneralApp;
using YannickSCF.LSTournaments.Common.Scriptables.Data;

namespace YannickSCF.LSTournaments.Common.Controllers {
    public class DataManager : GlobalSingleton<DataManager> {

        private const string DRAW_CONFIGURATION_PLAYER_PREF = "Tournament-Data_";

        [Header("Tournament Data")]
        [SerializeField] private TournamentData _appData;
        [SerializeField, Range(0, 254), Tooltip("When value is 0, it means infinite saves.")]
        private byte _numberOfSaves;

        [Header("Debug Data")]
        [SerializeField] private bool _debug = false;
        [SerializeField] private TournamentData _debugAppData;

        public TournamentData AppData {
            get { return _debug ? _debugAppData : _appData; }
            set {
                if (value == null) {
                    Debug.LogWarning("You cannot set to null Tournament Data!");
                }

                if (_debug) {
                    _debugAppData = value;
                } else {
                    _appData = value;
                }
            }
        }

        public void ResetData() {
            if (_debug) {
                _appData.ResetData();
            } else {
                Debug.LogWarning("You are trying to reset Test Tournament Data!");
            }
        }

        #region Draw Configuration
        public bool HasDataSaved() {
            for(int i = 0; i < _numberOfSaves; ++i) {
                if (PlayerPrefs.HasKey(DRAW_CONFIGURATION_PLAYER_PREF + i)) {
                    return true;
                }
            }

            return false;
        }

        public TournamentData GetData(int index) {
            string drawConfigJSON = PlayerPrefs.GetString(DRAW_CONFIGURATION_PLAYER_PREF + index);

            TournamentData res = null;
            JsonUtility.FromJsonOverwrite(drawConfigJSON, res);
            return res;
        }

        public void SaveData(int index = -1) {
            string dataSaveName = string.Empty;
            if (index <= -1) {
                for (int i = 0; i < _numberOfSaves; ++i) {
                    if (!PlayerPrefs.HasKey(DRAW_CONFIGURATION_PLAYER_PREF + i)) {
                        dataSaveName = DRAW_CONFIGURATION_PLAYER_PREF + i;
                        break;
                    }
                }
            } else {
                dataSaveName = DRAW_CONFIGURATION_PLAYER_PREF + index;
            }

            PlayerPrefs.SetString(dataSaveName, JsonUtility.ToJson(AppData));
            PlayerPrefs.Save();
        }

        public TournamentData LoadData(int index) {
            string drawConfigJSON = PlayerPrefs.GetString(DRAW_CONFIGURATION_PLAYER_PREF + index);
            JsonUtility.FromJsonOverwrite(drawConfigJSON, _appData);
            return AppData;
        }

        public void DeleteData(int index) {
            PlayerPrefs.DeleteKey(DRAW_CONFIGURATION_PLAYER_PREF + index);
        }
        #endregion
    }
}
