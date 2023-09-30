// Dependencies
using TMPro;
using UnityEngine;

namespace YannickSCF.LSTournaments.Common.Views.MainPanel.AthletesPanel.Table.Content.Row.RowColumns.SpecificCols {
    public class DropdownColView : RowColumnView {

        [Header("Dropdown Col References")]
        [SerializeField] private TMP_Dropdown _dropdown;

        public TMP_Dropdown.DropdownEvent OnValueChanged() {
            return _dropdown.onValueChanged;
        }

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
