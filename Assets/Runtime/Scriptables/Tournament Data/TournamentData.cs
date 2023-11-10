/**
 * Author:      Yannick Santa Cruz Feuillias
 * Created:     02/10/2023
 **/

// Dependencies
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
// Custom dependencies
using YannickSCF.LSTournaments.Common.Models.Athletes;
using YannickSCF.LSTournaments.Common.Models.Matches;
using YannickSCF.LSTournaments.Common.Models.Poules;
using YannickSCF.LSTournaments.Common.Scriptables.Formulas;
using YannickSCF.LSTournaments.Common.Tools.Poule;

namespace YannickSCF.LSTournaments.Common.Scriptables.Data {

    [Serializable]
    public struct RoundCombats {
        public EliminationRound Round;
        public List<MatchModel> Matches;

        public RoundCombats(EliminationRound round, List<MatchModel> matches) {
            Round = round;
            Matches = matches;
        }
    }

    [Serializable]
    public struct AthleteInfoUsed {
        public AthleteInfoType Info;
        public bool IsUsed;

        public AthleteInfoUsed(AthleteInfoType info, bool isUsed) {
            Info = info;
            IsUsed = isUsed;
        }
    }

    [CreateAssetMenu(fileName = "Tournament Data", menuName = "YannickSCF/LS Tournaments/New Tournament Data")]
    public class TournamentData : ScriptableObject {
        [Header("------ Basic Tournament data ------")]
        [SerializeField] private string _tournamentName;
        [SerializeField] private TournamentType _tournamentType;
        [SerializeField] private string _tournamentFormulaName;
        [SerializeField] private int _seed;

        #region Basic Tournament Data Properties
        public string TournamentName { get => _tournamentName; set => _tournamentName = value; }
        public TournamentType TournamentType { get => _tournamentType; set => _tournamentType = value; }
        public string TournamentFormulaName { get => _tournamentFormulaName; set => _tournamentFormulaName = value; }
        public int Seed { get => _seed; set => _seed = value; }
        #endregion



        // ------------------------------------------------------------------------------------------------------



        [Space(10), Header("------ Poules Tournament data ------")]
        [SerializeField] private PouleNamingType _namingInfo;
        [SerializeField] private int _roundsOfPoules;

        [SerializeField] private PouleFillerType _fillerTypeInfo;
        [SerializeField] private PouleFillerSubtype _fillerSubtypeInfo;

        [SerializeField] private Vector2 _pouleCountMaxSize;
        [SerializeField] private Vector2 _pouleCountMinSize;

        [SerializeField] private List<PouleDataModel> _data;

        #region Poule Tournament Data Properties
        public PouleNamingType NamingInfo { get => _namingInfo; set => _namingInfo = value; }
        public int RoundsOfPoules { get => _roundsOfPoules; set => _roundsOfPoules = value; }

        public PouleFillerType FillerTypeInfo {
            get {
                if (_fillerTypeInfo == PouleFillerType.TBD) {
                    TournamentFormula formula = TournamentFormulaUtils.GetFormulaByName(_tournamentFormulaName);
                    if (formula != null && formula.FillerType != PouleFillerType.TBD) {
                        _fillerTypeInfo = formula.FillerType;
                    }
                }

                return _fillerTypeInfo;
            }
            set {
                TournamentFormula formula = TournamentFormulaUtils.GetFormulaByName(_tournamentFormulaName);
                if (formula != null && formula.FillerType == PouleFillerType.TBD) {
                    _fillerTypeInfo = value;
                } else {
                    Debug.LogWarning($"The formula '{_tournamentFormulaName}' doesn't exists or cannot set Poule Filler Type");
                }
            }
        }
        public PouleFillerSubtype FillerSubtypeInfo { get => _fillerSubtypeInfo; set => _fillerSubtypeInfo = value; }

        public int[,] PouleCountAndSizes {
            get {
                int[,] countAndSize = new int[2, 2];
                countAndSize[0, 0] = (int)_pouleCountMaxSize.x;
                countAndSize[0, 1] = (int)_pouleCountMaxSize.y;
                countAndSize[1, 0] = (int)_pouleCountMinSize.x;
                countAndSize[1, 1] = (int)_pouleCountMinSize.y;
                return countAndSize;
            }
            set {
                _pouleCountMaxSize.x = value[0, 0];
                _pouleCountMaxSize.y = value[0, 1];
                _pouleCountMinSize.x = value[1, 0];
                _pouleCountMinSize.y = value[1, 1];
            }
        }

        public List<PouleDataModel> Data { get => _data; set => _data = value; }

        #endregion

        #region Poule Tournament Data Methods
        public PouleNamingObject? GetNamingData() {
            if (GetPouleCount() <= 0) {
                return null;
            }

            switch (NamingInfo) {
                default:
                case PouleNamingType.Letters:
                    return new PouleNamingObject(NamingInfo, GetPouleCount(), RoundsOfPoules);
                case PouleNamingType.Numbers:
                    return new PouleNamingObject(NamingInfo, GetPouleCount());
            }
        }

