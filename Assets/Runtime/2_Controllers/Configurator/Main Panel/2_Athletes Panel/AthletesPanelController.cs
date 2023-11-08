/**
 * Author:      Yannick Santa Cruz Feuillias
 * Created:     29/09/2023
 **/

// Dependencies
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
// Custom Dependencies
using YannickSCF.CountriesData;
using YannickSCF.LSTournaments.Common.Models.Athletes;
using YannickSCF.LSTournaments.Common.Scriptables.Data;
using YannickSCF.LSTournaments.Common.Scriptables.Formulas;
using YannickSCF.LSTournaments.Common.Tools.Importer;
using YannickSCF.LSTournaments.Common.Views.MainPanel.AthletesPanel.Events;
using YannickSCF.LSTournaments.Common.Views.MainPanel.AthletesPanel.Table.Bottom;
using YannickSCF.LSTournaments.Common.Views.MainPanel.AthletesPanel.Table.Content;
using YannickSCF.LSTournaments.Common.Views.MainPanel.AthletesPanel.Table.Header;

namespace YannickSCF.LSTournaments.Common.Controllers.MainPanel.AthletesPanel {
    public class AthletesPanelController : PanelController {

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
            EnableColumn(checkboxInfo, isChecked);

            ValidateAll();
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

            ValidateAll();
        }

        private void OnAthleteRemovedByButton() {
            int athleteCount = _contentView.RemoveLastAthleteRow();
            _bottomView.SetAthletesCount(GetAthletesCountText(athleteCount));

            RemoveAllIndexErrors(_currentAthletes.Count - 1);
            _currentAthletes.RemoveAt(_currentAthletes.Count - 1);

            UpdateBottomButtons(athleteCount);

            ValidateAll();
        }

        private void OnAthletesLoadedByFile() {
            string filePath = FileImporter.SelectAthletesFileWithBrowser();
            if (!string.IsNullOrEmpty(filePath)) {
                RemoveAllErrors();
                // Get all participants
                List<AthleteInfoModel> athletes = FileImporter.ImportAthletesFromFile(filePath);
                GetAthleteInfoImported(filePath);

                if (athletes != null && athletes.Count > 0) {
                    _loadingPanel.SetActive(true);
                    // Reset all table content
                    _contentView.ResetContent();
                    // Add all needed rows with the athlete information
                    StartCoroutine(AddRowsCoroutine(athletes));
                    _currentAthletes = athletes;

                    UpdateBottomButtons(_currentAthletes.Count);
                }
            }
        }
        private void GetAthleteInfoImported(string filePath) {
            List<AthleteInfoType> athleteInfo = FileImporter.ImportAthletesInfoFromFile(filePath);

            Dictionary<AthleteInfoType, bool> data = new Dictionary<AthleteInfoType, bool>();
            Array infoTypes = Enum.GetValues(typeof(AthleteInfoType));
            foreach (AthleteInfoType type in infoTypes) {
                data[type] = athleteInfo.Contains(type);
            }

            UpdateColumnsEnabled(data);
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

            ValidateAll();
            _loadingPanel.SetActive(false);
        }
        #endregion

        #region Validators
        private bool ValidateAthletesMinimum() {
            string minAthletes = LocalizationSettings.StringDatabase.GetLocalizedString("Configurator Texts", "AthletesPanel_MinAthletesError");
            if (_currentAthletes.Count < MIN_ATHLETES) {
                AddError(string.Format(minAthletes, MIN_ATHLETES));
                return false;
            }

            RemoveError(string.Format(minAthletes, MIN_ATHLETES));
            return true;
        }

