/**
 * Author:      Yannick Santa Cruz Feuillias
 * Created:     11/10/2023
 **/

// Dependencies
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.Localization.Settings;
using UnityEngine.UI;
// Custom dependencies
using static YannickSCF.GeneralApp.CommonEventsDelegates;

namespace YannickSCF.LSTournaments.Common.Views.MainPanel.PoulesDataPanel {
    public class PoulesDataPanelView : PanelView {

        public event IntegerEventDelegate NamingTypeChanged;
        public event IntegerEventDelegate PouleRoundChanged;
        public event IntegerEventDelegate HowToDefinePoulesChanged;
        public event IntegerEventDelegate SelectedPouleDataChanged;

        [Header("Poule Naming Fields")]
        [SerializeField] private TMP_Dropdown _pouleNamingType;
        [SerializeField] private CanvasGroup _pouleRoundsCanvasGroup;
        [SerializeField] private TMP_InputField _pouleRoundsInput;
        [SerializeField] private TextMeshProUGUI _pouleNamingExample;

        [Header("Poule Count and Size Fields")]
        [SerializeField] private Transform _pouleCountSizeContent;
        [SerializeField] private TMP_Dropdown _howToDefinePouleAttributes;
        [SerializeField] private TMP_Dropdown _selectedHowToDefineDropdown;
        [SerializeField] private TextMeshProUGUI _numberOfAthletesText;
        [SerializeField] private TextMeshProUGUI _pouleAttributesResultText;
        [SerializeField] private GameObject _pouleAttributesResultAdd;

        [SerializeField] private Image _solutionTextArea;
        [SerializeField] private TextMeshProUGUI _errorCountAndSizeText;

        private Coroutine _incorrectNaming;
        private Coroutine _incorrectCountAndSize;
        private Coroutine _incorrectCountAndSizePanel;

        #region Mono
        private void Awake() {
            _pouleNamingType.ClearOptions();
            _pouleNamingType.AddOptions(LSTournamentEnums.GetEnumsLocalizations<PouleNamingType>());

            _howToDefinePouleAttributes.ClearOptions();
            _howToDefinePouleAttributes.AddOptions(LSTournamentEnums.GetEnumsLocalizations<PoulesBy>());
        }

        private void OnEnable() {
            _pouleNamingType.onValueChanged.AddListener(PouleNamingChanged);
            _pouleRoundsInput.onValueChanged.AddListener(PouleNamingRoundsChanged);

            _howToDefinePouleAttributes.onValueChanged.AddListener(HowToDefinePouleAttributesChanged);
            _selectedHowToDefineDropdown.onValueChanged.AddListener(SelectedHowToDefineChanged);
        }

        private void OnDisable() {
            _pouleNamingType.onValueChanged.RemoveAllListeners();
            _pouleRoundsInput.onValueChanged.RemoveAllListeners();

            _howToDefinePouleAttributes.onValueChanged.RemoveAllListeners();
            _selectedHowToDefineDropdown.onValueChanged.RemoveAllListeners();
        }
        #endregion

        #region Event Listeners methods
        private void PouleNamingChanged(int pouleNamingIndex) {
            NamingTypeChanged?.Invoke(pouleNamingIndex);
        }

        private void PouleNamingRoundsChanged(string pouleRounds) {
            if (string.IsNullOrEmpty(pouleRounds)) {
                _pouleRoundsInput.text = "1";
            } else {
                PouleRoundChanged?.Invoke(int.Parse(pouleRounds));
            }
        }

        private void HowToDefinePouleAttributesChanged(int howToDefinePoulesIndex) {
            HowToDefinePoulesChanged?.Invoke(howToDefinePoulesIndex);
        }

        private void SelectedHowToDefineChanged(int selectedPouleData) {
            if (_selectedHowToDefineDropdown.options[selectedPouleData].text == "-") {
                SelectedPouleDataChanged?.Invoke(-1);
            } else {
                SelectedPouleDataChanged?.Invoke(int.Parse(_selectedHowToDefineDropdown.options[selectedPouleData].text));
            }
        }
        #endregion

