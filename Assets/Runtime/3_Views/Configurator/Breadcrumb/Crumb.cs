// Dependencies
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace YannickSCF.LSTournaments.Common.Views.Breadcrumb {
    public class Crumb : MonoBehaviour {

        [SerializeField] private TextMeshProUGUI _crumbText;
        [SerializeField] private Image _disableCrumbImage;

        public void SetCrumbText(string newCrumbText) {
            _crumbText.text = newCrumbText;
        }

        public void EnableCrumb(bool isEnable) {
            _disableCrumbImage.gameObject.SetActive(isEnable);
        }
    }
}
