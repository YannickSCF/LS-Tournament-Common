/**
 * Author:      Yannick Santa Cruz Feuillias
 * Created:     02/10/2023
 **/

// Dependencies
using System;
using System.Collections.Generic;
using UnityEngine;
// Custom dependencies
using YannickSCF.GeneralApp.Controller.UI.Windows;
using YannickSCF.LSTournaments.Common.Controllers.MainPanel;
using YannickSCF.LSTournaments.Common.Scriptables.Data;
using YannickSCF.LSTournaments.Common.Scriptables.Formulas;
using YannickSCF.LSTournaments.Common.Views;
using YannickSCF.LSTournaments.Common.Views.Breadcrumb;

namespace YannickSCF.LSTournaments.Common.Controllers {
    public class ConfiguratorController : WindowController<ConfiguratorView> {

        [Header("Tournament Formulas")]
        [SerializeField] private List<TournamentFormula> _allTournamentFormulas;
        [SerializeField] private TournamentFormula _customFormula;

        [Header("Tournament Data")]
        [SerializeField] private TournamentData _data;

        [Header("Configurator pages")]
        [SerializeField] private BreadcrumbView _breadcrumbView;
        [SerializeField] private List<PanelController> _allConfiguratorPanelsPrefabs;
        [SerializeField] private Transform _configurationContent;

        private List<PanelController> _allConfiguratorPanels;
        private int _panelIndex = 0;

        private Action _onCloseAction;
        private Action _onFinishAction;

        #region Mono
        private void Awake() {
            TournamentFormulaUtils.SetTournamentFormulas(_allTournamentFormulas, _customFormula);

            Initialize();
        }

        protected override void OnEnable() {
            base.OnEnable();

            _breadcrumbView.NavigationBreadCrumbPressed += OnBreadcrumbNavigation;
        }

        protected override void OnDisable() {
            base.OnDisable();

            _breadcrumbView.NavigationBreadCrumbPressed -= OnBreadcrumbNavigation;
        }
        #endregion

        #region Events Listeners methods
        private void OnBreadcrumbNavigation(bool clickedNext) {
            if ((clickedNext && !_allConfiguratorPanels[_panelIndex].IsDataValidated)
                || (!clickedNext && _panelIndex <= 0)) {
                Debug.LogWarning("Not validated Yet!");
                _allConfiguratorPanels[_panelIndex].ValidateAll();
                return;
            }

            if (clickedNext && _panelIndex >= _allConfiguratorPanels.Count - 1) {
                _data = _allConfiguratorPanels[_panelIndex].RetrieveData(_data);
                _onFinishAction?.Invoke();
                return;
            }

            _data = _allConfiguratorPanels[_panelIndex].RetrieveData(_data);
            _allConfiguratorPanels[_panelIndex].gameObject.SetActive(false);

            if (clickedNext) {
                ++_panelIndex;
            } else {
                --_panelIndex;
            }

            _allConfiguratorPanels[_panelIndex].gameObject.SetActive(true);
            _allConfiguratorPanels[_panelIndex].GiveData(_data);

            _breadcrumbView.UpdateCurrentCrumb(_panelIndex);
        }
        #endregion

        private void Initialize() {
            List<string> breadcrumbNames = new List<string>();
            _allConfiguratorPanels = new List<PanelController>();
            foreach (PanelController configuratorPanelPrefab in _allConfiguratorPanelsPrefabs) {
                PanelController newPanelController = Instantiate(configuratorPanelPrefab, _configurationContent);
                newPanelController.gameObject.SetActive(false);
                breadcrumbNames.Add(newPanelController.GetTitle());

                _allConfiguratorPanels.Add(newPanelController);
            }

            _allConfiguratorPanels[0].gameObject.SetActive(true);
            _allConfiguratorPanels[0].GiveData(_data);
            // Set breadcrumb
            _breadcrumbView.SetBreadcrumb(breadcrumbNames);
            _breadcrumbView.UpdateCurrentCrumb(0);
        }

        public void SetCallbacks(Action closedCallback, Action finishedCallback) {
            _onCloseAction = closedCallback;
            _onFinishAction = finishedCallback;
        }

        public void CloseConfigurator() {
            _data = new TournamentData();
            _onCloseAction?.Invoke();
        }
    }
}
