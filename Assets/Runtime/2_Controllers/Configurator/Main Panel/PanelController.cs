/**
 * Author:      Yannick Santa Cruz Feuillias
 * Created:     06/10/2023
 **/

// Dependencies
using UnityEngine;
// Custom dependencies
using YannickSCF.LSTournaments.Common.Views.MainPanel;

namespace YannickSCF.LSTournaments.Common.Controllers.MainPanel {
    public abstract class PanelController : MonoBehaviour {

        public delegate void PanelMovedDelegate(PanelController panel);
        public static event PanelMovedDelegate PanelViewCentered;
        public static event PanelMovedDelegate PanelViewMovedApart;
        public delegate void AskLoadingEventDelegate(PanelController panel, bool show);
        public static event AskLoadingEventDelegate AskedLoading;

        public enum PanelPosition { Left = -1, Center = 0, Right = 1 };

        protected bool _IsDataValidated = false;
        public bool IsDataValidated { get => _IsDataValidated; }

        private static bool _isLoadingActive = false;
        public static bool IsLoadingActive { get => _isLoadingActive; }
        protected static bool _IsLoadingActive { set => _isLoadingActive = value; }

        public abstract string GetTitle();
        public abstract void ValidateAll(bool showErrorAdvices = true);

        public abstract void InitPanel();
        public abstract void FinishPanel();

        public virtual void ResetPanel() { }

        public virtual void MovePanel(PanelPosition position, bool moveInmediate = false) { }

        protected virtual void OnPanelMovedApart() {
            PanelViewMovedApart?.Invoke(this);
        }

        protected virtual void OnPanelCentered() {
            PanelViewCentered?.Invoke(this);
            AskLoading(false);
        }

        protected void AskLoading(bool show) {
            AskedLoading?.Invoke(this, show);
            _IsLoadingActive = show;
        }
    }

    public abstract class PanelController<T> : PanelController where T : PanelView {

        [SerializeField] protected T _View;

        #region Mono
        private void Awake() {
            MovePanel(PanelPosition.Right, true);
        }
        protected virtual void OnEnable() {
            _View.PanelMovedApart += OnPanelMovedApart;
            _View.PanelCentered += OnPanelCentered;
        }

        protected virtual void OnDisable() {
            _View.PanelMovedApart -= OnPanelMovedApart;
            _View.PanelCentered -= OnPanelCentered;
        }
        #endregion

        public override void ResetPanel() {
            base.ResetPanel();
            _View.ResetView();
        }

        public override void MovePanel(PanelPosition position, bool moveInmediate = false) {
            base.MovePanel(position);

            switch (position) {
                case PanelPosition.Left: _View.MovePanelLeft(moveInmediate); break;
                case PanelPosition.Right: _View.MovePanelRight(moveInmediate); break;
                case PanelPosition.Center:
                default:
                    AskLoading(true);
                    _View.MovePanelCenter(moveInmediate);
                    break;
            }
        }
    }
}
