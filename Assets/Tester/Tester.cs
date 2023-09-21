using System.Collections.Generic;
using UnityEngine;
using YannickSCF.LSTournaments.Common;
using YannickSCF.LSTournaments.Common.Models;

[CreateAssetMenu(fileName = "Tester", menuName = "YannickSCF/Tester")]
public class Tester : ScriptableObject {

    [Header("Athletes")]
    [SerializeField] private List<AthleteInfoModel> _athletes;
    [Space]
    [Header("Poules Sizes")]
    [SerializeField] private int _numPoules = 1;
    [SerializeField] private int _maxPouleSize = 8;
    [Header("Poule Naming")]
    [SerializeField] private PouleNamingType _namingType;
    [SerializeField] private int _pouleRounds = 1;
    [Header("Poule Sorter")]
    [SerializeField] private PouleBuilderType _builderType;
    [SerializeField] private PouleBuilderSubtype _builderSubtype;

    public List<AthleteInfoModel> Athletes { get => _athletes; }
    public int NumPoules { get => _numPoules; }
    public int MaxPouleSize { get => _maxPouleSize; }
    public PouleNamingType NamingType { get => _namingType; }
    public int PouleRounds { get => _pouleRounds; }
    public PouleBuilderType BuilderType { get => _builderType; }
    public PouleBuilderSubtype BuilderSubtype { get => _builderSubtype; }
}
