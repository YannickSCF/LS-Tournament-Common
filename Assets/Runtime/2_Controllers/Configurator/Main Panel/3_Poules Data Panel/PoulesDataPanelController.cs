// Dependencies
using System;
using System.Collections.Generic;
using UnityEngine;
using YannickSCF.LSTournaments.Common.Models.Poules;
// Custom dependencies
using YannickSCF.LSTournaments.Common.Scriptables.Data;
using YannickSCF.LSTournaments.Common.Scriptables.Formulas;
using YannickSCF.LSTournaments.Common.Tools.Poule;
using YannickSCF.LSTournaments.Common.Views.MainPanel.PoulesDataPanel;

namespace YannickSCF.LSTournaments.Common.Controllers.MainPanel.PoulesDataPanel {
    public class PoulesDataPanelController : PanelController {

        private const string TRANSAPARENT_TAG = "<color=#00000000>{0}</color>";

        [SerializeField] private PoulesDataPanelView _poulesDataPanelView;

        private PouleNamingType _namingType;
        private int _pouleRounds;
        private int _pouleSizeCountDropdownValue;

        private PoulesBy _howToDefine = PoulesBy.NumberOfPoules;
        private int _athletesCount = 0;

        private int[,] _currentPouleCountAndSize;
        private Dictionary<int, int[,]> _possiblePoulesByPouleCount;
        private Dictionary<int, int[,]> _possiblePoulesByMaxSize;

        #region Mono
        private void OnEnable() {
            _poulesDataPanelView.NamingTypeChanged += OnNamingChanged;
            _poulesDataPanelView.PouleRoundChanged += OnPouleRoundChanged;

            _poulesDataPanelView.HowToDefinePoulesChanged += OnHowToDefinePoulesChanged;
            _poulesDataPanelView.SelectedPouleDataChanged += OnSelectedPouleDataChanged;
        }

        private void OnDisable() {
            _poulesDataPanelView.NamingTypeChanged -= OnNamingChanged;
            _poulesDataPanelView.PouleRoundChanged -= OnPouleRoundChanged;

            _poulesDataPanelView.HowToDefinePoulesChanged -= OnHowToDefinePoulesChanged;
            _poulesDataPanelView.SelectedPouleDataChanged -= OnSelectedPouleDataChanged;
        }
        #endregion

        #region Event Listeners methods
        private void OnNamingChanged(int namingTypeIndex) {
            UpdateNamingExample((PouleNamingType)namingTypeIndex, _pouleRounds);
        }

        private void OnPouleRoundChanged(int pouleRounds) {
            UpdateNamingExample(_namingType, pouleRounds);
        }

        private void OnHowToDefinePoulesChanged(int hotToDefineIndex) {
            _howToDefine = (PoulesBy)hotToDefineIndex;
            UpdatePouleSelectionOptions();
            UpdatePouleAttribute(true);
            UpdateNamingExample(_namingType, _pouleRounds);

            ValidateAll();
        }

        private void OnSelectedPouleDataChanged(int selectedValue) {
            _pouleSizeCountDropdownValue = selectedValue;
            UpdatePouleAttribute();
            UpdateNamingExample(_namingType, _pouleRounds);

            ValidateAll();
        }
        #endregion

        #region PanelController abstract methods overrided
        public override string GetTitle() { return "Poules Data"; }

        public override void ValidateAll() {
            _IsDataValidated = _currentPouleCountAndSize != null;
        }

        public override void GiveData(TournamentData data) {
            _namingType = data.NamingInfo;
            _pouleRounds = data.RoundsOfPoules;
            _poulesDataPanelView.SetPouleNamingType((int)_namingType, _pouleRounds);

            _athletesCount = data.Athletes.Count;

            _poulesDataPanelView.SetSelectableCountSize(TournamentFormulaUtils.IsCustomFormula(data.TournamentFormulaName));
            if (TournamentFormulaUtils.IsCustomFormula(data.TournamentFormulaName)) {
                _possiblePoulesByPouleCount = PouleUtils.GetPossiblePoulesByNumberOfPoules(_athletesCount);
                _possiblePoulesByMaxSize = PouleUtils.GetPossiblePoulesByMaxPouleSize(_athletesCount);
                UpdatePouleSelectionOptions();
                UpdatePouleAttribute();
            } else {
                _currentPouleCountAndSize = PouleUtils.GetPoulesAndSize(_athletesCount,
                        TournamentFormulaUtils.GetFormulaByName(data.TournamentFormulaName));

                _poulesDataPanelView.SetPoulesCountSizeAttributes(_athletesCount, _currentPouleCountAndSize);
            }

            ValidateAll();
            UpdateNamingExample(_namingType, _pouleRounds);
        }

