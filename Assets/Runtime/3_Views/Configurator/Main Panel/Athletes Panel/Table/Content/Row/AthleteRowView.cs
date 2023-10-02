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
            _countryRow.OnColumnValueSetted += OnColumnSetted;

            _surnameRow.OnColumnValueSetted += OnColumnSetted;
            _nameRow.OnColumnValueSetted += OnColumnSetted;
            _academyRow.OnColumnValueSetted += OnColumnSetted;
            _schoolRow.OnColumnValueSetted += OnColumnSetted;

            _rankRow.OnColumnValueSetted += OnColumnSetted;
            _stylesRow.OnColumnValueSetted += OnColumnSetted;

            _tierRow.OnColumnValueSetted += OnColumnSetted;

            _colorRow.OnColumnValueSetted += OnColumnSetted;
            _birthDateRow.OnColumnValueSetted += OnColumnSetted;
            _startDateRow.OnColumnValueSetted += OnColumnSetted;
        }

        private void OnDisable() {
            _countryRow.OnColumnValueSetted -= OnColumnSetted;

            _surnameRow.OnColumnValueSetted -= OnColumnSetted;
            _nameRow.OnColumnValueSetted -= OnColumnSetted;
            _academyRow.OnColumnValueSetted -= OnColumnSetted;
            _schoolRow.OnColumnValueSetted -= OnColumnSetted;

            _rankRow.OnColumnValueSetted -= OnColumnSetted;
            _stylesRow.OnColumnValueSetted -= OnColumnSetted;

            _tierRow.OnColumnValueSetted -= OnColumnSetted;

            _colorRow.OnColumnValueSetted -= OnColumnSetted;
            _birthDateRow.OnColumnValueSetted -= OnColumnSetted;
            _startDateRow.OnColumnValueSetted -= OnColumnSetted;
        }
        #endregion

        #region GETTERS
        public string GetCountryField() { return _countryRow.GetCurrentValue(); }
        public string GetSurnameField() { return _surnameRow.GetText(); }
        public string GetNameField() { return _nameRow.GetText(); }
        public string GetAcademyField() { return _academyRow.GetText(); }
        public string GetSchoolField() { return _schoolRow.GetText(); }
        public RankType GetRankField() { return (RankType)_rankRow.GetValue(); }
        public List<StyleType> GetStylesField() {
            List<StyleType> styles = new List<StyleType>();
            List<bool> stylesBools = _stylesRow.GetStyles();
            for (int i = 0; i < stylesBools.Count; ++i) {
                if (stylesBools[i]) {
                    styles.Add((StyleType)i);
                }
            }
            return styles;
        }
        public int GetTierField() { return int.Parse(_tierRow.GetText()); }
        public Color GetColorField() { return _colorRow.GetColor(); }
        public DateTime GetBirthDateField() { return _birthDateRow.GetDate(); }
        public DateTime GetStartDateField() { return _startDateRow.GetDate(); }
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
        private void OnColumnSetted(AthleteInfoType columnSetted) {
            AthletesPanelViewEvents.ThrowOnAthleteDataUpdated(
                AthleteInfoType.Country, _athleteRowIndex);
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
