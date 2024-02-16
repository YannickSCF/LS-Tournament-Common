/**
 * Author:      Yannick Santa Cruz Feuillias
 * Created:     14/02/2024
 **/

// Dependencies
using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using YannickSCF.LSTournaments.Common.Views.PouleTable.Design.Objects;
//Custom dependencies

namespace YannickSCF.LSTournaments.Common.Views.PouleTable.Objects {
    public class PouleAthleteView : MonoBehaviour {

        private const string ORIGIN_NAME_SIZE = "75%";
        private const float ONLY_FLAG_FLEX_SIZE = 0.1f;
        private const float ORIGIN_FLEX_SIZE = 0.25f;

        [Header("----------------------\nAthlete Name parameters\n----------------------")]

        [SerializeField] private RectTransform _athleteColumn;
        [Header("Name panel objects")]
        [SerializeField] private TextMeshProUGUI _nameText;

        [Header("Origin panel objects")]
        [SerializeField] private Image _originParent;
        [SerializeField] private TextMeshProUGUI _originText;
        [Space(20)]
        [SerializeField] private PouleBorder _border;

        [Header("----------------------\nScores parameters\n----------------------")]

        [SerializeField] private RectTransform _allScoresColumns;

        [Header("Scores objects")]
        [SerializeField] private List<PouleDataBoxView> _allDataBoxes;

        #region Properties
        public PouleBorder Border { get => _border; }
        public List<PouleDataBoxView> AllDataBoxes { get => _allDataBoxes; }
        public bool activeSelf {
            get {
                return gameObject.activeSelf;
            }
        }
        #endregion

        #region Mono
        private void Awake() {
            if (_allDataBoxes[0].BoxType == PouleDataBoxView.DataBoxType.Blocked) {
                _allDataBoxes[0].SetMainScoreText((transform.GetSiblingIndex() + 1).ToString());
            }

            for (int i = 0; i < _allDataBoxes.Count; ++i) {
                _allDataBoxes[i].transform.SetSiblingIndex(i);
            }
        }
        #endregion

        public void SetName(string newName) {
            _nameText.text = newName;

            DisableOriginPanel();
        }

        public void SetNameWithFlag(string newName, string countryId) {
            _nameText.text = newName;

            EnableFlagPanel(countryId);
        }

        public void SetNameWithOrigin(string newName, string origin) {
            _nameText.text = newName;

            EnableOriginPanel(origin);
        }

        public void SetNameWithOrigin(string newName, string origin, string countryId) {
            _nameText.text = newName;

            EnableOriginPanel(origin, countryId);
        }

        public void ShowStyle(bool show) {
            foreach (PouleDataBoxView box in _allDataBoxes) {
                if (box.BoxType == PouleDataBoxView.DataBoxType.Score) {
                    box.ActiveStylePanel(show);
                }
            }
        }

        public void SetScore(int againstIndex, int score) {
            for (int i = 1; i < _allDataBoxes.Count; ++i) {
                if (againstIndex == i - 1) {
                    if (_allDataBoxes[i].BoxType == PouleDataBoxView.DataBoxType.Score) {
                        _allDataBoxes[i].SetMainScoreText(score.ToString());
                        break;
                    }
                }
            }
        }

        public void SetStyle(int againstIndex, float score) {
            for (int i = 1; i < _allDataBoxes.Count; ++i) {
                if (againstIndex == i - 1) {
                    if (_allDataBoxes[i].BoxType == PouleDataBoxView.DataBoxType.Score) {
                        _allDataBoxes[i].SetStyleScore(score.ToString());
                        break;
                    }
                }
            }
        }

        public void SetResultScore(PouleDataBoxView.DataBoxType resultScore, string score) {
            foreach (PouleDataBoxView box in _allDataBoxes) {
                if (box.BoxType == resultScore) {
                    box.SetMainScoreText(score);
                }
            }
        }

        public void ResetAllScores() {
            foreach (PouleDataBoxView box in _allDataBoxes) {
                box.ResetBox();
            }
        }

        #region Methods to control size and position
        public void SetColumnsActive(int activeAthletes, List<int> fixedColumns) {
            int countActives = 0;
            for (int i = 0; i < _allDataBoxes.Count; ++i) {
                if (fixedColumns != null && fixedColumns.Contains(i)) {
                    _allDataBoxes[i].gameObject.SetActive(true);
                } else {
                    _allDataBoxes[i].gameObject.SetActive(countActives < activeAthletes);
                    ++countActives;
                }
            }
        }

        public void SetNameColumnWidth(int newWidth) {
            _athleteColumn.sizeDelta = new Vector2(newWidth, _athleteColumn.sizeDelta.y);
            _allScoresColumns.offsetMin = new Vector2(newWidth, _allScoresColumns.offsetMin.y);
        }
        
        public void SetRowHeight(int newHeight) {
            ((RectTransform)transform).sizeDelta =
                new Vector2(((RectTransform)transform).sizeDelta.x, newHeight);
        }

        public void SetYPosition(int yPosition) {
            ((RectTransform)transform).anchoredPosition =
                new Vector2(((RectTransform)transform).anchoredPosition.x, yPosition);
        }

        public void SetWidthAndXOfDataBox(int columnIndex, int newWidth, int newXPosition) {
            _allDataBoxes[columnIndex].SetWidth(newWidth);
            _allDataBoxes[columnIndex].SetXPosition(newXPosition);
        }
        #endregion

        #region Methods to manage origin left panel
        private void EnableFlagPanel(string countryId) {
            _nameText.rectTransform.anchorMin = new Vector2(ONLY_FLAG_FLEX_SIZE, 0);
            _nameText.rectTransform.offsetMin = Vector2.zero;

            _originParent.rectTransform.gameObject.SetActive(true);
            _originParent.rectTransform.anchorMax = new Vector2(ONLY_FLAG_FLEX_SIZE, 1);
            _nameText.rectTransform.offsetMax = Vector2.zero;

            _originText.text = string.Format(LSTournamentConsts.SPRITE_TAG, countryId);
        }
        private void EnableOriginPanel(string origin) {
            _nameText.rectTransform.anchorMin = new Vector2(ORIGIN_FLEX_SIZE, 0);
            _nameText.rectTransform.offsetMin = Vector2.zero;

            _originParent.rectTransform.gameObject.SetActive(true);
            _originParent.rectTransform.anchorMax = new Vector2(ORIGIN_FLEX_SIZE, 1);
            _nameText.rectTransform.offsetMax = Vector2.zero;

            _originText.text = origin;
        }
        private void EnableOriginPanel(string origin, string countryId) {
            _nameText.rectTransform.anchorMin = new Vector2(ORIGIN_FLEX_SIZE, 0);
            _nameText.rectTransform.offsetMin = Vector2.zero;

            _originParent.rectTransform.gameObject.SetActive(true);
            _originParent.rectTransform.anchorMax = new Vector2(ORIGIN_FLEX_SIZE, 1);
            _nameText.rectTransform.offsetMax = Vector2.zero;

            _originText.text = string.Format(LSTournamentConsts.SPRITE_TAG, countryId) + "\n" +
                string.Format(LSTournamentConsts.SIZE_TAG_INIT, ORIGIN_NAME_SIZE) + origin;
        }

        private void DisableOriginPanel() {
            _nameText.rectTransform.anchorMin = Vector2.zero;
            _nameText.rectTransform.offsetMin = Vector2.zero;

            _originParent.gameObject.SetActive(false);
        }
        #endregion
    }
}
