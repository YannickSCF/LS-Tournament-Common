// Dependencies
using System.Collections.Generic;
using UnityEngine;

namespace YannickSCF.LSTournaments.Common.Scriptables.Formulas {
    [CreateAssetMenu(fileName = "Tournament Formula", menuName = "YannickSCF/LS Tournaments/New Tournament Formula")]
    public class TournamentFormula : ScriptableObject {

        [SerializeField] private string _formulaName = "New Tournament Formula";

        // ---------------------------------------------- POULES

        [SerializeField] private bool _infinitePoules;
        [SerializeField] private List<int> _possibleNumberOfPoules;
        [SerializeField] private int _minPouleSize = 4;
        [SerializeField] private int _maxPouleSize = 8;

        // ---------------------------------------------- FILLER

        [SerializeField] private PouleFillerType _fillerType;

        // ---------------------------------------------- SCORING

        [SerializeField] private ScoringType _scoring;

        // ---------------------------------------------- ELIMINATION PHASE

        [SerializeField] private ClassificationType _classification;
        [SerializeField] private EliminationRound _maxEliminationPhases;

        // ---------------------------------------------- RANKINGS

        [SerializeField] private StyleRankingType _styleSorter;
        [SerializeField] private WarRankingType _warSorter;
        [SerializeField] private MixedRankingType _mixedSorter;


        #region PROPERTIES
        public string FormulaName { get => _formulaName; }
        internal void SetFormulaName(string newName) { _formulaName = newName; }

        // ---------------------------------------------- POULES

        public bool InfinitePoules { get => _infinitePoules; }
        internal void SetInfinitePoules(bool isInfinite) { _infinitePoules = isInfinite; }

        public List<int> PossibleNumberOfPoules { get => _possibleNumberOfPoules; }
        internal void SetNumberOfPoules(List<int> newNumberOfPoules) { _possibleNumberOfPoules = newNumberOfPoules; }

        public int MinPouleSize { get => _minPouleSize; }
        internal void SetMinPouleSize(int newMinPouleSize) { _minPouleSize = newMinPouleSize; }

        public int MaxPouleSize { get => _maxPouleSize; }
        internal void SetMaxPouleSize(int newMaxPouleSize) { _maxPouleSize = newMaxPouleSize; }

        // ---------------------------------------------- FILLER

        public PouleFillerType FillerType { get => _fillerType; }
        internal void SetFillerType(PouleFillerType newFillerType) { _fillerType = newFillerType; }

        // ---------------------------------------------- SCORING

        public ScoringType Scoring { get => _scoring; }
        internal void SetScoring(ScoringType newScoring) { _scoring = newScoring; }

        // ---------------------------------------------- ELIMINATION PHASE

        public ClassificationType Classification { get => _classification; }
        internal void SetClassification(ClassificationType newClassification) { _classification = newClassification; }

        public EliminationRound MaxEliminationPhases { get => _maxEliminationPhases; }
        internal void SetMaxEliminationPhases(EliminationRound newMaxEliminationPhases) { _maxEliminationPhases = newMaxEliminationPhases; }

        // ---------------------------------------------- RANKINGS

        public StyleRankingType StyleSorter { get => _styleSorter; }
        internal void SetStyleSorter(StyleRankingType newStyleSorter) { _styleSorter = newStyleSorter; }

        public WarRankingType WarSorter { get => _warSorter; }
        internal void SetWarSorter(WarRankingType newWarSorter) { _warSorter = newWarSorter; }


        public MixedRankingType MixedSorter { get => _mixedSorter; }
        internal void SetMixedSorter(MixedRankingType newMixedSorter) { _mixedSorter = newMixedSorter; }
        #endregion
    }
}
