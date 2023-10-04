// Dependencies
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using YannickSCF.CountriesData;
// Custom Dependencies
using YannickSCF.LSTournaments.Common.Models.Athletes;
using YannickSCF.LSTournaments.Common.Scriptables.Data;
using YannickSCF.LSTournaments.Common.Tools.Importers;
using YannickSCF.LSTournaments.Common.Views.MainPanel.AthletesPanel.Events;
using YannickSCF.LSTournaments.Common.Views.MainPanel.AthletesPanel.Table.Bottom;
using YannickSCF.LSTournaments.Common.Views.MainPanel.AthletesPanel.Table.Content;
using YannickSCF.LSTournaments.Common.Views.MainPanel.AthletesPanel.Table.Header;

namespace YannickSCF.LSTournaments.Common.Controllers.MainPanel.AthletesPanel {
    public class AthletesPanelController : MonoBehaviour {

        private const int MIN_ATHLETES = 4;

        [Header("Views")]
        [SerializeField] private AthleteTableHeaderView _headerView;
        [SerializeField] private AthleteTableContentView _contentView;
        [SerializeField] private AthleteBottomTableView _bottomView;
        [Header("Other objects")]
        [SerializeField] private GameObject _loadingPanel;

        private Dictionary<AthleteInfoType, bool> _columnsShown;
        private Dictionary<AthleteInfoType, bool> _columnsEnabled;

        private List<AthleteInfoModel> _currentAthletes;
        private List<RowsErrors> _errorsList;

        private bool _isTableValidated = false;
        public bool IsTableValidated { get => _isTableValidated; }

        #region Mono
        private void Awake() {
            _loadingPanel.SetActive(false);

            _columnsShown = new Dictionary<AthleteInfoType, bool>();
            _columnsEnabled = new Dictionary<AthleteInfoType, bool>();

            Array infoTypes = Enum.GetValues(typeof(AthleteInfoType));
            foreach (Enum infoType in infoTypes) {
                _columnsShown.Add((AthleteInfoType)infoType, true);
                _columnsEnabled.Add((AthleteInfoType)infoType, true);
            }

            _currentAthletes = new List<AthleteInfoModel>();
            _errorsList = new List<RowsErrors>();
        }

        private void OnEnable() {
            // Header events
            AthletesPanelViewEvents.OnAthleteInfoCheckboxToggle += OnToggleHeaderHide;
            // Content events
            AthletesPanelViewEvents.OnAthleteDataStringUpdated += OnAthleteStringUpdated;
            AthletesPanelViewEvents.OnAthleteDataDateTimeUpdated += OnAthleteDateUpdated;
            AthletesPanelViewEvents.OnAthleteDataRankUpdated += OnAthleteRankUpdated;
            AthletesPanelViewEvents.OnAthleteDataStylesUpdated += OnAthleteStylesUpdated;
            AthletesPanelViewEvents.OnAthleteDataColorUpdated += OnAthleteColorUpdated;
            // Bottom events
            AthletesPanelViewEvents.OnAthleteAdded += OnAthleteAddedByButton;
            AthletesPanelViewEvents.OnAthleteRemoved += OnAthleteRemovedByButton;
            AthletesPanelViewEvents.OnLoadAthletesFromFile += OnAthletesLoadedByFile;
        }

        private void OnDisable() {
            // Header events
            AthletesPanelViewEvents.OnAthleteInfoCheckboxToggle -= OnToggleHeaderHide;
            // Content events
            AthletesPanelViewEvents.OnAthleteDataStringUpdated -= OnAthleteStringUpdated;
            AthletesPanelViewEvents.OnAthleteDataDateTimeUpdated -= OnAthleteDateUpdated;
            AthletesPanelViewEvents.OnAthleteDataRankUpdated -= OnAthleteRankUpdated;
            AthletesPanelViewEvents.OnAthleteDataStylesUpdated -= OnAthleteStylesUpdated;
            AthletesPanelViewEvents.OnAthleteDataColorUpdated -= OnAthleteColorUpdated;
            // Bottom events
            AthletesPanelViewEvents.OnAthleteAdded -= OnAthleteAddedByButton;
            AthletesPanelViewEvents.OnAthleteRemoved -= OnAthleteRemovedByButton;
            AthletesPanelViewEvents.OnLoadAthletesFromFile -= OnAthletesLoadedByFile;
        }
        #endregion

        #region Events Listeners methods
        private void OnToggleHeaderHide(AthleteInfoType checkboxInfo, bool isChecked) {
            _headerView.EnableHeader(checkboxInfo, isChecked);

            _columnsEnabled[checkboxInfo] = isChecked;
            _contentView.EnableRowColumns(checkboxInfo, isChecked);

            ValidateCurrent();
        }

