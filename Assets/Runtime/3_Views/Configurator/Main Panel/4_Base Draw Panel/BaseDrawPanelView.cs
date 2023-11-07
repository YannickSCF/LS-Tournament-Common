/**
 * Author:      Yannick Santa Cruz Feuillias
 * Created:     12/10/2023
 **/

// Dependencies
using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
// Custom Dependencies
using YannickSCF.LSTournaments.Common.Views.MainPanel.BaseDrawPanel.ExamplePoules;
using static YannickSCF.GeneralApp.CommonEventsDelegates;

namespace YannickSCF.LSTournaments.Common.Views.MainPanel.BaseDrawPanel {
    public class BaseDrawPanelView : PanelView {

        public event IntegerEventDelegate FillerTypeChanged;
        public event IntegerEventDelegate FillerSubtypeChanged;

        [SerializeField] private CanvasGroup _fillerTypeCanvasGroup;
        [SerializeField] private TMP_Dropdown _fillerTypeDropdown;
        [SerializeField] private GameObject _fillerSubtypeGameObject;
        [SerializeField] private TMP_Dropdown _fillerSubtypeDropdown;
        [Header("Example references")]
        [SerializeField] private TextMeshProUGUI _exampleCompleteText;
        [SerializeField] private ScrollRect _examplePoulesScroll;
        [SerializeField] private ExamplePoulesView _examplePoulePrefab;

        private List<ExamplePoulesView> _examplePoules;

        private List<PouleFillerType> _addedTypes;
        private List<PouleFillerSubtype> _addedSubtypes;

        private Coroutine _incorrectFillerType;

        #region Mono
        private void Awake() {
            _examplePoules = new List<ExamplePoulesView>();

            _addedTypes = Enum.GetValues(typeof(PouleFillerType)).Cast<PouleFillerType>().ToList();
            _fillerTypeDropdown.ClearOptions();
            _fillerTypeDropdown.AddOptions(LSTournamentEnums.GetEnumsLocalizations<PouleFillerType>());

            _addedSubtypes = Enum.GetValues(typeof(PouleFillerSubtype)).Cast<PouleFillerSubtype>().ToList();
            _fillerSubtypeDropdown.ClearOptions();
            _fillerSubtypeDropdown.AddOptions(LSTournamentEnums.GetEnumsLocalizations<PouleFillerSubtype>());
        }

        private void OnEnable() {
            _fillerTypeDropdown.onValueChanged.AddListener(FillerTypeDropdownChanged);
            _fillerSubtypeDropdown.onValueChanged.AddListener(FillerSubtypeDropdownChanged);
        }

        private void OnDisable() {
            _fillerTypeDropdown.onValueChanged.RemoveAllListeners();
            _fillerSubtypeDropdown.onValueChanged.RemoveAllListeners();
        }
        #endregion

        #region Event Listeners methods
        private void FillerTypeDropdownChanged(int newFillerType) {
            FillerTypeChanged?.Invoke(newFillerType);
        }

        private void FillerSubtypeDropdownChanged(int newFillerSubtype) {
            PouleFillerSubtype subtype = _addedSubtypes[newFillerSubtype];
            FillerSubtypeChanged?.Invoke((int)subtype);
        }
        #endregion

        #region (PUBLIC) Methods to set view values
        /// <summary>
        /// Method to set filler type on view.
        /// This sets interactability too (Optional).
        /// </summary>
        /// <param name="fillerType">New filler type to set.</param>
        /// <param name="isInteractable">Optional: set interactability of this field.</param>
        public void SetFillerType(PouleFillerType fillerType, bool isInteractable = false) {
            _fillerTypeDropdown.SetValueWithoutNotify((int)fillerType);
            _fillerTypeDropdown.interactable = isInteractable;

            _fillerTypeCanvasGroup.alpha = isInteractable ? 1f : 0.5f;
        }

        public void SetFillerSubtype(PouleFillerSubtype fillerSubtype) {
            int subtypeIndex = _addedSubtypes.IndexOf(fillerSubtype);
            _fillerSubtypeDropdown.SetValueWithoutNotify(subtypeIndex);
        }

        /// <summary>
        /// Method to set a complete panel text on example panel.
        /// Usually to set an error o advice message.
        /// </summary>
        /// <param name="completePanelText">Message to show on complete example panel.</param>
        public void SetExample(string completePanelText) {
            CleanExample();

            _examplePoulesScroll.gameObject.SetActive(false);

            _exampleCompleteText.gameObject.SetActive(true);
            _exampleCompleteText.text = completePanelText;
        }

