// Dependencies
using System.Collections;
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

        public void ShowTournamentNameNotValidated(bool show) {
            if (_incorrectTournamentName != null) {
                StopCoroutine(_incorrectTournamentName);
            }

            if (show) {
                _incorrectTournamentName = StartCoroutine(ShowAndHideIncorrectTournamentNameCoroutine());
            } else {
                _name.targetGraphic.color = Color.white;
            }
        }

        private IEnumerator ShowAndHideIncorrectTournamentNameCoroutine() {
            _name.targetGraphic.color = Color.red;

            yield return new WaitForSeconds(1f);

            float timeLeft = 2f;
            while (timeLeft > 0f) {
                _name.targetGraphic.color = Color.Lerp(Color.red, Color.white, (2f - timeLeft) / 2f);

                yield return new WaitForEndOfFrame();
                timeLeft -= Time.deltaTime;
            }
        }
    }
}
