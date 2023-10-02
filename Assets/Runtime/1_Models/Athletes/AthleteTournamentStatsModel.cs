// Dependencies
using System;
using UnityEngine;

namespace YannickSCF.LSTournaments.Common.Models.Athletes {
    [Serializable]
    public class AthleteTournamentStatsModel {
        [SerializeField] private string _athleteId;
        [SerializeField] private byte _wonCombats;
        [SerializeField] private byte _lostCombats;
        [SerializeField] private byte _tiedCombats;

        [SerializeField] private byte _pointsInFavor;
        [SerializeField] private byte _pointsAgainst;

        [SerializeField] private float _totalStylePoints;

        [SerializeField] private TournamentPhase _lastTournamentPhase;
        [SerializeField] private EliminationRound _lastEliminationBracket;

        public AthleteTournamentStatsModel(string athleteId) {
            _athleteId = athleteId;
        }

        public AthleteTournamentStatsModel(string athleteId,
            byte wonCombats, byte lostCombats, byte tiedCombats,
            byte pointsInFavor, byte pointsAgainst,
            float totalStylePoints,
            TournamentPhase lastTournamentPhase,
            EliminationRound lastEliminationBracket) {
            _athleteId = athleteId;
            _wonCombats = wonCombats;
            _lostCombats = lostCombats;
            _tiedCombats = tiedCombats;
            _pointsInFavor = pointsInFavor;
            _pointsAgainst = pointsAgainst;
            _totalStylePoints = totalStylePoints;
            _lastTournamentPhase = lastTournamentPhase;
            _lastEliminationBracket = lastEliminationBracket;
        }

        public string AthleteId { get => _athleteId; }
        public byte WonCombats { get => _wonCombats; set => _wonCombats = value; }
        public byte LostCombats { get => _lostCombats; set => _lostCombats = value; }
        public byte TiedCombats { get => _tiedCombats; set => _tiedCombats = value; }
        public byte PointsInFavor { get => _pointsInFavor; set => _pointsInFavor = value; }
        public byte PointsAgainst { get => _pointsAgainst; set => _pointsAgainst = value; }
        public float TotalStylePoints { get => _totalStylePoints; set => _totalStylePoints = value; }
        public TournamentPhase LastTournamentPhase { get => _lastTournamentPhase; set => _lastTournamentPhase = value; }
        public EliminationRound LastEliminationBracket { get => _lastEliminationBracket; set => _lastEliminationBracket = value; }
    }
}
