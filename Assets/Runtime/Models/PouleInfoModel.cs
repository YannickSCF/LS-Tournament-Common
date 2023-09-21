using System.Collections.Generic;
using UnityEngine;

namespace YannickSCF.LSTournaments.Common.Models {
    [System.Serializable]
    public class PouleInfoModel {
        [SerializeField] private string _name;
        [SerializeField] private List<AthleteInfoModel> _athletes;

        #region Properties
        public string Name { get => _name; }
        public List<AthleteInfoModel> Athletes { get => _athletes; }
        #endregion

        #region Constuctors
        public PouleInfoModel(string name) {
            _name = name;
            _athletes = new List<AthleteInfoModel>();
        }

        public PouleInfoModel(string name, List<AthleteInfoModel> athletes) {
            _name = name;
            _athletes = athletes;
        }
        #endregion
    }
}
