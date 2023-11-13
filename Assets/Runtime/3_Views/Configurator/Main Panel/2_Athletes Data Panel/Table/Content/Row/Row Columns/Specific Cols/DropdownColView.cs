/**
 * Author:      Yannick Santa Cruz Feuillias
 * Created:     29/09/2023
 **/

// Dependencies
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace YannickSCF.LSTournaments.Common.Views.MainPanel.AthletesDataPanel.Table.Content.Row.RowColumns.SpecificCols {
    public class DropdownColView : RowColumnView {

        [Header("Dropdown Col References")]
        [SerializeField] private TMP_Dropdown _dropdown;

        #region Mono
        private void OnEnable() {
            _dropdown.onValueChanged.AddListener(DropdownSelected);
        }

        private void OnDisable() {
            _dropdown.onValueChanged.RemoveAllListeners();
        }

        private void Update() {
            if (EventSystem.current.currentSelectedGameObject == _dropdown.gameObject) {
                if (Input.GetKeyDown(KeyCode.DownArrow)) {
                    if (_dropdown.value + 1 >= _dropdown.options.Count) {
                        _dropdown.SetValueWithoutNotify(0);
                    } else {
                        _dropdown.SetValueWithoutNotify(_dropdown.value + 1);
                    }
                    ThrowColumnValueSetted((RankType)_dropdown.value);
                }

                if (Input.GetKeyDown(KeyCode.UpArrow)) {
                    if (_dropdown.value - 1 < 0) {
                        _dropdown.SetValueWithoutNotify(_dropdown.options.Count);
                    } else {
                        _dropdown.SetValueWithoutNotify(_dropdown.value - 1);
                    }
                    ThrowColumnValueSetted((RankType)_dropdown.value);
                }
            }
        } 
        #endregion

        #region Event Listeners methods
        private void DropdownSelected(int selectionIndex) {
            ThrowColumnValueSetted((RankType)selectionIndex, _dropdown);
        }
        #endregion

        #region Protected overrrided methods
        protected override void SetSelectablesInteractables(bool isInteractable) {
            _dropdown.interactable = isInteractable;
        }
        #endregion

        public int GetValue() {
            return _dropdown.value;
        }

        public void SetDropdown(int dropdownIndex, bool withoutNotify = false) {
            if (!withoutNotify) {
                _dropdown.value = dropdownIndex;
            } else {
                _dropdown.SetValueWithoutNotify(dropdownIndex);
            }
        }
    }
}
