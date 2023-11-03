// Dependencies
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.Localization.Settings;
// Custom dependencies
using static YannickSCF.GeneralApp.CommonEventsDelegates;

namespace YannickSCF.LSTournaments.Common.Views.MainPanel.PoulesDataPanel {
    public class PoulesDataPanelView : MonoBehaviour {

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

        #region Mono
        private void Awake() {
            _pouleNamingType.ClearOptions();
            _pouleNamingType.AddOptions(new List<string>(Enum.GetNames(typeof(PouleNamingType))));

            _howToDefinePouleAttributes.ClearOptions();
            _howToDefinePouleAttributes.AddOptions(new List<string>(Enum.GetNames(typeof(PoulesBy))));
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
            PouleRoundChanged?.Invoke(int.Parse(pouleRounds));
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

        public void SetPouleNamingType(int namingTypeIndex, int pouleRounds = 0) {
            _pouleNamingType.SetValueWithoutNotify(namingTypeIndex);

            SetInteractablePouleRounds(pouleRounds > 0);
            if (pouleRounds > 0) {
                _pouleRoundsInput.SetTextWithoutNotify(pouleRounds.ToString());
            }
        }

        public void SetInteractablePouleRounds(bool isInteractable) {
            _pouleRoundsInput.interactable = isInteractable;
            _pouleRoundsCanvasGroup.alpha = isInteractable ? 1f : 0.5f;
        }

        public void SetPouleNamingExample(string example) {
            _pouleNamingExample.text = example;
        }

        public void SetSelectableCountSize(bool isSelectable) {
            for (int i = 1; i < _pouleCountSizeContent.childCount; ++i) {
                _pouleCountSizeContent.GetChild(i).gameObject.SetActive(isSelectable);
            }
        }

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

        public void SetPoulesCountSizeAttributes(int totalAthletes, int[,] poulesCountAndSizes) {
            string textToWrite = LocalizationSettings.StringDatabase.GetLocalizedString("Configurator Texts", "PoulesData_Title_CountAndSize_Result");

            _numberOfAthletesText.gameObject.SetActive(poulesCountAndSizes != null);
            _pouleAttributesResultText.gameObject.SetActive(poulesCountAndSizes != null);
            _pouleAttributesResultAdd.SetActive(poulesCountAndSizes != null);

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
            }
        }
    }
}
