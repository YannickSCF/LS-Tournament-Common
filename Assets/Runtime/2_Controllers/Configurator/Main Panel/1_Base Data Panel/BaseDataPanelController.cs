// Dependencies
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization.Settings;
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
            ValidateAll();
        }

        private void OnNameChanged(string newName) {
            _name = newName;
            ValidateAll();
        }

        private void OnFormulaChanged(int formulaIndex) {
            if (formulaIndex >= 0 && formulaIndex < _allFormulas.Count) {
                _formula = _allFormulas[formulaIndex];
                UpdateFormulaDescription();
            }

            ValidateAll();
        }
        #endregion

        #region Validators
        #endregion

        #region PanelController abstract methods overrided
        public override string GetTitle() {
            return LocalizationSettings.StringDatabase.GetLocalizedString("Configurator Texts", "BaseDataPanel_BreadcrumbTitle");
        }

        public override void ValidateAll() {
            bool res = true;

            res &= ValidateName();

            _IsDataValidated = res;
        }

        private bool ValidateName() {
            bool res = !string.IsNullOrEmpty(_name);
            _baseDataPanelView.ShowTournamentNameNotValidated(!res);
            return res;
        }

        public override void GiveData(TournamentData data) {
            _type = data.TournamentType;
            _baseDataPanelView.SetTournamentType((int)_type, true);

            _name = data.TournamentName;
            _baseDataPanelView.SetTournamentName(_name, true);

            _formula = string.IsNullOrEmpty(data.TournamentFormulaName) ? _allFormulas[0] : data.TournamentFormulaName;
            _baseDataPanelView.SetTournamentFormula(_allFormulas.IndexOf(_formula), true);
            UpdateFormulaDescription();

            ValidateAll();
        }

        public override TournamentData RetrieveData(TournamentData data) {
            data.TournamentName = _name;
            data.TournamentType = _type;
            data.TournamentFormulaName = _formula;

            return data;
        }
        #endregion

        private void UpdateFormulaDescription() {
            _baseDataPanelView.SetFormulaDescription(
                LocalizationSettings.StringDatabase.GetLocalizedString("Configurator Texts", "FormulaDescription_" + _formula));
        }
    }
}
