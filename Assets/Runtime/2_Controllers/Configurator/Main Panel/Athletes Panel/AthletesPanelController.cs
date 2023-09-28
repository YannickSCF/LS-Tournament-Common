using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YannickSCF.LSTournaments.Common.Views.MainPanel.AthletesPanel.Events;
using YannickSCF.LSTournaments.Common.Views.MainPanel.AthletesPanel.Table.Header;
using YannickSCF.LSTournaments.Common.Views.MainPanel.AthletesPanel.Table.Row;

namespace YannickSCF.LSTournaments.Common.Controllers.MainPanel.AthletesPanel {
    public class AthletesPanelController : MonoBehaviour {

        [SerializeField] private AthleteTableHeaderView _header;
        [SerializeField] private AthleteRowView _athleteRowPrefab;

        private List<AthleteRowView> _rows;

        #region Mono
        private void OnEnable() {
            AthletesPanelViewEvents.OnAthleteInfoCheckboxToggle += OnToggleHeaderHide;
        }

        private void OnDisable() {
            AthletesPanelViewEvents.OnAthleteInfoCheckboxToggle -= OnToggleHeaderHide;
        }
        #endregion

        #region Events Listeners methods
        private void OnToggleHeaderHide(AthleteInfoType checkboxInfo, bool isChecked) {
            _header.EnableHeader(checkboxInfo, isChecked);
            foreach (AthleteRowView row in _rows) {
                row.DisableColumn(checkboxInfo, isChecked);
            }
        }
        #endregion
    }
}
