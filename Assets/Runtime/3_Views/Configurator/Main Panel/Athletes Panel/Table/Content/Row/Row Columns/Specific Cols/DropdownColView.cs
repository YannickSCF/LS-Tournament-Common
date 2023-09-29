// Dependencies
using TMPro;
using UnityEngine;

namespace YannickSCF.LSTournaments.Common.Views.MainPanel.AthletesPanel.Table.Content.Row.RowColumns.SpecificCols {
    public class DropdownColView : RowColumnView {

        [Header("Dropdown Col References")]
        [SerializeField] private TMP_Dropdown _dropdown;

        public TMP_Dropdown Dropdown { get => _dropdown; }
    }
}
