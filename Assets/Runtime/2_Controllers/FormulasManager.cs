/**
 * Author:      Yannick Santa Cruz Feuillias
 * Created:     06/12/2023
 **/

// Dependencies
using System.Collections.Generic;
using UnityEngine;
// Custom dependencies
using YannickSCF.LSTournaments.Common.Scriptables.Formulas;

namespace YannickSCF.LSTournaments.Common.Controllers {
    public class FormulasManager : MonoBehaviour {

        [Header("Tournament Formulas")]
        [SerializeField] private List<TournamentFormula> _allTournamentFormulas;
        [SerializeField] private TournamentFormula _customFormula;

        private void Awake() {
            TournamentFormulaUtils.SetTournamentFormulas(_allTournamentFormulas, _customFormula);
        }
    }
}
