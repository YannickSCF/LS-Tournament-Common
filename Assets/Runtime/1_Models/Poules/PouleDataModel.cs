/**
 * Author:      Yannick Santa Cruz Feuillias
 * Created:     21/09/2023
 **/

// Dependencies
using System;
using System.Collections.Generic;
using UnityEngine;
// Custom dependencies
using YannickSCF.LSTournaments.Common.Models.Matches;
using YannickSCF.LSTournaments.Common.Tools.Poule;

namespace YannickSCF.LSTournaments.Common.Models.Poules {
    [System.Serializable]
    public class PouleDataModel {
        [SerializeField] private string _name;
        [SerializeField] private List<string> _athletesIds;
        [SerializeField] private List<MatchModel> _matches;

        #region Properties
        public string Name { get => _name; }
        public List<string> AthletesIds { get => _athletesIds; }
        public List<MatchModel> Matches { get => _matches; }
        #endregion

        #region Constuctors
        public PouleDataModel(string name) {
            _name = name;
            _athletesIds = new List<string>();
        }

        public PouleDataModel(string name, List<string> athletesIds) {
            _name = name;
            _athletesIds = athletesIds;
        }

        public PouleDataModel(string name, List<string> athletesIds, List<MatchModel> matches) {
            _name = name;
            _athletesIds = athletesIds;
            _matches = matches;
        }
        #endregion

        public void CreateMatches() {
            int[,] matchesOrder = PouleUtils.GetMatchesOrder(_athletesIds.Count);
            _matches = new List<MatchModel>();

            for(int i = 0; i < matchesOrder.GetLength(0); ++i) {
                string firstAthleteId = _athletesIds[matchesOrder[i, 0] - 1];
                string secondAthleteId = _athletesIds[matchesOrder[i, 1] - 1];

                MatchModel newMatch = new MatchModel(new MatchType(), firstAthleteId, secondAthleteId);
                _matches.Add(newMatch);
            }
        }
    }
}
