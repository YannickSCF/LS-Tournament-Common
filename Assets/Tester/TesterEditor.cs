#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using YannickSCF.LSTournaments.Common.Tools.Poule;
using YannickSCF.LSTournaments.Common.Models;
using YannickSCF.LSTournaments.Common;
using YannickSCF.LSTournaments.Common.Tools.Poule.Filler;

[CustomEditor(typeof(Tester))]
public class TesterEditor : Editor {

    private PoulesFiller _filler;

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
            _filler = PoulesFiller.GetFiller(_tester.FillerType, _tester.MaxPouleSize);
            List<string> names = PouleUtils.GetPoulesNames(_tester.NamingType, _tester.NumPoules, _tester.PouleRounds);
            List<PouleInfoModel> poules = _filler.FillPoules(names, _tester.Athletes, _tester.FillerSubtype);
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
