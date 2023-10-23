// Dependencies
using System.Collections.Generic;
// Custom dependencies
using YannickSCF.LSTournaments.Common.Models.Poules;
using YannickSCF.LSTournaments.Common.Models.Athletes;

namespace YannickSCF.LSTournaments.Common.Tools.Importer.Deserializers {
    public class TSVDeserializer : IDeserializer {
        // TODO
        public List<PouleDataModel> GetPoulesFromFile(string path) {
            throw new System.NotImplementedException();
        }

        public List<AthleteInfoType> ImportAthletesInfoFromFile(string path) {
            throw new System.NotImplementedException();
        }

        public List<AthleteInfoModel> ImportAthletesFromFile(string path) {
            throw new System.NotImplementedException();
        }
    }
}