        public List<PouleFillerType> GetFillerTypesCannotBeUsed() {
            List<PouleFillerType> result = new List<PouleFillerType>();
            if (!AthletesInfoUsed[AthleteInfoType.Rank]) {
                result.Add(PouleFillerType.ByRank);
            }

            if (!AthletesInfoUsed[AthleteInfoType.Styles]) {
                result.Add(PouleFillerType.ByStyle);
            }

            if (!AthletesInfoUsed[AthleteInfoType.Tier]) {
                result.Add(PouleFillerType.ByTier);
            }

            return result;
        }
        public List<PouleFillerSubtype> GetFillerSubtypesCannotBeUsed() {
            List<PouleFillerSubtype> result = new List<PouleFillerSubtype>();
            if (!AthletesInfoUsed[AthleteInfoType.Country]) {
                result.Add(PouleFillerSubtype.Country);
            }

            if (!AthletesInfoUsed[AthleteInfoType.Academy]) {
                result.Add(PouleFillerSubtype.Academy);
            }

            if (!AthletesInfoUsed[AthleteInfoType.School]) {
                result.Add(PouleFillerSubtype.School);
            }

            return result;
        }


        public int GetPouleCount() {
            if (_pouleCountMaxSize != null && _pouleCountMinSize == null) {
                return (int)_pouleCountMaxSize.x;
            }

            if (_pouleCountMaxSize != null && _pouleCountMinSize != null) {
                return (int)(_pouleCountMaxSize.x + _pouleCountMinSize.x);
            }
            return -1;
        }
        public int GetPouleMaxSize() {
            if (_pouleCountMaxSize == null) return -1;
            return (int)_pouleCountMaxSize.y;
        }
        public int GetPouleMinSize() {
            if (_pouleCountMinSize == null) return -1;
            return (int)_pouleCountMinSize.y;
        }
        #endregion



        // ------------------------------------------------------------------------------------------------------



        [Space(10), Header("------ Elimination Phase Tournament data ------")]
        [SerializeField] private List<RoundCombats> _eliminationBracket;

        #region Elimination Phase Tournament Data Properties
        public Dictionary<EliminationRound, List<MatchModel>> EliminationBracket {
            get {
                Dictionary<EliminationRound, List<MatchModel>> result = new Dictionary<EliminationRound, List<MatchModel>>();
                foreach (RoundCombats round in _eliminationBracket) {
                    result.Add(round.Round, round.Matches);
                }

                return result;
            }
            set {
                if (_eliminationBracket == null) {
                    _eliminationBracket = new List<RoundCombats>();
                } else {
                    _eliminationBracket.Clear();
                }

                foreach (KeyValuePair<EliminationRound, List<MatchModel>> round in value) {
                    RoundCombats roundCombats = new RoundCombats(round.Key, round.Value);
                    _eliminationBracket.Add(roundCombats);
                }
            }
        }
        #endregion

        
        
        // ------------------------------------------------------------------------------------------------------



        [Space(10), Header("------ Athletes Tournament data ------")]
        [SerializeField] private List<AthleteInfoUsed> _athletesInfoUsed;
        [SerializeField] private List<AthleteInfoModel> _athletes;
        [SerializeField] private List<AthleteTournamentStatsModel> _athletesStats;

        #region Athletes Tournament Data Properties
        public Dictionary<AthleteInfoType, bool> AthletesInfoUsed {
            get {
                if (_athletesInfoUsed == null || _athletesInfoUsed.Count != Enum.GetValues(typeof(AthleteInfoType)).Length) {
                    _athletesInfoUsed = new List<AthleteInfoUsed>();
                    Array infoTypes = Enum.GetValues(typeof(AthleteInfoType));
                    foreach (Enum infoType in infoTypes) {
                        AthleteInfoUsed infoUsed = new AthleteInfoUsed((AthleteInfoType)infoType, true);
                        _athletesInfoUsed.Add(infoUsed);
                    }
                }

                Dictionary<AthleteInfoType, bool> result = new Dictionary<AthleteInfoType, bool>();
                foreach (AthleteInfoUsed athleteInfoUsed in _athletesInfoUsed) {
                    result.Add(athleteInfoUsed.Info, athleteInfoUsed.IsUsed);
                }

                return result;
            }
            set {
                if (_athletesInfoUsed == null) {
                    _athletesInfoUsed = new List<AthleteInfoUsed>();
                } else {
                    _athletesInfoUsed.Clear();
                }

                foreach (KeyValuePair<AthleteInfoType, bool> infoUsed in value) {
                    AthleteInfoUsed athleteInfoUsed = new AthleteInfoUsed(infoUsed.Key, infoUsed.Value);
                    _athletesInfoUsed.Add(athleteInfoUsed);
                }
            }
        }
        public List<AthleteInfoModel> Athletes { get => _athletes; set => _athletes = value; }
        public List<AthleteTournamentStatsModel> AthletesStats { get => _athletesStats; set => _athletesStats = value; }
        #endregion

        #region Athletes Tournament Data Methods
        public AthleteInfoModel GetAthleteById(string athleteId) {
            return _athletes.FirstOrDefault(x => x.Id == athleteId);
        }
        #endregion

        public void ResetData() {
            _tournamentName = string.Empty;
            _tournamentType = TournamentType.Unrated;
            _tournamentFormulaName = string.Empty;
            _seed = 0;
            // ---------------------
            _namingInfo = PouleNamingType.Letters;
            _roundsOfPoules = 1;
            
            _fillerTypeInfo = PouleFillerType.TBD;
            _fillerSubtypeInfo = PouleFillerSubtype.None;
            
            _pouleCountMaxSize = Vector2.zero;
            _pouleCountMinSize = Vector2.zero;
            
            _data.Clear();
            // ---------------------
            _eliminationBracket.Clear();
            // ---------------------
            _athletesInfoUsed = null;
            _ = AthletesInfoUsed;

            _athletes.Clear();
            _athletesStats.Clear();
        }
    }
}
