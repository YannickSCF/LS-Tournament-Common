// Dependencies
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
// Custom dependencies
using YannickSCF.LSTournaments.Common.Views.MainPanel.AthletesPanel.Events;
using YannickSCF.LSTournaments.Common.Views.MainPanel.AthletesPanel.Table.Content.Row.RowColumns.SpecificCols;

namespace YannickSCF.LSTournaments.Common.Views.MainPanel.AthletesPanel.Content.Table.Row {
    public class AthleteRowView : MonoBehaviour {

        [Header("Local references")]
        [SerializeField] private Image _rowBackground;
        [SerializeField] private Color _pairColor;
        [SerializeField] private Color _oddColor;
        [SerializeField] private TextMeshProUGUI _rowIndexText;

        [Header("Columns references")]
        [SerializeField] private CountryColView _countryRow;

        [SerializeField] private InputFieldColView _surnameRow;
        [SerializeField] private InputFieldColView _nameRow;
        [SerializeField] private InputFieldColView _academyRow;
        [SerializeField] private InputFieldColView _schoolRow;

        [SerializeField] private DropdownColView _rankRow;
        [SerializeField] private StylesColView _stylesRow;

        [SerializeField] private InputFieldColView _tierRow;

        [SerializeField] private ColorColView _colorRow;
        [SerializeField] private DateColView _birthDateRow;
        [SerializeField] private DateColView _startDateRow;

        private int _athleteRowIndex;

        public int AthleteRowIndex { get => _athleteRowIndex; }
        public void SetAthleteRowIndex (int index) {
            _athleteRowIndex = index;
            _rowIndexText.text = (index + 1).ToString();

            _rowBackground.color = index % 2 == 0 ? _pairColor : _oddColor;

            _countryRow.SetBackgroundColor(_rowBackground.color);

            _academyRow.SetBackgroundColor(_rowBackground.color);
            _schoolRow.SetBackgroundColor(_rowBackground.color);

            _rankRow.SetBackgroundColor(_rowBackground.color);
            _stylesRow.SetBackgroundColor(_rowBackground.color);

            _tierRow.SetBackgroundColor(_rowBackground.color);

            _colorRow.SetBackgroundColor(_rowBackground.color);
            _birthDateRow.SetBackgroundColor(_rowBackground.color);
            _startDateRow.SetBackgroundColor(_rowBackground.color);
        }

        #region Mono
        private void OnEnable() {
            _countryRow.OnColumnStringValueSetted += OnAthleteDataStringUpdated;

            _surnameRow.OnColumnStringValueSetted += OnAthleteDataStringUpdated;
            _nameRow.OnColumnStringValueSetted += OnAthleteDataStringUpdated;
            _academyRow.OnColumnStringValueSetted += OnAthleteDataStringUpdated;
            _schoolRow.OnColumnStringValueSetted += OnAthleteDataStringUpdated;

            _rankRow.OnColumnRankValueSetted += OnAthleteDataRankUpdated;
            _stylesRow.OnColumnStylesValueSetted += OnAthleteDataStylesUpdated;

            _tierRow.OnColumnStringValueSetted += OnAthleteDataStringUpdated;

            _colorRow.OnColumnColorValueSetted += OnAthleteDataColorUpdated;
            _birthDateRow.OnColumnDateTimeValueSetted += OnAthleteDataDateTimeUpdated;
            _startDateRow.OnColumnDateTimeValueSetted += OnAthleteDataDateTimeUpdated;
        }

        private void OnDisable() {
            _countryRow.OnColumnStringValueSetted -= OnAthleteDataStringUpdated;

            _surnameRow.OnColumnStringValueSetted -= OnAthleteDataStringUpdated;
            _nameRow.OnColumnStringValueSetted -= OnAthleteDataStringUpdated;
            _academyRow.OnColumnStringValueSetted -= OnAthleteDataStringUpdated;
            _schoolRow.OnColumnStringValueSetted -= OnAthleteDataStringUpdated;

            _rankRow.OnColumnRankValueSetted -= OnAthleteDataRankUpdated;
            _stylesRow.OnColumnStylesValueSetted -= OnAthleteDataStylesUpdated;

            _tierRow.OnColumnStringValueSetted -= OnAthleteDataStringUpdated;

            _colorRow.OnColumnColorValueSetted -= OnAthleteDataColorUpdated;
            _birthDateRow.OnColumnDateTimeValueSetted -= OnAthleteDataDateTimeUpdated;
            _startDateRow.OnColumnDateTimeValueSetted -= OnAthleteDataDateTimeUpdated;
        }
        #endregion

