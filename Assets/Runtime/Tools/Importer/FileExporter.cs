/**
 * Author:      Yannick Santa Cruz Feuillias
 * Created:     29/09/2023
 **/

// Dependencies
using System;
using System.IO;
using UnityEngine;
// Custom dependencies
using AnotherFileBrowser.Windows;

namespace YannickSCF.LSTournaments.Common.Tools.FileManagement {
    public static class FileExporter {
        public static void SaveFileBrowser(string tournamentName, string jsonContent) {
            var bp = new BrowserProperties();
            bp.filter = "json files (*.json)|*.json";
            bp.filterIndex = 0;

            new FileBrowser().SaveFileBrowser(bp, tournamentName, ".json", path => {
                Debug.Log(path);

                if(path != null) {
                    try {
                        using (StreamWriter writer = File.CreateText(path)) {
                            {
                                writer.Write(jsonContent);
                            }
                            writer.Close();
                        }
                    } catch (Exception e) {
                        Debug.LogWarning(e.StackTrace);
                        CommonExceptionEvents.ThrowExportError(path);
                    }
                }
            });
        }
    }
}
