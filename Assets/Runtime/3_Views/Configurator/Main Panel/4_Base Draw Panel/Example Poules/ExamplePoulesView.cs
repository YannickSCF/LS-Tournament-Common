// Dependencies
using System.Collections.Generic;
using TMPro;
using UnityEngine;
// Custom dependencies

namespace YannickSCF.LSTournaments.Common.Views.MainPanel.BaseDrawPanel.ExamplePoules {
    public class ExamplePoulesView : MonoBehaviour {

        [SerializeField] private TextMeshProUGUI _examplePouleTitle;
        [SerializeField] private Transform _examplePouleContent;

        [SerializeField] private ExamplePoulesAthleteView _pouleEntryPrefab;

        private bool _isPouleInitialized = false;
        private List<ExamplePoulesAthleteView> _currentEntries;

        public bool IsPouleInitialized { get => _isPouleInitialized; }

        #region Mono
        private void Awake() {
            _currentEntries = new List<ExamplePoulesAthleteView>();
        }
        #endregion

        public void SetPouleContent(string pouleName, List<string> allPouleEntries) {
            if (!_isPouleInitialized || allPouleEntries.Count != _currentEntries.Count) {
                InitPouleContent(pouleName, allPouleEntries);
            } else {
                UpdatePouleContent(pouleName, allPouleEntries);
            }
        }
        public void ResetPoule() {
            foreach (Transform child in _examplePouleContent) {
                DestroyImmediate(child.gameObject);
            }
            _currentEntries.Clear();

            _isPouleInitialized = false;
        }

        private void UpdatePouleContent(string pouleName, List<string> allPouleEntries) {
            _examplePouleTitle.text = pouleName;

            float fontSize = 0;
            for (int i = 0; i < allPouleEntries.Count; ++i) {
                float newFontSize = _currentEntries[i].SetAthleteText(allPouleEntries[i]);
                if (fontSize > newFontSize) {
                    fontSize = newFontSize;
                }
            }

            foreach (ExamplePoulesAthleteView entry in _currentEntries) {
                entry.SetFontSize(fontSize);
            }
        }

        private void InitPouleContent(string pouleName, List<string> allPouleEntries) {
            _examplePouleTitle.text = pouleName;

            ResetPoule();

            float fontSize = 0;
            for (int i = 0; i < allPouleEntries.Count; ++i) {
                ExamplePoulesAthleteView newAthlete = Instantiate(_pouleEntryPrefab, _examplePouleContent);
                float newFontSize = newAthlete.SetAthleteText(allPouleEntries[i]);

                if (fontSize > newFontSize) {
                    fontSize = newFontSize;
                }

                _currentEntries.Add(newAthlete);
            }

            foreach (ExamplePoulesAthleteView entry in _currentEntries) {
                entry.SetFontSize(fontSize);
            }

            _isPouleInitialized = true;
        }
    }
}
