/**
 * Author:      Yannick Santa Cruz Feuillias
 * Created:     23/10/2023
 **/

// Dependencies
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

namespace YannickSCF.LSTournaments.Common.Views.MainPanel.BaseDrawPanel.ExamplePoules {
    public class ExamplePoulesView : MonoBehaviour {

        [SerializeField] private TextMeshProUGUI _examplePouleTitle;
        [SerializeField] private Transform _examplePouleContent;
        [SerializeField] private Transform _examplePoulePool;

        [SerializeField] private ExamplePoulesAthleteView _pouleEntryPrefab;

        private List<ExamplePoulesAthleteView> _currentEntries;
        private List<ExamplePoulesAthleteView> _poolEntries;

        #region Mono
        private void Awake() {
            _currentEntries = new List<ExamplePoulesAthleteView>();
            _poolEntries = new List<ExamplePoulesAthleteView>();
        }
        #endregion

        public void SetPouleContent(string pouleName, List<string> allPouleEntries) {
            _examplePouleTitle.text = pouleName;

            ResetPoule();

            float fontSize = 0;
            foreach (string pouleEntry in allPouleEntries) {
                ExamplePoulesAthleteView newPouleEntry;
                if (_poolEntries.Count > 0) {
                    newPouleEntry = _poolEntries[0];

                    _poolEntries.Remove(newPouleEntry);
                    newPouleEntry.transform.SetParent(_examplePouleContent);
                    newPouleEntry.gameObject.SetActive(true);
                } else {
                    newPouleEntry = Instantiate(_pouleEntryPrefab, _examplePouleContent);
                }

                float newFontSize = newPouleEntry.SetAthleteText(pouleEntry);
                _currentEntries.Add(newPouleEntry);

                if (fontSize > newFontSize) {
                    fontSize = newFontSize;
                }
            }
        }
        
        public void ResetPoule() {
            for (int i = _currentEntries.Count - 1; i >= 0; --i) {
                ExamplePoulesAthleteView last = _currentEntries.ElementAt(_currentEntries.Count - 1);

                last.gameObject.SetActive(false);
                last.ResetAthlete();
                last.transform.SetParent(_examplePoulePool);

                _currentEntries.Remove(last);
                _poolEntries.Add(last);
            }
        }
    }
}
