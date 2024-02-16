/**
 * Author:      Yannick Santa Cruz Feuillias
 * Created:     14/02/2024
 **/

// Dependencies
using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
//Custom dependencies
using YannickSCF.LSTournaments.Common.Views.PouleTable.Design;
using YannickSCF.LSTournaments.Common.Views.PouleTable.Objects;

namespace YannickSCF.LSTournaments.Common.Views.PouleTable {
    public class PouleTableView : MonoBehaviour {

        [Header("Main Game Objects")]
        [SerializeField] private TextMeshProUGUI _titleText;
        [SerializeField] private List<PouleAthleteView> _athletesRows;

        private PouleTableViewEditor _editorView;
        private PouleTableViewEditor EditorView {
            get {
                if (_editorView == null)
                    _editorView = GetComponent<PouleTableViewEditor>();

                return _editorView;
            }
        }

        #region Mono
        //private void OnEnable() {
        //    BasicPouleScoreView.OnScoreHover += ActiveHelp;
        //    BasicPouleScoreView.OnScoreExit += DeactiveHelp;
        //}

        //private void OnDisable() {
        //    BasicPouleScoreView.OnScoreHover -= ActiveHelp;
        //    BasicPouleScoreView.OnScoreExit -= DeactiveHelp;
        //}
        #endregion

        //#region Local Event listeners
        //private void ActiveHelp(int pouleHash, int mainScoreIndex, int secondScoreIndex) {
        //    if (pouleHash == GetHashCode()) {
        //        _athletesListParent[mainScoreIndex].ActiveHelp(true, true);
        //        _athletesListParent[secondScoreIndex].ActiveHelp(true, false);

        //        _columnsListParent[mainScoreIndex].ActiveHelp(secondScoreIndex, true, false);
        //        _columnsListParent[secondScoreIndex].ActiveHelp(mainScoreIndex, true, true);
        //    }
        //}

        //private void DeactiveHelp(int pouleHash, int mainScoreIndex, int secondScoreIndex) {
        //    if (pouleHash == GetHashCode()) {
        //        _athletesListParent[mainScoreIndex].ActiveHelp(false, true);
        //        _athletesListParent[secondScoreIndex].ActiveHelp(false, false);

        //        _columnsListParent[mainScoreIndex].ActiveHelp(secondScoreIndex, false, false);
        //        _columnsListParent[secondScoreIndex].ActiveHelp(mainScoreIndex, false, true);
        //    }
        //}
        //#endregion

        public void SetPouleTitle(string title) {
            _titleText.text = title;
        }

        public void SetAthletes(List<string> athletesNames) {
            for (int i = 0; i < _athletesRows.Count; ++i) {
                if (i < athletesNames.Count) {
                    _athletesRows[i].SetName(athletesNames[i]);
                } else {
                    Debug.LogWarning("Too many athletes for this poule!");
                    break;
                }
            }

            EditorView.SetNumberOfAthletes(athletesNames.Count);
        }

        public void SetAthletes(List<AthleteBasicInfo> athletes) {
            for (int i = 0; i < _athletesRows.Count; ++i) {
                if (i < athletes.Count) {
                    if (!string.IsNullOrEmpty(athletes[i].CountryId)) {
                        if (!string.IsNullOrEmpty(athletes[i].SelectedOrigin)) {
                            _athletesRows[i].SetNameWithOrigin(athletes[i].CompleteName, athletes[i].SelectedOrigin, athletes[i].CountryId);
                        } else {
                            _athletesRows[i].SetNameWithFlag(athletes[i].CompleteName, athletes[i].CountryId);
                        }
                    } else {
                        if (!string.IsNullOrEmpty(athletes[i].SelectedOrigin)) {
                            _athletesRows[i].SetNameWithOrigin(athletes[i].CompleteName, athletes[i].SelectedOrigin);
                        } else {
                            _athletesRows[i].SetName(athletes[i].CompleteName);
                        }
                    }
                } else {
                    Debug.LogWarning("Too many athletes for this poule!");
                    break;
                }
            }

            EditorView.SetNumberOfAthletes(athletes.Count);
        }

        public void SetAthleteScoreAgainst(int athleteIndex, int againstIndex, int athleteScore) {
            _athletesRows[athleteIndex].SetScore(againstIndex, athleteScore);
        }

        public void SetAthleteStyleAgainst(int athleteIndex, int againstIndex, float athleteStyle) {
            _athletesRows[athleteIndex].SetStyle(againstIndex, athleteStyle);
        }

        public void SetVictories(int athleteIndex, int count) {
            _athletesRows[athleteIndex].SetResultScore(PouleDataBoxView.DataBoxType.ResultWin, count.ToString());
        }

        public void SetDefeats(int athleteIndex, int count) {
            _athletesRows[athleteIndex].SetResultScore(PouleDataBoxView.DataBoxType.ResultDefeat, count.ToString());
        }

        public void SetTies(int athleteIndex, int count) {
            _athletesRows[athleteIndex].SetResultScore(PouleDataBoxView.DataBoxType.ResultTie, count.ToString());
        }

        public void SetOHInFavor(int athleteIndex, int count) {
            _athletesRows[athleteIndex].SetResultScore(PouleDataBoxView.DataBoxType.ResultOHFavor, count.ToString());
        }

        public void SetOHAgainst(int athleteIndex, int count) {
            _athletesRows[athleteIndex].SetResultScore(PouleDataBoxView.DataBoxType.ResultOHAgainst, count.ToString());
        }

        public void SetTotalStyle(int athleteIndex, float count) {
            _athletesRows[athleteIndex].SetResultScore(PouleDataBoxView.DataBoxType.ResultStyle, count.ToString());
        }

        public void SetLSScore(int athleteIndex, float count) {
            _athletesRows[athleteIndex].SetResultScore(PouleDataBoxView.DataBoxType.ResultScore, count.ToString());
        }

        public void ResetAllScores() {
            foreach (PouleAthleteView row in _athletesRows) {
                row.ResetAllScores();
            }
        }
    }
}
