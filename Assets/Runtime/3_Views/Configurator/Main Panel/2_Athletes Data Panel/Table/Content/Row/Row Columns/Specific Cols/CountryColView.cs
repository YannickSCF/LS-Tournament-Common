/**
 * Author:      Yannick Santa Cruz Feuillias
 * Created:     28/09/2023
 **/

// Dependencies
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
// Custom dependencies
using YannickSCF.CountriesData;

namespace YannickSCF.LSTournaments.Common.Views.MainPanel.AthletesDataPanel.Table.Content.Row.RowColumns.SpecificCols {
    public class CountryColView : RowColumnView {
        private const string TWO_DIGITS_PLACEHOLDER = "--";
        private const string THREE_DIGITS_PLACEHOLDER = "---";

        [Header("Country Col References")]
        [SerializeField] private bool _twoDigitCode;
        [SerializeField] private TextMeshProUGUI _placeholder;
        [Header("Basic object references")]
        [SerializeField] private TMP_InputField _inputField;
        [SerializeField] private Image _captionImage;
        [SerializeField] private ScrollRect _optionsScrollRect;

        private Dictionary<string, Sprite> _options;
        private string _lastText = "";

        #region Mono
        public void Awake() {
            _options = new Dictionary<string, Sprite>();

            SetPlaceholder();
            SetFinalValue(string.Empty, true);
        }

        private void OnEnable() {
            _inputField.onValueChanged.AddListener(OnInputFieldChanged);
        }

        private void OnDisable() {
            _inputField.onValueChanged.RemoveAllListeners();
        }
        #endregion

        #region Protected overrrided methods
        protected override void SetSelectablesInteractables(bool isInteractable) {
            _inputField.interactable = isInteractable;
        }
        #endregion
        
        public bool SetInitValue(string countryCode, bool setTwoDigitCode, bool withoutNotify = false) {
            _twoDigitCode = setTwoDigitCode;
            SetPlaceholder();

            if (countryCode != null && countryCode.Length == 2 && _twoDigitCode) {
                _captionImage.sprite = CountriesDataUtils.GetFlagByCode(countryCode);
            } else if (countryCode != null && countryCode.Length == 3 && !_twoDigitCode) {
                _captionImage.sprite = CountriesDataUtils.GetFlagByLongCode(countryCode);
            } else {
                return false;
            }

            _captionImage.color = new Color(1, 1, 1, 1);
            if (withoutNotify) {
                _inputField.SetTextWithoutNotify(countryCode);
            } else {
                _inputField.text = countryCode;
            }

            return true;
        }

        public void ResetValue() {
            _inputField.SetTextWithoutNotify(string.Empty);
            _captionImage.sprite = null;
            _captionImage.color = new Color(1, 1, 1, 0);
        }

        public string GetCurrentValue() {
            return _inputField.text;
        }

        private void SetPlaceholder() {
            if (_twoDigitCode) {
                _placeholder.text = TWO_DIGITS_PLACEHOLDER;
            } else {
                _placeholder.text = THREE_DIGITS_PLACEHOLDER;
            }
        }

        private void OnInputFieldChanged(string text) {
            SetFinalValue(null, true);

            if (string.IsNullOrEmpty(text)) {
                _optionsScrollRect.gameObject.SetActive(false);
                ClearObjectList();
                _lastText = string.Empty;
                return;
            }

            if ((text.All(char.IsLetter) && text.Length <= 2 && _twoDigitCode) ||
                (text.All(char.IsLetter) && text.Length <= 3 && !_twoDigitCode)) {
                _lastText = text.ToUpper();
                _inputField.SetTextWithoutNotify(_lastText);
                SetValidOptions(_lastText);
            } else {
                _inputField.SetTextWithoutNotify(_lastText);
            }

            ShowValidOptions();
        }

        private void SetValidOptions(string code) {
            _options.Clear();

            _options = _twoDigitCode ?
                CountriesDataUtils.SearchCountriesByCode(code) :
                CountriesDataUtils.SearchCountriesByLongCode(code);
        }

        private void ShowValidOptions() {
            if (_options.Count == 0) {
                _optionsScrollRect.gameObject.SetActive(false);
            } else if (_options.Count == 1) {
                SetFinalValue(_options.Keys.ElementAt(0));
            } else {
                GameObject templateItem = _optionsScrollRect.content.GetChild(0).gameObject;
                for (int i = 0; i < _options.Count; ++i) {
                    KeyValuePair<string, Sprite> option = _options.ElementAt(i);

                    GameObject listItem;
                    if (i < _optionsScrollRect.content.childCount) {
                        listItem = _optionsScrollRect.content.GetChild(i).gameObject;
                    } else {
                        listItem = Instantiate(templateItem.gameObject, _optionsScrollRect.content.transform);
                    }

                    listItem.GetComponentInChildren<TextMeshProUGUI>().text = option.Key;
                    if (option.Value != null) {
                        listItem.GetComponentsInChildren<Image>()[1].sprite = option.Value;
                    }
                }

                _optionsScrollRect.gameObject.SetActive(true);
            }

            ClearObjectList();
        }

        private void ClearObjectList() {
            int index = 0;
            foreach (Transform child in _optionsScrollRect.content) {
                if (index != 0) {
                    if (index >= _options.Count) {
                        DestroyImmediate(child.gameObject);
                    }
                }

                index++;
            }
        }

        private void SetFinalValue(string code, bool withoutNotify = false) {
            if (!string.IsNullOrEmpty(code)) {
                _captionImage.sprite = _twoDigitCode ?
                    CountriesDataUtils.GetFlagByCode(code) : CountriesDataUtils.GetFlagByLongCode(code);

                _captionImage.color = Color.white;
                _inputField.SetTextWithoutNotify(code);
                _optionsScrollRect.gameObject.SetActive(false);

                _inputField.DeactivateInputField();
            } else {
                _captionImage.sprite = null;
                _captionImage.color = new Color(1, 1, 1, 0);
            }

            if (!withoutNotify) {
                ThrowColumnValueSetted(_inputField.text, _inputField);
            }
        }

        public void SetClickFinalValue(TextMeshProUGUI itemLabel) {
            SetFinalValue(itemLabel.text);
        }
    }
}
