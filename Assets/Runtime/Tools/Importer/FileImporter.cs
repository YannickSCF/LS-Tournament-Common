// Dependencies
using System;
using System.Collections.Generic;
using UnityEngine;
// Custom dependencies
using AnotherFileBrowser.Windows;
using YannickSCF.LSTournaments.Common.Models;
using YannickSCF.LSTournaments.Common.Tools.Importer.Deserializers;

namespace YannickSCF.LSTournaments.Common.Tools.Importers {
    public static class FileImporter {

        public static List<AthleteInfoModel> ImportAthletesFromFile(string filePath) {
            IDeserializer deserializer;

            if (filePath.ToLower().EndsWith(".json")) {
                deserializer = new JSONDeserializer();
            } else if (filePath.ToLower().EndsWith(".csv")) {
                deserializer = new CSVDeserializer();
            } else if (filePath.ToLower().EndsWith(".tsv")) {
                deserializer = new TSVDeserializer();
            } else {
                throw new Exception("File with incorrect extension");
            }

            return deserializer.ImportAthletesFromFile(filePath);
        }

        //public static DrawConfiguration ImportDrawFormJSON(string filePath) {
        //    JSONDeserializer deserializer = new JSONDeserializer();
        //    return deserializer.ImportDrawFormJSON(filePath);
        //}

        public static string SelectFileWithBrowser() {
            var browserProperties = new BrowserProperties();
            string res = "";

            new FileBrowser().OpenFileBrowser(browserProperties, path => {
                if (path.ToLower().EndsWith(".json") || path.ToLower().EndsWith(".csv") || path.ToLower().EndsWith(".tsv")) {
                    res = path;
                }
            });

            return res;
        }
    }
}
