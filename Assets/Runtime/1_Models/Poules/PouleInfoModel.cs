// Depdendencies
using System.Collections.Generic;
using UnityEngine;

namespace YannickSCF.LSTournaments.Common.Models.Poules {
    [System.Serializable]
    public class PouleInfoModel {
        [SerializeField] private PouleNamingType _namingInfo;
        [SerializeField] private int _roundsOfPoules;

        [SerializeField] private PouleFillerSubtype _fillerSubtypeInfo;

        [SerializeField] private List<PouleDataModel> _data;

        public PouleInfoModel(PouleNamingType namingInfo, int roundsOfPoules, PouleFillerSubtype fillerSubtypeInfo) {
            _namingInfo = namingInfo;
            _roundsOfPoules = roundsOfPoules;
            _fillerSubtypeInfo = fillerSubtypeInfo;
        }

        public PouleInfoModel(PouleNamingType namingInfo, int roundsOfPoules, PouleFillerSubtype fillerSubtypeInfo, List<PouleDataModel> data) {
            _namingInfo = namingInfo;
            _roundsOfPoules = roundsOfPoules;
            _fillerSubtypeInfo = fillerSubtypeInfo;
            _data = data;
        }

        public PouleNamingType NamingInfo { get => _namingInfo; }
        public int RoundsOfPoules { get => _roundsOfPoules; }
        public PouleFillerSubtype FillerSubtypeInfo { get => _fillerSubtypeInfo; }
        public List<PouleDataModel> Data { get => _data; }
    }
}
