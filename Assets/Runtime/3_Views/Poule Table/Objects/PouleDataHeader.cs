/**
 * Author:      Yannick Santa Cruz Feuillias
 * Created:     14/02/2024
 **/

// Dependencies
using System;
using TMPro;
using UnityEngine;
//Custom dependencies
using YannickSCF.LSTournaments.Common.Views.PouleTable.Design.Objects;

namespace YannickSCF.LSTournaments.Common.Views.PouleTable.Objects {
    [Serializable]
    public class PouleDataHeader {

        [SerializeField] private RectTransform _rectTransform;
        [SerializeField] private bool _columnOfSquares = true;
        [SerializeField, Range(25, 75)] private int _customColumnSize = 25;
        [SerializeField] private bool _fixedColumn = false;

        private PouleBorder _border;
        public PouleBorder Border {
            get {
                if (_border == null)
                    _border = RectTransform.GetComponentInChildren<PouleBorder>(true);

                return _border;
            }
        }

        public RectTransform RectTransform { get => _rectTransform; }
        public bool ColumnOfSquares { get => _columnOfSquares; }
        public int CustomColumnSize { get => _customColumnSize; }
        public bool FixedColumn { get => _fixedColumn; }

        public bool activeSelf {
            get {
                return RectTransform.gameObject.activeSelf;
            }
        }

        public void SetActive(bool active) {
            RectTransform.gameObject.SetActive(active);
        }

        public void SetWidth(int newWidth) {
            _customColumnSize = newWidth;
            _rectTransform.sizeDelta = new Vector2(_customColumnSize, _rectTransform.sizeDelta.y);
        }

        public void SetXPosition(int xPosition) {
            _rectTransform.anchoredPosition = new Vector2(xPosition, _rectTransform.anchoredPosition.y);
        }

        public void SetColumnOfSquares(bool isSquare) {
            _columnOfSquares = isSquare;
        }
    }
}
