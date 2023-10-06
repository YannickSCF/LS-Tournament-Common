// Dependencies
using System;
using System.Collections.Generic;
using UnityEngine;
// Custom Dependencies
using YannickSCF.LSTournaments.Common.Scriptables.Data;
using YannickSCF.LSTournaments.Common.Scriptables.Formulas;
using YannickSCF.LSTournaments.Common.Views.MainPanel.BaseDataPanel;

namespace YannickSCF.LSTournaments.Common.Controllers.MainPanel.BaseDataPanel {
    public class BaseDataPanelController : PanelController {

        [SerializeField] private BaseDataPanelView _baseDataPanelView;

        private TournamentType _type;
        private string _name;
        private string _formula;

        private List<string> _allFormulas;

        #region Mono
        private void Awake() {
            _allFormulas = TournamentFormulaUtils.GetTournamentFormulasNames();
            _baseDataPanelView.FillFormulaDropdown(_allFormulas);
            UpdateFormulaDescription();

            _baseDataPanelView.FillTypeDropdown(new List<string>(Enum.GetNames(typeof(TournamentType))));
        }

        private void OnEnable() {
            _baseDataPanelView.TypeChanged += OnTypeChanged;
            _baseDataPanelView.NameChanged += OnNameChanged;
            _baseDataPanelView.FormulaChanged += OnFormulaChanged;
        }

        private void OnDisable() {
            _baseDataPanelView.TypeChanged -= OnTypeChanged;
            _baseDataPanelView.NameChanged -= OnNameChanged;
            _baseDataPanelView.FormulaChanged -= OnFormulaChanged;
        }
        #endregion

        #region Events Listeners methods
        private void OnTypeChanged(int typeIndex) {
            _type = (TournamentType)typeIndex;
            ExecuteValidations();
        }

        private void OnNameChanged(string newName) {
            _name = newName;
            ExecuteValidations();
        }

        private void OnFormulaChanged(int formulaIndex) {
            if (formulaIndex >= 0 && formulaIndex < _allFormulas.Count) {
                _formula = _allFormulas[formulaIndex];
                UpdateFormulaDescription();
            }

            ExecuteValidations();
        }
        #endregion

        #region Validators
        private void ExecuteValidations() {
            bool res = true;

            res &= !string.IsNullOrEmpty(_name);
            res &= !string.IsNullOrEmpty(_formula);

            _IsDataValidated = res;
        }
        #endregion

        #region PanelController abstract methods overrided
        public override string GetTitle() { return "Base Data"; }

        public override void GiveData(TournamentData data) {
            _type = data.TournamentType;
            _baseDataPanelView.SetTournamentType((int)_type, true);

            _name = data.TournamentName;
            _baseDataPanelView.SetTournamentName(_name, true);

            _formula = data.TournamentFormulaName;
            _baseDataPanelView.SetTournamentFormula(_allFormulas.IndexOf(_formula), true);
            UpdateFormulaDescription();

            ExecuteValidations();
        }

        public override TournamentData RetrieveData(TournamentData data) {
            data.TournamentName = _name;
            data.TournamentType = _type;
            data.TournamentFormulaName = _formula;

            return data;
        }
        #endregion

        private void UpdateFormulaDescription() {
            _baseDataPanelView.SetFormulaDescription("Description of " + _formula);  // TODO: Add formula description
        }
    }
}