        private bool ValidateCountry(string toValidate, int athleteIndex) {
            if (_columnsShown[AthleteInfoType.Country] && _columnsEnabled[AthleteInfoType.Country]) {
                if (string.IsNullOrEmpty(toValidate)) {
                    AddError(athleteIndex, AthleteInfoType.Country,
                        LocalizationSettings.StringDatabase.GetLocalizedString("Configurator Texts", "AthletesPanel_EmptyError"));
                    return false;
                }

                if (toValidate.Length != 3 && toValidate.Length != 2) {
                    AddError(athleteIndex, AthleteInfoType.Country,
                        LocalizationSettings.StringDatabase.GetLocalizedString("Configurator Texts", "AthletesPanel_CodeLengthError"));
                    return false;
                }

                if (toValidate.Length == 2 && !CountriesDataUtils.IsCountryCodeInList(toValidate)) {
                    AddError(athleteIndex, AthleteInfoType.Country,
                        LocalizationSettings.StringDatabase.GetLocalizedString("Configurator Texts", "AthletesPanel_NonExistentCodeError"));
                    return false;
                }

                if (toValidate.Length == 3 && !CountriesDataUtils.IsCountryLongCodeInList(toValidate)) {
                    AddError(athleteIndex, AthleteInfoType.Country,
                        LocalizationSettings.StringDatabase.GetLocalizedString("Configurator Texts", "AthletesPanel_NonExistentCodeError"));
                    return false;
                }
            }

            RemoveAllErrors(athleteIndex, AthleteInfoType.Country);
            return true;
        }

        private bool ValidateSimpleName(AthleteInfoType infoType, string toValidate, int athleteIndex) {
            if (_columnsShown[infoType] && _columnsEnabled[infoType]) {
                if (string.IsNullOrEmpty(toValidate)) {
                    AddError(athleteIndex, infoType,
                        LocalizationSettings.StringDatabase.GetLocalizedString("Configurator Texts", "AthletesPanel_EmptyError"));
                    return false;
                }
            }

            RemoveAllErrors(athleteIndex, infoType);
            return true;
        }

        private bool ValidateIntInString(string toValidate, int athleteIndex) {
            if (_columnsShown[AthleteInfoType.Tier] && _columnsEnabled[AthleteInfoType.Tier]) {
                if (string.IsNullOrEmpty(toValidate) || toValidate == "0") {
                    AddError(athleteIndex, AthleteInfoType.Tier,
                        LocalizationSettings.StringDatabase.GetLocalizedString("Configurator Texts", "AthletesPanel_AddTierError"));
                    return false;
                }

                if (!int.TryParse(toValidate, out int newInt)) {
                    AddError(athleteIndex, AthleteInfoType.Tier,
                        LocalizationSettings.StringDatabase.GetLocalizedString("Configurator Texts", "AthletesPanel_IntTypeError"));
                    return false;
                }
            }

            RemoveAllErrors(athleteIndex, AthleteInfoType.Tier);
            return true;
        }

        private bool ValidateDate(AthleteInfoType infoType, DateTime toValidate, int athleteIndex) {
            if (_columnsShown[infoType] && _columnsEnabled[infoType]) {
                if (toValidate == DateTime.MinValue) {
                    AddError(athleteIndex, infoType,
                        LocalizationSettings.StringDatabase.GetLocalizedString("Configurator Texts", "AthletesPanel_AddDateError"));
                    return false;
                }

                if (toValidate > DateTime.Now) {
                    AddError(athleteIndex, infoType,
                        LocalizationSettings.StringDatabase.GetLocalizedString("Configurator Texts", "AthletesPanel_InvalidDateError"));
                    return false;
                }
            }

            RemoveAllErrors(athleteIndex, infoType);
            return true;
        }

        private bool ValidateStyles(List<StyleType> toValidate, int athleteIndex) {
            if (_columnsShown[AthleteInfoType.Styles] && _columnsEnabled[AthleteInfoType.Styles]) {
                if (toValidate == null || toValidate.Count == 0) {
                    AddError(athleteIndex, AthleteInfoType.Styles,
                        LocalizationSettings.StringDatabase.GetLocalizedString("Configurator Texts", "AthletesPanel_SelectStylesError"));
                    return false;
                }

                if (!toValidate.Contains(StyleType.Form1)) {
                    AddError(athleteIndex, AthleteInfoType.Styles,
                        LocalizationSettings.StringDatabase.GetLocalizedString("Configurator Texts", "AthletesPanel_SelectForm1Error"));
                    return false;
                }
            }

            RemoveAllErrors(athleteIndex, AthleteInfoType.Styles);
            return true;
        }

