/**
 * Author:      Yannick Santa Cruz Feuillias
 * Created:     10/11/2023
 **/

// Dependencies
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
// Custom dependencies
using YannickSCF.LSTournaments.Common.Views.MainPanel.AthletesDataPanel.Table.Content;
using YannickSCF.LSTournaments.Common.Views.MainPanel.AthletesDataPanel.Table.Header;
using YannickSCF.LSTournaments.Common.Views.MainPanel.AthletesPanel.Events;

namespace YannickSCF.LSTournaments.Common.Views.MainPanel.AthletesDataPanel {
    public class AthletesDataPanelView : PanelView {

        [Header("Table Header")]
        [SerializeField] private AthletesDataTableHeaderView _header;
        [Header("Table and content")]
        [SerializeField] private AthletesDataTableContentView _content;
        [Header("Bottom Objects")]
        [SerializeField] private Button _addAthletesFromFileButton;
        [SerializeField] private Image _errorsPanelsImage;
        [SerializeField] private TextMeshProUGUI _errorsPanels;

        private Coroutine _incorrectAthletes;

        #region Mono
        private void OnEnable() {
            _addAthletesFromFileButton.onClick.AddListener(() => AthletesPanelViewEvents.ThrowOnLoadAthletesFromFile());
        }

        private void OnDisable() {
            _addAthletesFromFileButton.onClick.RemoveAllListeners();
        }
        #endregion

        #region Calculates for columns anchors
        public void SetColumnsAnchors(Dictionary<AthleteInfoType, Vector2> columnsSizes, float rowHeight) {
            _header.UpdateHeaderAnchors(columnsSizes);
            _content.SetRowHeightAndColumnSizes(rowHeight, columnsSizes);
        }
        #endregion

        #region Method to add/remove athletes
        public void AddAthlete() {
            _content.AddAthleteRow();
        }

        public void AddAthlete(string countryCode, string surname, string name,
            string academy, string school, RankType rank, List<StyleType> styles,
            int tier, Color color, DateTime birthData, DateTime startDate) {

            _content.AddAthleteInfo(countryCode, surname, name,
                academy, school, rank, styles, tier,
                color, birthData, startDate);
        }

        public void RemoveAthlete() {
            _content.RemoveLastAthleteRow();
        }

        public void ResetAthletesList() {
            _content.ResetContent();
        }
        #endregion

        #region Methods to manage errors Text Area
        /// <summary>
        /// Method to set errors list on referenced panel.
        /// </summary>
        /// <param name="errorText">Errors list to show.</param>
        public void SetErrorPanelText(string errorText) {
            _errorsPanels.text = errorText;
        }

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
        #endregion

        #region Methods to manage columns visibility
        public void EnableColumn(AthleteInfoType checkboxInfo, bool isChecked) {
            _header.EnableHeader(checkboxInfo, isChecked);
            _content.EnableRowColumns(checkboxInfo, isChecked);
        }
        public void EnableRowColumn(int rowToUpdate, AthleteInfoType checkboxInfo, bool show) {
            _content.EnableRowColumn(rowToUpdate, checkboxInfo, show);
        }
        #endregion

        public override void MovePanelLeft() {
            base.MovePanelLeft();
            _content.ResetScrollAndHideInvisibleRows(false);
        }

        public override void MovePanelRight() {
            base.MovePanelRight();
            _content.ResetScrollAndHideInvisibleRows(false);
        }

        public override void PanelMovedCenter() {
            base.PanelMovedCenter();
            _content.ResetScrollAndHideInvisibleRows(true);
        }

        public void UpdateColumnsBlockedByFillerType(PouleFillerType fillerType) {
            _header.BlockHeader(AthleteInfoType.Rank, fillerType == PouleFillerType.ByRank);
            _header.BlockHeader(AthleteInfoType.Styles, fillerType == PouleFillerType.ByStyle);
            _header.BlockHeader(AthleteInfoType.Tier, fillerType == PouleFillerType.ByTier);
        }

        public void SetRemoveButtonInteractable(bool isInteractable) {
            _content.SetRemoveButtonInteractable(isInteractable);
        }
    }
}
