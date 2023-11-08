/**
 * Author:      Yannick Santa Cruz Feuillias
 * Created:     29/09/2023
 **/

// Dependencies
using System.Collections.Generic;
using UnityEngine;

namespace YannickSCF.LSTournaments.Common.Views.MainPanel.AthletesPanel.Table.Header {
    public class AthleteTableHeaderView : MonoBehaviour {

        [SerializeField] private List<HeaderColumnView> _allHeaders;

        public void BlockHeader(AthleteInfoType column, bool block) {
            foreach (HeaderColumnView header in _allHeaders) {
                if (header.HeaderType == column) {
                    header.SetHeaderEnabled(block);
                    header.SetHeaderBlocked(block);
                    break;
                }
            }
        }

        public void EnableHeader(AthleteInfoType column, bool enable) {
            foreach (HeaderColumnView header in _allHeaders) {
                if (header.HeaderType == column) {
                    header.SetHeaderEnabled(enable);
                    break;
                }
            }
        }

        public void ShowColumn(AthleteInfoType column, bool show) {
            foreach (HeaderColumnView header in _allHeaders) {
                if (header.HeaderType == column) {
                    header.ShowHeader(show);
                    break;
                }
            }
        }

    }
}
