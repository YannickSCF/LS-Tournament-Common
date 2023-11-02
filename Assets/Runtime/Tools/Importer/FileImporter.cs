// Dependencies
using System;
using System.Collections.Generic;
using UnityEngine;
// Custom dependencies
using AnotherFileBrowser.Windows;
using YannickSCF.LSTournaments.Common.Models.Athletes;
using YannickSCF.LSTournaments.Common.Tools.Importer.Deserializers;
using static YannickSCF.LSTournaments.Common.Tools.Importer.Deserializers.AthletesListDeserializer;

namespace YannickSCF.LSTournaments.Common.Tools.Importers {
    public static class FileImporter {

        #region Athletes File Importer
        public static string SelectAthletesFileWithBrowser() {
            var browserProperties = new BrowserProperties();
            string res = "";

            new FileBrowser().OpenFileBrowser(browserProperties, path => {
                if (path.ToLower().EndsWith(".csv") || path.ToLower().EndsWith(".tsv")) {
                    res = path;
                } else {
                    CommonExceptionEvents.ThrowImportError(
                        CommonExceptionEvents.ImportErrorType.BadExtension, path);
                }
            });

            return res;
        }

        public static List<AthleteInfoModel> ImportAthletesFromFile(string filePath) {
            if (filePath.ToLower().EndsWith(".csv")) {
                return AthletesListDeserializer.ImportAthletesFromFile(SeparatorType.CSV, filePath);
            } else if (filePath.ToLower().EndsWith(".tsv")) {
                return AthletesListDeserializer.ImportAthletesFromFile(SeparatorType.TSV, filePath);
            }

            CommonExceptionEvents.ThrowImportError(
                CommonExceptionEvents.ImportErrorType.BadExtension, filePath);
            return null;
        }

        public static List<AthleteInfoType> ImportAthletesInfoFromFile(string filePath) {
            if (filePath.ToLower().EndsWith(".csv")) {
                return AthletesListDeserializer.ImportAthletesInfoFromFile(SeparatorType.CSV, filePath);
            } else if (filePath.ToLower().EndsWith(".tsv")) {
                return AthletesListDeserializer.ImportAthletesInfoFromFile(SeparatorType.TSV, filePath);
            }

            CommonExceptionEvents.ThrowImportError(
                CommonExceptionEvents.ImportErrorType.BadExtension, filePath);
            return null;
        }
        #endregion

        #region Draw Importer
        #endregion
    }
}
