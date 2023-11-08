/**
 * Author:      Yannick Santa Cruz Feuillias
 * Created:     29/09/2023
 **/

// Dependencies
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
// Custom dependencies
using YannickSCF.LSTournaments.Common.Views.MainPanel.AthletesPanel.Content.Table.Row;

namespace YannickSCF.LSTournaments.Common.Views.MainPanel.AthletesPanel.Table.Content {
    public class AthleteTableContentView : MonoBehaviour {

        [SerializeField] private ScrollRect _tableScrollRect;
        [SerializeField] private AthleteRowView _athleteRowPrefab;

        private List<AthleteRowView> _rows;

        #region Mono
        private void Awake() {
            _rows = new List<AthleteRowView>();
        }
        #endregion

        #region Methods to Add/Remove athletes from table
        public int AddAthleteRow() {
            AthleteRowView newRow = Instantiate(_athleteRowPrefab, _tableScrollRect.content);
            
            newRow.SetAthleteRowIndex(_rows.Count);

            _rows.Add(newRow);
            return _rows.Count;
        }

        public void AddAthleteInfo(string countryCode, string surname, string name,
            string academy, string school, RankType rank, List<StyleType> styles,
            int tier, Color color, DateTime birthData, DateTime startDate) {

            AthleteRowView newRow = Instantiate(_athleteRowPrefab, _tableScrollRect.content);
            newRow.SetAthleteRowIndex(_rows.Count);

            newRow.SetCountryField(countryCode, true);
            newRow.SetSurnameField(surname, true);
            newRow.SetNameField(name, true);
            newRow.SetAcademyField(academy, true);
            newRow.SetSchoolField(school, true);
            newRow.SetRankField(rank, true);
            newRow.SetStylesField(styles, true);
            newRow.SetTierField(tier, true);
            newRow.SetColorField(color, true);
            newRow.SetBirthDateField(birthData, true);
            newRow.SetStartDateField(startDate, true);

            _rows.Add(newRow);
        }

        public int RemoveLastAthleteRow() {
            if (_rows.Count > 0) {
                AthleteRowView rowToDelete = _rows[_rows.Count - 1];
                _rows.RemoveAt(_rows.Count - 1);
                DestroyImmediate(rowToDelete.gameObject);
            }

            return _rows.Count;
        }

        public void ResetContent() {
            for (int i = 0; i < _rows.Count; ++i) {
                _rows[i].gameObject.SetActive(false);
                DestroyImmediate(_rows[i].gameObject);
            }
            _rows.Clear();
        }
        #endregion

        #region Methods to change athletes info visibility
        public void ShowRowColumn(int rowToUpdate, AthleteInfoType checkboxInfo, bool show) {
            _rows[rowToUpdate].ShowRowColumn(checkboxInfo, show);
        }

        public void ShowRowColumns(AthleteInfoType checkboxInfo, bool show) {
            foreach (AthleteRowView row in _rows) {
                row.ShowRowColumn(checkboxInfo, show);
            }
        }

        public void EnableRowColumn(int rowToUpdate, AthleteInfoType checkboxInfo, bool enable) {
            _rows[rowToUpdate].EnableRowColumn(checkboxInfo, enable);
        }

        public void EnableRowColumns(AthleteInfoType checkboxInfo, bool enable) {
            foreach (AthleteRowView row in _rows) {
                row.EnableRowColumn(checkboxInfo, enable);
            }
        }
        #endregion
    }
}
