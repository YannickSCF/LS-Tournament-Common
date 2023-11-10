/**
 * Author:      Yannick Santa Cruz Feuillias
 * Created:     06/10/2023
 **/

// Dependencies
using UnityEngine;
// Custom dependencies
using YannickSCF.LSTournaments.Common.Scriptables.Data;
using YannickSCF.LSTournaments.Common.Views.MainPanel;

namespace YannickSCF.LSTournaments.Common.Controllers.MainPanel {
    public abstract class PanelController : MonoBehaviour {
        public enum PanelPosition { Left = -1, Center = 0, Right = 1 };

        protected bool _IsDataValidated = false;
        public bool IsDataValidated { get => _IsDataValidated; }

        public abstract string GetTitle();
        public abstract void ValidateAll(bool showErrorAdvices = true);

        public abstract void GiveData(TournamentData data);
        public abstract TournamentData RetrieveData(TournamentData data);

        public virtual void MovePanel(PanelPosition position) { }
    }

    public abstract class PanelController<T> : PanelController where T : PanelView {

        [SerializeField] protected T _View;

        public override void MovePanel(PanelPosition position) {
            switch (position) {
                case PanelPosition.Left: _View.MovePanelLeft(); break;
                case PanelPosition.Right: _View.MovePanelRight(); break;
                case PanelPosition.Center:
                default: _View.MovePanelCenter(); break;
            }
        }
    }
}
