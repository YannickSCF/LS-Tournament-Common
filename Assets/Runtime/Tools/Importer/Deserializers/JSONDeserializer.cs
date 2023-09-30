// Dependencies
using System.Collections.Generic;
using System.IO;
using UnityEngine;
// Custom dependencies
using YannickSCF.LSTournaments.Common.Models;

namespace YannickSCF.LSTournaments.Common.Tools.Importer.Deserializers {
    public class JSONDeserializer : IDeserializer {

        public List<PouleInfoModel> GetPoulesFromFile(string path) {
            throw new System.NotImplementedException();
        }

        public List<AthleteInfoModel> ImportAthletesFromFile(string path) {
            string jsonText = File.ReadAllText(path);

            List<AthleteInfoModel> athletes = JsonUtility.FromJson<List<AthleteInfoModel>>(jsonText);

            return athletes.Count == 0 ? null : athletes;
        }

        //public DrawConfiguration ImportDrawFormJSON(string filePath) {
        //    string jsonText = File.ReadAllText(filePath);
        //    DrawConfiguration drawConfig = JsonUtility.FromJson<DrawConfiguration>(jsonText);
        //    return drawConfig;
        //}
    }
}
