using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using YannickSCF.LSTournaments.Common;
using YannickSCF.LSTournaments.Common.Controllers;
using YannickSCF.LSTournaments.Common.Controllers.PouleTable;
using YannickSCF.LSTournaments.Common.Models.Athletes;
using YannickSCF.LSTournaments.Common.Models.Poules;
using YannickSCF.LSTournaments.Common.Tools.Poule;
using YannickSCF.LSTournaments.Common.Views.Tables.Scoreboard.QuickView;

namespace YannickSCF.LSTournaments {

    public class ObjectsTester : MonoBehaviour {

        [Header("Views to test")]
        [SerializeField] private List<PouleTableController> _testeablesPoules;
        [SerializeField] private int _selectedPoule = 0;
        [SerializeField] private ScoreboardQuickView _scoreboardPrefab;
        [SerializeField] private ScrollRect _allScoreboards;

        [Header("Tester Tools")]
        [SerializeField] private Button _updateData;
        [SerializeField] private Button _nextPouleBtn;
        [SerializeField] private Button _prevPouleBtn;
        [SerializeField] private TMP_Dropdown _combatSelector;
        [Space(10)]
        [SerializeField] private Button _addScoreFirst;
        [SerializeField] private Button _removeScoreFirst;
        [SerializeField] private TMP_InputField _styleFirst;
        [Space(10)]
        [SerializeField] private Button _addWhiteFirst;
        [SerializeField] private Button _removeWhiteFirst;
        [SerializeField] private Button _addYellowFirst;
        [SerializeField] private Button _removeYellowFirst;
        [SerializeField] private Button _addRedFirst;
        [SerializeField] private Button _removeRedFirst;
        [SerializeField] private Button _addBlackFirst;
        [SerializeField] private Button _removeBlackFirst;
        [Space(10)]
        [SerializeField] private Button _addScoreSecond;
        [SerializeField] private Button _removeScoreSecond;
        [SerializeField] private TMP_InputField _styleSecond;
        [Space(10)]
        [SerializeField] private Button _addWhiteSecond;
        [SerializeField] private Button _removeWhiteSecond;
        [SerializeField] private Button _addYellowSecond;
        [SerializeField] private Button _removeYellowSecond;
        [SerializeField] private Button _addRedSecond;
        [SerializeField] private Button _removeRedSecond;
        [SerializeField] private Button _addBlackSecond;
        [SerializeField] private Button _removeBlackSecond;

        private PouleDataModel _pouleDataModel;

        private List<ScoreboardQuickView> _allMatches;
        private int _scoreboardIndex = 0;

        #region Mono
        private void Awake() {
            if (_selectedPoule >= _testeablesPoules.Count) {
                _selectedPoule = _testeablesPoules.Count - 1;
            } else if (_selectedPoule < 0) {
                _selectedPoule = 0;
            }

            foreach(PouleTableController poule in _testeablesPoules) {
                poule.gameObject.SetActive(false);
            }

            _testeablesPoules[_selectedPoule].gameObject.SetActive(true);
        }

        private void OnEnable() {
            _updateData.onClick.AddListener(UpdateData);
            _nextPouleBtn.onClick.AddListener(NextPouleToTest);
            _prevPouleBtn.onClick.AddListener(PrevPouleToTest);
            _combatSelector.onValueChanged.AddListener(SelectCombat);

            _addScoreFirst.onClick.AddListener(AddScoreFirst);
            _removeScoreFirst.onClick.AddListener(RemoveScoreFirst);
            _styleFirst.onValueChanged.AddListener(SetStyleFirst);

            _addWhiteFirst.onClick.AddListener(AddWhiteFirst);
            _removeWhiteFirst.onClick.AddListener(RemoveWhiteFirst);
            _addYellowFirst.onClick.AddListener(AddYellowFirst);
            _removeYellowFirst.onClick.AddListener(RemoveYellowFirst);
            _addRedFirst.onClick.AddListener(AddRedFirst);
            _removeRedFirst.onClick.AddListener(RemoveRedFirst);
            _addBlackFirst.onClick.AddListener(AddBlackFirst);
            _removeBlackFirst.onClick.AddListener(RemoveBlackFirst);

            _addScoreSecond.onClick.AddListener(AddScoreSecond);
            _removeScoreSecond.onClick.AddListener(RemoveScoreSecond);
            _styleSecond.onValueChanged.AddListener(SetStyleSecond);

            _addWhiteSecond.onClick.AddListener(AddWhiteSecond);
            _removeWhiteSecond.onClick.AddListener(RemoveWhiteSecond);
            _addYellowSecond.onClick.AddListener(AddYellowSecond);
            _removeYellowSecond.onClick.AddListener(RemoveYellowSecond);
            _addRedSecond.onClick.AddListener(AddRedSecond);
            _removeRedSecond.onClick.AddListener(RemoveRedSecond);
            _addBlackSecond.onClick.AddListener(AddBlackSecond);
            _removeBlackSecond.onClick.AddListener(RemoveBlackSecond);
        }