        #region SETTERS
        public void SetCountryField(string countryCode, bool withoutNotify = false) {
            _countryRow.SetInitValue(countryCode, withoutNotify);
        }
        public void SetSurnameField(string surname, bool withoutNotify = false) {
            _surnameRow.SetInputField(surname, withoutNotify);
        }
        public void SetNameField(string name, bool withoutNotify = false) {
            _nameRow.SetInputField(name, withoutNotify);
        }
        public void SetAcademyField(string academy, bool withoutNotify = false) {
            _academyRow.SetInputField(academy, withoutNotify);
        }
        public void SetSchoolField(string school, bool withoutNotify = false) {
            _schoolRow.SetInputField(school, withoutNotify);
        }
        public void SetRankField(RankType rank, bool withoutNotify = false) {
            _rankRow.SetDropdown((int)rank, withoutNotify);
        }
        public void SetStylesField(List<StyleType> styles, bool withoutNotify = false) {
            if (_stylesRow != null) {
                try {
                    List<bool> stylesBools = new List<bool>();

                    Array allStyles = Enum.GetValues(typeof(StyleType));
                    for (int i = 0; i < allStyles.Length; ++i) {
                        stylesBools.Add(styles.Contains((StyleType)allStyles.GetValue(i)));
                    }

                    _stylesRow.SetStyles(stylesBools, withoutNotify);
                } catch (Exception ex) {
                    Debug.LogError(ex.Message);
                }
            }
        }
        public void SetTierField(int tier, bool withoutNotify = false) {
            _tierRow.SetInputField(tier > 0 ? tier.ToString() : string.Empty, withoutNotify);
        }
        public void SetColorField(Color color, bool withoutNotify = false) {
            _colorRow.SetColor(color, withoutNotify);
        }
        public void SetBirthDateField(DateTime birthDate, bool withoutNotify = false) {
            _birthDateRow.SetDate(birthDate, withoutNotify);
        }
        public void SetStartDateField(DateTime startDate, bool withoutNotify = false) {
            _startDateRow.SetDate(startDate, withoutNotify);
        }
        #endregion

        #region Event listeners methods
        private void OnAthleteDataStringUpdated(AthleteInfoType columnSetted, string newData) {
            AthletesPanelViewEvents.ThrowOnAthleteDataStringUpdated(
                columnSetted, newData, _athleteRowIndex);
        }
        private void OnAthleteDataDateTimeUpdated(AthleteInfoType columnSetted, DateTime newData) {
            AthletesPanelViewEvents.ThrowOnAthleteDataDateTimeUpdated(
                columnSetted, newData, _athleteRowIndex);
        }
        private void OnAthleteDataRankUpdated(AthleteInfoType columnSetted, RankType newData) {
            AthletesPanelViewEvents.ThrowOnAthleteDataRankUpdated(
                newData, _athleteRowIndex);
        }
        private void OnAthleteDataColorUpdated(AthleteInfoType columnSetted, Color newData) {
            AthletesPanelViewEvents.ThrowOnAthleteDataColorUpdated(
                newData, _athleteRowIndex);
        }
        private void OnAthleteDataStylesUpdated(AthleteInfoType columnSetted, List<StyleType> newData) {
            AthletesPanelViewEvents.ThrowOnAthleteDataStylesUpdated(
                newData, _athleteRowIndex);
        }
        #endregion

        public void ShowRowColumn(AthleteInfoType columnToShow, bool show) {
            switch (columnToShow) {
                case AthleteInfoType.Country: _countryRow.gameObject.SetActive(show); break;
                case AthleteInfoType.Surname: _surnameRow.gameObject.SetActive(show); break;
                case AthleteInfoType.Name: _nameRow.gameObject.SetActive(show); break;
                case AthleteInfoType.Academy: _academyRow.gameObject.SetActive(show); break;
                case AthleteInfoType.School: _schoolRow.gameObject.SetActive(show); break;
                case AthleteInfoType.Rank: _rankRow.gameObject.SetActive(show); break;
                case AthleteInfoType.Styles: _stylesRow.gameObject.SetActive(show); break;
                case AthleteInfoType.Tier: _tierRow.gameObject.SetActive(show); break;
                case AthleteInfoType.SaberColor: _colorRow.gameObject.SetActive(show); break;
                case AthleteInfoType.BirthDate: _birthDateRow.gameObject.SetActive(show); break;
                case AthleteInfoType.StartDate: _startDateRow.gameObject.SetActive(show); break;
                default:
                    Debug.LogWarning($"{Enum.GetName(typeof(AthleteInfoType), columnToShow)} is not prepared to Show/Hide function!");
                    break;
            }
        }

        public void EnableRowColumn(AthleteInfoType columnToShow, bool enable) {
            switch (columnToShow) {
                case AthleteInfoType.Country: _countryRow.EnableColumn(enable); break;
                case AthleteInfoType.Surname: _surnameRow.EnableColumn(enable); break;
                case AthleteInfoType.Name: _nameRow.EnableColumn(enable); break;
                case AthleteInfoType.Academy: _academyRow.EnableColumn(enable); break;
                case AthleteInfoType.School: _schoolRow.EnableColumn(enable); break;
                case AthleteInfoType.Rank: _rankRow.EnableColumn(enable); break;
                case AthleteInfoType.Styles: _stylesRow.EnableColumn(enable); break;
                case AthleteInfoType.Tier: _tierRow.EnableColumn(enable); break;
                case AthleteInfoType.SaberColor: _colorRow.EnableColumn(enable); break;
                case AthleteInfoType.BirthDate: _birthDateRow.EnableColumn(enable); break;
                case AthleteInfoType.StartDate: _startDateRow.EnableColumn(enable); break;
                default:
                    Debug.LogWarning($"{Enum.GetName(typeof(AthleteInfoType), columnToShow)} is not prepared to Enable/Disable function!");
                    break;
            }
        }
    }
}