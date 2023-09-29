using System.Collections.Generic;
using YannickSCF.LSTournaments.Common.Models;

namespace YannickSCF.LSTournaments.Common.Tools.Importers {
    public interface IDeserializer {
        public List<PouleInfoModel> GetPoulesFromFile(string path);
        public List<AthleteInfoModel> ImportAthletesFromFile(string path);
    }
}
