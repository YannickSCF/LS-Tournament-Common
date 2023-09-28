// Dependencies
using System.Collections.Generic;
using UnityEngine;

namespace YannickSCF.LSTournaments.Common.Views.MainPanel.AthletesPanel.Table.Header {
    public class AthleteTableHeaderView : MonoBehaviour {

        [SerializeField] private List<HeaderColumnView> _allHeaders;

        public void EnableHeader(AthleteInfoType column, bool enable) {
            foreach (HeaderColumnView header in _allHeaders) {
                if (header.HeaderType == column) {
                    header.SetHeaderEnabled(enable);
                    break;
                }
            }
        }

        public void HideColumn(AthleteInfoType column, bool hide) {
            foreach (HeaderColumnView header in _allHeaders) {
                if (header.HeaderType == column) {
                    header.HideHeader(hide);
                    break;
                }
            }
        }

    }
}
