// Dependencies
using TMPro;
using UnityEngine;

namespace YannickSCF.LSTournaments.Common.Views.MainPanel.AthletesPanel.Table.Content.Row.RowColumns.SpecificCols {
    public class InputFieldColView : RowColumnView {

        [Header("Input Field Col References")]
        [SerializeField] private TMP_InputField _inputField;

        #region Mono
        private void OnEnable() {
            _inputField.onSubmit.AddListener(OnTextSetted);
        }

        private void OnDisable() {
            _inputField.onSubmit.RemoveAllListeners();
        }
        #endregion

        #region Event Listeners methods
        private void OnTextSetted(string inputText) {
            ThrowColumnValueSetted(_inputField);
        }
        #endregion

        public string GetText() {
            return _inputField.text;
        }

        public void SetInputField(string name, bool withoutNotify = false) {
            if (!withoutNotify) {
                _inputField.text = name;
            } else {
                _inputField.SetTextWithoutNotify(name);
            }
        }
    }
}
