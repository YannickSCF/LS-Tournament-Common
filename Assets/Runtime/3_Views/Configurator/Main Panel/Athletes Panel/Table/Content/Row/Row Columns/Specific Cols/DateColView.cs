// Dependencies
using System;
using System.Globalization;
using TMPro;
using UnityEngine;

namespace YannickSCF.LSTournaments.Common.Views.MainPanel.AthletesPanel.Table.Content.Row.RowColumns.SpecificCols {
    public class DateColView : RowColumnView {
        [Header("Color Col References")]
        [SerializeField] private TMP_InputField _inputField;
        // TODO Create validations in controller
        public TMP_InputField.OnChangeEvent OnValueChanged() {
            return _inputField.onValueChanged;
        }

        public DateTime GetDate() {
            return DateTime.ParseExact(_inputField.text, "dd/MM/yyyy", CultureInfo.InvariantCulture); ;
        }

        public void SetDate(DateTime date, bool withoutNotify = false) {
            if (!withoutNotify) {
                _inputField.text = date.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
            } else {
                _inputField.SetTextWithoutNotify(date.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture));
            }
        }
    }
}
