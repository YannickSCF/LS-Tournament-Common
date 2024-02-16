/**
 * Author:      Yannick Santa Cruz Feuillias
 * Created:     14/02/2024
 **/

// Dependencies
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
//Custom dependencies

namespace YannickSCF.LSTournaments.Common.Views.PouleTable.Objects {
    public class PouleDataBoxView : MonoBehaviour {
        public enum DataBoxType { Score, Blocked, ResultScore, ResultWin, ResultDefeat, ResultTie, ResultOHFavor, ResultOHAgainst, ResultStyle, ScoreBlocked };

        private const string BASIC_RESET = "_";
        private const string STYLE_RESET = "-";

        private readonly Vector2 ANCHORS_STYLE_VISIBLE = new Vector2(0.4f, 0.9f);
        private readonly Vector2 ANCHORS_STYLE_NOT_VISIBLE = new Vector2(0.2f, 0.8f);

        [SerializeField] private DataBoxType _boxType;
        [SerializeField] private TextMeshProUGUI _mainScoreText;
        [SerializeField] private Image _mainBackground;
        [Space(20)]
        [SerializeField] private GameObject _styleScoreSubBox;
        [SerializeField] private TextMeshProUGUI _styleScoreText;

        public DataBoxType BoxType { get => _boxType; }

        public void SetMainScoreText(string text) {
            _mainScoreText.text = text;
        }

        public void ActiveStylePanel(bool active) {
            if (_boxType == DataBoxType.Score) {
                _styleScoreSubBox.SetActive(active);

                _mainScoreText.rectTransform.anchorMin = new Vector2(_mainScoreText.rectTransform.anchorMin.x,
                    active ? ANCHORS_STYLE_VISIBLE.x : ANCHORS_STYLE_NOT_VISIBLE.x);
                _mainScoreText.rectTransform.anchorMax = new Vector2(_mainScoreText.rectTransform.anchorMax.x,
                    active ? ANCHORS_STYLE_VISIBLE.y : ANCHORS_STYLE_NOT_VISIBLE.y);

                _mainScoreText.rectTransform.offsetMin = Vector2.zero;
                _mainScoreText.rectTransform.offsetMax = Vector2.zero;
            }
        }

        public void SetStyleScore(string text) {
            if (_boxType == DataBoxType.Score) {
                _styleScoreText.text = text;
            }
        }

        public void ResetBox() {
            if (_boxType != DataBoxType.Blocked && _boxType != DataBoxType.ScoreBlocked) {
                _mainScoreText.text = BASIC_RESET;
                _styleScoreText.text = STYLE_RESET;
            }
        }

        #region Methods to control size and position
        public void SetWidth(int newWidth) {
            ((RectTransform)transform).sizeDelta =
                new Vector2(newWidth, ((RectTransform)transform).sizeDelta.y);
        }

        public void SetXPosition(int xPosition) {
            ((RectTransform)transform).anchoredPosition =
                new Vector2(xPosition, ((RectTransform)transform).anchoredPosition.y);
        }
        #endregion
    }
}
