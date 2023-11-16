/**
 * Author:      Yannick Santa Cruz Feuillias
 * Created:     02/10/2023
 **/

// Dependencies
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
// Custom dependencies
using YannickSCF.GeneralApp.Controller.UI.Windows;
using YannickSCF.LSTournaments.Common.Controllers.MainPanel;
using YannickSCF.LSTournaments.Common.Scriptables.Data;
using YannickSCF.LSTournaments.Common.Scriptables.Formulas;
using YannickSCF.LSTournaments.Common.Views;
using YannickSCF.LSTournaments.Common.Views.Breadcrumb;
using static YannickSCF.LSTournaments.Common.Controllers.MainPanel.PanelController;

namespace YannickSCF.LSTournaments.Common.Controllers {
    public class ConfiguratorController : WindowController<ConfiguratorView> {

        [SerializeField] private GameObject _loadingPanel;
        [SerializeField] private Button _closeButton;

        [Header("Tournament Formulas")]
        [SerializeField] private List<TournamentFormula> _allTournamentFormulas;
        [SerializeField] private TournamentFormula _customFormula;

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
            Init("");
            _breadcrumbView.EnablePrevNavigationButton(false);
        }
        protected override void OnEnable() {
            base.OnEnable();

            _closeButton.onClick.AddListener(() => CloseConfigurator());

            _breadcrumbView.NavigationBreadCrumbPressed += OnBreadcrumbNavigation;
            PanelController.AskedLoading += OnAskedLoading;
        }

        protected override void OnDisable() {
            base.OnDisable();

            _closeButton.onClick.RemoveAllListeners();

            _breadcrumbView.NavigationBreadCrumbPressed -= OnBreadcrumbNavigation;
            PanelController.AskedLoading -= OnAskedLoading;
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
                _allConfiguratorPanels[_panelIndex].FinishPanel();
                _onFinishAction?.Invoke();
                return;
            }

            _allConfiguratorPanels[_panelIndex].FinishPanel();

            if (clickedNext) {
                _allConfiguratorPanels[_panelIndex].MovePanel(PanelPosition.Left);
                ++_panelIndex;
            } else {
                _allConfiguratorPanels[_panelIndex].MovePanel(PanelPosition.Right);
                --_panelIndex;
            }

            _allConfiguratorPanels[_panelIndex].MovePanel(PanelPosition.Center);
            _allConfiguratorPanels[_panelIndex].InitPanel();

            _breadcrumbView.UpdateCurrentCrumb(_panelIndex);
        }
        #endregion

        public override void Init(string windowId) {
            base.Init(windowId);

            TournamentFormulaUtils.SetTournamentFormulas(_allTournamentFormulas, _customFormula);

            List<string> breadcrumbNames = new List<string>();
            ResetConfigurator();

            for (int i = 0; i < _allConfiguratorPanelsPrefabs.Count; ++i) {
                PanelController newPanelController = Instantiate(_allConfiguratorPanelsPrefabs[i], _configurationContent);
                breadcrumbNames.Add(newPanelController.GetTitle());
                if (i > 0) {
                    newPanelController.MovePanel(PanelPosition.Right, true);
                }

                _allConfiguratorPanels.Add(newPanelController);
            }

            _allConfiguratorPanels[0].InitPanel();
            _allConfiguratorPanels[0].MovePanel(PanelPosition.Center, true);
            // Set breadcrumb
            _breadcrumbView.SetBreadcrumb(breadcrumbNames);
            _breadcrumbView.UpdateCurrentCrumb(0);
        }

        public void SetCallbacks(Action closedCallback, Action finishedCallback) {
            _onCloseAction = closedCallback;
            _onFinishAction = finishedCallback;
        }

        private void ResetConfigurator() {
            _breadcrumbView.ResetBreadcrumb();

            if (_allConfiguratorPanels != null) {
                List<PanelController> auxConfiguratorPanels = new List<PanelController>(_allConfiguratorPanels);
                foreach (PanelController auxConfiguratorPanel in auxConfiguratorPanels) {
                    DestroyImmediate(auxConfiguratorPanel.gameObject);
                }

                _allConfiguratorPanels.Clear();
            }

            _allConfiguratorPanels = new List<PanelController>();
        }

        private void OnAskedLoading(PanelController panel, bool show) {
            _loadingPanel.SetActive(show);

            _breadcrumbView.EnableNextNavigationButton(!show);
            if (_panelIndex > 0) {
                _breadcrumbView.EnablePrevNavigationButton(!show);
            } else {
                _breadcrumbView.EnablePrevNavigationButton(false);
            }
        }

        public void CloseConfigurator() {
            DataManager.Instance.AppData.ResetData();
            _onCloseAction?.Invoke();
        }
    }
}
