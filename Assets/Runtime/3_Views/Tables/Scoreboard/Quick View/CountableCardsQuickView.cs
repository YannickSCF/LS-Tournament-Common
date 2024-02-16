/**
 * Author:      Yannick Santa Cruz Feuillias
 * Created:     07/02/2024
 **/

// Dependencies
using TMPro;
using UnityEngine;
using UnityEngine.UI;
//Custom dependencies

namespace YannickSCF.LSTournaments.Common.Views.Tables.Scoreboard.QuickView {

    public class CountableCardsQuickView : MonoBehaviour {

        [SerializeField] private Image _cardImage;
        [SerializeField] private GameObject _countBubble;
        [SerializeField] private TextMeshProUGUI _countText;

        private int _count = 0;

        public void SetCards(int newCount) {
            if (newCount >= 0) {
                _count = newCount;
                UpdateVisuals();
            }
        }

        public void AddCard() {
            ++_count;
            UpdateVisuals();
        }

        public void RemoveCard() {
            if (_count > 0) {
                --_count;
                UpdateVisuals();
            }
        }

        public void ResetCards() {
            _count = 0;
            UpdateVisuals();
        }

        private void UpdateVisuals() {
            _cardImage.gameObject.SetActive(_count > 0);
            _countBubble.SetActive(_count > 1);
            _countText.text = _count.ToString();
        }
    }
}
