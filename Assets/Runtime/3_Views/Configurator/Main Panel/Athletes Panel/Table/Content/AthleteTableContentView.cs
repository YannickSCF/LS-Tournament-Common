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
        private Dictionary<AthleteInfoType, bool> _columnsHidden;
        private Dictionary<AthleteInfoType, bool> _columnsDisabled;

        #region Mono
        private void Awake() {
            _rows = new List<AthleteRowView>();

            _columnsHidden = new Dictionary<AthleteInfoType, bool>();
            _columnsDisabled = new Dictionary<AthleteInfoType, bool>();

            Array infoTypes = Enum.GetValues(typeof(AthleteInfoType));
            foreach (Enum infoType in infoTypes) {
                _columnsHidden.Add((AthleteInfoType)infoType, false);
                _columnsDisabled.Add((AthleteInfoType)infoType, false);
            }
        }
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
            //newRow.SetColor(color, true);
            //newRow.SetBirthDate(birthData, true);
            //newRow.SetStartDate(startDate, true);

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
            foreach (KeyValuePair<AthleteInfoType, bool> columnHideInfo in _columnsHidden) {
                rowToUpdate.HideColumn(columnHideInfo.Key, columnHideInfo.Value, true);
            }

            foreach (KeyValuePair<AthleteInfoType, bool> columnDisabledInfo in _columnsHidden) {
                rowToUpdate.HideColumn(columnDisabledInfo.Key, columnDisabledInfo.Value, true);
            }
        }

        public void HideRowColumns(AthleteInfoType checkboxInfo, bool hide) {
            _columnsHidden[checkboxInfo] = hide;
            foreach (AthleteRowView row in _rows) {
                row.HideColumn(checkboxInfo, hide);
            }
        }

        public void DisableRowColumns(AthleteInfoType checkboxInfo, bool isChecked) {
            _columnsDisabled[checkboxInfo] = isChecked;
            foreach (AthleteRowView row in _rows) {
                row.DisableColumn(checkboxInfo, isChecked);
            }
        }
    }
}
