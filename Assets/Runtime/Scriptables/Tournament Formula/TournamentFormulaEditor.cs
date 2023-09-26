#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

namespace YannickSCF.LSTournaments.Common.Scriptables.Formulas.FormulasEditor {
    [CustomEditor(typeof(TournamentFormula))]
    internal class TournamentFormulaEditor : Editor {
        private TournamentFormula _formula;
        SerializedObject _formulaSerialized;

        float minPouleSize = 0;
        float maxPouleSize = 0;

        private void OnEnable() {
            _formula = (TournamentFormula)target;
            _formulaSerialized = new SerializedObject(_formula);
        }

        public override void OnInspectorGUI() {

            CreateTitle("Formula Name");
            GUIStyle style = new GUIStyle(GUI.skin.textField) { alignment = TextAnchor.MiddleCenter };
            _formula.SetFormulaName(EditorGUILayout.TextField(_formula.FormulaName, style, GUILayout.ExpandWidth(true)));

            DrawHorizontalGUILine(); // Line

            DrawPouleOptions();

            DrawHorizontalGUILine(); // Line

            TournamentParameters();

            DrawHorizontalGUILine(); // Line

            DrawRankings();

            EditorUtility.SetDirty(_formula);
        }

        private void DrawPouleOptions() {
            CreateTitle("Poule Options");
            minPouleSize = _formula.MinPouleSize;
            maxPouleSize = _formula.MaxPouleSize;

            _formula.SetInfinitePoules(EditorGUILayout.Toggle("Infinite Poules: ", _formula.InfinitePoules));
            if (!_formula.InfinitePoules) {
                ++EditorGUI.indentLevel;
                SerializedProperty numberOfPoules = _formulaSerialized.FindProperty("_possibleNumberOfPoules");
                EditorGUILayout.PropertyField(numberOfPoules);
                _formulaSerialized.ApplyModifiedPropertiesWithoutUndo();
                --EditorGUI.indentLevel;
            }

            // Min Max Poule Size
            EditorGUILayout.LabelField("Poule Size:");

            ++EditorGUI.indentLevel;
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Min: " + (int)minPouleSize, GUILayout.MaxWidth(60));
            
            --EditorGUI.indentLevel;
            EditorGUILayout.MinMaxSlider(ref minPouleSize, ref maxPouleSize, 3, 11);
            ++EditorGUI.indentLevel;

            minPouleSize = (int)minPouleSize;
            _formula.SetMinPouleSize((int)minPouleSize);
            maxPouleSize = (int)maxPouleSize;
            _formula.SetMaxPouleSize((int)maxPouleSize);
            
            EditorGUILayout.LabelField("Max: " + (int)maxPouleSize, GUILayout.MaxWidth(60));
            EditorGUILayout.EndHorizontal();
            --EditorGUI.indentLevel;

            EditorGUILayout.Space();
            _formula.SetFillerType((PouleFillerType)EditorGUILayout.EnumPopup("Poule Fill Type", _formula.FillerType));
        }

        private void TournamentParameters() {
            CreateTitle("Tournament Parameters");

            _formula.SetScoring((ScoringType)EditorGUILayout.EnumPopup("Scoring Method", _formula.Scoring));
            _formula.SetClassification((ClassificationType)EditorGUILayout.EnumPopup("Classification Method", _formula.Classification));
            _formula.SetMaxEliminationPhases((EliminationRound)EditorGUILayout.EnumPopup("Max Elimination Phase", _formula.MaxEliminationPhases));
        }

        private void DrawRankings() {
            CreateTitle("Ranking Sorters");
            _formula.SetStyleSorter((StyleRankingType)EditorGUILayout.EnumPopup("Style Sorter", _formula.StyleSorter));
            _formula.SetWarSorter((WarRankingType)EditorGUILayout.EnumPopup("War Sorter", _formula.WarSorter));
            _formula.SetMixedSorter((MixedRankingType)EditorGUILayout.EnumPopup("Mixed Sorter", _formula.MixedSorter));
        }

        private void CreateTitle(string titleText) {
            EditorGUILayout.Space();
            GUIStyle style = new GUIStyle(GUI.skin.label) { alignment = TextAnchor.UpperCenter, fontSize = 16, fontStyle = FontStyle.Bold };
            EditorGUILayout.LabelField(titleText, style, GUILayout.ExpandWidth(true), GUILayout.MinHeight(22), GUILayout.ExpandHeight(true));
            EditorGUILayout.Space();
        }

        private static void DrawHorizontalGUILine(int height = 1) {
            GUILayout.Space(20);

            Rect rect = GUILayoutUtility.GetRect(10, height, GUILayout.ExpandWidth(true));
            rect.height = height;
            rect.xMin = 0;
            rect.xMax = EditorGUIUtility.currentViewWidth;

            Color lineColor = new Color(0.10196f, 0.10196f, 0.10196f, 1);
            EditorGUI.DrawRect(rect, lineColor);
            GUILayout.Space(10);
        }
    }
}
#endif
