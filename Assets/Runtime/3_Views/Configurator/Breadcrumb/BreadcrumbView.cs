using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
            crumbs = new List<CrumbView>();
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
            foreach (Transform oldCrumbs in _crumbsParent) {
                DestroyImmediate(oldCrumbs.gameObject);
            }
            crumbs.Clear();

            for (int i = crumbNames.Count - 1; i >= 0; --i) {
                CrumbView newCrumb = Instantiate(_crumbPrefab, _crumbsParent);
                newCrumb.SetCrumbText(crumbNames[i]);
                newCrumb.EnableCrumb(i == 0);

                crumbs.Add(newCrumb);
            }
        }

        public void UpdateCurrentCrumb(int indexCurrentCrumb) {
            for (int i = 0; i < crumbs.Count; ++i) {
                crumbs[i].EnableCrumb(i <= indexCurrentCrumb);
            }

            _prevStepButton.interactable = indexCurrentCrumb != 0;
        }
    }
}
