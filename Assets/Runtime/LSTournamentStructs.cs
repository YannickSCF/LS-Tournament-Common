/**
 * Author:      Yannick Santa Cruz Feuillias
 * Created:     08/02/2024
 **/

// Dependencies
using UnityEngine;

namespace YannickSCF.LSTournaments.Common {

    public struct AthleteBasicInfo {
        public string CompleteName;
        public string CountryId;
        public string SelectedOrigin;

        public AthleteBasicInfo(string completeName, string countryId, string origin) {
            CompleteName = completeName;
            CountryId = countryId;
            SelectedOrigin = origin;
        }
    }

    public struct AthleteCards {
        public int White;
        public int Yellow;
        public bool Red;
        public bool Black;
    }
}