        /// <summary>
        /// Method to create an example with data given.
        /// This method creates all objects needed to represent the poules
        /// as they come in 'poulesExamples' dictionary data.
        /// </summary>
        /// <param name="poulesExamples">
        /// Dictionary with data to show. This data is represented as a list (dictionary value) of
        /// names, countries, number of styles, etc,... for each poule (dictionary key).
        /// </param>
        public void SetExample(Dictionary<string, List<string>> poulesExamples) {
            _examplePoulesScroll.gameObject.SetActive(true);
            _exampleCompleteText.gameObject.SetActive(false);

            if (poulesExamples.Count != _examplePoulesScroll.content.childCount) {
                CleanExample();

                foreach (KeyValuePair<string, List<string>> pouleExample in poulesExamples) {
                    ExamplePoulesView examplePoule = Instantiate(_examplePoulePrefab, _examplePoulesScroll.content);
                    examplePoule.SetPouleContent(pouleExample.Key, pouleExample.Value);
                    _examplePoules.Add(examplePoule);
                }
            } else {
                int exampleCount = 0;
                foreach (KeyValuePair<string, List<string>> pouleExample in poulesExamples) {
                    _examplePoules[exampleCount].SetPouleContent(pouleExample.Key, pouleExample.Value);
                    ++exampleCount;
                }
            }
        }

        private void CleanExample() {
            foreach (Transform child in _examplePoulesScroll.content) {
                DestroyImmediate(child.gameObject);
            }
            _examplePoules.Clear();
        }
        #endregion

        /// <summary>
        /// Method to modify options of Filler types dropdown field.
        /// The entry parameter indicates which enums remove from list of 'PouleFillerType'.
        /// </summary>
        /// <param name="typesToRemove">Enums from 'PouleFillerType' list to remove</param>
        public void RemoveSelectableTypes(List<PouleFillerType> typesToRemove) {
            if (typesToRemove == null || typesToRemove.Count <= 0) return;

            _fillerTypeDropdown.ClearOptions();
            _addedTypes.Clear();

            List<PouleFillerType> allTypes = Enum.GetValues(typeof(PouleFillerType)).Cast<PouleFillerType>().ToList();
            foreach (PouleFillerType type in allTypes) {
                if (!typesToRemove.Contains(type)) {
                    _addedTypes.Add(type);
                }
            }

            _fillerTypeDropdown.AddOptions(LSTournamentEnums.GetEnumsLocalizations(_addedTypes));
        }

        /// <summary>
        /// Method to modify options of Filler subtypes dropdown field.
        /// The entry parameter indicates which enums remove from list of 'PouleFillerSubtype'.
        /// </summary>
        /// <param name="subtypesToRemove">Enums from 'PouleFillerSubtype' list to remove</param>
        public void RemoveSelectableSubtypes(List<PouleFillerSubtype> subtypesToRemove) {
            if (subtypesToRemove == null || subtypesToRemove.Count <= 0) return;

            _fillerSubtypeDropdown.ClearOptions();
            _addedSubtypes.Clear();

            List<PouleFillerSubtype> allSubtypes = Enum.GetValues(typeof(PouleFillerSubtype)).Cast<PouleFillerSubtype>().ToList();
            foreach (PouleFillerSubtype subtype in allSubtypes) {
                if (!subtypesToRemove.Contains(subtype)) {
                    _addedSubtypes.Add(subtype);
                }
            }

            List<string> optionsToAdd = LSTournamentEnums.GetEnumsLocalizations(_addedSubtypes);

            _fillerSubtypeDropdown.AddOptions(optionsToAdd);
            _fillerSubtypeGameObject.SetActive(optionsToAdd.Count != 1);
        }


        /// <summary>
        /// Method to Show/Hide validation error on Poule Filler type field.
        /// </summary>
        /// <param name="show">
        /// If this value is 'true' force to show (or reset) the validation animation.
        /// If it is 'false', it cancels the animation.
        /// </param>
        public void ShowFillerTypeNotValidated(bool show) {
            if (_incorrectFillerType != null) {
                StopCoroutine(_incorrectFillerType);
            }

            if (show) {
                _incorrectFillerType = StartCoroutine(ShowAndHideSelectableErrorCoroutine(_fillerTypeDropdown));
            } else {
                ResetSelectableError(_fillerTypeDropdown);
            }
        }
    }
}
