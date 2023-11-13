/**
 * Author:      Yannick Santa Cruz Feuillias
 * Created:     10/11/2023
 **/

// Dependencies
using TMPro;
using UnityEngine;
using UnityEngine.UI;
// Custom dependencies
using YannickSCF.LSTournaments.Common.Views.MainPanel.AthletesPanel.Events;

namespace YannickSCF.LSTournaments.Common.Views.MainPanel.AthletesDataPanel.Table.Header.ColumnHeader {
    public class ColumnHeaderView : MonoBehaviour {

        [SerializeField] private RectTransform _localTransform;
        [SerializeField] private AthleteInfoType _columnType;
        [Header("Header components")]
        [SerializeField] private TextMeshProUGUI _headerText;
        [Header("Toggle components")]
        [SerializeField] private Toggle _toggleToHide;
        [SerializeField] private CanvasGroup _toggleCanvasGroup;

        #region Mono
        private void OnEnable() {
            _toggleToHide.onValueChanged.AddListener(OnToggleClicked);
        }

        private void OnDisable() {
            _toggleToHide.onValueChanged.RemoveAllListeners();
        }
        #endregion

        public AthleteInfoType ColumnType { get => _columnType; }

        #region Events Listeners methods
        private void OnToggleClicked(bool isOn) {
            AthletesPanelViewEvents.ThrowOnAthleteInfoCheckboxToggle(_columnType, isOn);
        }
        #endregion

        public void SetHeaderAnchors(float minX, float maxX) {
            Vector2 oldSizeDelta = _localTransform.sizeDelta;
            Vector2 oldAnchoredPosition = _localTransform.anchoredPosition;

            _localTransform.anchorMin = new Vector2(minX, _localTransform.anchorMin.y);
            _localTransform.anchorMax = new Vector2(maxX, _localTransform.anchorMax.y);

            _localTransform.sizeDelta = oldSizeDelta;
            _localTransform.anchoredPosition = oldAnchoredPosition;
        }

        public void SetHeaderEnabled(bool enable) {
            if (_columnType == AthleteInfoType.Styles) {
                TextMeshProUGUI[] allTexts = GetComponentsInChildren<TextMeshProUGUI>();
                foreach (TextMeshProUGUI text in allTexts) {
                    text.color = new Color(
                        text.color.r,
                        text.color.g,
                        text.color.b,
                        enable ? 1 : 0.5f);
                }
            }

            _headerText.color = new Color(
                _headerText.color.r,
                _headerText.color.g,
                _headerText.color.b,
                enable ? 1 : 0.5f);

            _toggleToHide.SetIsOnWithoutNotify(enable);
        }

        public void SetHeaderBlocked(bool block) {
            _toggleCanvasGroup.alpha = block ? 0.3f : 1f;
            _toggleToHide.interactable = !block;
        }

        public void ShowHeader(bool show) {
            gameObject.SetActive(show);
        }
    }
}