        private void OnAthleteStringUpdated(AthleteInfoType infoType, string updatedData, int athleteIndex) {
            if (infoType == AthleteInfoType.Country) {
                if (ValidateCountry(updatedData, athleteIndex)) {
                    _currentAthletes[athleteIndex].Country = updatedData;
                }
            }

            if (infoType == AthleteInfoType.Surname || infoType == AthleteInfoType.Name ||
                infoType == AthleteInfoType.Academy || infoType == AthleteInfoType.School) {
                if (ValidateSimpleName(infoType, updatedData, athleteIndex)) {
                    switch (infoType) {
                        case AthleteInfoType.Surname: _currentAthletes[athleteIndex].Surname = updatedData; break;
                        case AthleteInfoType.Name: _currentAthletes[athleteIndex].Name = updatedData; break;
                        case AthleteInfoType.Academy: _currentAthletes[athleteIndex].Academy = updatedData; break;
                        case AthleteInfoType.School: _currentAthletes[athleteIndex].School = updatedData; break;
                    }
                }
            }

            if (infoType == AthleteInfoType.Tier) {
                if (ValidateIntInString(updatedData, athleteIndex)) {
                    _currentAthletes[athleteIndex].Tier = int.Parse(updatedData);
                }
            }

            UpdateErrorsPanel();
        }

        private void OnAthleteDateUpdated(AthleteInfoType infoType, DateTime updatedData, int athleteIndex) {
            if (infoType == AthleteInfoType.BirthDate || infoType == AthleteInfoType.StartDate) {
                if (ValidateDate(infoType, updatedData, athleteIndex)) {
                    switch (infoType) {
                        case AthleteInfoType.BirthDate:
                            _currentAthletes[athleteIndex].BirthDate = updatedData;
                            break;
                        case AthleteInfoType.StartDate:
                            _currentAthletes[athleteIndex].StartDate = updatedData;
                            break;
                    }
                }
            } else {
                Debug.LogWarning($"You are trying to save a Date in column {Enum.GetName(typeof(AthleteInfoType), infoType)}!");
            }

            UpdateErrorsPanel();
        }

        private void OnAthleteRankUpdated(RankType updatedData, int athleteIndex) {
            _currentAthletes[athleteIndex].Rank = updatedData;
        }

        private void OnAthleteStylesUpdated(List<StyleType> updatedData, int athleteIndex) {
            if (ValidateStyles(updatedData, athleteIndex)) {
                _currentAthletes[athleteIndex].Styles = updatedData;
            }
            UpdateErrorsPanel();
        }

        private void OnAthleteColorUpdated(Color updatedData, int athleteIndex) {
            if (ValidateColor(updatedData, athleteIndex)) {
                _currentAthletes[athleteIndex].SaberColor = updatedData;
            }
            UpdateErrorsPanel();
        }

        private void OnAthleteAddedByButton() {
            int athleteCount = _contentView.AddAthleteRow();
            _bottomView.SetAthletesCount(GetAthletesCountText(athleteCount));

            _currentAthletes.Add(new AthleteInfoModel());

            UpdateAllHiddenAndDisableColumnsInARow(athleteCount - 1);
            UpdateBottomButtons(athleteCount);

            ValidateCurrent();
        }

        private void OnAthleteRemovedByButton() {
            int athleteCount = _contentView.RemoveLastAthleteRow();
            _bottomView.SetAthletesCount(GetAthletesCountText(athleteCount));

            _currentAthletes.RemoveAt(_currentAthletes.Count - 1);

            UpdateBottomButtons(athleteCount);

            ValidateCurrent();
        }

        private void OnAthletesLoadedByFile() {
            string filePath = FileImporter.SelectFileWithBrowser();
            if (!string.IsNullOrEmpty(filePath)) {
                // Get all participants
                List<AthleteInfoModel> athletes = FileImporter.ImportAthletesFromFile(filePath);
                if (athletes != null && athletes.Count > 0) {
                    _loadingPanel.SetActive(true);
                    // Reset all table content
                    _contentView.ResetContent();
                    // Add all needed rows with the athlete information
                    StartCoroutine(AddRowsCoroutine(athletes));
                    _currentAthletes = athletes;
                }
            }
        }
        private IEnumerator AddRowsCoroutine(List<AthleteInfoModel> athletes) {
            yield return new WaitForSeconds(0.25f);

            for (int i = 0; i < athletes.Count; ++i) {
                yield return new WaitForEndOfFrame();
                _contentView.AddAthleteInfo(
                    athletes[i].Country,
                    athletes[i].Surname,
                    athletes[i].Name,
                    athletes[i].Academy,
                    athletes[i].School,
                    athletes[i].Rank,
                    athletes[i].Styles,
                    athletes[i].Tier,
                    athletes[i].SaberColor,
                    athletes[i].BirthDate,
                    athletes[i].StartDate);

                _bottomView.SetAthletesCount(GetAthletesCountText(i + 1));
                UpdateAllHiddenAndDisableColumnsInARow(i);
            }

            ValidateCurrent();
            _loadingPanel.SetActive(false);
        }
        #endregion

