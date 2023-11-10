/**
 * Author:      Yannick Santa Cruz Feuillias
 * Created:     10/11/2023
 **/

// Dependencies
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;
// Custom Dependencies
using YannickSCF.LSTournaments.Common.Views.MainPanel.AthletesPanel.Table.Bottom;
using YannickSCF.LSTournaments.Common.Views.MainPanel.AthletesPanel.Table.Content;
using YannickSCF.LSTournaments.Common.Views.MainPanel.AthletesPanel.Table.Header;

namespace YannickSCF.LSTournaments.Common.Views.MainPanel.AthletesPanel {
    public class AthletesPanelView : PanelView {

        [SerializeField] private AthleteTableHeaderView _headerView;
        [SerializeField] private AthleteTableContentView _contentView;
        [SerializeField] private AthleteBottomTableView _bottomView;

        #region Method to add/remove athletes
        public int AddAthlete() {
            int athleteCount = _contentView.AddAthleteRow();
            SetAthletesCount(athleteCount);
            return athleteCount;
        }

        public void AddAthlete(int index, string countryCode, string surname, string name,
            string academy, string school, RankType rank, List<StyleType> styles,
            int tier, Color color, DateTime birthData, DateTime startDate) {

            _contentView.AddAthleteInfo( countryCode, surname, name,
                academy, school, rank, styles, tier,
                color, birthData, startDate);

            SetAthletesCount(index);
        }

        public int RemoveAthlete() {
            int athleteCount = _contentView.RemoveLastAthleteRow();
            SetAthletesCount(athleteCount);
            return athleteCount;
        }

        public void ResetAthletesList() {
            _contentView.ResetContent();
        }

        public void SetAthletesCount(int count) {
            var localizedString = new LocalizedString("Configurator Texts", "AthletesPanel_Athletes");
            localizedString.Arguments = new object[] { count };
            _bottomView.SetAthletesCount(localizedString.GetLocalizedString());
        }
        #endregion

        #region Methods to manage errors Text Area
        public void UpdateErrorsList(string errorsText) {
            _bottomView.SetErrorPanelText(errorsText);
        }

        public void ShowAthletesNotValidated(bool show) {
            _bottomView.ShowAthletesNotValidated(show);
        }
        #endregion

        #region Methods to manage columns visibility
        public void ShowColumn(AthleteInfoType columnToShow, bool show) {
            _headerView.ShowColumn(columnToShow, show);
            _contentView.ShowRowColumns(columnToShow, show);
        }
        public void ShowRowColumn(int rowToUpdate, AthleteInfoType checkboxInfo, bool show) {
            _contentView.ShowRowColumn(rowToUpdate, checkboxInfo, show);
        }

        public void EnableColumn(AthleteInfoType checkboxInfo, bool isChecked) {
            _headerView.EnableHeader(checkboxInfo, isChecked);
            _contentView.EnableRowColumns(checkboxInfo, isChecked);
        }
        public void EnableRowColumn(int rowToUpdate, AthleteInfoType checkboxInfo, bool show) {
            _contentView.EnableRowColumn(rowToUpdate, checkboxInfo, show);
        }
        #endregion

        public void UpdateColumnsBlockedByFillerType(PouleFillerType fillerType) {
            _headerView.BlockHeader(AthleteInfoType.Rank, fillerType == PouleFillerType.ByRank);
            _headerView.BlockHeader(AthleteInfoType.Styles, fillerType == PouleFillerType.ByStyle);
            _headerView.BlockHeader(AthleteInfoType.Tier, fillerType == PouleFillerType.ByTier);
        }

        public void SetRemoveButtonInteractable(bool isInteractable) {
            _bottomView.SetRemoveButtonInteractable(isInteractable);
        }
    }
}
