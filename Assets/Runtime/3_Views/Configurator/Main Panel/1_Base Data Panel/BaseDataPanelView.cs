// Dependencies
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Localization.Settings;
// Custom dependencies
using static YannickSCF.GeneralApp.CommonEventsDelegates;

namespace YannickSCF.LSTournaments.Common.Views.MainPanel.BaseDataPanel {
    public class BaseDataPanelView : MonoBehaviour {

        public event IntegerEventDelegate TypeChanged;
        public event StringEventDelegate NameChanged;
        public event IntegerEventDelegate FormulaChanged;

        [SerializeField] private TMP_Dropdown _type;
        [SerializeField] private TMP_InputField _name;
        [SerializeField] private TMP_Dropdown _formula;
        [SerializeField] private TextMeshProUGUI _formulaDescription;

        #region Mono
        private void Awake() {
            _type.ClearOptions();

            List<string> typeOptions = new List<string>();
            Array types = Enum.GetValues(typeof(TournamentType));
            foreach (Enum type in types) {
                string localized = LocalizationSettings.StringDatabase.GetLocalizedString("Common Enums", nameof(TournamentType) + "." + type.ToString());
                typeOptions.Add(localized);
            }
            _type.AddOptions(typeOptions);
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

        public void FillFormulaDropdown(List<string> formulaNames) {
            _formula.ClearOptions();

            List<string> formulaOptions = new List<string>();
            foreach (string formulaName in formulaNames) {
                string localized = LocalizationSettings.StringDatabase.GetLocalizedString("Configurator Texts", "FormulaName_" + formulaName);
                formulaOptions.Add(localized);
            }
            _formula.AddOptions(formulaOptions);
        }

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
    }
}
