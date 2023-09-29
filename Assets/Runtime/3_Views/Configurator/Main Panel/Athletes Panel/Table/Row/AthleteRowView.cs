using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using YannickSCF.LSTournaments.Common.Views.MainPanel.AthletesPanel.Events;
using YannickSCF.LSTournaments.Common.Views.MainPanel.AthletesPanel.Table.Row.RowColumns;
using YannickSCF.LSTournaments.Common.Views.MainPanel.AthletesPanel.Table.Row.RowColumns.CountryCol;
using YannickSCF.LSTournaments.Common.Views.MainPanel.AthletesPanel.Table.Row.RowColumns.StylesCol;

namespace YannickSCF.LSTournaments.Common.Views.MainPanel.AthletesPanel.Table.Row {
    public class AthleteRowView : MonoBehaviour {

        [Header("Local references")]
        [SerializeField] private Image _rowBackground;
        [SerializeField] private Color _pairColor;
        [SerializeField] private Color _oddColor;
        [SerializeField] private TextMeshProUGUI _rowIndexText;

        [Header("Columns references")]
        [SerializeField] private CountryColView _countryRow;

        [SerializeField] private TMP_InputField _surnameRow;
        [SerializeField] private TMP_InputField _nameRow;
        [SerializeField] private TMP_InputField _academyRow;
        [SerializeField] private TMP_InputField _schoolRow;

        [SerializeField] private TMP_Dropdown _rankRow;
        [SerializeField] private StylesColView _stylesRow;

        [SerializeField] private TMP_InputField _tierRow;

        [Header("TO DO (Temporal references)")]
        [SerializeField] private RowColumnView _colorRow;
        [SerializeField] private RowColumnView _birthDateRow;
        [SerializeField] private RowColumnView _startDateRow;

        private RowColumnView _countryRowBase;

        private RowColumnView _academyRowBase;
        private RowColumnView _schoolRowBase;

        private RowColumnView _rankRowBase;
        private RowColumnView _stylesRowBase;

        private RowColumnView _tierRowBase;

        private RowColumnView _colorRowBase;
        private RowColumnView _birthDateRowBase;
        private RowColumnView _startDateRowBase;

        private int _athleteRowIndex;

        public int AthleteRowIndex { get => _athleteRowIndex; }
        public void SetAthleteRowIndex (int index) {
            _athleteRowIndex = index;
            _rowIndexText.text = (index + 1).ToString();

            _rowBackground.color = index % 2 == 0 ? _pairColor : _oddColor;

            _countryRowBase.SetBackgroundColor(_rowBackground.color);
            _academyRowBase.SetBackgroundColor(_rowBackground.color);
            _schoolRowBase.SetBackgroundColor(_rowBackground.color);
            _rankRowBase.SetBackgroundColor(_rowBackground.color);
            _stylesRowBase.SetBackgroundColor(_rowBackground.color);
            _tierRowBase.SetBackgroundColor(_rowBackground.color);
            _colorRowBase.SetBackgroundColor(_rowBackground.color);
            _birthDateRowBase.SetBackgroundColor(_rowBackground.color);
            _startDateRowBase.SetBackgroundColor(_rowBackground.color);
        }

        #region Mono
        private void Awake() {
            _countryRowBase = _countryRow.GetComponent<RowColumnView>();

            _academyRowBase = _academyRow.GetComponent<RowColumnView>();
            _schoolRowBase = _schoolRow.GetComponent<RowColumnView>();

            _rankRowBase = _rankRow.GetComponent<RowColumnView>();
            _stylesRowBase = _stylesRow.GetComponent<RowColumnView>();

            _tierRowBase = _tierRow.GetComponent<RowColumnView>();

            _colorRowBase = _colorRow.GetComponent<RowColumnView>();
            _birthDateRowBase = _birthDateRow.GetComponent<RowColumnView>();
            _startDateRowBase = _startDateRow.GetComponent<RowColumnView>();
        }

        private void OnEnable() {
            _countryRow.OnFinalValueSetted += CountryFieldChanged;

            _surnameRow.onValueChanged.AddListener(SurnameFieldChanged);
            _nameRow.onValueChanged.AddListener(NameFieldChanged);
            _academyRow.onValueChanged.AddListener(AcademyFieldChanged);
            _academyRow.onValueChanged.AddListener(SchoolFieldChanged);

            _rankRow.onValueChanged.AddListener(RankFieldChanged);
            _stylesRow.StyleToggleClicked += StylesFieldChanged;

            _tierRow.onValueChanged.AddListener(TierFieldChanged);
        }

