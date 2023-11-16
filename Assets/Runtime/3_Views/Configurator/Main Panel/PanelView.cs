/**
 * Author:      Yannick Santa Cruz Feuillias
 * Created:     07/11/2023
 **/

// Dependencies
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using YannickSCF.GeneralApp;
using static YannickSCF.GeneralApp.CommonEventsDelegates;

namespace YannickSCF.LSTournaments.Common.Views.MainPanel {
    public class PanelView : MonoBehaviour {

        public event SimpleEventDelegate PanelMovedApart;
        public event SimpleEventDelegate PanelCentered;

        [SerializeField] private RectTransform _localTransform;

        private Vector2 _leftPosition = new Vector2(-1.1f, -0.1f);
        private Vector2 _centerPosition = new Vector2(0f, 1f);
        private Vector2 _rightPosition = new Vector2(1.1f, 2.1f);

        private readonly Color _ErrorColor = Color.red;
        private readonly Color _NormalColor = Color.white;

        private const float WAIT_TO_HIDE_VALIDATION_ERROR = 1f;
        private const float TIME_TO_HIDE_VALIDATION_ERROR = 2f;

        /// <summary>
        /// Method to reset the entry parameter object to the usual initial state.
        /// </summary>
        /// <param name="selectable">Selectable object to reset</param>
        protected void ResetSelectableError(Selectable selectable) {
            selectable.targetGraphic.color = _NormalColor;
        }

        /// <summary>
        /// Coroutine to show instantly the error color on target graphic
        /// of selectable and after WAIT_TO_HIDE_VALIDATION_ERROR seconds
        /// hide it slowly in TIME_TO_HIDE_VALIDATION_ERROR seconds.
        /// </summary>
        /// <param name="selectable">Selectable object to represent the error.</param>
        /// <returns>Coroutine IEnumerator</returns>
        protected IEnumerator ShowAndHideSelectableErrorCoroutine(Selectable selectable) {
            selectable.targetGraphic.color = _ErrorColor;

            yield return new WaitForSeconds(WAIT_TO_HIDE_VALIDATION_ERROR);

            float timeLeft = TIME_TO_HIDE_VALIDATION_ERROR;
            while (timeLeft > 0f) {
                selectable.targetGraphic.color = Color.Lerp(_ErrorColor, _NormalColor,
                    (TIME_TO_HIDE_VALIDATION_ERROR - timeLeft) / TIME_TO_HIDE_VALIDATION_ERROR);

                yield return new WaitForEndOfFrame();
                timeLeft -= Time.deltaTime;
            }
        }

        /// <summary>
        /// Method to reset the entry parameter UnityEngine.UI.Image to the usual initial state.
        /// </summary>
        /// <param name="image">UnityEngine.UI.Image object to reset</param>
        protected void ResetImageError(Image image) {
            image.CrossFadeColor(_NormalColor, 0f, true, true);
        }

        /// <summary>
        /// Coroutine to show instantly the error color on image (using CrossFadeColor)
        /// of selectable and after WAIT_TO_HIDE_VALIDATION_ERROR seconds
        /// hide it slowly in TIME_TO_HIDE_VALIDATION_ERROR seconds.
        /// </summary>
        /// <param name="image">UnityEngine.UI.Image object to represent the error.</param>
        /// <returns>Coroutine IEnumerator</returns>
        protected IEnumerator ShowAndHideImageErrorCoroutine(Image image) {
            image.CrossFadeColor(Color.red, 0f, true, true);

            yield return new WaitForSeconds(WAIT_TO_HIDE_VALIDATION_ERROR);
            image.CrossFadeColor(Color.white, TIME_TO_HIDE_VALIDATION_ERROR, true, true);
        }

        public virtual void ResetView() { }

        public virtual void MovePanelLeft(bool moveInmediate = false) {
            if (!moveInmediate) {
                StartCoroutine(MovePanel(_leftPosition));
            } else {
                MovePanelInmediate(_leftPosition);
            }
        }

        public virtual void MovePanelRight(bool moveInmediate = false) {
            if (!moveInmediate) {
                StartCoroutine(MovePanel(_rightPosition));
            } else {
                MovePanelInmediate(_rightPosition);
            }
        }

        public virtual void MovePanelCenter(bool moveInmediate = false) {
            gameObject.SetActive(true);
            if (!moveInmediate) {
                StartCoroutine(MovePanel(_centerPosition));
            } else {
                MovePanelInmediate(_centerPosition);
            }
        }

        private IEnumerator MovePanel(Vector2 position) {
            float cTime = 0f;
            float TotalTime = 0.5f;

            Vector2 initAnchorMin = _localTransform.anchorMin;
            Vector2 initAnchorMax = _localTransform.anchorMax;

            Vector2 anchorMin = new Vector2(position.x, _localTransform.anchorMin.y);
            Vector2 anchorMax = new Vector2(position.y, _localTransform.anchorMax.y);

            Vector2 oldSizeDelta = _localTransform.sizeDelta;
            Vector2 oldAnchoredPosition = _localTransform.anchoredPosition;

            while (cTime < TotalTime) {
                _localTransform.anchorMin = Vector2.Lerp(initAnchorMin, anchorMin, cTime / TotalTime);
                _localTransform.anchorMax = Vector2.Lerp(initAnchorMax, anchorMax, cTime / TotalTime);

                yield return new WaitForEndOfFrame();
                cTime += Time.deltaTime;
            }

            _localTransform.anchorMin = new Vector2(position.x, _localTransform.anchorMin.y);
            _localTransform.anchorMax = new Vector2(position.y, _localTransform.anchorMax.y);

            _localTransform.sizeDelta = oldSizeDelta;
            _localTransform.anchoredPosition = oldAnchoredPosition;

            yield return new WaitForSeconds(0.25f);
            if (position == _centerPosition) {
                PanelMovedCenter();
            } else if (position == _leftPosition) {
                PanelMovedLeft();
            } else if (position == _rightPosition) {
                PanelMovedRight();
            }
        }

        private void MovePanelInmediate(Vector2 position) {
            Vector2 oldSizeDelta = _localTransform.sizeDelta;
            Vector2 oldAnchoredPosition = _localTransform.anchoredPosition;

            _localTransform.anchorMin = new Vector2(position.x, _localTransform.anchorMin.y);
            _localTransform.anchorMax = new Vector2(position.y, _localTransform.anchorMax.y);

            _localTransform.sizeDelta = oldSizeDelta;
            _localTransform.anchoredPosition = oldAnchoredPosition;

            if (position == _centerPosition) {
                PanelMovedCenter();
            } else if (position == _leftPosition) {
                PanelMovedLeft();
            } else if (position == _rightPosition) {
                PanelMovedRight();
            }
        }

        public virtual void PanelMovedLeft() {
            gameObject.SetActive(false);
            PanelMovedApart?.Invoke();
        }

        public virtual void PanelMovedRight() {
            gameObject.SetActive(false);
            PanelMovedApart?.Invoke();
        }

        public virtual void PanelMovedCenter() {
            PanelCentered?.Invoke();
        }
    }
}
