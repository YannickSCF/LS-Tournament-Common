// Dependencies
using System;
using System.Globalization;
using TMPro;
using UnityEngine;
// Custom dependencies
using static YannickSCF.GeneralApp.CommonEventsDelegates;

namespace YannickSCF.LSTournaments.Common.Views.MainPanel.AthletesPanel.Table.Content.Row.RowColumns.SpecificCols {
    public class DateColView : RowColumnView {

        public event StringEventDelegate OnFinalValueSetted;

        [Header("Color Col References")]
        [SerializeField] private TMP_InputField _inputField;

        #region Mono
        private void OnEnable() {
            _inputField.onValueChanged.AddListener(ValidateDateText);
        }

        private void OnDisable() {
            _inputField.onValueChanged.RemoveAllListeners();
        }
        #endregion

        #region Events listeners methods
        private void ValidateDateText(string inputText) {
            // TODO Create validations

            OnFinalValueSetted?.Invoke(inputText);
        }
        #endregion

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