        private void OnDisable() {
            _countryRow.OnFinalValueSetted -= CountryFieldChanged;

            _surnameRow.onValueChanged.RemoveAllListeners();
            _nameRow.onValueChanged.RemoveAllListeners();
            _academyRow.onValueChanged.RemoveAllListeners();
            _schoolRow.onValueChanged.RemoveAllListeners();

            _rankRow.onValueChanged.RemoveAllListeners();
            _stylesRow.StyleToggleClicked -= StylesFieldChanged;

            _tierRow.onValueChanged.RemoveAllListeners();
        }
        #endregion

        #region GETTERS
        public string GetCountryField() { return _countryRow.GetCurrentValue(); }
        public string GetSurnameField() { return _surnameRow.text; }
        public string GetNameField() { return _nameRow.text; }
        public string GetAcademyField() { return _academyRow.text; }
        public string GetSchoolField() { return _schoolRow.text; }
        public RankType GetRankField() { return (RankType)_rankRow.value; }
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
        public int GetTierField() { return int.Parse(_tierRow.text); }
        #endregion

        #region SETTERS
        public void SetCountryField(string countryCode, bool withoutNotify = false) {
            _countryRow.SetInitValue(countryCode, withoutNotify);
        }
        public void SetSurnameField(string surname, bool withoutNotify = false) {
            if (!withoutNotify) {
                _surnameRow.text = surname;
            } else {
                _surnameRow.SetTextWithoutNotify(surname);
            }
        }
        public void SetNameField(string name, bool withoutNotify = false) {
            if (!withoutNotify) {
                _nameRow.text = name;
            } else {
                _nameRow.SetTextWithoutNotify(name);
            }
        }
        public void SetAcademyField(string academy, bool withoutNotify = false) {
            if (!withoutNotify) {
                _academyRow.text = academy;
            } else {
                _academyRow.SetTextWithoutNotify(academy);
            }
        }
        public void SetSchoolField(string school, bool withoutNotify = false) {
            if (!withoutNotify) {
                _schoolRow.text = school;
            } else {
                _schoolRow.SetTextWithoutNotify(school);
            }
        }
        public void SetRankField(RankType rank, bool withoutNotify = false) {
            if (!withoutNotify) {
                _rankRow.value = (int)rank;
            } else {
                _rankRow.SetValueWithoutNotify((int)rank);
            }
        }
        public void SetStylesField(List<StyleType> styles, bool withoutNotify = false) {
            if (_stylesRow != null) {
                List<bool> stylesBools = new List<bool>();

                Array allStyles = Enum.GetValues(typeof(StyleType));
                for (int i = 0; i < allStyles.Length; ++i) {
                    stylesBools[i] = styles.Contains((StyleType)allStyles.GetValue(i));
                }

                _stylesRow.SetStyles(stylesBools, withoutNotify);
            }
        }
        public void SetTierField(int tier, bool withoutNotify = false) {
            if (!withoutNotify) {
                _tierRow.text = tier.ToString();
            } else {
                _tierRow.SetTextWithoutNotify(tier.ToString());
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

        public void HideColumn(AthleteInfoType column, bool hide) {
            switch (column) {
                case AthleteInfoType.Country:
                    _countryRowBase.gameObject.SetActive(hide);
                    break;
                case AthleteInfoType.Academy:
                    _academyRowBase.gameObject.SetActive(hide);
                    break;
                default:
                    Debug.LogWarning($"{Enum.GetName(typeof(AthleteInfoType), column)} cannot be {(hide ? "hide" : "show")}!");
                    break;
            }
        }

        public void DisableColumn(AthleteInfoType column, bool disable) {
            switch (column) {
                case AthleteInfoType.Country:
                    _countryRowBase.Disable(disable);
                    break;
                case AthleteInfoType.Academy:
                    _academyRowBase.Disable(disable);
                    break;
                case AthleteInfoType.School:
                    _schoolRowBase.Disable(disable);
                    break;
                case AthleteInfoType.Rank:
                    _rankRowBase.Disable(disable);
                    break;
                case AthleteInfoType.Styles:
                    _stylesRowBase.Disable(disable);
                    break;
                case AthleteInfoType.Tier:
                    _tierRowBase.Disable(disable);
                    break;
                case AthleteInfoType.SaberColor:
                    _colorRowBase.Disable(disable);
                    break;
                case AthleteInfoType.BirthDate:
                    _birthDateRowBase.Disable(disable);
                    break;
                case AthleteInfoType.StartDate:
                    _startDateRowBase.Disable(disable);
                    break;
                default:
                    Debug.LogWarning($"{Enum.GetName(typeof(AthleteInfoType), column)} cannot be {(disable ? "disable" : "enable")}!");
                    break;
            }
        }
    }
}
