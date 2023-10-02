// Dependencies
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
// Custom dependencies
using YannickSCF.LSTournaments.Common.Models.Poules;
using YannickSCF.LSTournaments.Common.Models.Athletes;

namespace YannickSCF.LSTournaments.Common.Tools.Importer.Deserializers {
    public class JSONDeserializer : IDeserializer {

        public List<PouleDataModel> GetPoulesFromFile(string path) {
            throw new System.NotImplementedException();
        }

        public List<AthleteInfoModel> ImportAthletesFromFile(string path) {
            string jsonText = File.ReadAllText(path);

            List<AthleteInfoModel> athletes = JsonConvert.DeserializeObject<List<AthleteInfoModel>>(jsonText);

            return athletes.Count == 0 ? null : athletes;
        }

        //public DrawConfiguration ImportDrawFormJSON(string filePath) {
        //    string jsonText = File.ReadAllText(filePath);
        //    DrawConfiguration drawConfig = JsonUtility.FromJson<DrawConfiguration>(jsonText);
        //    return drawConfig;
        //}
    }
}
