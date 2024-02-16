/**
 * Author:      Yannick Santa Cruz Feuillias
 * Created:     10/02/2024
 **/

// Dependencies
using UnityEngine;
using UnityEngine.UI;
//Custom dependencies

namespace YannickSCF.LSTournaments.Common.Views.PouleTable.Design.Objects {
    [RequireComponent(typeof(Image))]
    public class PouleBorder : MonoBehaviour {
        public enum BorderType { Internal, MainInternal, External }

        [SerializeField] private BorderType _type;
        public BorderType Type { get => _type; }

        private Image _border;
        public Image Border {
            get {
                if (_border == null)
                    _border = gameObject.GetComponent<Image>();

                return _border;
            }
        }

        public RectTransform RectTransform {
            get {
                return Border.rectTransform;
            }
        }

        public virtual void SetBorderWidth(float borderWidth) {
            if (_type == BorderType.External) {
                if (RectTransform.anchorMax.x == RectTransform.anchorMin.x) {
                    RectTransform.sizeDelta = new Vector2(borderWidth, borderWidth * 2);
                }

                if (RectTransform.anchorMax.y == RectTransform.anchorMin.y) {
                    RectTransform.sizeDelta = new Vector2(borderWidth * 2, borderWidth);
                }
            } else {
                if (RectTransform.anchorMax.x == RectTransform.anchorMin.x) {
                    RectTransform.sizeDelta = new Vector2(borderWidth, RectTransform.sizeDelta.y);
                }

                if (RectTransform.anchorMax.y == RectTransform.anchorMin.y) {
                    RectTransform.sizeDelta = new Vector2(0, borderWidth);
                }
            }
        }

        public void LengthenDown(int length) {
            RectTransform.offsetMin = new Vector2(RectTransform.offsetMin.x, -length);
        }

        public void SetColor(Color color) {
            Border.color = color;
        }
    }
}
