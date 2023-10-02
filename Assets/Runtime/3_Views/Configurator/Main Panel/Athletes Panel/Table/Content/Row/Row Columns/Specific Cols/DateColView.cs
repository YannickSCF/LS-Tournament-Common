// Dependencies
using System;
using System.Globalization;
using TMPro;
using UnityEngine;

namespace YannickSCF.LSTournaments.Common.Views.MainPanel.AthletesPanel.Table.Content.Row.RowColumns.SpecificCols {
    public class DateColView : RowColumnView {

        private const string DATE_FORMAT = "dd/MM/yyyy";
        private const string DATE_SEPARATOR = "/";

        [Header("Color Col References")]
        [SerializeField] private TMP_InputField _dayInputField;
        [SerializeField] private TMP_InputField _monthInputField;
        [SerializeField] private TMP_InputField _yearInputField;

        #region Mono
        private void OnEnable() {
            _dayInputField.onValueChanged.AddListener(ValidateDayDateText);
            _dayInputField.onEndEdit.AddListener(OnEndDayEdit);

            _monthInputField.onValueChanged.AddListener(ValidateMonthDateText);
            _monthInputField.onEndEdit.AddListener(OnEndMonthEdit);

            _yearInputField.onValueChanged.AddListener(ValidateYearDateText);
            _yearInputField.onEndEdit.AddListener(OnEndYearEdit);
        }

        private void OnDisable() {
            _dayInputField.onValueChanged.RemoveAllListeners();
            _dayInputField.onEndEdit.RemoveAllListeners();

            _monthInputField.onValueChanged.RemoveAllListeners();
            _monthInputField.onEndEdit.RemoveAllListeners();

            _yearInputField.onValueChanged.RemoveAllListeners();
            _yearInputField.onEndEdit.RemoveAllListeners();
        }
        #endregion

        #region Events listeners methods
        private void ValidateDayDateText(string inputText) {
            if (string.IsNullOrEmpty(inputText)) return;

            int day = int.Parse(inputText);

            if (day < 0) { day = 0; }
            if (day > 31) { day = 31; }

            if (IsDateFilled()) {
                int month = int.Parse(_monthInputField.text);
                int year = int.Parse(_yearInputField.text);

                if (!IsDateValid(day, month, year)) {
                    _dayInputField.SetTextWithoutNotify(string.Empty);
                    return;
                }
            }

            _dayInputField.SetTextWithoutNotify(day.ToString());
            if (day > 9 || (day > 0 && day <= 9 && inputText.StartsWith("0"))) {
                _dayInputField.DeactivateInputField();
            }
        }

        private void OnEndDayEdit(string inputText) {
            if (string.IsNullOrEmpty(inputText)) return;

            int day = int.Parse(inputText);
            if (day == 0) day = 1;
            _dayInputField.SetTextWithoutNotify(GetTwoDigitNumber(day));

            if (IsDateFilled()) {
                ThrowColumnValueSetted(GetDate(), _dayInputField);
            } else {
                _monthInputField.Select();
            }
        }

        private void ValidateMonthDateText(string inputText) {
            if (string.IsNullOrEmpty(inputText)) return;

            int month = int.Parse(inputText);

            if (month < 0) { month = 0; }
            if (month > 12) { month = 12; }

            if (IsDateFilled()) {
                int day = int.Parse(_dayInputField.text);
                int year = int.Parse(_yearInputField.text);

                if (month > 0 && !IsDateValid(day, month, year)) {
                    _monthInputField.SetTextWithoutNotify(string.Empty);
                    return;
                }
            }

            _monthInputField.SetTextWithoutNotify(month.ToString());
            if (month > 9 || (month > 0 && month <= 9 && inputText.StartsWith("0"))) {
                _monthInputField.DeactivateInputField();
            }
        }

        private void OnEndMonthEdit(string inputText) {
            if (string.IsNullOrEmpty(inputText)) return;

            int month = int.Parse(inputText);
            if (month == 0) month = 1;
            _monthInputField.SetTextWithoutNotify(GetTwoDigitNumber(month));

            if (IsDateFilled()) {
                ThrowColumnValueSetted(GetDate(), _monthInputField);
            } else {
                _yearInputField.Select();
            }
        }

        private void ValidateYearDateText(string inputText) {
            if (string.IsNullOrEmpty(inputText)) return;

            int year = int.Parse(inputText);
            DateTime dateNow = DateTime.Now;

            if (year < 0) { year = 0; }
            if (year > dateNow.Year) { year = dateNow.Year; }

            if (IsDateFilled()) {
                int day = int.Parse(_dayInputField.text);
                int month = int.Parse(_monthInputField.text);

                if (year > 999 && !IsDateValid(day, month, year)) {
                    _yearInputField.SetTextWithoutNotify(string.Empty);
                    return;
                }
            }

            _yearInputField.SetTextWithoutNotify(year.ToString());

            if (year > 999) {
                _yearInputField.DeactivateInputField();
            }
        }

        private void OnEndYearEdit(string inputText) {
            if (string.IsNullOrEmpty(inputText)) return;

            int year = int.Parse(inputText);
            _yearInputField.SetTextWithoutNotify(year.ToString());

            if (IsDateFilled()) {
                ThrowColumnValueSetted(GetDate(), _yearInputField);
            } else {
                _dayInputField.Select();
            }
        }
        #endregion

        public DateTime GetDate() {
            return DateTime.ParseExact(GetDateString(), DATE_FORMAT, CultureInfo.InvariantCulture);
        }

        public string GetDateString() {
            return _dayInputField.text + DATE_SEPARATOR + _monthInputField.text + DATE_SEPARATOR + _yearInputField.text;
        }

        public void SetDate(DateTime date, bool withoutNotify = false) {
            string[] dateSeparated = date.ToString(DATE_FORMAT, CultureInfo.InvariantCulture).Split(DATE_SEPARATOR);
            if (dateSeparated.Length != 3) {
                Debug.LogWarning("Date given has a bad format");
                return;
            }

            if (!withoutNotify) {
                _dayInputField.text = dateSeparated[0];
                _monthInputField.text = dateSeparated[1];
                _yearInputField.text = dateSeparated[2];
            } else {
                _dayInputField.SetTextWithoutNotify(dateSeparated[0]);
                _monthInputField.SetTextWithoutNotify(dateSeparated[1]);
                _yearInputField.SetTextWithoutNotify(dateSeparated[2]);
            }
        }

        private bool IsDateFilled() {
            return !string.IsNullOrEmpty(_dayInputField.text)
                && !string.IsNullOrEmpty(_monthInputField.text)
                && !string.IsNullOrEmpty(_yearInputField.text);
        }

        private bool IsDateValid(int day, int month, int year) {
            return day <= DateTime.DaysInMonth(year, month);
        }

        private string GetTwoDigitNumber(int number) {
            string numberString = string.Empty;
            if (number <= 9) {
                numberString = "0";
            }

            numberString += number.ToString();

            return numberString;
        }
    }
}
