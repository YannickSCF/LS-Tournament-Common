// Dependencies
using UnityEngine;
using UnityEngine.UI;

namespace YannickSCF.LSTournaments.Common.Views.MainPanel.AthletesPanel.Table.Content.Row.RowColumns {
    public class RowColumnView : MonoBehaviour {

        [Header("Basic Col References")]
        [SerializeField] private Image _hidder;

        public void SetBackgroundColor(Color bgColor) {
            _hidder.color = bgColor;
        }

        public void Disable(bool hide) {
            _hidder.gameObject.SetActive(!hide);
        }
    }
}
