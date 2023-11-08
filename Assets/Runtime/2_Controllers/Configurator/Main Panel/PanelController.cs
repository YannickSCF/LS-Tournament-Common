/**
 * Author:      Yannick Santa Cruz Feuillias
 * Created:     06/10/2023
 **/

// Dependencies
using UnityEngine;
// Custom dependencies
using YannickSCF.LSTournaments.Common.Scriptables.Data;

namespace YannickSCF.LSTournaments.Common.Controllers.MainPanel {
    public abstract class PanelController : MonoBehaviour {

        protected bool _IsDataValidated = false;
        public bool IsDataValidated { get => _IsDataValidated; }

        public abstract string GetTitle();
        public abstract void ValidateAll(bool showErrorAdvices = true);

        public abstract void GiveData(TournamentData data);
        public abstract TournamentData RetrieveData(TournamentData data);
    }
}
