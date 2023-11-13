/**
 * Author:      Yannick Santa Cruz Feuillias
 * Created:     10/11/2023
 **/

// Dependencies
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
// Custom dependencies
using YannickSCF.LSTournaments.Common.Scriptables.Settings.AthleteTables;
using YannickSCF.LSTournaments.Common.Views.MainPanel.AthletesDataPanel.Table.Header.ColumnHeader;

namespace YannickSCF.LSTournaments.Common.Views.MainPanel.AthletesDataPanel.Table.Header {
    public class AthletesDataTableHeaderView : MonoBehaviour {

        [SerializeField] private List<ColumnHeaderView> _allHeaders;

        public void UpdateHeaderAnchors(Dictionary<AthleteInfoType, Vector2> columnsSizes) {
            foreach (KeyValuePair<AthleteInfoType, Vector2> columnSize in columnsSizes) {
                ColumnHeaderView column = _allHeaders.FirstOrDefault(x => x.ColumnType == columnSize.Key);
                column.gameObject.SetActive(columnSize.Value != Vector2.zero);
                column.SetHeaderAnchors(columnSize.Value.x, columnSize.Value.y);
            }
        }

        public void BlockHeader(AthleteInfoType column, bool block) {
            foreach (ColumnHeaderView header in _allHeaders) {
                if (header.ColumnType == column) {
                    header.SetHeaderEnabled(block);
                    header.SetHeaderBlocked(block);
                    break;
                }
            }
        }

        public void EnableHeader(AthleteInfoType column, bool enable) {
            foreach (ColumnHeaderView header in _allHeaders) {
                if (header.ColumnType == column) {
                    header.SetHeaderEnabled(enable);
                    break;
                }
            }
        }

        public void ShowColumn(AthleteInfoType column, bool show) {
            foreach (ColumnHeaderView header in _allHeaders) {
                if (header.ColumnType == column) {
                    header.ShowHeader(show);
                    break;
                }
            }
        }
    }
}
