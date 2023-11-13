/**
 * Author:      Yannick Santa Cruz Feuillias
 * Created:     10/11/2023
 **/

#if UNITY_EDITOR
// Dependencies
using UnityEditor;
using UnityEngine;

namespace YannickSCF.LSTournaments.Common.Scriptables.Settings.AthleteTables {
    [CustomEditor(typeof(AthleteInfoTableSettings))]
    public class AthleteInfoTableSettingsEditor : Editor {

        private AthleteInfoTableSettings _settings;
        private SerializedObject _settingsSerialized;

        private float _checkTotal = 0f;

        private void OnEnable() {
            _settings = (AthleteInfoTableSettings)target;
            _settingsSerialized = new SerializedObject(_settings);
        }

        private void OnDisable() {
            if (_checkTotal != 100) {
                Debug.LogError("AthleteInfoTableSettings must add 100!");
            }
        }

        public override void OnInspectorGUI() {
            _checkTotal = 0f;
            CreateTitle("Athlete Info Table Settings");

            SerializedProperty rowHeight = _settingsSerialized.FindProperty("_rowHeight");
            rowHeight.floatValue = EditorGUILayout.FloatField("Row Height", rowHeight.floatValue);

            DrawHorizontalGUILine(); // Line
            _checkTotal += GenerateSetting("Country", "_countryIsVisible", "_countryCanExpand", "_countrySize");
            DrawHorizontalGUILine(); // Line
            _checkTotal += GenerateSetting("Surname", "_surnameIsVisible", "_surnameCanExpand", "_surnameSize");
            DrawHorizontalGUILine(); // Line
            _checkTotal += GenerateSetting("Name", "_nameIsVisible", "_nameCanExpand", "_nameSize");
            DrawHorizontalGUILine(); // Line
            _checkTotal += GenerateSetting("Academy", "_academyIsVisible", "_academyCanExpand", "_academySize");
            DrawHorizontalGUILine(); // Line
            _checkTotal += GenerateSetting("School", "_schoolIsVisible", "_schoolCanExpand", "_schoolSize");
            DrawHorizontalGUILine(); // Line
            _checkTotal += GenerateSetting("Rank", "_rankIsVisible", "_rankCanExpand", "_rankSize");
            DrawHorizontalGUILine(); // Line
            _checkTotal += GenerateSetting("Styles", "_stylesIsVisible", "_stylesCanExpand", "_stylesSize");
            DrawHorizontalGUILine(); // Line
            _checkTotal += GenerateSetting("Tier", "_tierIsVisible", "_tierCanExpand", "_tierSize");
            DrawHorizontalGUILine(); // Line
            _checkTotal += GenerateSetting("Saber Color", "_saberColorIsVisible", "_saberColorCanExpand", "_saberColorSize");
            DrawHorizontalGUILine(); // Line
            _checkTotal += GenerateSetting("Birth Date", "_birthDateIsVisible", "_birthDateCanExpand", "_birthDateSize");
            DrawHorizontalGUILine(); // Line
            _checkTotal += GenerateSetting("Start Date", "_startDateIsVisible", "_startDateCanExpand", "_startDateSize");

            EditorUtility.SetDirty(_settings);
        }

        private float GenerateSetting(string name, string visibleProperty, string expandProperty, string sizeProperty) {
            float res = 0f;

            SerializedProperty isVisible = _settingsSerialized.FindProperty(visibleProperty);
            SerializedProperty canExpand = _settingsSerialized.FindProperty(expandProperty);
            SerializedProperty size = _settingsSerialized.FindProperty(sizeProperty);

            isVisible.boolValue = EditorGUILayout.ToggleLeft($"{name}", isVisible.boolValue);
            if (isVisible.boolValue) {
                canExpand.boolValue = EditorGUILayout.Toggle("Can Expand", canExpand.boolValue);
                size.floatValue = EditorGUILayout.Slider("Min Size", size.floatValue, 1, 100);
                res = size.floatValue;
            }

            _settingsSerialized.ApplyModifiedPropertiesWithoutUndo();
            return res;
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
