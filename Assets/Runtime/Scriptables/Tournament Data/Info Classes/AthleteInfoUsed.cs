/**
 * Author:      Yannick Santa Cruz Feuillias
 * Created:     13/11/2023
 **/

// Dependencies
using UnityEngine;

namespace YannickSCF.LSTournaments.Common.Scriptables.Data.Objects {
    [System.Serializable]
    public class AthleteInfoUsed {

        [SerializeField] private AthleteInfoType _info;
        [SerializeField] private AthleteInfoStatus _status;

        public AthleteInfoUsed(AthleteInfoType info, AthleteInfoStatus status) {
            _info = info;
            _status = status;
        }

        public AthleteInfoType Info { get => _info; set => _info = value; }
        public AthleteInfoStatus Status { get => _status; set => _status = value; }
    }
}
