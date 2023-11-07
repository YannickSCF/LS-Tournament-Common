/**
 * Author:      Yannick Santa Cruz Feuillias
 * Created:     29/09/2023
 **/

// Dependencies
using TMPro;
using UnityEngine;
using UnityEngine.UI;
// Custom dependencies
using YannickSCF.LSTournaments.Common.Views.MainPanel.AthletesPanel.Events;

namespace YannickSCF.LSTournaments.Common.Views.MainPanel.AthletesPanel.Table.Bottom {
    public class AthleteBottomTableView : PanelView {

        [SerializeField] private Button _addAthleteButton;
        [SerializeField] private Button _removeLastAthleteButton;
        [SerializeField] private TextMeshProUGUI _athletesCountText;

        [SerializeField] private Button _addAthletesFromFileButton;

        [SerializeField] private Image _errorsPanelsImage;
        [SerializeField] private TextMeshProUGUI _errorsPanels;

        private Coroutine _incorrectAthletes;

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

        #region (PUBLIC) Methods to set view values or characteristics
        public void SetAddButtonInteractable(bool isInteractable) {
            _addAthleteButton.interactable = isInteractable;
        }

        public void SetRemoveButtonInteractable(bool isInteractable) {
            _removeLastAthleteButton.interactable = isInteractable;
        }

        /// <summary>
        /// Method to set count of athletes text in table.
        /// </summary>
        /// <param name="athletesText"> Athletes count text. Must contains all text that must be shown.</param>
        public void SetAthletesCount(string athletesText) {
            _athletesCountText.text = athletesText;
        }

        /// <summary>
        /// Method to set errors list on referenced panel.
        /// </summary>
        /// <param name="errorText">Errors list to show.</param>
        public void SetErrorPanelText(string errorText) {
            _errorsPanels.text = errorText;
        }
        #endregion

        /// <summary>
        /// Method to Show/Hide all validation errors on athletes list.
        /// </summary>
        /// <param name="show">
        /// If this value is 'true' force to show (or reset) the validation animation.
        /// If it is 'false', it cancels the animation.
        /// </param>
        public void ShowAthletesNotValidated(bool show) {
            if (_incorrectAthletes != null) {
                StopCoroutine(_incorrectAthletes);
            }

            if (show) {
                _incorrectAthletes = StartCoroutine(ShowAndHideImageErrorCoroutine(_errorsPanelsImage));
            } else {
                ResetImageError(_errorsPanelsImage);
            }
        }
    }
}
