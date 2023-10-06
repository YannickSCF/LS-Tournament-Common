// Dependencies
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace YannickSCF.LSTournaments.Common.Scriptables.Formulas {
    public static class TournamentFormulaUtils {
        private static List<TournamentFormula> _allTournamentFormulas;

        public static void SetTournamentFormulas(List<TournamentFormula> tournamentFormulas) {
            _allTournamentFormulas = tournamentFormulas;
        }

        public static List<string> GetTournamentFormulasNames() {
            if (_allTournamentFormulas == null) {
                Debug.LogError("List of Formulas is not set up!");
                return null;
            }
            return _allTournamentFormulas.Select(x => x.FormulaName).ToList();
        }

        public static TournamentFormula GetFormulaByName(string formulaToLookFor) {
            if (_allTournamentFormulas == null) {
                Debug.LogError("List of Formulas is not set up!");
                return null;
            }
            return _allTournamentFormulas.Where(x => x.FormulaName == formulaToLookFor).FirstOrDefault();
        }
    }
}