        private bool ValidateColor(Color toValidate, int athleteIndex) {
            if (_columnsShown[AthleteInfoType.SaberColor] && _columnsEnabled[AthleteInfoType.SaberColor]) {
                if (toValidate == new Color(0, 0, 0, 0)) {
                    AddError(athleteIndex, AthleteInfoType.SaberColor,
                        LocalizationSettings.StringDatabase.GetLocalizedString("Configurator Texts", "AthletesPanel_EmptyColorError"));
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
            }

            _IsDataValidated = _errorsList?.Count == 0;
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

        private void RemoveAllIndexErrors(int index) {
            List<RowsErrors> errors = _errorsList.Where(x => !x.IsGeneric && x.RowIndex == index).ToList();
            for (int i = 0; i < errors.Count; ++i) {
                RemoveError(errors[i]);
            }
        }

        private void RemoveAllErrors() {
            _errorsList?.Clear();
            _IsDataValidated = _errorsList?.Count == 0;
        }

        private void RemoveError(RowsErrors error) {
            if (_errorsList.Contains(error)) {
                _errorsList.Remove(error);
            }

            _IsDataValidated = _errorsList?.Count == 0;
        }

        private void UpdateErrorsPanel() {
            string errors = string.Empty;
            foreach (RowsErrors error in _errorsList) {
                if (error.IsGeneric) {
                    string genericText = LocalizationSettings.StringDatabase.GetLocalizedString("Configurator Texts", "AthletesPanel_BaseGenericError");
                    errors += string.Format(genericText, error.Description);
                } else {
                    string rowText = LocalizationSettings.StringDatabase.GetLocalizedString("Configurator Texts", "AthletesPanel_BaseRowError");
                    string columnText = LocalizationSettings.StringDatabase.GetLocalizedString("Common Enums", nameof(AthleteInfoType) + "." + error.Column.ToString());
                    errors += string.Format(rowText, error.RowIndex + 1, columnText, error.Description);
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

        #region PanelController abstract methods overrided
        public override string GetTitle() {
            return LocalizationSettings.StringDatabase.GetLocalizedString("Configurator Texts", "AthletesPanel_BreadcrumbTitle");
        }

        public override void ValidateAll(bool showErrorAdvices = true) {
            bool res = true;
            res &= ValidateAthletesMinimum();

            for (int i = 0; i < _currentAthletes.Count; ++i) {
                res &= ValidateCountry(_currentAthletes[i].Country, i);
                res &= ValidateSimpleName(AthleteInfoType.Surname, _currentAthletes[i].Surname, i);
                res &= ValidateSimpleName(AthleteInfoType.Name, _currentAthletes[i].Name, i);
                res &= ValidateSimpleName(AthleteInfoType.Academy, _currentAthletes[i].Academy, i);
                res &= ValidateSimpleName(AthleteInfoType.School, _currentAthletes[i].School, i);
                res &= ValidateIntInString(_currentAthletes[i].Tier.ToString(), i);
                res &= ValidateDate(AthleteInfoType.BirthDate, _currentAthletes[i].BirthDate, i);
                res &= ValidateDate(AthleteInfoType.StartDate, _currentAthletes[i].StartDate, i);
                res &= ValidateStyles(_currentAthletes[i].Styles, i);
                res &= ValidateColor(_currentAthletes[i].SaberColor, i);
            }

            UpdateErrorsPanel();

            if (showErrorAdvices) {
                _bottomView.ShowAthletesNotValidated(!res);
            }
        }

        public override void GiveData(TournamentData data) {
            _currentAthletes = data.Athletes;
            _bottomView.SetAthletesCount(GetAthletesCountText(_currentAthletes.Count));
            if (_currentAthletes.Count != 0) {
                _loadingPanel.SetActive(true);
                // Reset all table content
                _contentView.ResetContent();
                // Add all needed rows with the athlete information
                StartCoroutine(AddRowsCoroutine(_currentAthletes));
            }

            UpdateColumnsToShow(data.TournamentType);
            UpdateColumnsEnabled(data.AthletesInfoUsed);

            UpdateColumnsBlocked(data.TournamentFormulaName);

            Array infoTypes = Enum.GetValues(typeof(AthleteInfoType));
            foreach (AthleteInfoType type in infoTypes) {
                data.AthletesInfoUsed[type] = _columnsShown[type] && _columnsEnabled[type];
            }

            ValidateAll(false);
        }

        public override TournamentData RetrieveData(TournamentData data) {
            Dictionary<AthleteInfoType, bool> infoUsed = new Dictionary<AthleteInfoType, bool>();
            foreach (Enum infoType in Enum.GetValues(typeof(AthleteInfoType))) {
                infoUsed[(AthleteInfoType)infoType] =
                    _columnsShown[(AthleteInfoType)infoType] && _columnsEnabled[(AthleteInfoType)infoType];
            }

            data.AthletesInfoUsed = infoUsed;
            data.Athletes = _currentAthletes;
            return data;
        }
        #endregion

        public void ShowColumn(AthleteInfoType columnToShow, bool show) {
            switch (columnToShow) {
                case AthleteInfoType.Country:
                case AthleteInfoType.Academy:
                case AthleteInfoType.School:
                    _headerView.ShowColumn(columnToShow, show);

                    _columnsShown[columnToShow] = show;
                    _contentView.ShowRowColumns(columnToShow, show);
                    break;
                default:
                    Debug.LogWarning($"You cannot Show/Hide {Enum.GetName(typeof(AthleteInfoType), columnToShow)} column!");
                    break;
            }
        }

        public void EnableColumn(AthleteInfoType checkboxInfo, bool isChecked) {
            _headerView.EnableHeader(checkboxInfo, isChecked);

            _columnsEnabled[checkboxInfo] = isChecked;
            _contentView.EnableRowColumns(checkboxInfo, isChecked);
        }

        private void UpdateColumnsToShow(TournamentType tournamentType) {
            ShowColumn(AthleteInfoType.Country,
                tournamentType != TournamentType.School &&
                tournamentType != TournamentType.Academy &&
                tournamentType != TournamentType.National);


            ShowColumn(AthleteInfoType.Academy,
                tournamentType != TournamentType.School &&
                tournamentType != TournamentType.Academy);

            ShowColumn(AthleteInfoType.School,
                tournamentType != TournamentType.School);
        }

        private void UpdateColumnsEnabled(Dictionary<AthleteInfoType, bool> athletesInfoUsed) {
            foreach (KeyValuePair<AthleteInfoType, bool> infoUsed in athletesInfoUsed) {
                EnableColumn(infoUsed.Key, infoUsed.Value);
            }
        }

        private void UpdateColumnsBlocked(string tournamentFormulaName) {
            TournamentFormula formula = TournamentFormulaUtils.GetFormulaByName(tournamentFormulaName);
            if (!TournamentFormulaUtils.IsCustomFormula(tournamentFormulaName)) {
                _headerView.BlockHeader(AthleteInfoType.Rank, formula.FillerType == PouleFillerType.ByRank);
                _headerView.BlockHeader(AthleteInfoType.Styles, formula.FillerType == PouleFillerType.ByStyle);
                _headerView.BlockHeader(AthleteInfoType.Tier, formula.FillerType == PouleFillerType.ByTier);
            }
        }

        private string GetAthletesCountText(int count) {
            var localizedString = new LocalizedString("Configurator Texts", "AthletesPanel_Athletes");
            localizedString.Arguments = new object[] { count };
            return localizedString.GetLocalizedString();
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
