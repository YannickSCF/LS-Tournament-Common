/**
 * Author:      Yannick Santa Cruz Feuillias
 * Created:     27/09/2023
 **/

// Dependencies
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
// Custom dependencies
using static YannickSCF.GeneralApp.CommonEventsDelegates;

namespace YannickSCF.LSTournaments.Common.Views.Breadcrumb {
    public class BreadcrumbView : MonoBehaviour {

        public event BooleanEventDelegate NavigationBreadCrumbPressed;

        [SerializeField] private Button _prevStepButton;
        [SerializeField] private Transform _crumbsParent;
        [SerializeField] private Button _nextStepButton;

        [SerializeField] private CrumbView _crumbPrefab;

        private List<CrumbView> crumbs;

        #region Mono
        private void Awake() {
            if (crumbs == null) {
                crumbs = new List<CrumbView>();
            }
        }

        private void OnEnable() {
            _prevStepButton.onClick.AddListener(() => NavigationBreadCrumbPressed?.Invoke(false));
            _nextStepButton.onClick.AddListener(() => NavigationBreadCrumbPressed?.Invoke(true));
        }

        private void OnDisable() {
            _prevStepButton.onClick.RemoveAllListeners();
            _nextStepButton.onClick.RemoveAllListeners();
        }
        #endregion

        public void SetBreadcrumb(List<string> crumbNames) {
            ResetBreadcrumb();

            for (int i = crumbNames.Count - 1; i >= 0; --i) {
                CrumbView newCrumb = Instantiate(_crumbPrefab, _crumbsParent);
                newCrumb.SetCrumbText(crumbNames[i]);
                newCrumb.EnableCrumb(i == 0);

                crumbs.Add(newCrumb);
            }

            crumbs.Reverse();
        }

        public void ResetBreadcrumb() {

            if (crumbs != null) {
                List<CrumbView> auxCrumbs = new List<CrumbView>(crumbs);
                foreach (CrumbView auxCrumb in auxCrumbs) {
                    DestroyImmediate(auxCrumb.gameObject);
                }
            }

            if (crumbs == null) {
                crumbs = new List<CrumbView>();
            }
            crumbs.Clear();
        }

        public void UpdateCurrentCrumb(int indexCurrentCrumb) {
            for (int i = 0; i < crumbs.Count; ++i) {
                crumbs[i].EnableCrumb(i <= indexCurrentCrumb);
                crumbs[i].FocusCrumb(i == indexCurrentCrumb);
            }
        }

        public void EnablePrevNavigationButton(bool enable) {
            _prevStepButton.interactable = enable;
        }

        public void EnableNextNavigationButton(bool enable) {
            _nextStepButton.interactable = enable;
        }
    }
}
