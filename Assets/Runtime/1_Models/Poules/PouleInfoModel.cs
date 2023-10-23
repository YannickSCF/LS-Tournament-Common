// Depdendencies
using System.Collections.Generic;
using UnityEngine;

namespace YannickSCF.LSTournaments.Common.Models.Poules {
    [System.Serializable]
    public class PouleInfoModel {
        [SerializeField] private PouleNamingType _namingInfo;
        [SerializeField] private int _roundsOfPoules;

        [SerializeField] private PouleFillerSubtype _fillerSubtypeInfo;

        [SerializeField] private int[,] _pouleCountAndSizes;
        [SerializeField] private List<PouleDataModel> _data;

        public PouleInfoModel(PouleNamingType namingInfo, int roundsOfPoules, PouleFillerSubtype fillerSubtypeInfo = PouleFillerSubtype.None) {
            _namingInfo = namingInfo;
            _roundsOfPoules = roundsOfPoules;
            _fillerSubtypeInfo = fillerSubtypeInfo;
            _data = new List<PouleDataModel>();
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
        public int[,] PouleCountAndSizes { get => _pouleCountAndSizes; set => _pouleCountAndSizes = value; }

        public int GetPouleCount() {
            if (_pouleCountAndSizes == null) return -1;
            return _pouleCountAndSizes[0, 0] + _pouleCountAndSizes[1, 0];
        }
        public int GetPouleMaxSize() {
            if (_pouleCountAndSizes == null) return -1;
            return _pouleCountAndSizes[0, 1];
        }
        public int GetPouleMinSize() {
            if (_pouleCountAndSizes == null) return -1;
            return _pouleCountAndSizes[1, 1];
        }
    }
}