        private void OnDisable() {
            _updateData.onClick.RemoveAllListeners();
            _nextPouleBtn.onClick.RemoveAllListeners();
            _prevPouleBtn.onClick.RemoveAllListeners();
            _combatSelector.onValueChanged.RemoveAllListeners();

            _addScoreFirst.onClick.RemoveAllListeners();
            _removeScoreFirst.onClick.RemoveAllListeners();
            _styleFirst.onValueChanged.RemoveAllListeners();

            _addWhiteFirst.onClick.RemoveAllListeners();
            _removeWhiteFirst.onClick.RemoveAllListeners();
            _addYellowFirst.onClick.RemoveAllListeners();
            _removeYellowFirst.onClick.RemoveAllListeners();
            _addRedFirst.onClick.RemoveAllListeners();
            _removeRedFirst.onClick.RemoveAllListeners();
            _addBlackFirst.onClick.RemoveAllListeners();
            _removeBlackFirst.onClick.RemoveAllListeners();

            _addScoreSecond.onClick.RemoveAllListeners();
            _removeScoreSecond.onClick.RemoveAllListeners();
            _styleSecond.onValueChanged.RemoveAllListeners();

            _addWhiteSecond.onClick.RemoveAllListeners();
            _removeWhiteSecond.onClick.RemoveAllListeners();
            _addYellowSecond.onClick.RemoveAllListeners();
            _removeYellowSecond.onClick.RemoveAllListeners();
            _addRedSecond.onClick.RemoveAllListeners();
            _removeRedSecond.onClick.RemoveAllListeners();
            _addBlackSecond.onClick.RemoveAllListeners();
            _removeBlackSecond.onClick.RemoveAllListeners();
        }
        #endregion

        private void PrevPouleToTest() {
            _testeablesPoules[_selectedPoule].gameObject.SetActive(false);

            --_selectedPoule;
            if (_selectedPoule < 0) {
                _selectedPoule = _testeablesPoules.Count - 1;
            }

            _testeablesPoules[_selectedPoule].gameObject.SetActive(true);
            _testeablesPoules[_selectedPoule].SetPouleTableData(_pouleDataModel);
        }

        private void NextPouleToTest() {
            _testeablesPoules[_selectedPoule].gameObject.SetActive(false);

            ++_selectedPoule;
            if (_selectedPoule >= _testeablesPoules.Count) {
                _selectedPoule = 0;
            }

            _testeablesPoules[_selectedPoule].gameObject.SetActive(true);
            _testeablesPoules[_selectedPoule].SetPouleTableData(_pouleDataModel);
        }

