/**
 * Author:      Yannick Santa Cruz Feuillias
 * Created:     10/11/2023
 **/

// Dependencies
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Localization.Settings;
// Custom dependencies
using YannickSCF.CountriesData;
using YannickSCF.LSTournaments.Common.Models.Athletes;
using YannickSCF.LSTournaments.Common.Scriptables.Data;
using YannickSCF.LSTournaments.Common.Scriptables.Formulas;
using YannickSCF.LSTournaments.Common.Scriptables.Settings.AthleteTables;
using YannickSCF.LSTournaments.Common.Tools.Importer;
using YannickSCF.LSTournaments.Common.Views.MainPanel.AthletesDataPanel;
using YannickSCF.LSTournaments.Common.Views.MainPanel.AthletesDataPanel.Events;

namespace YannickSCF.LSTournaments.Common.Controllers.MainPanel.AthletesDataPanel {
    public class AthletesDataPanelController : PanelController<AthletesDataPanelView> {

        private const int MIN_ATHLETES = 4;

        [Header("Other objects")]
        [SerializeField] private AthleteInfoTableSettings _settings;

        private Dictionary<AthleteInfoType, AthleteInfoStatus> _athletesInfoUsed;

        private List<AthleteInfoModel> _currentAthletes;
        private List<RowsErrors> _errorsList;

        // TODO: Revisar errores al cambiar columnas ocultas una vez cargados los atletas

        #region Mono
        private void Awake() {
            _currentAthletes = new List<AthleteInfoModel>();
            _errorsList = new List<RowsErrors>();
        }

