// Dependencies
using System.Collections.Generic;
using UnityEngine;
// Custom Dependencies
using YannickSCF.LSTournaments.Common.Scriptables.Data;
using YannickSCF.LSTournaments.Common.Scriptables.Formulas;
using YannickSCF.LSTournaments.Common.Views.MainPanel.BaseDrawPanel;

namespace YannickSCF.LSTournaments.Common.Controllers.MainPanel.BaseDrawPanel {
    public class BaseDrawPanelController : PanelController {

        [SerializeField] private BaseDrawPanelView _baseDrawPanelView;

        private PouleFillerType _fillerType;
        private PouleFillerSubtype _fillerSubtype;

        #region Mono
        private void OnEnable() {
            _baseDrawPanelView.FillerTypeChanged += OnFillerTypeChanged;
            _baseDrawPanelView.FillerSubtypeChanged += OnFillerSubtypeChanged;
        }

        private void OnDisable() {
            _baseDrawPanelView.FillerTypeChanged -= OnFillerTypeChanged;
            _baseDrawPanelView.FillerSubtypeChanged -= OnFillerSubtypeChanged;
        }
        #endregion

        #region Events Listeners methods
        private void OnFillerTypeChanged(int fillerTypeIndex) {
            _fillerType = (PouleFillerType)fillerTypeIndex;
            ValidateAll();
        }

        private void OnFillerSubtypeChanged(int fillerSubtypeIndex) {
            _fillerSubtype = (PouleFillerSubtype)fillerSubtypeIndex;
        }
        #endregion

        #region PanelController abstract methods overrided
        public override string GetTitle() { return "Draw Options"; }

        public override void ValidateAll() {
            _IsDataValidated = _fillerType != PouleFillerType.TBD;
        }

        public override void GiveData(TournamentData data) {
            TournamentFormula formula = TournamentFormulaUtils.GetFormulaByName(data.TournamentFormulaName);
            _fillerType = formula.FillerType;
            _fillerSubtype = data.PouleInfo.FillerSubtypeInfo;

            _baseDrawPanelView.RemoveSelectableSubtypes(data.GetFillerSubtypesCannotBeUsed());

            _baseDrawPanelView.SetFillerType(_fillerType, TournamentFormulaUtils.IsCustomFormula(data.TournamentFormulaName));
            _baseDrawPanelView.SetFillerSubtype(_fillerSubtype);

            ValidateAll();
        }

        public override TournamentData RetrieveData(TournamentData data) {
            //data.PouleInfo.FillerSubtypeInfo = _fillerSubtype;
            return data;
        }
        #endregion

        private void FillExampleByTypeAndSubtype() {
            if (_fillerType == PouleFillerType.TBD) {
                _baseDrawPanelView.SetExample("Select a Filler type...");
                return;
            }

            Dictionary<int, string> examplePoules = new Dictionary<int, string>();


            if (_fillerSubtype != PouleFillerSubtype.None) {

            }
        }
    }
}
