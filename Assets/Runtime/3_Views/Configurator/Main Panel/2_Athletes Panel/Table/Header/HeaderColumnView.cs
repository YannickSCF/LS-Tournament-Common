// Dependencies
using TMPro;
using UnityEngine;
using UnityEngine.UI;
// Custom dependencies
using YannickSCF.LSTournaments.Common.Views.MainPanel.AthletesPanel.Events;

namespace YannickSCF.LSTournaments.Common.Views.MainPanel.AthletesPanel.Table.Header {
    public class HeaderColumnView : MonoBehaviour {

        [SerializeField] private AthleteInfoType _headerType;
        [SerializeField] private TextMeshProUGUI _headerText;
        [SerializeField] private Toggle _toggleToHide;
        [SerializeField] private CanvasGroup _toggleCanvasGroup;

        public AthleteInfoType HeaderType { get => _headerType; }

        #region Mono
        private void OnEnable() {
            _toggleToHide.onValueChanged.AddListener(OnToggleClicked);
        }

        private void OnDisable() {
            _toggleToHide.onValueChanged.RemoveAllListeners();
        }
        #endregion

        #region Events Listeners methods
        private void OnToggleClicked(bool isOn) {
            AthletesPanelViewEvents.ThrowOnAthleteInfoCheckboxToggle(_headerType, isOn);
        }
        #endregion

        public void SetHeaderEnabled(bool enable) {
            if (_headerType == AthleteInfoType.Styles) {
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
