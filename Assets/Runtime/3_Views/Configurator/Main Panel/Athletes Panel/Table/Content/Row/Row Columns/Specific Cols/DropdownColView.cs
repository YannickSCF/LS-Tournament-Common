// Dependencies
using TMPro;
using UnityEngine;

namespace YannickSCF.LSTournaments.Common.Views.MainPanel.AthletesPanel.Table.Content.Row.RowColumns.SpecificCols {
    public class DropdownColView : RowColumnView {

        [Header("Dropdown Col References")]
        [SerializeField] private TMP_Dropdown _dropdown;

        #region Mono
        private void OnEnable() {
            _dropdown.onValueChanged.AddListener(DropdownSelected);
        }

        private void OnDisable() {
            _dropdown.onValueChanged.RemoveAllListeners();
        }
        #endregion

        #region Event Listeners methods
        private void DropdownSelected(int selectionIndex) {
            ThrowColumnValueSetted(_dropdown);
        }
        #endregion

        public int GetValue() {
            return _dropdown.value;
        }

        public void SetDropdown(int dropdownIndex, bool withoutNotify = false) {
            if (!withoutNotify) {
                _dropdown.value = dropdownIndex;
            } else {
                _dropdown.SetValueWithoutNotify(dropdownIndex);
            }
        }
    }
}
