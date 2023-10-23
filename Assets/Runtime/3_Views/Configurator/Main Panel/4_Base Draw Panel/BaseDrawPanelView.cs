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
    public class BaseDrawPanelView : MonoBehaviour {

        public event IntegerEventDelegate FillerTypeChanged;
        public event IntegerEventDelegate FillerSubtypeChanged;

        [SerializeField] private TMP_Dropdown _fillerTypeDropdown;
        [SerializeField] private TMP_Dropdown _fillerSubtypeDropdown;
        [Header("Example references")]
        [SerializeField] private TextMeshProUGUI _exampleCompleteText;
        [SerializeField] private ScrollRect _examplePoulesScroll;
        [SerializeField] private ExamplePoulesView _examplePoulePrefab;

        private List<ExamplePoulesView> _examplePoules;

        private List<PouleFillerType> _addedTypes;
        private List<PouleFillerSubtype> _addedSubtypes;

        #region Mono
        private void Awake() {
            _examplePoules = new List<ExamplePoulesView>();

            _addedTypes = Enum.GetValues(typeof(PouleFillerType)).Cast<PouleFillerType>().ToList();
            _fillerTypeDropdown.ClearOptions();
            _fillerTypeDropdown.AddOptions(new List<string>(Enum.GetNames(typeof(PouleFillerType))));

            _addedSubtypes = Enum.GetValues(typeof(PouleFillerSubtype)).Cast<PouleFillerSubtype>().ToList();
            _fillerSubtypeDropdown.ClearOptions();
            _fillerSubtypeDropdown.AddOptions(new List<string>(Enum.GetNames(typeof(PouleFillerSubtype))));
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

        public void RemoveSelectableTypes(List<PouleFillerType> typesToRemove) {
            if (typesToRemove == null || typesToRemove.Count <= 0) return;

            _fillerTypeDropdown.ClearOptions();
            _addedTypes.Clear();

            List<string> optionsToAdd = new List<string>();
            List<PouleFillerType> allTypes = Enum.GetValues(typeof(PouleFillerType)).Cast<PouleFillerType>().ToList();
            foreach (PouleFillerType type in allTypes) {
                if (!typesToRemove.Contains(type)) {
                    _addedTypes.Add(type);
                    optionsToAdd.Add(Enum.GetName(typeof(PouleFillerType), type));
                }
            }

            _fillerTypeDropdown.AddOptions(optionsToAdd);
        }

        public void RemoveSelectableSubtypes(List<PouleFillerSubtype> subtypesToRemove) {
            if (subtypesToRemove == null || subtypesToRemove.Count <= 0) return;

            _fillerSubtypeDropdown.ClearOptions();
            _addedSubtypes.Clear();

            List<string> optionsToAdd = new List<string>();
            List<PouleFillerSubtype> allSubtypes = Enum.GetValues(typeof(PouleFillerSubtype)).Cast<PouleFillerSubtype>().ToList();
            foreach (PouleFillerSubtype subtype in allSubtypes) {
                if (!subtypesToRemove.Contains(subtype)) {
                    _addedSubtypes.Add(subtype);
                    optionsToAdd.Add(Enum.GetName(typeof(PouleFillerSubtype), subtype));
                }
            }

            _fillerSubtypeDropdown.AddOptions(optionsToAdd);
        }

        public void SetFillerType(PouleFillerType fillerType, bool isInteractable = false) {
            _fillerTypeDropdown.SetValueWithoutNotify((int)fillerType);
            _fillerTypeDropdown.interactable = isInteractable;
        }

        public void SetFillerSubtype(PouleFillerSubtype fillerSubtype) {
            int subtypeIndex = _addedSubtypes.IndexOf(fillerSubtype);
            _fillerSubtypeDropdown.SetValueWithoutNotify(subtypeIndex);
        }

        public void SetExample(string completePanelText) {
            foreach (Transform child in _examplePoulesScroll.content) {
                DestroyImmediate(child.gameObject);
            }

            _examplePoulesScroll.gameObject.SetActive(false);

            _exampleCompleteText.gameObject.SetActive(true);
            _exampleCompleteText.text = completePanelText;
        }

        public void SetExample(Dictionary<string, List<string>> poulesExamples) {
            _examplePoulesScroll.gameObject.SetActive(true);
            _exampleCompleteText.gameObject.SetActive(false);

            if (poulesExamples.Count != _examplePoulesScroll.content.childCount) {
                foreach (Transform child in _examplePoulesScroll.content) {
                    DestroyImmediate(child.gameObject);
                }
                _examplePoules.Clear();

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
    }
}
