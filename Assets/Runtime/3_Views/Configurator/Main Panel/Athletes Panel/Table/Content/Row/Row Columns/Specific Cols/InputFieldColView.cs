// Dependencies
using TMPro;
using UnityEngine;

namespace YannickSCF.LSTournaments.Common.Views.MainPanel.AthletesPanel.Table.Content.Row.RowColumns.SpecificCols {
    public class InputFieldColView : RowColumnView {

        [Header("Input Field Col References")]
        [SerializeField] private TMP_InputField _inputField;

        public TMP_InputField InputField { get => _inputField; }
    }
}
