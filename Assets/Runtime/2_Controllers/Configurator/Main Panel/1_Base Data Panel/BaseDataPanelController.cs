/**
 * Author:      Yannick Santa Cruz Feuillias
 * Created:     06/10/2023
 **/

// Dependencies
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization.Settings;
// Custom Dependencies
using YannickSCF.LSTournaments.Common.Scriptables.Data;
using YannickSCF.LSTournaments.Common.Scriptables.Formulas;
using YannickSCF.LSTournaments.Common.Views.MainPanel.BaseDataPanel;

namespace YannickSCF.LSTournaments.Common.Controllers.MainPanel.BaseDataPanel {
    public class BaseDataPanelController : PanelController<BaseDataPanelView>  {

        private TournamentType _type;
        private string _name;
        private string _formula;

        private List<string> _allFormulas;

        #region Mono
        private void Awake() {
            _allFormulas = TournamentFormulaUtils.GetTournamentFormulasNames();
            _View.FillFormulaDropdown(_allFormulas);
            UpdateFormulaDescription();
        }

        private void OnEnable() {
            _View.TypeChanged += OnTypeChanged;
            _View.NameChanged += OnNameChanged;
            _View.FormulaChanged += OnFormulaChanged;
        }

        private void OnDisable() {
            _View.TypeChanged -= OnTypeChanged;
            _View.NameChanged -= OnNameChanged;
            _View.FormulaChanged -= OnFormulaChanged;
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
        private bool ValidateName(bool showErrorAdvices) {
            bool res = !string.IsNullOrEmpty(_name);

            if (showErrorAdvices) {
                _View.ShowTournamentNameNotValidated(!res);
            }

            return res;
        }
        #endregion

        #region PanelController abstract methods overrided
        public override string GetTitle() {
            return LocalizationSettings.StringDatabase.GetLocalizedString("Configurator Texts", "BaseDataPanel_BreadcrumbTitle");
        }

        public override void ValidateAll(bool showErrorAdvices = true) {
            bool res = true;

            res &= ValidateName(showErrorAdvices);

            _IsDataValidated = res;
        }

        public override void GiveData(TournamentData data) {
            _type = data.TournamentType;
            _View.SetTournamentType((int)_type, true);

            _name = data.TournamentName;
            _View.SetTournamentName(_name, true);

            _formula = string.IsNullOrEmpty(data.TournamentFormulaName) ? _allFormulas[0] : data.TournamentFormulaName;
            _View.SetTournamentFormula(_allFormulas.IndexOf(_formula), true);
            UpdateFormulaDescription();

            ValidateAll(false);
        }

        public override TournamentData RetrieveData(TournamentData data) {
            data.TournamentName = _name;
            data.TournamentType = _type;
            data.TournamentFormulaName = _formula;

            return data;
        }
        #endregion

        private void UpdateFormulaDescription() {
            _View.SetFormulaDescription(
                LocalizationSettings.StringDatabase.GetLocalizedString("Configurator Texts", "FormulaDescription_" + _formula));
        }
    }
}
