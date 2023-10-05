// Dependencies
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using YannickSCF.LSTournaments.Common.Views.MainPanel.AthletesPanel.Events;

namespace YannickSCF.LSTournaments.Common.Views.MainPanel.AthletesPanel.Table.Bottom {
    public class AthleteBottomTableView : MonoBehaviour {

        [SerializeField] private Button _addAthleteButton;
        [SerializeField] private Button _removeLastAthleteButton;
        [SerializeField] private TextMeshProUGUI _athletesCountText;

        [SerializeField] private Button _addAthletesFromFileButton;

        [SerializeField] private TextMeshProUGUI _errorsPanels;

        #region Mono
        private void OnEnable() {
            _addAthleteButton.onClick.AddListener(() => AthletesPanelViewEvents.ThrowOnAthleteAdded());
            _removeLastAthleteButton.onClick.AddListener(() => AthletesPanelViewEvents.ThrowOnAthleteRemoved());

            _addAthletesFromFileButton.onClick.AddListener(() => AthletesPanelViewEvents.ThrowOnLoadAthletesFromFile());
        }

        private void OnDisable() {
            _addAthleteButton.onClick.RemoveAllListeners();
            _removeLastAthleteButton.onClick.RemoveAllListeners();

            _addAthletesFromFileButton.onClick.RemoveAllListeners();
        }
        #endregion

        public void SetAddButtonInteractable(bool isInteractable) {
            _addAthleteButton.interactable = isInteractable;
        }

        public void SetRemoveButtonInteractable(bool isInteractable) {
            _removeLastAthleteButton.interactable = isInteractable;
        }

        public void SetAthletesCount(string athletesText) {
            _athletesCountText.text = athletesText;
        }

        public void SetErrorPanelText(string errorText) {
            _errorsPanels.text = errorText;
        }
    }
}
