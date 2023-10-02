// Dependencies
using System.Collections.Generic;
// Custom dependencies
using YannickSCF.LSTournaments.Common.Models.Poules;
using YannickSCF.LSTournaments.Common.Models.Athletes;

namespace YannickSCF.LSTournaments.Common.Tools.Importer.Deserializers {
    public interface IDeserializer {
        public List<PouleDataModel> GetPoulesFromFile(string path);
        public List<AthleteInfoModel> ImportAthletesFromFile(string path);
    }
}
