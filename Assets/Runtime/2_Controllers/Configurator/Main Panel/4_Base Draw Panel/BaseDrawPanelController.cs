// Dependencies
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
// Custom Dependencies
using YannickSCF.LSTournaments.Common.Models.Athletes;
using YannickSCF.LSTournaments.Common.Models.Poules;
using YannickSCF.LSTournaments.Common.Scriptables.Data;
using YannickSCF.LSTournaments.Common.Scriptables.Formulas;
using YannickSCF.LSTournaments.Common.Tools.Poule;
using YannickSCF.LSTournaments.Common.Views.MainPanel.BaseDrawPanel;

namespace YannickSCF.LSTournaments.Common.Controllers.MainPanel.BaseDrawPanel {
    public class BaseDrawPanelController : PanelController {

        private const string COLOR_TAG = "<color={0}>{1}</color>";
        private const string POULE_NAME_STRUCTURE = "  - {0} {1}{2}";
        private const string RETURN_CARRIAGE = "\n";

        private readonly List<Color> RANK_COLORS = new List<Color>(){ Color.blue, Color.yellow, Color.red, Color.green, Color.magenta };
        private const string RANK_SEPARATOR = " / ";

        [SerializeField] private BaseDrawPanelView _baseDrawPanelView;

        private TournamentData _tempData;

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
            
            FillExampleByTypeAndSubtype();
            ValidateAll();
        }

        private void OnFillerSubtypeChanged(int fillerSubtypeIndex) {
            _fillerSubtype = (PouleFillerSubtype)fillerSubtypeIndex;
            FillExampleByTypeAndSubtype();
        }
        #endregion

        #region PanelController abstract methods overrided
        public override string GetTitle() { return "Draw Options"; }

        public override void ValidateAll() {
            _IsDataValidated = _fillerType != PouleFillerType.TBD;
        }

        public override void GiveData(TournamentData data) {
            _tempData = data;

            TournamentFormula formula = TournamentFormulaUtils.GetFormulaByName(data.TournamentFormulaName);
            _fillerType = formula.FillerType;
            _fillerSubtype = data.PouleInfo.FillerSubtypeInfo;

            _baseDrawPanelView.RemoveSelectableTypes(data.GetFillerTypesCannotBeUsed());
            _baseDrawPanelView.RemoveSelectableSubtypes(data.GetFillerSubtypesCannotBeUsed());

            _baseDrawPanelView.SetFillerType(_fillerType, TournamentFormulaUtils.IsCustomFormula(data.TournamentFormulaName));
            _baseDrawPanelView.SetFillerSubtype(_fillerSubtype);

            FillExampleByTypeAndSubtype();
            ValidateAll();
        }

        public override TournamentData RetrieveData(TournamentData data) {
            // TODO
            return data;
        }
        #endregion

        private void FillExampleByTypeAndSubtype() {
            Randomizer.SetSeed(0);

            if (_fillerType == PouleFillerType.TBD) {
                _baseDrawPanelView.SetExample("Select a Filler type...");
                return;
            }
            PouleNamingObject? namingData = _tempData.GetNamingData();
            if (namingData == null) {
                Debug.LogError("At this point something is missing on data saved!");
                return;
            }

            Dictionary<int, string> examplePoules = new Dictionary<int, string>();
            List<PouleDataModel> tempPoules = PouleUtils.CreatePoules(_tempData.GetNamingData().Value, _tempData.Athletes, _fillerType, _fillerSubtype, _tempData.PouleInfo.GetPouleMaxSize());

            for(int i = 0; i < 4 && i < tempPoules.Count; ++i) {
                examplePoules.Add(i, GetPouleText(tempPoules[i]));
            }

            string legendText = GetLegendText();

            _baseDrawPanelView.SetExample(examplePoules, legendText);
        }

        private string GetPouleText(PouleDataModel pouleData) {
            string result = pouleData.Name + RETURN_CARRIAGE;

            for (int i = 0; i < pouleData.AthletesIds.Count; ++i) {
                AthleteInfoModel athlete = _tempData.GetAthleteById(pouleData.AthletesIds[i]);

                string fillerTypeText = string.Empty;
                switch (_fillerType) {
                    case PouleFillerType.ByRank: fillerTypeText = GetRankAbreviationWithColor(athlete.Rank); break;
                    case PouleFillerType.ByStyle: fillerTypeText = athlete.Styles.Count.ToString(); break;
                    case PouleFillerType.ByTier: fillerTypeText = athlete.Tier.ToString(); break;
                    case PouleFillerType.TBD:
                    case PouleFillerType.Random:
                    default:
                        break;
                }

                string fillerSubtypeText = string.Empty;
                switch (_fillerSubtype) {
                    case PouleFillerSubtype.Country: fillerSubtypeText = athlete.Country + "|"; break;
                    case PouleFillerSubtype.Academy: fillerSubtypeText = athlete.Academy + "|"; break;
                    case PouleFillerSubtype.School: fillerSubtypeText = athlete.School + "|"; break;
                    case PouleFillerSubtype.None:
                    default: break;
                }

                result += string.Format(POULE_NAME_STRUCTURE, fillerTypeText, fillerSubtypeText,
                            athlete.GetFullName(FullNameType.NameSurname));
                if (i < pouleData.AthletesIds.Count - 1) {
                    result += RETURN_CARRIAGE;
                }
            }

            return result;
        }

        private string GetRankAbreviationWithColor(RankType rank) {
            return string.Format(COLOR_TAG,
                "#" + ColorUtility.ToHtmlStringRGB(RANK_COLORS[(int)rank]),
                "(" + Enum.GetName(typeof(RankType), rank).Substring(0, 1) + ")");
        }

        private string GetLegendText() {
            string legendText = POULE_NAME_STRUCTURE;

            string subtypeText = string.Empty;
            switch (_fillerSubtype) {
                case PouleFillerSubtype.Country: subtypeText = "Country|"; break;
                case PouleFillerSubtype.Academy: subtypeText = "Academy|"; break;
                case PouleFillerSubtype.School: subtypeText = "School|"; break;
                case PouleFillerSubtype.None:
                default: break;
            }

            switch (_fillerType) {
                case PouleFillerType.ByRank:
                    legendText = string.Format(legendText, "(Rank)", subtypeText, "Athlete Name");
                    legendText += GetLegendForRank();
                    break;
                case PouleFillerType.ByStyle:
                    legendText = string.Format(legendText, "(# Styles)", subtypeText, "Athlete Name");
                    break;
                case PouleFillerType.ByTier:
                    legendText = string.Format(legendText, "(Tier)", subtypeText, "Athlete Name");
                    break;
                case PouleFillerType.TBD:
                case PouleFillerType.Random:
                default: legendText = string.Empty; break;
            }
            return legendText;
        }

        private string GetLegendForRank() {
            string result = RETURN_CARRIAGE;
            List<RankType> ranksUsed = _tempData.Athletes.Select(x => x.Rank).Distinct().ToList();
            ranksUsed.Reverse();

            for (int i = 0; i < ranksUsed.Count; ++i) {
                Color valueColor = RANK_COLORS[(int)ranksUsed[i]];
                string rankName = Enum.GetName(typeof(RankType), ranksUsed[i]);
                string rankText = "(" + rankName.Substring(0, 1) + ") " + rankName;

                result += string.Format(COLOR_TAG, "#"+ColorUtility.ToHtmlStringRGB(valueColor), rankText);
                if (i < ranksUsed.Count - 1) {
                    result += RANK_SEPARATOR;
                }
            }

            return result;
        }
    }
}
