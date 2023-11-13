/**
 * Author:      Yannick Santa Cruz Feuillias
 * Created:     12/10/2023
 **/

// Dependencies
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
// Custom Dependencies
using YannickSCF.LSTournaments.Common.Models.Athletes;
using YannickSCF.LSTournaments.Common.Models.Poules;
using YannickSCF.LSTournaments.Common.Scriptables.Data;
using YannickSCF.LSTournaments.Common.Scriptables.Formulas;
using YannickSCF.LSTournaments.Common.Tools.Poule;
using YannickSCF.LSTournaments.Common.Views.MainPanel.BaseDrawPanel;

namespace YannickSCF.LSTournaments.Common.Controllers.MainPanel.BaseDrawPanel {
    public class BaseDrawPanelController : PanelController<BaseDrawPanelView> {

        private TournamentData _tempData;

        private PouleFillerType _fillerType;
        private PouleFillerSubtype _fillerSubtype;

        #region Mono
        protected override void OnEnable() {
            base.OnEnable();

            _View.FillerTypeChanged += OnFillerTypeChanged;
            _View.FillerSubtypeChanged += OnFillerSubtypeChanged;
        }

        protected override void OnDisable() {
            base.OnDisable();

            _View.FillerTypeChanged -= OnFillerTypeChanged;
            _View.FillerSubtypeChanged -= OnFillerSubtypeChanged;
        }
        #endregion

        #region Events Listeners methods
        private void OnFillerTypeChanged(int fillerTypeIndex) {
            _fillerType = (PouleFillerType)fillerTypeIndex;
            
            FillExampleByTypeAndSubtype();
            ValidateAll();
        }

        private void OnFillerSubtypeChanged(int fillerSubtypeIndex) {
            _fillerSubtype = (PouleFillerSubtype)fillerSubtypeIndex;
            FillExampleByTypeAndSubtype();
        }
        #endregion

        #region PanelController abstract methods overrided
        public override string GetTitle() {
            return LocalizationSettings.StringDatabase.GetLocalizedString("Configurator Texts", "BaseDraw_BreadcrumbTitle");
        }

        public override void ValidateAll(bool showErrorAdvices = true) {
            _IsDataValidated = _fillerType != PouleFillerType.TBD;
            
            if (showErrorAdvices) {
                _View.ShowFillerTypeNotValidated(!IsDataValidated);
            }
        }

        public override void InitPanel() {
            TournamentData data = DataManager.Instance.AppData;
            _tempData = data;

            TournamentFormula formula = TournamentFormulaUtils.GetFormulaByName(data.TournamentFormulaName);
            if (TournamentFormulaUtils.IsCustomFormula(data.TournamentFormulaName) || formula.FillerType == PouleFillerType.TBD) {
                _fillerType = data.FillerTypeInfo;
            } else {
                _fillerType = formula.FillerType;
            }
            _fillerSubtype = data.FillerSubtypeInfo;

            _View.RemoveSelectableTypes(data.GetFillerTypesCannotBeUsed());
            _View.RemoveSelectableSubtypes(data.GetFillerSubtypesCannotBeUsed());

            _View.SetFillerType(_fillerType, TournamentFormulaUtils.IsCustomFormula(data.TournamentFormulaName) || formula.FillerType == PouleFillerType.TBD);
            _View.SetFillerSubtype(_fillerSubtype);

            FillExampleByTypeAndSubtype();

            ValidateAll(false);
        }

        public override void FinishPanel() {
            TournamentData data = DataManager.Instance.AppData;
            data.FillerTypeInfo = _fillerType;
            data.FillerSubtypeInfo = _fillerSubtype;

            DataManager.Instance.AppData = data;
        }
        #endregion

        private void FillExampleByTypeAndSubtype() {
            Randomizer.SetSeed(0);

            if (_fillerType == PouleFillerType.TBD) {
                _View.SetExample(LocalizationSettings.StringDatabase.GetLocalizedString("Configurator Texts", "BaseDraw_Title_Conditions_Example_Empty"));
                return;
            }
            PouleNamingObject? namingData = _tempData.GetNamingData();
            if (namingData == null) {
                Debug.LogError("At this point something is missing on data saved!");
                return;
            }

            Dictionary<string, List<string>> examplePoules = new Dictionary<string, List<string>>();
            List<PouleDataModel> tempPoules = PouleUtils.CreatePoules(_tempData.GetNamingData().Value, _tempData.Athletes, _fillerType, _fillerSubtype, _tempData.GetPouleMaxSize());

            for(int i = 0; i < tempPoules.Count; ++i) {
                examplePoules.Add(tempPoules[i].Name, GetPouleText(tempPoules[i]));
            }

            _View.SetExample(examplePoules);
        }

        private List<string> GetPouleText(PouleDataModel pouleData) {
            List<string> result = new List<string>();

            for (int i = 0; i < pouleData.AthletesIds.Count; ++i) {
                AthleteInfoModel athlete = _tempData.GetAthleteById(pouleData.AthletesIds[i]);

                string entryResult = _fillerSubtype == PouleFillerSubtype.Country ?
                    athlete.Country + " | " : string.Empty;
                switch (_fillerType) {
                    case PouleFillerType.ByRank: entryResult += GetRankWithColor(athlete.Rank); break;
                    case PouleFillerType.ByStyle:
                        var localizedString = new LocalizedString("Configurator Texts", "BaseDraw_Title_Conditions_Example_Styles");
                        localizedString.Arguments = new object[] { athlete.Styles.Count };
                        string baseData = localizedString.GetLocalizedString();

                        entryResult += baseData;
                        break;
                    case PouleFillerType.ByTier: entryResult += athlete.Tier.ToString(); break;
                    case PouleFillerType.Random: entryResult += athlete.Name; break;
                    case PouleFillerType.TBD:
                    default:
                        break;
                }

                switch (_fillerSubtype) {
                    case PouleFillerSubtype.Academy: entryResult += " | " + athlete.Academy; break;
                    case PouleFillerSubtype.School: entryResult += " | " + athlete.School; break;
                    case PouleFillerSubtype.Country: //entryResult += " | " + athlete.Country; break;
                    case PouleFillerSubtype.None:
                    default: break;
                }

                result.Add(entryResult);
            }

            return result;
        }

        private string GetRankWithColor(RankType rank) {
            return string.Format(LSTournamentConsts.COLOR_TAG,
                "#" + ColorUtility.ToHtmlStringRGB(LSTournamentConsts.RANK_COLORS[(int)rank]),
                Enum.GetName(typeof(RankType), rank));
        }
    }
}
