using System.Collections.Generic;
using System.IO;
using UnityEngine;
using YannickSCF.LSTournaments.Common.Models;

namespace YannickSCF.LSTournaments.Common.Tools.Importers {
    public class JSONDeserializer : IDeserializer {

        public List<PouleInfoModel> GetPoulesFromFile(string path) {
            throw new System.NotImplementedException();
        }

        public List<AthleteInfoModel> ImportAthletesFromFile(string path) {
            string jsonText = File.ReadAllText(path);

            List<AthleteInfoModel> athletes = JsonUtility.FromJson<List<AthleteInfoModel>>(jsonText);

            if (athletes.Count == 0) return null;
            else return athletes;
        }

        //public DrawConfiguration ImportDrawFormJSON(string filePath) {
        //    string jsonText = File.ReadAllText(filePath);
        //    DrawConfiguration drawConfig = JsonUtility.FromJson<DrawConfiguration>(jsonText);
        //    return drawConfig;
        //}
    }
}
