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

    public class ScoreboardQuickView : MonoBehaviour {

        private const string SCOREBOARD = "{0} - {1}";
        private const string ORIGIN_SIZE = "75%";

        [Header("Common objects")]
        [SerializeField] private TextMeshProUGUI _combatPhaseName;
        [SerializeField] private TextMeshProUGUI _warScoreboard;
        [SerializeField] private TextMeshProUGUI _timer;

        [Header("First (Left) Athlete Specifics")]
        [SerializeField] private TextMeshProUGUI _firstAthleteName;
        [SerializeField] private TextMeshProUGUI _firstAthleteStyleScore;
        [SerializeField] private CountableCardsQuickView _firstAthleteWhiteCards;
        [SerializeField] private CountableCardsQuickView _firstAthleteYellowCards;
        [SerializeField] private Image _firstAthleteRedCard;
        [SerializeField] private Image _firstAthleteBlackCard;

        [Header("Second (Right) Athlete Specifics")]
        [SerializeField] private TextMeshProUGUI _secondAthleteName;
        [SerializeField] private TextMeshProUGUI _secondAthleteStyleScore;
        [SerializeField] private CountableCardsQuickView _secondAthleteWhiteCards;
        [SerializeField] private CountableCardsQuickView _secondAthleteYellowCards;
        [SerializeField] private Image _secondAthleteRedCard;
        [SerializeField] private Image _secondAthleteBlackCard;

        #region Methods to Initialize Scoreboard
        public void InitializeCombat(string combatPhase, string time, string firstAthleteName, string secondAthleteName) {
            _combatPhaseName.text = combatPhase;
            
            _firstAthleteName.text = firstAthleteName;
            _secondAthleteName.text = secondAthleteName;

            SetScoreboard(0, 0);
            _timer.text = time;

            SetStyles(-1, -1);
            ResetCards();
        }

        public void InitializeCombat(string combatPhase, string time,
            string firstAthleteName, string firstAthleteOrigin,
            string secondAthleteName, string secondAthleteOrigin) {
            _combatPhaseName.text = combatPhase;

            string originSize = string.Format(LSTournamentConsts.SIZE_TAG_INIT, ORIGIN_SIZE);
            _firstAthleteName.text = $"{firstAthleteName}\n{originSize}{firstAthleteOrigin}";
            _secondAthleteName.text = $"{secondAthleteName}\n{originSize}{secondAthleteOrigin}";

            SetScoreboard(0, 0);
            _timer.text = time;

            SetStyles(-1, -1);
            ResetCards();
        }

        public void InitializeCombat(string combatPhase, string time,
            string firstAthleteName, string firstAthleteCountryCode, string firstAthleteOrigin,
            string secondAthleteName, string secondAthleteCountryCode, string secondAthleteOrigin) {
            _combatPhaseName.text = combatPhase;

            string originSize = string.Format(LSTournamentConsts.SIZE_TAG_INIT, ORIGIN_SIZE);
            _firstAthleteName.text = $"{firstAthleteName}\n" +
                $"{originSize}{string.Format(LSTournamentConsts.SPRITE_TAG, firstAthleteCountryCode)} - {firstAthleteOrigin}";
            _secondAthleteName.text = $"{secondAthleteName}\n" +
                $"{originSize}{string.Format(LSTournamentConsts.SPRITE_TAG, secondAthleteCountryCode)} - {secondAthleteOrigin}";

            SetScoreboard(0, 0);
            _timer.text = time;

            SetStyles(-1, -1);
            ResetCards();
        }
        #endregion


        public void SetTime(byte minutes, byte seconds, byte milliseconds = 0) {
            string minutesStr = minutes < 10 ? "0" + minutes : minutes.ToString();
            string secondsStr = seconds < 10 ? "0" + seconds : seconds.ToString();
            string millisecondsStr = minutes < 100 ? "0" + milliseconds : minutes < 10 ? "00" + milliseconds : milliseconds.ToString();

            _warScoreboard.text = string.Format(LSTournamentConsts.TIMER_FORMAT, minutesStr, secondsStr, millisecondsStr);
        }

        #region Methods for Scoreboard
        public void SetScoreboard(int firstScore, int secondScore) {
            _warScoreboard.text = string.Format(SCOREBOARD,
                firstScore < 0 ? 0 : firstScore,
                secondScore < 0 ? 0 : secondScore);
        }
        #endregion

        #region Methods for styles
        public void SetStyles(float firstAthleteStyle, float secondAthleteStyle) {
            SetStyleFirstAthlete(firstAthleteStyle);
            SetStyleSecondAthlete(secondAthleteStyle);
        }
        public void SetStyleFirstAthlete(float newStyle) {
            _firstAthleteStyleScore.text = newStyle < 0 ? string.Empty : newStyle.ToString();
        }
        public void SetStyleSecondAthlete(float newStyle) {
            _secondAthleteStyleScore.text = newStyle < 0 ? string.Empty : newStyle.ToString();
        }
        #endregion

        #region Methods for Cards
        public void SetCards(AthleteCards firstAthlete, AthleteCards secondAthlete) {
            _firstAthleteWhiteCards.SetCards(firstAthlete.White);
            _firstAthleteYellowCards.SetCards(firstAthlete.Yellow);
            _firstAthleteRedCard.gameObject.SetActive(firstAthlete.Red);
            _firstAthleteBlackCard.gameObject.SetActive(firstAthlete.Black);

            _secondAthleteWhiteCards.SetCards(secondAthlete.White);
            _secondAthleteYellowCards.SetCards(secondAthlete.Yellow);
            _secondAthleteRedCard.gameObject.SetActive(secondAthlete.Red);
            _secondAthleteBlackCard.gameObject.SetActive(secondAthlete.Black);
        }
        public void ResetCards() {
            _firstAthleteWhiteCards.ResetCards();
            _firstAthleteYellowCards.ResetCards();
            _firstAthleteRedCard.gameObject.SetActive(false);
            _firstAthleteBlackCard.gameObject.SetActive(false);

            _secondAthleteWhiteCards.ResetCards();
            _secondAthleteYellowCards.ResetCards();
            _secondAthleteRedCard.gameObject.SetActive(false);
            _secondAthleteBlackCard.gameObject.SetActive(false);
        }

        public void AddCardToFirstAthlete(CombatCards cardToAdd) {
            switch (cardToAdd) {
                case CombatCards.Yellow: _firstAthleteYellowCards.AddCard(); break;
                case CombatCards.Red: _firstAthleteRedCard.gameObject.SetActive(true); break;
                case CombatCards.Black: _firstAthleteBlackCard.gameObject.SetActive(true); break;
                case CombatCards.White:
                default: _firstAthleteWhiteCards.AddCard(); break;
            }
        }
        public void AddCardToSecondAthlete(CombatCards cardToAdd) {
            switch (cardToAdd) {
                case CombatCards.Yellow: _secondAthleteYellowCards.AddCard(); break;
                case CombatCards.Red: _secondAthleteRedCard.gameObject.SetActive(true); break;
                case CombatCards.Black: _secondAthleteBlackCard.gameObject.SetActive(true); break;
                case CombatCards.White:
                default: _secondAthleteWhiteCards.AddCard(); break;
            }
        }

        public void RemoveCardToFirstAthlete(CombatCards cardToRemove) {
            switch (cardToRemove) {
                case CombatCards.Yellow: _firstAthleteYellowCards.RemoveCard(); break;
                case CombatCards.Red: _firstAthleteRedCard.gameObject.SetActive(false); break;
                case CombatCards.Black: _firstAthleteBlackCard.gameObject.SetActive(false); break;
                case CombatCards.White:
                default: _firstAthleteWhiteCards.RemoveCard(); break;
            }
        }
        public void RemoveCardToSecondAthlete(CombatCards cardToRemove) {
            switch (cardToRemove) {
                case CombatCards.Yellow: _secondAthleteYellowCards.RemoveCard(); break;
                case CombatCards.Red: _secondAthleteRedCard.gameObject.SetActive(false); break;
                case CombatCards.Black: _secondAthleteBlackCard.gameObject.SetActive(false); break;
                case CombatCards.White:
                default: _secondAthleteWhiteCards.RemoveCard(); break;
            }
        }
        #endregion
    }
}
