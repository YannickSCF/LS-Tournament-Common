/**
 * Author:      Yannick Santa Cruz Feuillias
 * Created:     28/09/2023
 **/

// Dependencies
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace YannickSCF.LSTournaments.Common.Views.MainPanel.AthletesPanel.Table.Content.Row.RowColumns.SpecificCols {
    public class StylesColView : RowColumnView {

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
            ThrowColumnValueSetted(GetStyles());
        }
        #endregion

        #region Protected overrrided methods
        protected override void SetSelectablesInteractables(bool isInteractable) {
            for (int i = 1; i < _styleToggles.Count; ++i) {
                _styleToggles[i].interactable = isInteractable;
            }
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

        public List<StyleType> GetStyles() {
            List<StyleType> styles = new List<StyleType>();
            for (int i = 0; i < _styleSelections.Count; ++i) {
                if (_styleSelections[i]) {
                    styles.Add((StyleType)i);
                }
            }
            return styles;
        }
    }
}