        #region (PUBLIC) Methods to set view values or characteristics
        /// <summary>
        /// Method to Set Poule naming type and count of rounds to know how to divide them.
        /// </summary>
        /// <param name="namingTypeIndex">Naming type value to set on dropdown</param>
        /// <param name="pouleRounds">
        /// Optional: Number of poule rounds.
        /// If "namingTypeIndex" is setted to 'PouleNamingType.Letters', it is advisable to use set it.
        /// </param>
        public void SetPouleNamingType(int namingTypeIndex, int pouleRounds = 1) {
            _pouleNamingType.SetValueWithoutNotify(namingTypeIndex);

            if (pouleRounds > 0) {
                _pouleRoundsInput.SetTextWithoutNotify(pouleRounds.ToString());
            }
        }

        public void SetInteractablePouleRounds(bool isInteractable) {
            _pouleRoundsInput.interactable = isInteractable;
            _pouleRoundsCanvasGroup.alpha = isInteractable ? 1f : 0.5f;
        }

        /// <summary>
        /// Method to set poule naming example.
        /// This method just represent string given, it must come formatted.
        /// </summary>
        /// <param name="example">Value to set on example</param>
        public void SetPouleNamingExample(string example) {
            _pouleNamingExample.text = example;
        }

        /// <summary>
        /// Method to show/hide objects to select count or size of poules.
        /// This method can only be used to hide this objects if this size is defined
        /// previously by a formula.
        /// </summary>
        /// <param name="isSelectable">Defines it this method is to show (true) or hide (false)</param>
        public void SetSelectableCountSize(bool isSelectable) {
            for (int i = 1; i < _pouleCountSizeContent.childCount; ++i) {
                _pouleCountSizeContent.GetChild(i).gameObject.SetActive(isSelectable);
            }
        }

        /// <summary>
        /// Set the options of the dropdown to define count and size of poules.
        /// This method includes a first option that represent an TBD value.
        /// </summary>
        /// <param name="options">List of options.</param>
        public void SetPoulesCountSizeDropdownOptions(List<int> options) {
            _selectedHowToDefineDropdown.ClearOptions();

            List<string> optionsString = new List<string>();
            optionsString.Add("-");

            foreach (int option in options) {
                optionsString.Add(option.ToString());
            }

            _selectedHowToDefineDropdown.AddOptions(optionsString);
        }

        public void SetPoulesCountSizeDropdownOptionSelected(int optionValue) {
            TMP_Dropdown.OptionData optionData = _selectedHowToDefineDropdown.options.FirstOrDefault(x => x.text == optionValue.ToString());

            if (optionData != null) {
                int selectedOption = _selectedHowToDefineDropdown.options.IndexOf(optionData);
                _selectedHowToDefineDropdown.SetValueWithoutNotify(selectedOption);
            }
        }

