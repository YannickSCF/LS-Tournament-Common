// Dependencies
using System;
using System.Collections.Generic;
using UnityEngine;
// Custom dependencies
using AnotherFileBrowser.Windows;
using YannickSCF.LSTournaments.Common.Models;

namespace YannickSCF.LSTournaments.Common.Tools.Importers {
    public static class FileImporter {

        public static void OpenFileBrowser() {
            var browserProperties = new BrowserProperties();
            string myPath = "(holi)";

            new FileBrowser().OpenFileBrowser(browserProperties, path => {
                if (path != null) {
                    myPath = path;
                    IDeserializer deserializer;
                    if (path.ToLower().EndsWith(".json")) {
                        deserializer = new JSONDeserializer();
                    } else if (path.ToLower().EndsWith(".csv")) {
                        deserializer = new CSVDeserializer();
                    } else {
                        deserializer = new CSVDeserializer();
                    }

                    List<AthleteInfoModel> athletes = deserializer.ImportAthletesFromFile(path);

                    //PouleBuilder pouleBuilder = new AlphaPouleBuilder();
                    //ITierListBuilder tierListBuilder = new StyleTierListBuilder();
                    //_ = pouleBuilder.BuildPoules(athletes, tierListBuilder);
                    Debug.Log("OpenFileBrowser Finished!");
                }
            });

            Debug.Log("Returned path: " + myPath);
        }

        public static List<AthleteInfoModel> ImportAthletesFromFile(string filePath) {
            IDeserializer deserializer;

            if (filePath.ToLower().EndsWith(".json")) {
                deserializer = new JSONDeserializer();
            } else if (filePath.ToLower().EndsWith(".csv")) {
                deserializer = new CSVDeserializer();
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
                if (path.ToLower().EndsWith(".json") || path.ToLower().EndsWith(".csv")) {
                    res = path;
                }
            });

            return res;
        }
    }
}
