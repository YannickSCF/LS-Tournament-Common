using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using YannickSCF.LSTournaments.Common.Models.Athletes;
using YannickSCF.LSTournaments.Common.Models.Matches;
using YannickSCF.LSTournaments.Common.Models.Poules;
using YannickSCF.LSTournaments.Common.Tools.Poule;

namespace YannickSCF.LSTournaments.Common.Scriptables.Data {
    [CreateAssetMenu(fileName = "Tournament Data", menuName = "YannickSCF/LS Tournaments/New Tournament Data")]
    public class TournamentData : ScriptableObject {
        [SerializeField] private string _tournamentName;
        [SerializeField] private TournamentType _tournamentType;
        [SerializeField] private string _tournamentFormulaName;

        [SerializeField] private int _seed;

        [SerializeField] private PouleInfoModel _pouleInfo;

        [SerializeField] private Dictionary<EliminationRound, List<MatchModel>> _eliminationBracket;

        [SerializeField] private Dictionary<AthleteInfoType, bool> _athletesInfoUsed;
        [SerializeField] private List<AthleteInfoModel> _athletes;
        [SerializeField] private List<AthleteTournamentStatsModel> _athletesStats;

        public string TournamentName { get => _tournamentName; set => _tournamentName = value; }
        public TournamentType TournamentType { get => _tournamentType; set => _tournamentType = value; }
        public string TournamentFormulaName { get => _tournamentFormulaName; set => _tournamentFormulaName = value; }

        public int Seed { get => _seed; set => _seed = value; }

        public PouleInfoModel PouleInfo { get => _pouleInfo; set => _pouleInfo = value; }

        public Dictionary<EliminationRound, List<MatchModel>> EliminationBracket { get => _eliminationBracket; set => _eliminationBracket = value; }

        public Dictionary<AthleteInfoType, bool> AthletesInfoUsed {
            get {
                if (_athletesInfoUsed == null || _athletesInfoUsed.Count != Enum.GetValues(typeof(AthleteInfoType)).Length) {
                    _athletesInfoUsed = new Dictionary<AthleteInfoType, bool>();
                    Array infoTypes = Enum.GetValues(typeof(AthleteInfoType));
                    foreach (Enum infoType in infoTypes) {
                        _athletesInfoUsed.Add((AthleteInfoType)infoType, true);
                    }
                }
                return _athletesInfoUsed;
            }
            set => _athletesInfoUsed = value;
        }

        public List<AthleteInfoModel> Athletes { get => _athletes; set => _athletes = value; }
        public List<AthleteTournamentStatsModel> AthletesStats { get => _athletesStats; set => _athletesStats = value; }

        public AthleteInfoModel GetAthleteById(string athleteId) {
            return _athletes.FirstOrDefault(x => x.Id == athleteId);
        }

        public List<PouleFillerType> GetFillerTypesCannotBeUsed() {
            List<PouleFillerType> result = new List<PouleFillerType>();
            if (!_athletesInfoUsed[AthleteInfoType.Rank]) {
                result.Add(PouleFillerType.ByRank);
            }

            if (!_athletesInfoUsed[AthleteInfoType.Styles]) {
                result.Add(PouleFillerType.ByStyle);
            }

            if (!_athletesInfoUsed[AthleteInfoType.Tier]) {
                result.Add(PouleFillerType.ByTier);
            }

            return result;
        }

        public List<PouleFillerSubtype> GetFillerSubtypesCannotBeUsed() {
            List<PouleFillerSubtype> result = new List<PouleFillerSubtype>();
            if (!_athletesInfoUsed[AthleteInfoType.Country]) {
                result.Add(PouleFillerSubtype.Country);
            }

            if (!_athletesInfoUsed[AthleteInfoType.Academy]) {
                result.Add(PouleFillerSubtype.Academy);
            }

            if (!_athletesInfoUsed[AthleteInfoType.School]) {
                result.Add(PouleFillerSubtype.School);
            }

            return result;
        }

        public PouleNamingObject? GetNamingData() {
            if (_pouleInfo.GetPouleCount() <= 0) {
                return null;
            }

            switch (_pouleInfo.NamingInfo) {
                default:
                case PouleNamingType.Letters:
                    return new PouleNamingObject(_pouleInfo.NamingInfo, _pouleInfo.GetPouleCount(), _pouleInfo.RoundsOfPoules);
                case PouleNamingType.Numbers:
                    return new PouleNamingObject(_pouleInfo.NamingInfo, _pouleInfo.GetPouleCount());
            }
        }
    }
}