        private void UpdateData() {
            _testeablesPoules[_selectedPoule].ResetView();

            foreach (Transform child in _allScoreboards.content) {
                Destroy(child.gameObject);
            }

            if (DataManager.Instance.AppData.Poules[0].Matches.Count == 0) {
                DataManager.Instance.AppData.Poules[0].CreateMatches();
            }

            _pouleDataModel = DataManager.Instance.AppData.Poules[0];
            _testeablesPoules[_selectedPoule].SetPouleTableData(_pouleDataModel);

            _scoreboardIndex = 0;

            _combatSelector.ClearOptions();
            for (int i = 0; i < _pouleDataModel.Matches.Count; ++i) {
                _combatSelector.options.Add(new TMP_Dropdown.OptionData((i + 1).ToString()));
            }

            _styleFirst.SetTextWithoutNotify("");
            _styleSecond.SetTextWithoutNotify("");

            _allMatches = new List<ScoreboardQuickView>();
            for (int i = 0; i < _pouleDataModel.Matches.Count; ++i) {
                ScoreboardQuickView newScoreboard = Instantiate(_scoreboardPrefab, _allScoreboards.content);

                AthleteInfoModel firstAthlete = DataManager.Instance.AppData.GetAthleteById(_pouleDataModel.Matches[i].FirstAthlete.AthleteId);
                AthleteInfoModel secondAthlete = DataManager.Instance.AppData.GetAthleteById(_pouleDataModel.Matches[i].SecondAthlete.AthleteId);

                newScoreboard.InitializeCombat("Combat " + (i + 1), "2:30",
                    firstAthlete.GetFullName(), firstAthlete.Country, firstAthlete.Academy,
                    secondAthlete.GetFullName(), secondAthlete.Country, secondAthlete.Academy);

                newScoreboard.SetScoreboard(_pouleDataModel.Matches[i].FirstAthlete.PointsInFavor, _pouleDataModel.Matches[i].SecondAthlete.PointsInFavor);
                newScoreboard.SetStyles(_pouleDataModel.Matches[i].FirstAthlete.StyleAverage(), _pouleDataModel.Matches[i].SecondAthlete.StyleAverage());

                _allMatches.Add(newScoreboard);
            }
        }

        private void SelectCombat(int arg0) {
            _pouleDataModel.Matches[_scoreboardIndex].IsFinished =
                _pouleDataModel.Matches[_scoreboardIndex].FirstAthlete.StyleAverage() != 0 &&
                _pouleDataModel.Matches[_scoreboardIndex].SecondAthlete.StyleAverage() != 0;

            if (_pouleDataModel.Matches[_scoreboardIndex].IsFinished) {
                _testeablesPoules[_selectedPoule].SetPouleTableData(_pouleDataModel);
            }

            _scoreboardIndex = arg0;
        }

        #region First Athlete methods
        private void AddScoreFirst() {
            ++_pouleDataModel.Matches[_scoreboardIndex].FirstAthlete.PointsInFavor;
            ++_pouleDataModel.Matches[_scoreboardIndex].SecondAthlete.PointsAgainst;
            UpdateScoreboard();
        }

        private void RemoveScoreFirst() {
            --_pouleDataModel.Matches[_scoreboardIndex].FirstAthlete.PointsInFavor;
            --_pouleDataModel.Matches[_scoreboardIndex].SecondAthlete.PointsAgainst;
            UpdateScoreboard();
        }

        private void SetStyleFirst(string arg0) {
            arg0 = arg0.Replace(".", ",");
            if (float.TryParse(arg0, out float style)) {
                _pouleDataModel.Matches[_scoreboardIndex].FirstAthlete.StylePoints = new List<float>() { style };
                _allMatches[_scoreboardIndex].SetStyleFirstAthlete(style);
            }
        }

        private void AddWhiteFirst() {
            _allMatches[_scoreboardIndex].AddCardToFirstAthlete(CombatCards.White);
            ++_pouleDataModel.Matches[_scoreboardIndex].FirstAthlete.WhiteCards;
        }
        private void RemoveWhiteFirst() {
            _allMatches[_scoreboardIndex].RemoveCardToFirstAthlete(CombatCards.White);
            --_pouleDataModel.Matches[_scoreboardIndex].FirstAthlete.WhiteCards;
        }
        private void AddYellowFirst() {
            _allMatches[_scoreboardIndex].AddCardToFirstAthlete(CombatCards.Yellow);
            ++_pouleDataModel.Matches[_scoreboardIndex].FirstAthlete.YellowCards;
        }
        private void RemoveYellowFirst() {
            _allMatches[_scoreboardIndex].RemoveCardToFirstAthlete(CombatCards.Yellow);
            --_pouleDataModel.Matches[_scoreboardIndex].FirstAthlete.YellowCards;
        }
        private void AddRedFirst() {
            _allMatches[_scoreboardIndex].AddCardToFirstAthlete(CombatCards.Red);
            _pouleDataModel.Matches[_scoreboardIndex].FirstAthlete.RedCard = true;
        }
        private void RemoveRedFirst() {
            _allMatches[_scoreboardIndex].RemoveCardToFirstAthlete(CombatCards.Red);
            _pouleDataModel.Matches[_scoreboardIndex].FirstAthlete.RedCard = false;
        }
        private void AddBlackFirst() {
            _allMatches[_scoreboardIndex].AddCardToFirstAthlete(CombatCards.Black);
            _pouleDataModel.Matches[_scoreboardIndex].FirstAthlete.BlackCard = true;
        }
        private void RemoveBlackFirst() {
            _allMatches[_scoreboardIndex].RemoveCardToFirstAthlete(CombatCards.Black);
            _pouleDataModel.Matches[_scoreboardIndex].FirstAthlete.BlackCard = false;
        }
        #endregion

