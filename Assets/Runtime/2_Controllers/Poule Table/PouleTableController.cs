/**
 * Author:      Yannick Santa Cruz Feuillias
 * Created:     15/02/2024
 **/

// Dependencies
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using YannickSCF.LSTournaments.Common.Models.Matches;
//Custom dependencies
using YannickSCF.LSTournaments.Common.Models.Poules;
using YannickSCF.LSTournaments.Common.Views.PouleTable;

namespace YannickSCF.LSTournaments.Common.Controllers.PouleTable {

    public class PouleTableController : MonoBehaviour {

        [SerializeField] private PouleTableView _view;

        private PouleDataModel _pouleData;

        private List<PouleAtheleteData> _allAthletesData;

        public void SetPouleTableData(PouleDataModel pouleData) {
            _pouleData = pouleData;
            _view.SetPouleTitle(_pouleData.Name);

            _allAthletesData = new List<PouleAtheleteData>();
            List<AthleteBasicInfo> allBasicInfo = new List<AthleteBasicInfo>();
            foreach (string athleteId in _pouleData.AthletesIds) {
                allBasicInfo.Add(DataManager.Instance.AppData.GetAthleteBasicInfoById(athleteId));
                _allAthletesData.Add(new PouleAtheleteData(athleteId));
            }
            _view.SetAthletes(allBasicInfo);

            foreach (MatchModel match in _pouleData.Matches) {
                if (match.IsFinished) {
                    PouleAtheleteData firstAthlete = _allAthletesData.FirstOrDefault(x => x.Id.Equals(match.FirstAthlete.AthleteId));
                    PouleAtheleteData secondAthlete = _allAthletesData.FirstOrDefault(x => x.Id.Equals(match.SecondAthlete.AthleteId));

                    firstAthlete.AddResult(match.FirstAthlete.PointsInFavor, match.FirstAthlete.PointsAgainst, match.FirstAthlete.StyleAverage());
                    secondAthlete.AddResult(match.SecondAthlete.PointsInFavor, match.SecondAthlete.PointsAgainst, match.SecondAthlete.StyleAverage());
                    //
                    _view.SetAthleteScoreAgainst(_allAthletesData.IndexOf(firstAthlete), _allAthletesData.IndexOf(secondAthlete), match.FirstAthlete.PointsInFavor);
                    _view.SetAthleteStyleAgainst(_allAthletesData.IndexOf(firstAthlete), _allAthletesData.IndexOf(secondAthlete), match.FirstAthlete.StyleAverage());
                    //
                    _view.SetAthleteScoreAgainst(_allAthletesData.IndexOf(secondAthlete), _allAthletesData.IndexOf(firstAthlete), match.SecondAthlete.PointsInFavor);
                    _view.SetAthleteStyleAgainst(_allAthletesData.IndexOf(secondAthlete), _allAthletesData.IndexOf(firstAthlete), match.SecondAthlete.StyleAverage());
                }
            }

            for (int i = 0; i < _allAthletesData.Count; ++i) {
                _allAthletesData[i].CalculateScore();

                _view.SetVictories(i, _allAthletesData[i].Victories);
                _view.SetDefeats(i, _allAthletesData[i].Defeats);
                _view.SetTies(i, _allAthletesData[i].Ties);
                _view.SetOHInFavor(i, _allAthletesData[i].OhInFavor);
                _view.SetOHAgainst(i, _allAthletesData[i].OhAgainst);
                _view.SetTotalStyle(i, _allAthletesData[i].TotalStyle);
                _view.SetLSScore(i, _allAthletesData[i].Score);
            }
        }

        public void ResetView() {
            _view.ResetAllScores();
        }

        private class PouleAtheleteData {
            private string _id;
            private int _victories;
            private int _defeats;
            private int _ties;
            private int _ohInFavor;
            private int _ohAgainst;
            private float _totalStyle;
            private float _score;

            public PouleAtheleteData(string id) {
                _id = id;
            }

            #region Properties
            public string Id { get => _id; }
            public int Victories { get => _victories; }
            public int Defeats { get => _defeats; }
            public int Ties { get => _ties; }
            public int OhInFavor { get => _ohInFavor; }
            public int OhAgainst { get => _ohAgainst; }
            public float TotalStyle { get => _totalStyle; }
            public float Score { get => _score; }
            #endregion

            public void AddResult(int ohInFavor, int ohAgainst, float style) {
                if (ohInFavor == ohAgainst) ++_ties;
                else if (ohInFavor > ohAgainst) ++_victories;
                else ++_defeats;

                _ohInFavor += ohInFavor;
                _ohAgainst += ohAgainst;

                _totalStyle += style;
            }

            public void CalculateScore() {
                float totalMatches = _victories + _defeats + _ties;
                _score = _ohInFavor + (_totalStyle / 10 / totalMatches);
                _score = (float)(Math.Truncate(_score * 10000) / 10000);
            }
        }
    }
}