        #region Validators
        private void ValidateCurrent() {
            ValidateAthletesMinimum();

            for (int i = 0; i < _currentAthletes.Count; ++i) {
                ValidateCountry(_currentAthletes[i].Country, i);
                ValidateSimpleName(AthleteInfoType.Surname ,_currentAthletes[i].Surname, i);
                ValidateSimpleName(AthleteInfoType.Name, _currentAthletes[i].Name, i);
                ValidateSimpleName(AthleteInfoType.Academy, _currentAthletes[i].Academy, i);
                ValidateSimpleName(AthleteInfoType.School, _currentAthletes[i].School, i);
                ValidateIntInString(_currentAthletes[i].Tier.ToString(), i);
                ValidateDate(AthleteInfoType.BirthDate, _currentAthletes[i].BirthDate, i);
                ValidateDate(AthleteInfoType.StartDate, _currentAthletes[i].StartDate, i);
                ValidateStyles(_currentAthletes[i].Styles, i);
                ValidateColor(_currentAthletes[i].SaberColor, i);
            }

            UpdateErrorsPanel();
        }

        private bool ValidateAthletesMinimum() {
            if (_currentAthletes.Count < MIN_ATHLETES) {
                AddError($"At least {MIN_ATHLETES} athletes");
                return false;
            }

            RemoveError($"At least {MIN_ATHLETES} athletes");
            return true;
        }

        private bool ValidateCountry(string toValidate, int athleteIndex) {
            if (_columnsShown[AthleteInfoType.Country] && _columnsEnabled[AthleteInfoType.Country]) {
                if (string.IsNullOrEmpty(toValidate)) {
                    AddError(athleteIndex, AthleteInfoType.Country, "Empty value");
                    return false;
                }

                if (toValidate.Length != 3 && toValidate.Length != 2) {
                    AddError(athleteIndex, AthleteInfoType.Country, "Invalid code lenght");
                    return false;
                }

                if (toValidate.Length == 2 && !CountriesDataUtils.IsCountryCodeInList(toValidate)) {
                    AddError(athleteIndex, AthleteInfoType.Country, "This code doesn�t exist in database");
                    return false;
                }

                if (toValidate.Length == 3 && !CountriesDataUtils.IsCountryLongCodeInList(toValidate)) {
                    AddError(athleteIndex, AthleteInfoType.Country, "This code doesn�t exist in database");
                    return false;
                }
            }

            RemoveAllErrors(athleteIndex, AthleteInfoType.Country);
            return true;
        }

        private bool ValidateSimpleName(AthleteInfoType infoType, string toValidate, int athleteIndex) {
            if (_columnsShown[infoType] && _columnsEnabled[infoType]) {
                if (string.IsNullOrEmpty(toValidate)) {
                    AddError(athleteIndex, infoType, "Empty value");
                    return false;
                }
            }

            RemoveAllErrors(athleteIndex, infoType);
            return true;
        }

        private bool ValidateIntInString(string toValidate, int athleteIndex) {
            if (_columnsShown[AthleteInfoType.Tier] && _columnsEnabled[AthleteInfoType.Tier]) {
                if (string.IsNullOrEmpty(toValidate) || toValidate == "0") {
                    AddError(athleteIndex, AthleteInfoType.Tier, "No tier added");
                    return false;
                }

                if (!int.TryParse(toValidate, out int newInt)) {
                    AddError(athleteIndex, AthleteInfoType.Tier, "Tier must be a number");
                    return false;
                }
            }

            RemoveAllErrors(athleteIndex, AthleteInfoType.Tier);
            return true;
        }

        private bool ValidateDate(AthleteInfoType infoType, DateTime toValidate, int athleteIndex) {
            if (_columnsShown[infoType] && _columnsEnabled[infoType]) {
                if (toValidate == DateTime.MinValue) {
                    AddError(athleteIndex, infoType, "Date not setted");
                    return false;
                }

                if (toValidate > DateTime.Now) {
                    AddError(athleteIndex, infoType, "Invalid Date");
                    return false;
                }
            }

            RemoveAllErrors(athleteIndex, infoType);
            return true;
        }

        private bool ValidateStyles(List<StyleType> toValidate, int athleteIndex) {
            if (_columnsShown[AthleteInfoType.Styles] && _columnsEnabled[AthleteInfoType.Styles]) {
                if (toValidate == null || toValidate.Count == 0) {
                    AddError(athleteIndex, AthleteInfoType.Styles, "No styles selected");
                    return false;
                }

                if (!toValidate.Contains(StyleType.Form1)) {
                    AddError(athleteIndex, AthleteInfoType.Styles, "At least form 1");
                    return false;
                }
            }

            RemoveAllErrors(athleteIndex, AthleteInfoType.Styles);
            return true;
        }

