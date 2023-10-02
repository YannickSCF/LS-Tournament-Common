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
        private Dictionary<AthleteInfoType, bool> _columnsShown;
        private Dictionary<AthleteInfoType, bool> _columnsEnabled;

        #region Mono
        private void Awake() {
            _rows = new List<AthleteRowView>();

            _columnsShown = new Dictionary<AthleteInfoType, bool>();
            _columnsEnabled = new Dictionary<AthleteInfoType, bool>();

            Array infoTypes = Enum.GetValues(typeof(AthleteInfoType));
            foreach (Enum infoType in infoTypes) {
                _columnsShown.Add((AthleteInfoType)infoType, true);
                _columnsEnabled.Add((AthleteInfoType)infoType, true);
            }
        }
        #endregion

        #region GETTERS
        public string GetCountryField(int row) { return _rows[row].GetCountryField(); }
        public string GetSurnameField(int row) { return _rows[row].GetSurnameField(); }
        public string GetNameField(int row) { return _rows[row].GetNameField(); }
        public string GetAcademyField(int row) { return _rows[row].GetAcademyField(); }
        public string GetSchoolField(int row) { return _rows[row].GetSchoolField(); }
        public RankType GetRankField(int row) { return _rows[row].GetRankField(); }
        public List<StyleType> GetStylesField(int row) { return _rows[row].GetStylesField(); }
        public int GetTierField(int row) { return _rows[row].GetTierField(); }
        public Color GetColorField(int row) { return _rows[row].GetColorField(); }
        public DateTime GetBirthDateField(int row) { return _rows[row].GetBirthDateField(); }
        public DateTime GetStartDateField(int row) { return _rows[row].GetStartDateField(); }
        #endregion

        public int AddAthleteRow() {
            AthleteRowView newRow = Instantiate(_athleteRowPrefab, _tableScrollRect.content);
            
            newRow.SetAthleteRowIndex(_rows.Count);
            UpdateAllHiddenAndDisableColumns(newRow);

            _rows.Add(newRow);
            return _rows.Count;
        }

        public void AddAthleteRow(int count) {
            for (int i = 0; i < count; ++i) {
                AddAthleteRow();
            }
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

            UpdateAllHiddenAndDisableColumns(newRow);
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

        private void UpdateAllHiddenAndDisableColumns(AthleteRowView rowToUpdate) {
            foreach (KeyValuePair<AthleteInfoType, bool> columnShownInfo in _columnsShown) {
                rowToUpdate.ShowRowColumn(columnShownInfo.Key, columnShownInfo.Value);
            }

            foreach (KeyValuePair<AthleteInfoType, bool> columnEnabledInfo in _columnsEnabled) {
                rowToUpdate.EnableRowColumn(columnEnabledInfo.Key, columnEnabledInfo.Value);
            }
        }

        public void ShowRowColumns(AthleteInfoType checkboxInfo, bool show) {
            _columnsShown[checkboxInfo] = show;
            foreach (AthleteRowView row in _rows) {
                row.ShowRowColumn(checkboxInfo, show);
            }
        }

        public void EnableRowColumns(AthleteInfoType checkboxInfo, bool enable) {
            _columnsEnabled[checkboxInfo] = enable;
            foreach (AthleteRowView row in _rows) {
                row.EnableRowColumn(checkboxInfo, enable);
            }
        }
    }
}