        /// <summary>
        /// Method to set the calculation of poules by parameters given.
        /// </summary>
        /// <param name="totalAthletes">Total count of athletes</param>
        /// <param name="poulesCountAndSizes">
        /// The poules count and sizes.
        /// Defined in array as defined in 'TournamentData.PouleCountAndSizes' property.
        /// </param>
        public void SetPoulesCountSizeAttributes(int totalAthletes, int[,] poulesCountAndSizes) {
            string textToWrite = LocalizationSettings.StringDatabase.GetLocalizedString("Configurator Texts", "PoulesData_Title_CountAndSize_Result");
            
            _numberOfAthletesText.gameObject.SetActive(poulesCountAndSizes != null);
            _pouleAttributesResultText.gameObject.SetActive(poulesCountAndSizes != null);
            _pouleAttributesResultAdd.SetActive(poulesCountAndSizes != null);

            _errorCountAndSizeText.gameObject.SetActive(poulesCountAndSizes == null);

            if (poulesCountAndSizes != null) {
                _numberOfAthletesText.text = totalAthletes + " =";
                _pouleAttributesResultAdd.SetActive(poulesCountAndSizes[1, 0] != 0);

                if (poulesCountAndSizes[1, 0] != 0) {
                    textToWrite = string.Format(textToWrite,
                        poulesCountAndSizes[0, 0].ToString(), poulesCountAndSizes[0, 1].ToString(),
                        poulesCountAndSizes[1, 0].ToString(), poulesCountAndSizes[1, 1].ToString());
                } else {
                    textToWrite = Regex.Replace(textToWrite, @"<br>.+", "");

                    textToWrite = string.Format(textToWrite,
                        poulesCountAndSizes[0, 0].ToString(), poulesCountAndSizes[0, 1].ToString());
                }

                _pouleAttributesResultText.text = textToWrite;
            } else {
                _errorCountAndSizeText.text =
                    string.Format(LocalizationSettings.StringDatabase.GetLocalizedString(
                            "Configurator Texts", "PoulesData_Title_CountAndSize_ErrorSelect"),
                        _howToDefinePouleAttributes.options[_howToDefinePouleAttributes.value].text);
            }
        }

        /// <summary>
        /// Method to set the calculation of poules by parameters given.
        /// If it cannot have any combination, this manages the error and message too.
        /// </summary>
        /// <param name="formula">Formula name to show a guide on error (if it appears)</param>
        /// <param name="totalAthletes">Total count of athletes</param>
        /// <param name="poulesCountAndSizes">
        /// The poules count and sizes.
        /// Defined in array as defined in 'TournamentData.PouleCountAndSizes' property.
        /// </param>
        public void SetPoulesCountSizeAttributes(string formula, int totalAthletes, int[,] poulesCountAndSizes) {
            SetPoulesCountSizeAttributes(totalAthletes, poulesCountAndSizes);

            // If 'poulesCountAndSizes' is null, means that there is no possible combinations.
            if (poulesCountAndSizes == null) {
                string baseString = LocalizationSettings.StringDatabase.GetLocalizedString(
                        "Configurator Texts", "PoulesData_Title_CountAndSize_ErrorSize");
                _errorCountAndSizeText.text = string.Format(
                    baseString,
                    LocalizationSettings.StringDatabase.GetLocalizedString("Configurator Texts", "FormulaName_" + formula),
                    totalAthletes.ToString());
            }
        }
        #endregion


        /// <summary>
        /// Method to Show/Hide validation error on Naming fields.
        /// </summary>
        /// <param name="show">
        /// If this value is 'true' force to show (or reset) the validation animation.
        /// If it is 'false', it cancels the animation.
        /// </param>
        public void ShowNamingNotValidated(bool show) {
            if (_incorrectNaming != null) {
                StopCoroutine(_incorrectNaming);
            }

            if (show) {
                _incorrectNaming = StartCoroutine(ShowAndHideSelectableErrorCoroutine(_pouleRoundsInput));
            } else {
                ResetSelectableError(_pouleRoundsInput);
            }
        }

        /// <summary>
        /// Method to Show/Hide validation error on Poules count and size fields.
        /// </summary>
        /// <param name="show">
        /// If this value is 'true' force to show (or reset) the validation animation.
        /// If it is 'false', it cancels the animation.
        /// </param>
        public void ShowCountAndSizeNotValidated(bool show) {
            if (_incorrectCountAndSize != null) {
                StopCoroutine(_incorrectCountAndSize);
            }
            if (_incorrectCountAndSizePanel != null) {
                StopCoroutine(_incorrectCountAndSizePanel);
            }

            if (show) {
                _incorrectCountAndSize =
                    StartCoroutine(ShowAndHideSelectableErrorCoroutine(_selectedHowToDefineDropdown));
                _incorrectCountAndSizePanel =
                    StartCoroutine(ShowAndHideImageErrorCoroutine(_solutionTextArea));
            } else {
                ResetSelectableError(_selectedHowToDefineDropdown);
                ResetImageError(_solutionTextArea);
            }
        }
    }
}