        private bool ValidateColor(Color toValidate, int athleteIndex) {
            if (_columnsShown[AthleteInfoType.SaberColor] && _columnsEnabled[AthleteInfoType.SaberColor]) {
                if (toValidate == new Color(0, 0, 0, 0)) {
                    AddError(athleteIndex, AthleteInfoType.SaberColor, "Empty Color");
                    return false;
                }
            }

            RemoveAllErrors(athleteIndex, AthleteInfoType.SaberColor);
            return true;
        }

        #region Validation Errors Management
        private void AddError(string errorId) {
            AddError(new RowsErrors(errorId));
        }

        private void AddError(int index, AthleteInfoType column, string errorId) {
            AddError(new RowsErrors(index, column, errorId));
        }

        private void AddError(RowsErrors error) {
            if (!_errorsList.Contains(error)) {
                _errorsList.Add(error);
                _isTableValidated = _errorsList.Count == 0;
            }
        }

        private void RemoveError(string errorId) {
            RemoveError(new RowsErrors(errorId));
        }

        private void RemoveError(int index, AthleteInfoType column, string errorId) {
            _errorsList.Remove(new RowsErrors(index, column, errorId));
        }

        private void RemoveAllErrors(int index, AthleteInfoType column) {
            List<RowsErrors> errors = _errorsList.Where(x => !x.IsGeneric && x.RowIndex == index && x.Column == column).ToList();
            for (int i = 0; i < errors.Count; ++i) {
                RemoveError(errors[i]);
            }
        }

        private void RemoveError(RowsErrors error) {
            if (_errorsList.Contains(error)) {
                _errorsList.Remove(error);
                _isTableValidated = _errorsList.Count == 0;
            }
        }

        private void UpdateErrorsPanel() {
            string errors = string.Empty;
            foreach (RowsErrors error in _errorsList) {
                if (error.IsGeneric) {
                    errors += $" - Generic error: {error.Description}\n";
                } else {
                    errors += $" - Row {error.RowIndex + 1} ({Enum.GetName(typeof(AthleteInfoType), error.Column)}): {error.Description}\n";
                }
            }

            _bottomView.SetErrorPanelText(errors);
        }

        private class RowsErrors {
            public int RowIndex;
            public AthleteInfoType Column;
            public string Description;
            public bool IsGeneric;

            public RowsErrors(string description) {
                IsGeneric = true;

                Description = description;
            }

            public RowsErrors(int rowIndex, AthleteInfoType column, string description) {
                IsGeneric = false;

                RowIndex = rowIndex;
                Column = column;
                Description = description;
            }

            public override bool Equals(object obj) {
                RowsErrors objCast = obj as RowsErrors;
                if (objCast == null) {
                    return false;
                }

                return RowIndex == objCast.RowIndex
                    && Column == objCast.Column
                    && Description == objCast.Description;
            }

            public override int GetHashCode() {
                return base.GetHashCode();
            }
        }
        #endregion
        #endregion

        public void FillData(TournamentData data) {
            data.Athletes = _currentAthletes;

            Array infoTypes = Enum.GetValues(typeof(AthleteInfoType));
            foreach(AthleteInfoType type in infoTypes) {
                data.AthletesInfoUsed[type] = _columnsShown[type] && _columnsEnabled[type];
            }
        }

        public void ShowColumn(AthleteInfoType columnToShow, bool show) {
            switch (columnToShow) {
                case AthleteInfoType.Country:
                case AthleteInfoType.Academy:
                    _headerView.ShowColumn(columnToShow, show);

                    _columnsShown[columnToShow] = show;
                    _contentView.ShowRowColumns(columnToShow, show);
                    break;
                default:
                    Debug.LogWarning($"You cannot Show/Hide {Enum.GetName(typeof(AthleteInfoType), columnToShow)} column!");
                    break;
            }
        }

        private string GetAthletesCountText(int count) {
            return count + " Athletes";
        }

        private void UpdateBottomButtons(int athleteCount) {
            _bottomView.SetRemoveButtonInteractable(athleteCount > 0);
        }

        private void UpdateAllHiddenAndDisableColumnsInARow(int rowToUpdate) {
            foreach (KeyValuePair<AthleteInfoType, bool> columnShownInfo in _columnsShown) {
                _contentView.ShowRowColumn(rowToUpdate, columnShownInfo.Key, columnShownInfo.Value);
            }

            foreach (KeyValuePair<AthleteInfoType, bool> columnEnabledInfo in _columnsEnabled) {
                _contentView.EnableRowColumn(rowToUpdate, columnEnabledInfo.Key, columnEnabledInfo.Value);
            }
        }
    }
}
