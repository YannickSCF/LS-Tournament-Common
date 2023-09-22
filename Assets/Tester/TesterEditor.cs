#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using YannickSCF.LSTournaments.Common.Tools.Poule;
using YannickSCF.LSTournaments.Common.Models;
using YannickSCF.LSTournaments.Common;

[CustomEditor(typeof(Tester))]
public class TesterEditor : Editor {

    private PoulesBuilder _builder;

    private Tester _tester;

    private void OnEnable() {
        _tester = (Tester)target;
    }

    public override void OnInspectorGUI() {
        base.OnInspectorGUI();

        EditorGUILayout.Space(20);
        if (GUILayout.Button("Create 4 new Athletes")) {
            CreateAthletes();
        }
        if (GUILayout.Button("Execute")) {
            Randomizer.SetSeed(0);
            _builder = PoulesBuilder.GetBuilder(_tester.BuilderType, _tester.MaxPouleSize);
            List<string> names = _builder.GetPoulesNames(_tester.NamingType, _tester.NumPoules, _tester.PouleRounds);
            List<PouleInfoModel> poules = _builder.BuildPoules(names, _tester.Athletes, _tester.BuilderSubtype);
            ShowPoules(poules);
        }
    }

    private void CreateAthletes() {
        int existingAthletes = _tester.Athletes.Count;
        for (int i = 0; i < 4; ++i) {
            AthleteInfoModel athlete = new AthleteInfoModel();
            athlete.Name = "Name " + (existingAthletes + i).ToString();
            athlete.Surname = "Surname " + (existingAthletes + i).ToString();
            _tester.Athletes.Add(athlete);
        }
    }

    private void ShowPoules(List<PouleInfoModel> poules) {
        string toShow = string.Empty;

        foreach (PouleInfoModel poule in poules) {
            toShow += poule.Name +":\n";
            foreach(AthleteInfoModel athlete in poule.Athletes) {
                toShow += "    - (" + athlete.Country + ") " + athlete.Name + "\n";
            }
        }

        Debug.Log(toShow);
    }
}
#endif
