/**
 * Author:      Yannick Santa Cruz Feuillias
 * Created:     06/10/2023
 **/

// Dependencies
using System;
using System.Collections.Generic;
using System.Linq;

namespace YannickSCF.LSTournaments.Common.Scriptables.Formulas {
    public static class TournamentFormulaUtils {
        private static List<TournamentFormula> _allTournamentFormulas;
        private static TournamentFormula _customFormula;

        public static void SetTournamentFormulas(List<TournamentFormula> tournamentFormulas, TournamentFormula customFormula) {
            _allTournamentFormulas = tournamentFormulas;
            _customFormula = customFormula;
        }

        public static List<string> GetTournamentFormulasNames() {
            CheckUtilsInitialized();

            List<string> result = _allTournamentFormulas.Select(x => x.FormulaName).ToList();
            result.Add(_customFormula.FormulaName);
            return result;
        }

        public static TournamentFormula GetFormulaByName(string formulaToLookFor) {
            CheckUtilsInitialized();

            TournamentFormula result = _allTournamentFormulas.Where(x => x.FormulaName == formulaToLookFor).FirstOrDefault();
            if (result == null) {
                result = _customFormula.FormulaName == formulaToLookFor ? _customFormula : null;
            }
            return result;
        }

        public static bool IsCustomFormula(TournamentFormula formulaToCheck) {
            CheckUtilsInitialized();
            return formulaToCheck == _customFormula;
        }
        public static bool IsCustomFormula(string formulaNameToCheck) {
            CheckUtilsInitialized();
            return formulaNameToCheck == _customFormula.FormulaName;
        }

        private static void CheckUtilsInitialized() {
            if (_allTournamentFormulas == null || _allTournamentFormulas.Count <= 0 || _customFormula == null)
                throw new Exception("List of Formulas is not set up!");
        }
    }
}
