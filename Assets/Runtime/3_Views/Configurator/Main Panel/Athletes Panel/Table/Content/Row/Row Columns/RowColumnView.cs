// Dependencies
using UnityEngine;
using UnityEngine.UI;

namespace YannickSCF.LSTournaments.Common.Views.MainPanel.AthletesPanel.Table.Content.Row.RowColumns {
    public class RowColumnView : MonoBehaviour {

        public delegate void ColumnSettedEventDelegate(AthleteInfoType infoType);
        public event ColumnSettedEventDelegate OnColumnValueSetted;

        [Header("Basic Col References")]
        [SerializeField] private AthleteInfoType _infoType;
        [SerializeField] private Image _hidder;

        protected void ThrowColumnValueSetted(Selectable currentField = null) {
            if (currentField != null) {
                currentField.FindSelectableOnRight()?.Select();
            }

            OnColumnValueSetted?.Invoke(_infoType);
        }

        public void SetBackgroundColor(Color bgColor) {
            _hidder.color = bgColor;
        }

        public void EnableColumn(bool enable) {
            _hidder.gameObject.SetActive(!enable);
        }
    }
}
