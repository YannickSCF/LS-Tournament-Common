/**
 * Author:      Yannick Santa Cruz Feuillias
 * Created:     02/10/2023
 **/

// Dependencies
using System;
using System.Collections.Generic;
using UnityEngine;

namespace YannickSCF.LSTournaments.Common.Models.Matches {
    [Serializable]
    public class MatchAthleteModel {
        [SerializeField] private string _athleteId;

        [SerializeField] private int _pointsInFavor;
        [SerializeField] private int _pointsAgainst;

        [SerializeField] private List<float> _stylePoints;

        [SerializeField] private int _whiteCards;
        [SerializeField] private int _yellowCards;
        [SerializeField] private bool _redCard;
        [SerializeField] private bool _blackCard;

        public MatchAthleteModel(string athleteId) {
            _athleteId = athleteId;

            _stylePoints = new List<float>();
        }

        public string AthleteId { get => _athleteId; }
        public int PointsInFavor { get => _pointsInFavor; set => _pointsInFavor = value; }
        public int PointsAgainst { get => _pointsAgainst; set => _pointsAgainst = value; }
        public List<float> StylePoints { get => _stylePoints; set => _stylePoints = value; }
        public int WhiteCards { get => _whiteCards; set => _whiteCards = value; }
        public int YellowCards { get => _yellowCards; set => _yellowCards = value; }
        public bool RedCard { get => _redCard; set => _redCard = value; }
        public bool BlackCard { get => _blackCard; set => _blackCard = value; }

        public float StyleAverage() {
            float totalStyle = 0f;
            foreach (float points in _stylePoints) {
                totalStyle += points;
            }

            if (totalStyle == 0)
                return 0f;
            return totalStyle / _stylePoints.Count;
        }

        public AthleteCards GetCards() {
            AthleteCards res;

            res.White = _whiteCards;
            res.Yellow = _yellowCards;
            res.Red = _redCard;
            res.Black = _blackCard;

            return res;
        }
    }
}
