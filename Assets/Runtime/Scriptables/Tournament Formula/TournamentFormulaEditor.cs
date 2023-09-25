#if UNITY_EDITOR
using UnityEditor;

namespace YannickSCF.LSTournaments.Common.Scriptables.Formulas.FormulasEditor {
    [CustomEditor(typeof(TournamentFormulaEditor))]
    internal class TournamentFormulaEditor : Editor {
        private TournamentFormula _formula;

        private void OnEnable() {
            _formula = (TournamentFormula)target;
        }

        public override void OnInspectorGUI() {
            base.OnInspectorGUI();

        }
    }
}
#endif
