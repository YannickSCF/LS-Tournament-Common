/**
 * Author:      Yannick Santa Cruz Feuillias
 * Created:     10/11/2023
 **/

// Dependencies
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.UI;
// Custom dependencies
using YannickSCF.LSTournaments.Common.Views.MainPanel.AthletesDataPanel.TableRowsList.Row;
using YannickSCF.LSTournaments.Common.Views.MainPanel.AthletesDataPanel.Events;

namespace YannickSCF.LSTournaments.Common.Views.MainPanel.AthletesDataPanel.Table.Content {

    public class AthletesDataTableContentView : MonoBehaviour {

        [Header("Table Rows")]
        [SerializeField] private ScrollRect _tableScrollRect;
        [SerializeField] private AthleteDataRowView _athleteDataRowPrefab;
        [SerializeField] private Transform _rowsPool;
        [Header("Bottom buttons and text")]
        [SerializeField] private Button _addAthleteButton;
        [SerializeField] private Button _removeLastAthleteButton;
        [SerializeField] private TextMeshProUGUI _athletesCountText;

        private List<AthleteDataRowView> _rowsActive;
        private List<AthleteDataRowView> _rowsUnused;

        private Dictionary<AthleteInfoType, Vector2> _columnsSizes;
        private float _rowHeight = 0f;

        #region Mono
        private void Awake() {
            if (_rowsActive == null)
                _rowsActive = new List<AthleteDataRowView>();

            if (_rowsUnused == null) {
                _rowsUnused = new List<AthleteDataRowView>();
                CreatePoolRows();
            }

            if (_columnsSizes == null)
                _columnsSizes = new Dictionary<AthleteInfoType, Vector2>();

            UpdateAthletesCount();
        }
        private void OnEnable() {
            _addAthleteButton.onClick.AddListener(() => AthletesPanelViewEvents.ThrowOnAthleteAdded());
            _removeLastAthleteButton.onClick.AddListener(() => AthletesPanelViewEvents.ThrowOnAthleteRemoved());
        }
        private void OnDisable() {
            _addAthleteButton.onClick.RemoveAllListeners();
            _removeLastAthleteButton.onClick.RemoveAllListeners();
        }
        #endregion

        public void SetRowHeightAndColumnSizes(float rowHeight, Dictionary<AthleteInfoType, Vector2> columnsSizes) {
            _columnsSizes = columnsSizes;
            _rowHeight = rowHeight;
        }

        #region Methods to Add/Remove athletes from table
        private void CreatePoolRows() {
            for(int i = 0; i < 64; ++i) {
                AthleteDataRowView rowToAdd = Instantiate(_athleteDataRowPrefab, _rowsPool);
                rowToAdd.gameObject.SetActive(false);
                _rowsUnused.Add(rowToAdd);
            }
        }

        public void AddAthleteRow() {
            _ = GetNewAthleteRow();

            UpdateAthletesCount();
            UpdateScrollRectContentSize();

            _tableScrollRect.verticalScrollbar.value = 0;
        }

        public void AddAthleteInfo(string countryCode, string surname, string name,
            string academy, string school, RankType rank, List<StyleType> styles,
            int tier, Color color, DateTime birthData, DateTime startDate) {

            AthleteDataRowView newRow = GetNewAthleteRow();

            newRow.SetAthleteRowIndex(_rowsActive.Count - 1, _rowHeight);
            newRow.SetCountryField(countryCode, true, true);
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

            UpdateAthletesCount();
            UpdateScrollRectContentSize();
        }

        private AthleteDataRowView GetNewAthleteRow() {
            AthleteDataRowView rowToAdd;
            if (_rowsUnused.Count > 0) {
                rowToAdd = _rowsUnused[0];
                rowToAdd.ActiveRow(_tableScrollRect.content);

                _rowsUnused.RemoveAt(0);
            } else {
                rowToAdd = Instantiate(_athleteDataRowPrefab, _tableScrollRect.content);
            }

            rowToAdd.SetColumnsAnchors(_columnsSizes);
            rowToAdd.SetAthleteRowIndex(_rowsActive.Count, _rowHeight);

            _rowsActive.Add(rowToAdd);

            return rowToAdd;
        }

        public void RemoveLastAthleteRow() {
            RemoveAthleteRow(_rowsActive.Count - 1);
            UpdateAthletesCount();
        }

        public void ResetContent() {
            for (int i = _rowsActive.Count - 1; i >= 0; --i) {
                RemoveAthleteRow(i);
            }

            UpdateAthletesCount();
        }

        private void RemoveAthleteRow(int index) {
            if (_rowsActive.Count > 0) {
                AthleteDataRowView rowToDelete = _rowsActive[index];
                _rowsActive.RemoveAt(index);

                rowToDelete.DeactiveRow(_rowsPool);
                _rowsUnused.Add(rowToDelete);

                UpdateScrollRectContentSize();
            }
        }
        #endregion

        #region Methods to set view values or characteristics
        public void SetAddButtonInteractable(bool isInteractable) {
            _addAthleteButton.interactable = isInteractable;
        }

        public void SetRemoveButtonInteractable(bool isInteractable) {
            _removeLastAthleteButton.interactable = isInteractable;
        }

        /// <summary>
        /// Method to set count of athletes text in table.
        /// </summary>
        /// <param name="athletesText"> Athletes count text. Must contains all text that must be shown.</param>
        private void UpdateAthletesCount() {
            var localizedString = new LocalizedString("Configurator Texts", "AthletesPanel_Athletes");
            localizedString.Arguments = new object[] { _rowsActive.Count };
            _athletesCountText.text = localizedString.GetLocalizedString();

            SetRemoveButtonInteractable(_rowsActive.Count > 0);
        }
        private void UpdateScrollRectContentSize() {
            _tableScrollRect.content.sizeDelta = new Vector2(
                _tableScrollRect.content.sizeDelta.x, _rowHeight * _rowsActive.Count);
        }
        #endregion

        #region Methods to change athletes info visibility
        public void EnableRowColumn(int rowToUpdate, AthleteInfoType checkboxInfo, bool enable) {
            _rowsActive[rowToUpdate].EnableRowColumn(checkboxInfo, enable);
        }

        public void EnableRowColumns(AthleteInfoType checkboxInfo, bool enable) {
            foreach (AthleteDataRowView row in _rowsActive) {
                row.EnableRowColumn(checkboxInfo, enable);
            }
        }
        #endregion
    
        public void ResetScrollAndHideInvisibleRows(bool show) {
            _tableScrollRect.verticalScrollbar.value = 1;
            int rowsVisible = (int)(_tableScrollRect.viewport.rect.height / _rowHeight) + 2;

            if(rowsVisible > 0 && _rowsActive != null && _rowsActive.Count > rowsVisible) {
                for (int i = rowsVisible; i < _rowsActive.Count; ++i) {
                    _rowsActive[i].gameObject.SetActive(show);
                }
            }
        }
    }
}
