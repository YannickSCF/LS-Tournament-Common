// Dependencies
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YannickSCF.LSTournaments.Common.Models;
// Custom Dependencies
using YannickSCF.LSTournaments.Common.Tools.Importers;
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
        [Header("Other objects")]
        [SerializeField] private GameObject _loadingPanel;

        #region Mono
        private void Awake() {
            _loadingPanel.SetActive(false);
        }

        private void OnEnable() {
            // Header events
            AthletesPanelViewEvents.OnAthleteInfoCheckboxToggle += OnToggleHeaderHide;
            // Content events
            // Bottom events
            AthletesPanelViewEvents.OnAthleteAdded += OnAthleteAddedByButton;
            AthletesPanelViewEvents.OnAthleteRemoved += OnAthleteRemovedByButton;
            AthletesPanelViewEvents.OnLoadAthletesFromFile += OnAthletesLoadedByFile;
        }

        private void OnDisable() {
            // Header events
            AthletesPanelViewEvents.OnAthleteInfoCheckboxToggle -= OnToggleHeaderHide;
            // Content events
            // Bottom events
            AthletesPanelViewEvents.OnAthleteAdded -= OnAthleteAddedByButton;
            AthletesPanelViewEvents.OnAthleteRemoved -= OnAthleteRemovedByButton;
            AthletesPanelViewEvents.OnLoadAthletesFromFile -= OnAthletesLoadedByFile;
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

        private void OnAthletesLoadedByFile() {
            string filePath = FileImporter.SelectFileWithBrowser();
            if (!string.IsNullOrEmpty(filePath)) {
                // Get all participants
                List<AthleteInfoModel> athletes = FileImporter.ImportAthletesFromFile(filePath);
                if (athletes.Count > 0) {
                    _loadingPanel.SetActive(true);
                    // Reset all table content
                    _contentView.ResetContent();
                    // Add all needed rows with the athlete information
                    StartCoroutine(AddRowsCoroutine(athletes));
                }
            }
        }

        private IEnumerator AddRowsCoroutine(List<AthleteInfoModel> athletes) {
            yield return new WaitForSeconds(0.25f);

            for (int i = 0; i < athletes.Count; ++i) {
                yield return new WaitForEndOfFrame();
                _contentView.AddAthleteInfo(
                    athletes[i].Country,
                    athletes[i].Surname,
                    athletes[i].Name,
                    athletes[i].Academy,
                    athletes[i].School,
                    athletes[i].Rank,
                    athletes[i].Styles,
                    athletes[i].Tier,
                    athletes[i].SaberColor,
                    athletes[i].BirthDate,
                    athletes[i].StartDate);

                _bottomView.SetAthletesCount(GetAthletesCountText(i + 1));
            }

            _loadingPanel.SetActive(false);
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