        public override TournamentData RetrieveData(TournamentData data) {
            data.NamingInfo = _namingType;
            data.RoundsOfPoules = _pouleRounds;
            data.PouleCountAndSizes = _currentPouleCountAndSize;

            return data;
        }
        #endregion

        private void UpdateNamingExample(PouleNamingType newNaming, int pouleRounds) {
            if (_namingType != newNaming) {
                _namingType = newNaming;
                switch (_namingType) {
                    case PouleNamingType.Letters:
                    default:
                        _pouleRounds = 1;
                        _poulesDataPanelView.SetInteractablePouleRounds(true);
                        break;
                    case PouleNamingType.Numbers:
                        _pouleRounds = 0;
                        _poulesDataPanelView.SetInteractablePouleRounds(false);
                        break;
                }
            } else {
                _pouleRounds = pouleRounds;
            }

            _poulesDataPanelView.SetPouleNamingType((int)_namingType, _pouleRounds);

            string example = GetNamingText();
            _poulesDataPanelView.SetPouleNamingExample(example);
        }

        private string GetNamingText() {
            if (_currentPouleCountAndSize == null) {
                return "Set Poule Count and size to visuelize example!";
            }

            int poulesCount = _currentPouleCountAndSize[0, 0] + _currentPouleCountAndSize[1, 0];
            int poulesByLine;
            switch (_namingType) {
                case PouleNamingType.Letters:
                default:
                    if (_pouleRounds > 1) {
                        poulesByLine = (int)Math.Ceiling((double)poulesCount / _pouleRounds);
                    } else {
                        poulesByLine = 4;
                    }
                    break;
                case PouleNamingType.Numbers:
                    poulesByLine = 4;
                    break;
            }

            string example = string.Empty;
            List<string> exampleNames = PouleUtils.GetPoulesNames(_namingType, poulesCount, _pouleRounds);
            for (int i = 0; i < exampleNames.Count; ++i) {
                if (i < exampleNames.Count - 1) {
                    if (i % poulesByLine == poulesByLine - 1) {
                        example += exampleNames[i] + "\n";
                    } else {
                        example += exampleNames[i] + "\t";
                    }
                } else {
                    example += exampleNames[i];
                }
            }

            if (exampleNames.Count % poulesByLine != 0) {
                int pouleIndex = exampleNames.Count;
                string transparentName = string.Format(TRANSAPARENT_TAG, exampleNames[exampleNames.Count - 1]);
                while (pouleIndex % poulesByLine != 0) {
                    example += "\t" + transparentName;
                    ++pouleIndex;
                }
            }

            return example;
        }

        private void UpdatePouleSelectionOptions() {
            List<int> options = new List<int>();
            switch (_howToDefine) {
                case PoulesBy.MaxPoulesSize:
                    options = new List<int>(_possiblePoulesByMaxSize.Keys);
                    break;
                case PoulesBy.NumberOfPoules:
                default:
                    options = new List<int>(_possiblePoulesByPouleCount.Keys);
                    break;
            }

            _poulesDataPanelView.SetPoulesCountSizeDropdownOptions(
                "Poules by" + Enum.GetName(typeof(PoulesBy), _howToDefine),
                options);
        }

        private void UpdatePouleAttribute(bool noValue = false) {
            if (!noValue) {
                _poulesDataPanelView.SetPoulesCountSizeDropdownOptionSelected(_pouleSizeCountDropdownValue);
                switch (_howToDefine) {
                    case PoulesBy.MaxPoulesSize:
                        if (_possiblePoulesByMaxSize.ContainsKey(_pouleSizeCountDropdownValue)) {
                            _currentPouleCountAndSize = _possiblePoulesByMaxSize[_pouleSizeCountDropdownValue];
                        }
                        break;
                    case PoulesBy.NumberOfPoules:
                    default:
                        if (_possiblePoulesByPouleCount.ContainsKey(_pouleSizeCountDropdownValue)) {
                            _currentPouleCountAndSize = _possiblePoulesByPouleCount[_pouleSizeCountDropdownValue];
                        }
                        break;
                }
            } else {
                _currentPouleCountAndSize = null;
            }

            _poulesDataPanelView.SetPoulesCountSizeAttributes(_athletesCount, _currentPouleCountAndSize);
        }
    }
}
