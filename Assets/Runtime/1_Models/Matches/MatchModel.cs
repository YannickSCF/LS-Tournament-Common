// Dependencies
using System;
using UnityEngine;

namespace YannickSCF.LSTournaments.Common.Models.Matches {
    [Serializable]
    public class MatchModel {
        [SerializeField] private MatchType _matchType;

        [SerializeField] private MatchAthleteModel _firstAthlete;
        [SerializeField] private MatchAthleteModel _secondAthlete;

        public MatchModel(MatchType type, string firstAthleteId, string secondAthleteId) {
            _matchType = type;
            _firstAthlete = new MatchAthleteModel(firstAthleteId);
            _secondAthlete = new MatchAthleteModel(secondAthleteId);
        }

        public MatchModel(MatchType matchType, MatchAthleteModel firstAthlete, MatchAthleteModel secondAthlete) {
            _matchType = matchType;
            _firstAthlete = firstAthlete;
            _secondAthlete = secondAthlete;
        }

        public MatchType MatchType { get => _matchType; }

        public MatchAthleteModel FirstAthlete { get => _firstAthlete; }
        public MatchAthleteModel SecondAthlete { get => _secondAthlete; }

        public bool IsATie() {
            return _firstAthlete.PointsInFavor == _secondAthlete.PointsInFavor;
        }

        public string GetWinner() {
            if (IsATie()) return null;
            return _firstAthlete.PointsInFavor > _secondAthlete.PointsInFavor ?
                _firstAthlete.AthleteId : _secondAthlete.AthleteId;
        }

        public string GetLoser() {
            if (IsATie()) return null;
            return _firstAthlete.PointsInFavor > _secondAthlete.PointsInFavor ?
                _secondAthlete.AthleteId : _firstAthlete.AthleteId;
        }
    }

    [Serializable]
    public class MatchType {
        [SerializeField] private TournamentPhase _phase;
        [SerializeField] private EliminationRound _round;

        public MatchType() {
            _phase = TournamentPhase.Poules;
        }

        public MatchType(EliminationRound round) {
            _phase = TournamentPhase.EliminationPhase;
            _round = round;
        }

        public MatchType(TournamentPhase phase, EliminationRound round) {
            _phase = phase;
            _round = round;
        }

        public TournamentPhase Phase { get => _phase; }
        public EliminationRound Round { get => _round; }
    }
}
