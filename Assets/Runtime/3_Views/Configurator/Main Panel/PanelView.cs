/**
 * Author:      Yannick Santa Cruz Feuillias
 * Created:     07/11/2023
 **/

// Dependencies
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace YannickSCF.LSTournaments.Common.Views.MainPanel {
    public class PanelView : MonoBehaviour {

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
    }
}
