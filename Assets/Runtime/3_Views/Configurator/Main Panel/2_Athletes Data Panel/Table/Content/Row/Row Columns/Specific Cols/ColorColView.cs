/**
 * Author:      Yannick Santa Cruz Feuillias
 * Created:     30/09/2023
 **/

// Dependencies
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace YannickSCF.LSTournaments.Common.Views.MainPanel.AthletesDataPanel.Table.Content.Row.RowColumns.SpecificCols {
    public class ColorColView : RowColumnView {

        [Header("Color Col References")]
        [SerializeField] private TMP_InputField _inputField;
        [SerializeField] private Image _exampleColor;

        #region Mono
        private void OnEnable() {
            _inputField.onSelect.AddListener(CheckFirstSelect);
            _inputField.onValueChanged.AddListener(ValidateColorText);
            _inputField.onEndEdit.AddListener(OnEndEditColor);
        }

        private void OnDisable() {
            _inputField.onSelect.RemoveAllListeners();
            _inputField.onValueChanged.RemoveAllListeners();
            _inputField.onEndEdit.RemoveAllListeners();
        }
        #endregion

        #region Event Listeners methods
        private void CheckFirstSelect(string inputText) {
            if (string.IsNullOrEmpty(inputText) || !inputText.Contains("#")) {
                _inputField.onFocusSelectAll = false;
                _inputField.SetTextWithoutNotify("#");
            } else {
                _inputField.onFocusSelectAll = true;
            }
        }

        private void ValidateColorText(string inputText) {
            if (string.IsNullOrEmpty(inputText) || inputText.Equals("#")) {
                SetExampleColor(inputText);
                return;
            }

            inputText = inputText.ToUpper();
            inputText = inputText.Replace("#", string.Empty);

            Match searchHex = Regex.Match(inputText, "^([A-F0-9]{0,6})$");
            if (!searchHex.Success) {
                inputText = Regex.Replace(inputText, "[^A-F0-9]", string.Empty);
            }

            inputText = "#" + inputText;
            // Code needed: MoveText doesn't work well if last har is a number
            _inputField.SetTextWithoutNotify(inputText + "*");
            _inputField.MoveTextEnd(false);
            _inputField.SetTextWithoutNotify(_inputField.text.Replace("*", string.Empty));

            SetExampleColor(inputText);
        }

        private void OnEndEditColor(string inputText) {
            if (inputText.Length < 7) {
                int zerosNeeded = 7 - inputText.Length;
                for (int i = 0; i < zerosNeeded; ++i) {
                    inputText += "0";
                }
            }

            _inputField.SetTextWithoutNotify(inputText);
            SetExampleColor(inputText);

            ThrowColumnValueSetted(GetColor(), _inputField);
        }
        #endregion

        #region Protected overrrided methods
        protected override void SetSelectablesInteractables(bool isInteractable) {
            _inputField.interactable = isInteractable;
        }
        #endregion

        public Color GetColor() {
            if (string.IsNullOrEmpty(_inputField.text)) {
                return Color.black;
            }

            return _exampleColor.color;
        }

        public void SetColor(Color color, bool withoutNotify = false) {
            if (!withoutNotify) {
                _inputField.text = "#" + ColorUtility.ToHtmlStringRGB(color);
            } else {
                _inputField.SetTextWithoutNotify("#" + ColorUtility.ToHtmlStringRGB(color));
            }

            SetExampleColor(color);
        }

        public void ResetColor() {
            _inputField.SetTextWithoutNotify("#");
            SetExampleColor("#");
        }

        private void SetExampleColor(string colorHex) {
            if (colorHex.Length < 7) {
                int zerosNeeded = 7 - colorHex.Length;
                for (int i = 0; i < zerosNeeded; ++i) {
                    colorHex += "0";
                }
            }

            if (ColorUtility.TryParseHtmlString(colorHex, out Color colorParsed)) {
                SetExampleColor(colorParsed);
            }
        }

        private void SetExampleColor(Color color) {
            _exampleColor.color = color;
        }
    }
}
