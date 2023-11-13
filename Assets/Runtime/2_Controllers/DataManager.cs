
using UnityEngine;
using YannickSCF.GeneralApp;
using YannickSCF.LSTournaments.Common.Scriptables.Data;

namespace YannickSCF.LSTournaments.Common.Controllers {
    public class DataManager : GlobalSingleton<DataManager> {

        [SerializeField] private TournamentData _appData;
        
        public TournamentData AppData { get => _appData; set => _appData = value; }
    }
}
