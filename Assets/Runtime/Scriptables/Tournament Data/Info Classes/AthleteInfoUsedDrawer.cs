/**
 * Author:      Yannick Santa Cruz Feuillias
 * Created:     13/11/2023
 **/

// Dependencies
using UnityEditor;
using UnityEngine;

namespace YannickSCF.LSTournaments.Common.Scriptables.Data.Objects {
    [CustomPropertyDrawer(typeof(AthleteInfoUsed))]
    public class AthleteInfoUsedDrawer : PropertyDrawer {

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
            //base.OnGUI(position, property, label);

            EditorGUI.BeginProperty(position, label, property);

            SerializedProperty infoProperty = property.FindPropertyRelative("_info");
            var infoRect = new Rect(position.x, position.y, 100, position.height);
            EditorGUI.LabelField(infoRect, infoProperty.enumNames[infoProperty.enumValueIndex]);

            var statusRect = new Rect(position.x + 100, position.y, 100, position.height);
            EditorGUI.PropertyField(statusRect, property.FindPropertyRelative("_status"), GUIContent.none);
        }
    }
}
