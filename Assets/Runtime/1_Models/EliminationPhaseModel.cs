// Dependencies
using System;
using System.Collections.Generic;
using UnityEngine;
using YannickSCF.LSTournaments.Common.Models.Matches;

namespace YannickSCF.LSTournaments.Common.Models {
    [Serializable]
    public class EliminationPhaseModel {
        [SerializeField] private ClassificationType _classification;
        [SerializeField] private EliminationRound _maxEliminationRound;

        [SerializeField] private Dictionary<EliminationRound, List<MatchModel>> _eliminationBracket;

        public EliminationPhaseModel(
            ClassificationType classification,
            EliminationRound maxEliminationRound) {
            _classification = classification;
            _maxEliminationRound = maxEliminationRound;
        }

        public EliminationPhaseModel(
            ClassificationType classification,
            EliminationRound maxEliminationRound,
            Dictionary<EliminationRound, List<MatchModel>> eliminationBracket) {
            _classification = classification;
            _maxEliminationRound = maxEliminationRound;
            _eliminationBracket = eliminationBracket;
        }

        public ClassificationType Classification { get => _classification; }
        public EliminationRound MaxEliminationRound { get => _maxEliminationRound; }
        public Dictionary<EliminationRound, List<MatchModel>> EliminationBracket { get => _eliminationBracket; }

        public void CreateEliminationBracket() {
            // TODO
        }
    }
}
