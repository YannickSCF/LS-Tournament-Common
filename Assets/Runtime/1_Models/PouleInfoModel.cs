// Dependencies
using System.Collections.Generic;
using UnityEngine;

namespace YannickSCF.LSTournaments.Common.Models {
    [System.Serializable]
    public class PouleInfoModel {
        [SerializeField] private string _name;
        [SerializeField] private List<string> _athletesIds;
        //[SerializeField] private List<MatchInfoModel> _matches;

        #region Properties
        public string Name { get => _name; }
        public List<string> AthletesIds { get => _athletesIds; set => _athletesIds = value; }
        //public List<MatchInfoModel> Matches { get => _matches; set => _matches = value; }
        #endregion

        #region Constuctors
        public PouleInfoModel(string name) {
            _name = name;
            _athletesIds = new List<string>();
        }

        public PouleInfoModel(string name, List<string> athletesIds) {
            _name = name;
            _athletesIds = athletesIds;
        }
        #endregion
    }
}
