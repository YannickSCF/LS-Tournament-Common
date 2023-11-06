// Dependencies
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace YannickSCF.LSTournaments.Common.Views.Breadcrumb {
    public class CrumbView : MonoBehaviour {

        [SerializeField] private TextMeshProUGUI _crumbText;
        [SerializeField] private Image _disableCrumbImage;

        private RectTransform _crumbTransform;

        #region Mono
        private void Awake() {
            _crumbTransform = GetComponent<RectTransform>();
        }
        #endregion

        public void SetCrumbText(string newCrumbText) {
            _crumbText.text = newCrumbText;
        }

        public void EnableCrumb(bool enable) {
            _disableCrumbImage.gameObject.SetActive(!enable);
        }

        public void FocusCrumb(bool focus) {
            _crumbTransform.localScale = focus ? Vector3.one : new Vector3(1f, 0.9f, 1);
            _crumbText.fontStyle = focus ? FontStyles.Bold : FontStyles.Normal;
        }
    }
}