        protected override void OnEnable() {
            base.OnEnable();

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

        protected override void OnDisable() {
            base.OnDisable();

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
            _athletesInfoUsed[checkboxInfo] = isChecked ? AthleteInfoStatus.Active : AthleteInfoStatus.Disable;
            DataManager.Instance.AppData.SetAthleteInfoUsed(checkboxInfo, isChecked ? AthleteInfoStatus.Active : AthleteInfoStatus.Disable);
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
            _View.AddAthlete();
            _currentAthletes.Add(new AthleteInfoModel());

            UpdateColumnsInTheLastRow();
            ValidateAll();
        }

        private void OnAthleteRemovedByButton() {
            _View.RemoveAthlete();
            _currentAthletes.RemoveAt(_currentAthletes.Count - 1);

            RemoveAllIndexErrors(_currentAthletes.Count);

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
                    // Reset all table content
                    _View.ResetAthletesList();
                    // Add all needed rows with the athlete information
                    StartCoroutine(AddRowsCoroutine(athletes));
                    _currentAthletes = athletes;

                    UpdateBottomButtons(_currentAthletes.Count);
                }
            }
        }
        private void GetAthleteInfoImported(string filePath) {
            List<AthleteInfoType> athleteInfo = FileImporter.ImportAthletesInfoFromFile(filePath);

            TournamentData data = DataManager.Instance.AppData;
            Array infoTypes = Enum.GetValues(typeof(AthleteInfoType));
            foreach (AthleteInfoType type in infoTypes) {
                if (data.GetAthleteInfoUsed(type) != AthleteInfoStatus.Hide) {
                    if (!athleteInfo.Contains(type)) {
                        DataManager.Instance.AppData.SetAthleteInfoUsed(type, AthleteInfoStatus.Disable);
                    } else {
                        DataManager.Instance.AppData.SetAthleteInfoUsed(type, AthleteInfoStatus.Active);
                    }
                }
            }
            _athletesInfoUsed = DataManager.Instance.AppData.GetAthletesInfoUsed();

            UpdateColumnsEnabled();
        }
        private IEnumerator AddRowsCoroutine(List<AthleteInfoModel> athletes) {
            yield return new WaitForSeconds(0.25f);
            AskLoading(true);

            for (int i = 0; i < athletes.Count; ++i) {
                if (!IsLoadingActive) {
                    AskLoading(true);
                }

                yield return new WaitForEndOfFrame();
                _View.AddAthlete(athletes[i].Country, athletes[i].Surname, athletes[i].Name,
                    athletes[i].Academy, athletes[i].School, athletes[i].Rank,
                    athletes[i].Styles, athletes[i].Tier, athletes[i].SaberColor,
                    athletes[i].BirthDate, athletes[i].StartDate);

                UpdateAllHiddenAndDisableColumnsInARow(i);
            }

            ValidateAll();
            AskLoading(false);
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
            if (_athletesInfoUsed[AthleteInfoType.Country] == AthleteInfoStatus.Active) {
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
            if (_athletesInfoUsed[infoType] == AthleteInfoStatus.Active) {
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
            if (_athletesInfoUsed[AthleteInfoType.Tier] == AthleteInfoStatus.Active) {
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
            if (_athletesInfoUsed[infoType] == AthleteInfoStatus.Active) {
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
            if (_athletesInfoUsed[AthleteInfoType.Styles] == AthleteInfoStatus.Active) {
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
            if (_athletesInfoUsed[AthleteInfoType.SaberColor] == AthleteInfoStatus.Active) {
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

            _View.SetErrorPanelText(errors);
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
                _View.ShowAthletesNotValidated(!res);
            }
        }

        public override void InitPanel() {
            TournamentData data = DataManager.Instance.AppData;
            LoadAthletes(data.Athletes);

            foreach (KeyValuePair<AthleteInfoType, AthleteInfoStatus> columnShown in data.GetAthletesInfoUsed()) {
                if (!_settings.GetIsVisible(columnShown.Key)) {
                    DataManager.Instance.AppData.SetAthleteInfoUsed(columnShown.Key, AthleteInfoStatus.Hide);
                }
            }
            _athletesInfoUsed = data.GetAthletesInfoUsed();

            CalculateColumnsAnchors();
            UpdateColumnsEnabled();

            UpdateColumnsBlockedByFillerType(data.TournamentFormulaName);

            ValidateAll(false);
        }

        private void LoadAthletes(List<AthleteInfoModel> athletes) {
            if (_currentAthletes.Count == 0 && athletes.Count != 0) {
                // Show athletes saved in tournament data
                StartCoroutine(AddRowsCoroutine(athletes));
                // Save that list locally
                _currentAthletes = athletes;
            } else if (_currentAthletes.Count != 0 && _currentAthletes != athletes) {
                // Reset all table content
                _View.ResetAthletesList();
                // Add all needed rows with the athlete information
                _currentAthletes = athletes;
                StartCoroutine(AddRowsCoroutine(_currentAthletes));
            }
        }

        #region Calculates for columns anchors
        public void CalculateColumnsAnchors() {
            Dictionary<AthleteInfoType, Vector2> columnsSizes = new Dictionary<AthleteInfoType, Vector2>();
            float anchorAux = 0f;
            float extraSpaceDistributed = GetExtraSpaceDristributed(_athletesInfoUsed);

            foreach (KeyValuePair<AthleteInfoType, AthleteInfoStatus> columnShown in _athletesInfoUsed) {
                if (columnShown.Value == AthleteInfoStatus.Hide) {
                    columnsSizes.Add(columnShown.Key, Vector2.zero);
                    continue;
                }

                bool isVisibleBySetting = _settings.GetIsVisible(columnShown.Key);
                if (!isVisibleBySetting) {
                    columnsSizes.Add(columnShown.Key, Vector2.zero);
                    continue;
                }

                float anchorAuxMax = anchorAux + _settings.GetSize(columnShown.Key) / 100f;
                if (_settings.GetCanExpand(columnShown.Key)) {
                    anchorAuxMax += extraSpaceDistributed;
                }

                columnsSizes.Add(columnShown.Key, new Vector2(anchorAux, anchorAuxMax));
                anchorAux = anchorAuxMax;
            }

            _View.SetColumnsAnchors(columnsSizes, _settings.RowHeight);
        }

        private float GetExtraSpaceDristributed(Dictionary<AthleteInfoType, AthleteInfoStatus> columnsShown) {
            List<AthleteInfoType> hideInfos = columnsShown.Where(x => x.Value == AthleteInfoStatus.Hide).Select(x => x.Key).ToList();
            List<AthleteInfoType> isNotVisibleInfos = _settings.GetAllIsNotVisible();
            List<AthleteInfoType> finalInfos = hideInfos.Union(isNotVisibleInfos).ToList();

            if (finalInfos.Count != 0) {
                float extraSpace = 0f;
                foreach (AthleteInfoType finalInfo in finalInfos) {
                    extraSpace += _settings.GetSize(finalInfo) / 100f;
                }

                List<AthleteInfoType> canExpandInfos = _settings.GetAllCanExpandInfo();
                int expandablesAvailable = 0;
                foreach (AthleteInfoType canExpandInfo in canExpandInfos) {
                    if (!hideInfos.Contains(canExpandInfo)) {
                        expandablesAvailable++;
                    }
                }

                return extraSpace / expandablesAvailable;
            }

            return 0f;
        }
        #endregion

        public override void FinishPanel() {
            DataManager.Instance.AppData.Athletes = _currentAthletes;
        }
        #endregion

        public void EnableColumn(AthleteInfoType checkboxInfo, bool isChecked) {
            _View.EnableColumn(checkboxInfo, isChecked);
        }

        private void UpdateColumnsEnabled() {
            foreach (KeyValuePair<AthleteInfoType, AthleteInfoStatus> infoUsed in _athletesInfoUsed) {
                EnableColumn(infoUsed.Key, infoUsed.Value == AthleteInfoStatus.Active ? true : false);
            }
        }

        private void UpdateColumnsBlockedByFillerType(string tournamentFormulaName) {
            TournamentFormula formula = TournamentFormulaUtils.GetFormulaByName(tournamentFormulaName);
            if (!TournamentFormulaUtils.IsCustomFormula(tournamentFormulaName)) {
                _View.UpdateColumnsBlockedByFillerType(formula.FillerType);
            }
        }

        private void UpdateBottomButtons(int athleteCount) {
            _View.SetRemoveButtonInteractable(athleteCount > 0);
        }

        private void UpdateColumnsInTheLastRow() {
            foreach (KeyValuePair<AthleteInfoType, AthleteInfoStatus> columnEnabledInfo in _athletesInfoUsed) {
                _View.EnableRowColumn(_currentAthletes.Count - 1, columnEnabledInfo.Key, columnEnabledInfo.Value == AthleteInfoStatus.Active);
            }
        }

        private void UpdateAllHiddenAndDisableColumnsInARow(int rowToUpdate) {
            foreach (KeyValuePair<AthleteInfoType, AthleteInfoStatus> columnEnabledInfo in _athletesInfoUsed) {
                _View.EnableRowColumn(rowToUpdate, columnEnabledInfo.Key, columnEnabledInfo.Value == AthleteInfoStatus.Active);
            }
        }
    }
}
