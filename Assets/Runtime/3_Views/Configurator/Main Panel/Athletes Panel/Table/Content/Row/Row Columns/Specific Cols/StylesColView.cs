// Dependencies
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace YannickSCF.LSTournaments.Common.Views.MainPanel.AthletesPanel.Table.Content.Row.RowColumns.SpecificCols {
    public class StylesColView : RowColumnView {

        public delegate void StyleToggleClick(StyleType styleClicked, bool isOn);
        public event StyleToggleClick StyleToggleClicked;

        [Header("Styles Col References")]
        [SerializeField] private List<Toggle> _styleToggles;

        private List<bool> _styleSelections;

        #region Mono
        private void Awake() {
            _styleSelections = new List<bool>();

            if (_styleToggles.Count > 0) {
                _styleSelections.Add(true);
                _styleToggles[0].SetIsOnWithoutNotify(true);
                _styleToggles[0].interactable = false;

                if (_styleToggles.Count > 1) {
                    for (int i = 1; i < _styleToggles.Count; ++i) {
                        _styleSelections.Add(false);
                        _styleToggles[i].SetIsOnWithoutNotify(false);
                    }
                }
            } else {
                Debug.LogWarning("There are no Style Toggles added to Styles Field!");
            }
        }

        private void OnEnable() {
            foreach (Toggle styleToggle in _styleToggles) {
                styleToggle.onValueChanged.AddListener(OnStyleToggleClicked);
            }
        }

        private void OnDisable() {
            foreach (Toggle styleToggle in _styleToggles) {
                styleToggle.onValueChanged.RemoveAllListeners();
            }
        }
        #endregion

        #region Event Listeners methods
        private void OnStyleToggleClicked(bool isOn) {
            StyleType styleClicked = StyleType.Form2;
            for (int i = 0; i < _styleToggles.Count; ++i) {
                if (_styleSelections[i] != _styleToggles[i].isOn) {
                    _styleSelections[i] = isOn;
                    styleClicked = (StyleType)i;
                    break;
                }
            }

            StyleToggleClicked?.Invoke(styleClicked, isOn);
        }
        #endregion

        public void SetStyles(List<bool> stylesStatus, bool withoutNotify = false) {
            for (int i = 1; i < stylesStatus.Count; ++i) {
                _styleSelections[i] = stylesStatus[i];
                if (withoutNotify) {
                    _styleToggles[i].SetIsOnWithoutNotify(stylesStatus[i]);
                } else {
                    _styleToggles[i].isOn = stylesStatus[i];
                }
            }
        }

        public void SetStyle(StyleType style, bool isOn, bool withoutNotify = false) {
            _styleSelections[(int)style] = isOn;
            if (withoutNotify) {
                _styleToggles[(int)style].SetIsOnWithoutNotify(isOn);
            } else {
                _styleToggles[(int)style].isOn = isOn;
            }
        }

        public void SetStylesLocked(List<StyleType> styleToLock, bool isLocked) {
            List<int> stylesIndexes = styleToLock.ConvertAll(x => (int)x);
            foreach(int styleIndex in stylesIndexes) {
                _styleToggles[styleIndex].interactable = isLocked;
            }
        }

        public List<bool> GetStyles() {
            return _styleSelections;
        }
    }
}
