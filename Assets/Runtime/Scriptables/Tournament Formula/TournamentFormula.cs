using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace YannickSCF.LSTournaments.Common.Scriptables.Formulas {
    [CreateAssetMenu(fileName = "Tournament Formula", menuName = "YannickSCF/LS Tournaments/New Tournament Formula")]
    public class TournamentFormula : ScriptableObject {
        
        private string _formulaName = "New Tournament Formula";

        private bool _infinitePoules;
        private List<int> _possibleNumberOfPoules;
        private int _minPouleSize;
        private int _maxPouleSize;

        private PouleFillerType _type;

        private ScoringType _scoring;

        private ClassificationType _classification;
        private EliminationRound _maxEliminationPhases;

        private StyleRankingType _styleSorter;
        private WarRankingType _warSorter;
        private MixedRankingType _mixedSorter;

        public string FormulaName { get => _formulaName; }

        public bool InfinitePoules { get => _infinitePoules; }
        public List<int> PossibleNumberOfPoules { get => _possibleNumberOfPoules; }
        public int MinPouleSize { get => _minPouleSize; }
        public int MaxPouleSize { get => _maxPouleSize; }

        public PouleFillerType Type { get => _type; }

        public ScoringType Scoring { get => _scoring; }

        public ClassificationType Classification { get => _classification; }
        public EliminationRound MaxEliminationPhases { get => _maxEliminationPhases; }

        public StyleRankingType StyleSorter { get => _styleSorter; }
        public WarRankingType WarSorter { get => _warSorter; }
        public MixedRankingType MixedSorter { get => _mixedSorter; }

        public int[,] GetPouleSizes(int numberOfParticipants) {

            return null;
        }
    }
}
