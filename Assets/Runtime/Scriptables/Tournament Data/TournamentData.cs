using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YannickSCF.LSTournaments.Common.Models.Athletes;
using YannickSCF.LSTournaments.Common.Models.Matches;
using YannickSCF.LSTournaments.Common.Models.Poules;
using YannickSCF.LSTournaments.Common.Scriptables.Formulas;

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

        public Dictionary<AthleteInfoType, bool> AthletesInfoUsed { get => _athletesInfoUsed; set => _athletesInfoUsed = value; }
        public List<AthleteInfoModel> Athletes { get => _athletes; set => _athletes = value; }
        public List<AthleteTournamentStatsModel> AthletesStats { get => _athletesStats; set => _athletesStats = value; }
    }
}
