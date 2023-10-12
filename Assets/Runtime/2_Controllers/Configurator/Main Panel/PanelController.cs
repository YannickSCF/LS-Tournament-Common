// Dependencies
using System.Collections.Generic;
using UnityEngine;
// Custom dependencies
using YannickSCF.LSTournaments.Common.Scriptables.Data;

namespace YannickSCF.LSTournaments.Common.Controllers.MainPanel {
    public abstract class PanelController : MonoBehaviour {

        protected bool _IsDataValidated = false;
        public bool IsDataValidated { get => _IsDataValidated; }

        public abstract string GetTitle();
        public abstract void ValidateAll();

        public abstract void GiveData(TournamentData data);
        public abstract TournamentData RetrieveData(TournamentData data);
    }
}
