/**
 * Author:      Yannick Santa Cruz Feuillias
 * Created:     06/10/2023
 **/

// Dependencies
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Localization.Settings;
// Custom dependencies
using static YannickSCF.GeneralApp.CommonEventsDelegates;

namespace YannickSCF.LSTournaments.Common.Views.MainPanel.BaseDataPanel {
    public class BaseDataPanelView : PanelView {

        public event IntegerEventDelegate TypeChanged;
        public event StringEventDelegate NameChanged;
        public event IntegerEventDelegate FormulaChanged;

        [SerializeField] private TMP_Dropdown _type;
        [SerializeField] private TMP_InputField _name;
        [SerializeField] private TMP_Dropdown _formula;
        [SerializeField] private TextMeshProUGUI _formulaDescription;

        private Coroutine _incorrectTournamentName;

        #region Mono
        private void Awake() {
            _type.ClearOptions();
            _type.AddOptions(LSTournamentEnums.GetEnumsLocalizations<TournamentType>());
        }

        private void OnEnable() {
            _type.onValueChanged.AddListener(OnTypeChanged);
            _formula.onValueChanged.AddListener(OnFormulaChanged);

            _name.onEndEdit.AddListener(OnNameChanged);
        }

        private void OnDisable() {
            _type.onValueChanged.RemoveAllListeners();
            _formula.onValueChanged.RemoveAllListeners();

            _name.onEndEdit.RemoveAllListeners();
        }
        #endregion

        #region Events Listeners methods
        private void OnTypeChanged(int newTypeIndex) {
            TypeChanged?.Invoke(newTypeIndex);
        }

        private void OnNameChanged(string newName) {
            NameChanged?.Invoke(newName);
        }

        private void OnFormulaChanged(int newFormulaIndex) {
            FormulaChanged?.Invoke(newFormulaIndex);
        }
        #endregion

        #region (PUBLIC) Methods to set view values
        public void SetTournamentType(int tournamentType, bool withoutNotify = false) {
            if (withoutNotify) {
                _type.SetValueWithoutNotify(tournamentType);
            } else {
                _type.value = tournamentType;
            }
        }

        public void SetTournamentName(string newName, bool withoutNotify = false) {
            if (withoutNotify) {
                _name.SetTextWithoutNotify(newName);
            } else {
                _name.text = newName;
            }
        }

        public void SetTournamentFormula(int tournamentFormula, bool withoutNotify = false) {
            if (withoutNotify) {
                _formula.SetValueWithoutNotify(tournamentFormula);
            } else {
                _formula.value = tournamentFormula;
            }
        }

        public void SetFormulaDescription(string text) {
            _formulaDescription.text = text;
        }
        #endregion

        /// <summary>
        /// Method to modify options of Formulas dropdown field.
        /// </summary>
        /// <param name="formulaNames">List of formulas names to show in view.</param>
        public void FillFormulaDropdown(List<string> formulaNames) {
            _formula.ClearOptions();

            List<string> formulaOptions = new List<string>();
            foreach (string formulaName in formulaNames) {
                string localized = LocalizationSettings.StringDatabase.GetLocalizedString(
                    "Configurator Texts", "FormulaName_" + formulaName);
                formulaOptions.Add(localized);
            }

            _formula.AddOptions(formulaOptions);
        }

        /// <summary>
        /// Method to Show/Hide validation error on Tournament Name field.
        /// </summary>
        /// <param name="show">
        /// If this value is 'true' force to show (or reset) the validation animation.
        /// If it is 'false', it cancels the animation.
        /// </param>
        public void ShowTournamentNameNotValidated(bool show) {
            if (_incorrectTournamentName != null) {
                StopCoroutine(_incorrectTournamentName);
            }

            if (show) {
                _incorrectTournamentName = StartCoroutine(ShowAndHideSelectableErrorCoroutine(_name));
            } else {
                ResetSelectableError(_name);
            }
        }
    }
}
