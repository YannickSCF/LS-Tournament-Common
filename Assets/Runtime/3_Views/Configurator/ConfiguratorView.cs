/**
 * Author:      Yannick Santa Cruz Feuillias
 * Created:     08/11/2023
 **/

// Dependencies
using System;
using System.Collections;
using UnityEngine;
//Custom dependencies
using YannickSCF.GeneralApp.View.UI.Windows;

namespace YannickSCF.LSTournaments.Common.Views {
    public class ConfiguratorView : WindowView {

        [SerializeField] private Animator _animator;

        public override void Open() {
            base.Open();
            _animator.SetBool("Show", true);
        }

        public override void Show() {
            base.Show();
            _animator.SetBool("Show", true);
        }

        public override void Hide() {
            _animator.SetBool("Show", false);
            StartCoroutine(WaitToAnimationsEnds(base.Hide));
        }

        public override void Close() {
            _animator.SetBool("Show", false);
            StartCoroutine(WaitToAnimationsEnds(base.Close));
        }

        private IEnumerator WaitToAnimationsEnds(Action actionToDo) {
            yield return new WaitUntil(() => _animator.GetCurrentAnimatorStateInfo(0).IsName("configurator_hidden"));
            actionToDo?.Invoke();
        }
    }
}
