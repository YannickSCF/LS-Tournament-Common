// Dependencies
using TMPro;
using UnityEngine;

namespace YannickSCF.LSTournaments.Common.Views.MainPanel.AthletesPanel.Table.Content.Row.RowColumns.SpecificCols {
    public class InputFieldColView : RowColumnView {

        [Header("Input Field Col References")]
        [SerializeField] private TMP_InputField _inputField;

        public TMP_InputField.OnChangeEvent OnValueChanged() {
            return _inputField.onValueChanged;
        }

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