        #region Second Athlete methods
        private void AddScoreSecond() {
            ++_pouleDataModel.Matches[_scoreboardIndex].SecondAthlete.PointsInFavor;
            ++_pouleDataModel.Matches[_scoreboardIndex].FirstAthlete.PointsAgainst;
            UpdateScoreboard();
        }

        private void RemoveScoreSecond() {
            --_pouleDataModel.Matches[_scoreboardIndex].SecondAthlete.PointsInFavor;
            --_pouleDataModel.Matches[_scoreboardIndex].FirstAthlete.PointsAgainst;
            UpdateScoreboard();
        }

        private void SetStyleSecond(string arg0) {
            arg0 = arg0.Replace(".", ",");
            if (float.TryParse(arg0, out float style)) {
                _pouleDataModel.Matches[_scoreboardIndex].SecondAthlete.StylePoints = new List<float>() { style };
                _allMatches[_scoreboardIndex].SetStyleSecondAthlete(style);
            }
        }

        private void AddWhiteSecond() {
            _allMatches[_scoreboardIndex].AddCardToSecondAthlete(CombatCards.White);
            ++_pouleDataModel.Matches[_scoreboardIndex].SecondAthlete.WhiteCards;
        }
        private void RemoveWhiteSecond() {
            _allMatches[_scoreboardIndex].RemoveCardToSecondAthlete(CombatCards.White);
            --_pouleDataModel.Matches[_scoreboardIndex].SecondAthlete.WhiteCards;
        }
        private void AddYellowSecond() {
            _allMatches[_scoreboardIndex].AddCardToSecondAthlete(CombatCards.Yellow);
            ++_pouleDataModel.Matches[_scoreboardIndex].SecondAthlete.YellowCards;
        }
        private void RemoveYellowSecond() {
            _allMatches[_scoreboardIndex].RemoveCardToSecondAthlete(CombatCards.Yellow);
            --_pouleDataModel.Matches[_scoreboardIndex].SecondAthlete.YellowCards;
        }
        private void AddRedSecond() {
            _allMatches[_scoreboardIndex].AddCardToSecondAthlete(CombatCards.Red);
            _pouleDataModel.Matches[_scoreboardIndex].SecondAthlete.RedCard = true;
        }
        private void RemoveRedSecond() {
            _allMatches[_scoreboardIndex].RemoveCardToSecondAthlete(CombatCards.Red);
            _pouleDataModel.Matches[_scoreboardIndex].SecondAthlete.RedCard = false;
        }
        private void AddBlackSecond() {
            _allMatches[_scoreboardIndex].AddCardToSecondAthlete(CombatCards.Black);
            _pouleDataModel.Matches[_scoreboardIndex].SecondAthlete.BlackCard = true;
        }
        private void RemoveBlackSecond() {
            _allMatches[_scoreboardIndex].RemoveCardToSecondAthlete(CombatCards.Black);
            _pouleDataModel.Matches[_scoreboardIndex].SecondAthlete.BlackCard = false;
        }
        #endregion

        #region Private methods
        private void UpdateScoreboard() {
            //_basicWithoutStyle.SetCombatScore(_matchesOrder[_scoreboardIndex, 0] - 1, _cScoreFirst[_scoreboardIndex], _matchesOrder[_scoreboardIndex, 1] - 1, _cScoreSecond[_scoreboardIndex]);
            //_basicWithStyle.SetCombatScore(_matchesOrder[_scoreboardIndex, 0] - 1, _cScoreFirst[_scoreboardIndex], _matchesOrder[_scoreboardIndex, 1] - 1, _cScoreSecond[_scoreboardIndex]);

            _allMatches[_scoreboardIndex].SetScoreboard(
                _pouleDataModel.Matches[_scoreboardIndex].FirstAthlete.PointsInFavor,
                _pouleDataModel.Matches[_scoreboardIndex].SecondAthlete.PointsInFavor);
        }
        #endregion
    }
}