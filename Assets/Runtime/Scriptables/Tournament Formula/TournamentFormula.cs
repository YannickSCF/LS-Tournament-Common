using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace YannickSCF.LSTournaments.Common.Scriptables.Formulas {
    [CreateAssetMenu(fileName = "Tournament Formula", menuName = "YannickSCF/LS Tournaments/New Tournament Formula")]
    public class TournamentFormula : ScriptableObject {

        [SerializeField]
        private string _formulaName = "New Tournament Formula";
        public string FormulaName { get => _formulaName; }
        internal void SetFormulaName(string newName) { _formulaName = newName; }

        // ---------------------------------------------- POULES

        [SerializeField]
        private bool _infinitePoules;
        public bool InfinitePoules { get => _infinitePoules; }
        internal void SetInfinitePoules(bool isInfinite) { _infinitePoules = isInfinite; }


        [SerializeField]
        private List<int> _possibleNumberOfPoules;
        public List<int> PossibleNumberOfPoules { get => _possibleNumberOfPoules; }
        internal void SetNumberOfPoules(List<int> newNumberOfPoules) { _possibleNumberOfPoules = newNumberOfPoules; }


        [SerializeField]
        private int _minPouleSize = 4;
        public int MinPouleSize { get => _minPouleSize; }
        internal void SetMinPouleSize(int newMinPouleSize) { _minPouleSize = newMinPouleSize; }


        [SerializeField]
        private int _maxPouleSize = 8;
        public int MaxPouleSize { get => _maxPouleSize; }
        internal void SetMaxPouleSize(int newMaxPouleSize) { _maxPouleSize = newMaxPouleSize; }

        // ---------------------------------------------- FILLER

        [SerializeField]
        private PouleFillerType _fillerType;
        public PouleFillerType FillerType { get => _fillerType; }
        internal void SetFillerType(PouleFillerType newFillerType) { _fillerType = newFillerType; }

        // ---------------------------------------------- SCORING

        [SerializeField]
        private ScoringType _scoring;
        public ScoringType Scoring { get => _scoring; }
        internal void SetScoring(ScoringType newScoring) { _scoring = newScoring; }

        // ---------------------------------------------- ELIMINATION PHASE

        [SerializeField]
        private ClassificationType _classification;
        public ClassificationType Classification { get => _classification; }
        internal void SetClassification(ClassificationType newClassification) { _classification = newClassification; }


        [SerializeField]
        private EliminationRound _maxEliminationPhases;
        public EliminationRound MaxEliminationPhases { get => _maxEliminationPhases; }
        internal void SetMaxEliminationPhases(EliminationRound newMaxEliminationPhases) { _maxEliminationPhases = newMaxEliminationPhases; }

        // ---------------------------------------------- RANKINGS

        [SerializeField]
        private StyleRankingType _styleSorter;
        public StyleRankingType StyleSorter { get => _styleSorter; }
        internal void SetStyleSorter(StyleRankingType newStyleSorter) { _styleSorter = newStyleSorter; }


        [SerializeField]
        private WarRankingType _warSorter;
        public WarRankingType WarSorter { get => _warSorter; }
        internal void SetWarSorter(WarRankingType newWarSorter) { _warSorter = newWarSorter; }


        [SerializeField]
        private MixedRankingType _mixedSorter;
        public MixedRankingType MixedSorter { get => _mixedSorter; }
        internal void SetMixedSorter(MixedRankingType newMixedSorter) { _mixedSorter = newMixedSorter; }
    }
}
