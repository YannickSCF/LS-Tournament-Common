/**
 * Author:      Yannick Santa Cruz Feuillias
 * Created:     08/11/2023
 **/

// Dependencies
using System.Collections;
using UnityEngine;
//Custom dependencies
using YannickSCF.GeneralApp.View.UI.Windows;

namespace YannickSCF.LSTournaments.Common.Views {
    public class ConfiguratorView : WindowView {

        [SerializeField] private Animator _animator;

        public override void Show() {
            base.Show();
            _animator.SetBool("Show", true);
        }

        public override void Hide() {
            _animator.SetBool("Show", false);
            StartCoroutine(WaitToHideCoroutine());
        }

        private IEnumerator WaitToHideCoroutine() {
            yield return new WaitUntil(() => _animator.GetCurrentAnimatorStateInfo(0).IsName("configurator_hidden"));
            base.Hide();
        }
    }
}
