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
            _countryRow.OnFinalValueSetted += CountryFieldChanged;

            _surnameRow.InputField.onValueChanged.AddListener(SurnameFieldChanged);
            _nameRow.InputField.onValueChanged.AddListener(NameFieldChanged);
            _academyRow.InputField.onValueChanged.AddListener(AcademyFieldChanged);
            _academyRow.InputField.onValueChanged.AddListener(SchoolFieldChanged);

            _rankRow.Dropdown.onValueChanged.AddListener(RankFieldChanged);
            _stylesRow.StyleToggleClicked += StylesFieldChanged;

            _tierRow.InputField.onValueChanged.AddListener(TierFieldChanged);
        }

        private void OnDisable() {
            _countryRow.OnFinalValueSetted -= CountryFieldChanged;

            _surnameRow.InputField.onValueChanged.RemoveAllListeners();
            _nameRow.InputField.onValueChanged.RemoveAllListeners();
            _academyRow.InputField.onValueChanged.RemoveAllListeners();
            _schoolRow.InputField.onValueChanged.RemoveAllListeners();

            _rankRow.Dropdown.onValueChanged.RemoveAllListeners();
            _stylesRow.StyleToggleClicked -= StylesFieldChanged;

            _tierRow.InputField.onValueChanged.RemoveAllListeners();
        }
        #endregion

        #region GETTERS
        public string GetCountryField() { return _countryRow.GetCurrentValue(); }
        public string GetSurnameField() { return _surnameRow.InputField.text; }
        public string GetNameField() { return _nameRow.InputField.text; }
        public string GetAcademyField() { return _academyRow.InputField.text; }
        public string GetSchoolField() { return _schoolRow.InputField.text; }
        public RankType GetRankField() { return (RankType)_rankRow.Dropdown.value; }
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
        public int GetTierField() { return int.Parse(_tierRow.InputField.text); }
        #endregion

        #region SETTERS
        public void SetCountryField(string countryCode, bool withoutNotify = false) {
            _countryRow.SetInitValue(countryCode, withoutNotify);
        }
        public void SetSurnameField(string surname, bool withoutNotify = false) {
            if (!withoutNotify) {
                _surnameRow.InputField.text = surname;
            } else {
                _surnameRow.InputField.SetTextWithoutNotify(surname);
            }
        }
        public void SetNameField(string name, bool withoutNotify = false) {
            if (!withoutNotify) {
                _nameRow.InputField.text = name;
            } else {
                _nameRow.InputField.SetTextWithoutNotify(name);
            }
        }
        public void SetAcademyField(string academy, bool withoutNotify = false) {
            if (!withoutNotify) {
                _academyRow.InputField.text = academy;
            } else {
                _academyRow.InputField.SetTextWithoutNotify(academy);
            }
        }
        public void SetSchoolField(string school, bool withoutNotify = false) {
            if (!withoutNotify) {
                _schoolRow.InputField.text = school;
            } else {
                _schoolRow.InputField.SetTextWithoutNotify(school);
            }
        }
        public void SetRankField(RankType rank, bool withoutNotify = false) {
            if (!withoutNotify) {
                _rankRow.Dropdown.value = (int)rank;
            } else {
                _rankRow.Dropdown.SetValueWithoutNotify((int)rank);
            }
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
            if (!withoutNotify) {
                _tierRow.InputField.text = tier > 0 ? tier.ToString() : string.Empty;
            } else {
                _tierRow.InputField.SetTextWithoutNotify(tier > 0 ? tier.ToString() : string.Empty);
            }
        }
        #endregion

        #region Event listeners methods
        private void CountryFieldChanged(string finalValue) {
            AthletesPanelViewEvents.ThrowOnAthleteDataUpdated(
                AthleteInfoType.Country,
                finalValue, _athleteRowIndex);
        }

        private void SurnameFieldChanged(string strValue) {
            AthletesPanelViewEvents.ThrowOnAthleteDataUpdated(
                AthleteInfoType.Surname,
                strValue, _athleteRowIndex);
        }

        private void NameFieldChanged(string strValue) {
            AthletesPanelViewEvents.ThrowOnAthleteDataUpdated(
                AthleteInfoType.Name,
                strValue, _athleteRowIndex);
        }

        private void AcademyFieldChanged(string strValue) {
            AthletesPanelViewEvents.ThrowOnAthleteDataUpdated(
                AthleteInfoType.Academy,
                strValue, _athleteRowIndex);
        }

        private void SchoolFieldChanged(string strValue) {
            AthletesPanelViewEvents.ThrowOnAthleteDataUpdated(
                AthleteInfoType.School,
                strValue, _athleteRowIndex);
        }

        private void RankFieldChanged(int intValue) {
            AthletesPanelViewEvents.ThrowOnAthleteDataUpdated(
                AthleteInfoType.Rank,
                intValue.ToString(), _athleteRowIndex);
        }

        private void StylesFieldChanged(StyleType styleClicked, bool isOn) {
            string stylesInList = string.Empty;

            List<StyleType> styles = GetStylesField();
            foreach (StyleType style in styles) {
                stylesInList += (int)style + ",";
            }

            AthletesPanelViewEvents.ThrowOnAthleteDataUpdated(
                AthleteInfoType.Styles,
                stylesInList, _athleteRowIndex);
        }

        private void TierFieldChanged(string strValue) {
            AthletesPanelViewEvents.ThrowOnAthleteDataUpdated(
                AthleteInfoType.Tier,
                strValue, _athleteRowIndex);
        }
        #endregion

        public void HideColumn(AthleteInfoType column, bool hide, bool withoutNotify = false) {
            switch (column) {
                case AthleteInfoType.Country:
                    _countryRow.gameObject.SetActive(!hide);
                    break;
                case AthleteInfoType.Academy:
                    _academyRow.gameObject.SetActive(!hide);
                    break;
                default:
                    if (!withoutNotify) {
                        Debug.LogWarning($"{Enum.GetName(typeof(AthleteInfoType), column)} cannot be {(hide ? "hide" : "show")}!");
                    }
                    break;
            }
        }

        public void DisableColumn(AthleteInfoType column, bool disable, bool withoutNotify = false) {
            switch (column) {
                case AthleteInfoType.Country:
                    _countryRow.Disable(disable);
                    break;
                case AthleteInfoType.Academy:
                    _academyRow.Disable(disable);
                    break;
                case AthleteInfoType.School:
                    _schoolRow.Disable(disable);
                    break;
                case AthleteInfoType.Rank:
                    _rankRow.Disable(disable);
                    break;
                case AthleteInfoType.Styles:
                    _stylesRow.Disable(disable);
                    break;
                case AthleteInfoType.Tier:
                    _tierRow.Disable(disable);
                    break;
                case AthleteInfoType.SaberColor:
                    _colorRow.Disable(disable);
                    break;
                case AthleteInfoType.BirthDate:
                    _birthDateRow.Disable(disable);
                    break;
                case AthleteInfoType.StartDate:
                    _startDateRow.Disable(disable);
                    break;
                default:
                    if (!withoutNotify) {
                        Debug.LogWarning($"{Enum.GetName(typeof(AthleteInfoType), column)} cannot be {(disable ? "disable" : "enable")}!");
                    }
                    break;
            }
        }
    }
}
