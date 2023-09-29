// Dependencies
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

        public int AddAthleteRow() {
            AthleteRowView newRow = Instantiate(_athleteRowPrefab, _tableScrollRect.content);
            newRow.SetAthleteRowIndex(_rows.Count);
            _rows.Add(newRow);

            return _rows.Count;
        }

        public int RemoveLastAthleteRow() {
            if (_rows.Count > 0) {
                AthleteRowView rowToDelete = _rows[_rows.Count - 1];
                _rows.RemoveAt(_rows.Count - 1);
                DestroyImmediate(rowToDelete.gameObject);
            }

            return _rows.Count;
        }

        public void HideRowColumns(AthleteInfoType checkboxInfo, bool hide) {
            foreach (AthleteRowView row in _rows) {
                row.HideColumn(checkboxInfo, hide);
            }
        }

        public void DisableRowColumns(AthleteInfoType checkboxInfo, bool isChecked) {
            foreach (AthleteRowView row in _rows) {
                row.DisableColumn(checkboxInfo, isChecked);
            }
        }
    }
}
