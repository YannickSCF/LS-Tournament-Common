// Dependencies
using System;
using UnityEngine;
// Custom Dependencies
using YannickSCF.LSTournaments.Common.Views.MainPanel.AthletesPanel.Events;
using YannickSCF.LSTournaments.Common.Views.MainPanel.AthletesPanel.Table.Bottom;
using YannickSCF.LSTournaments.Common.Views.MainPanel.AthletesPanel.Table.Content;
using YannickSCF.LSTournaments.Common.Views.MainPanel.AthletesPanel.Table.Header;

namespace YannickSCF.LSTournaments.Common.Controllers.MainPanel.AthletesPanel {
    public class AthletesPanelController : MonoBehaviour {

        [Header("Views")]
        [SerializeField] private AthleteTableHeaderView _headerView;
        [SerializeField] private AthleteTableContentView _contentView;
        [SerializeField] private AthleteBottomTableView _bottomView;

        #region Mono
        private void OnEnable() {
            // Header events
            AthletesPanelViewEvents.OnAthleteInfoCheckboxToggle += OnToggleHeaderHide;
            // Content events
            // Bottom events
            AthletesPanelViewEvents.OnAthleteAdded += OnAthleteAddedByButton;
            AthletesPanelViewEvents.OnAthleteRemoved += OnAthleteRemovedByButton;
        }

        private void OnDisable() {
            // Header events
            AthletesPanelViewEvents.OnAthleteInfoCheckboxToggle -= OnToggleHeaderHide;
            // Content events
            // Bottom events
            AthletesPanelViewEvents.OnAthleteAdded -= OnAthleteAddedByButton;
            AthletesPanelViewEvents.OnAthleteRemoved -= OnAthleteRemovedByButton;
        }
        #endregion

        #region Events Listeners methods
        private void OnToggleHeaderHide(AthleteInfoType checkboxInfo, bool isChecked) {
            _headerView.EnableHeader(checkboxInfo, isChecked);
            _contentView.DisableRowColumns(checkboxInfo, isChecked);
        }

        private void OnAthleteAddedByButton() {
            int athleteCount = _contentView.AddAthleteRow();
            _bottomView.SetAthletesCount(GetAthletesCountText(athleteCount));

            UpdateBottomButtons(athleteCount);
        }

        private void OnAthleteRemovedByButton() {
            int athleteCount = _contentView.RemoveLastAthleteRow();
            _bottomView.SetAthletesCount(GetAthletesCountText(athleteCount));

            UpdateBottomButtons(athleteCount);
        }
        #endregion

        private string GetAthletesCountText(int count) {
            return count + " Athletes";
        }

        private void UpdateBottomButtons(int athleteCount) {
            _bottomView.SetRemoveButtonInteractable(athleteCount > 0);
        }
    }
}
