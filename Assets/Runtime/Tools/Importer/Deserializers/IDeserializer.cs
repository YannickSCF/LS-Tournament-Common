// Dependencies
using System.Collections.Generic;
// Custom dependencies
using YannickSCF.LSTournaments.Common.Models;

namespace YannickSCF.LSTournaments.Common.Tools.Importer.Deserializers {
    public interface IDeserializer {
        public List<PouleInfoModel> GetPoulesFromFile(string path);
        public List<AthleteInfoModel> ImportAthletesFromFile(string path);
    }
}
