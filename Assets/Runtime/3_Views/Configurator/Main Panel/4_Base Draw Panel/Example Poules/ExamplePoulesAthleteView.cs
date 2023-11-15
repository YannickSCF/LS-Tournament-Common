/**
 * Author:      Yannick Santa Cruz Feuillias
 * Created:     23/10/2023
 **/

// Dependencies
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace YannickSCF.LSTournaments.Common.Views.MainPanel.BaseDrawPanel.ExamplePoules {
    public class ExamplePoulesAthleteView : MonoBehaviour {

        [SerializeField] private TextMeshProUGUI _dashText;
        [SerializeField] private TextMeshProUGUI _athleteText;
        [SerializeField] private Image _athleteFlag;

        public float SetAthleteText(string athleteText) {
            _athleteText.text = athleteText;
            return _athleteText.fontSize;
        }

        public void SetFontSize(float fontSize) {
            _athleteText.enableAutoSizing = fontSize == 0f;
            _dashText.enableAutoSizing = fontSize == 0f;

            if (fontSize != 0f) {
                _athleteText.fontSize = fontSize;
                _dashText.fontSize = fontSize;
            }
        }

        public void SetFlag(Sprite athleteFlag) {
            _athleteFlag.sprite = athleteFlag;
        }

        public void ResetAthlete() {
            _athleteText.text = string.Empty;
        }
    }
}
