/**
 * Author:      Yannick Santa Cruz Feuillias
 * Created:     28/09/2023
 **/

// Dependencies
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace YannickSCF.LSTournaments.Common.Views.MainPanel.AthletesPanel.Table.Content.Row.RowColumns {
    public class RowColumnView : MonoBehaviour {

        #region COMMON Events
        public delegate void ColumnStringEventDelegate(AthleteInfoType infoType, string value);
        public event ColumnStringEventDelegate OnColumnStringValueSetted;
        protected void ThrowColumnValueSetted(string value, Selectable currentField = null) {
            GoNextSelectable(currentField);
            OnColumnStringValueSetted?.Invoke(_infoType, value);
        }

        public delegate void ColumnDateTimeEventDelegate(AthleteInfoType infoType, DateTime value);
        public event ColumnDateTimeEventDelegate OnColumnDateTimeValueSetted;
        protected void ThrowColumnValueSetted(DateTime value, Selectable currentField = null) {
            GoNextSelectable(currentField);
            OnColumnDateTimeValueSetted?.Invoke(_infoType, value);
        }

        public delegate void ColumnRankEventDelegate(AthleteInfoType infoType, RankType value);
        public event ColumnRankEventDelegate OnColumnRankValueSetted;
        protected void ThrowColumnValueSetted(RankType value, Selectable currentField = null) {
            GoNextSelectable(currentField);
            OnColumnRankValueSetted?.Invoke(_infoType, value);
        }

        public delegate void ColumnColorEventDelegate(AthleteInfoType infoType, Color value);
        public event ColumnColorEventDelegate OnColumnColorValueSetted;
        protected void ThrowColumnValueSetted(Color value, Selectable currentField = null) {
            GoNextSelectable(currentField);
            OnColumnColorValueSetted?.Invoke(_infoType, value);
        }

        public delegate void ColumnStylesEventDelegate(AthleteInfoType infoType, List<StyleType> value);
        public event ColumnStylesEventDelegate OnColumnStylesValueSetted;
        protected void ThrowColumnValueSetted(List<StyleType> value, Selectable currentField = null) {
            GoNextSelectable(currentField);
            OnColumnStylesValueSetted?.Invoke(_infoType, value);
        }
        #endregion

        [Header("Basic Col References")]
        [SerializeField] private RectTransform _localTransform;
        [SerializeField] private AthleteInfoType _infoType;
        [SerializeField] private Image _hidder;

        protected virtual void SetSelectablesInteractables(bool isInteractable) { }

        public void SetColumnAnchors(float minX, float maxX) {
            Vector2 oldSizeDelta = _localTransform.sizeDelta;
            Vector2 oldAnchoredPosition = _localTransform.anchoredPosition;

            _localTransform.anchorMin = new Vector2(minX, _localTransform.anchorMin.y);
            _localTransform.anchorMax = new Vector2(maxX, _localTransform.anchorMax.y);

            _localTransform.sizeDelta = oldSizeDelta;
            _localTransform.anchoredPosition = oldAnchoredPosition;
        }

        public void SetBackgroundColor(Color bgColor) {
            _hidder.color = bgColor;
        }

        public void EnableColumn(bool enable) {
            _hidder.gameObject.SetActive(!enable);
            SetSelectablesInteractables(enable);
        }

        public bool IsColumnEnabled() {
            return !_hidder.gameObject.activeSelf;
        }

        private void GoNextSelectable(Selectable currentField) {
            if (currentField != null) {
                currentField.FindSelectableOnRight()?.Select();
            }
        }
    }
}
