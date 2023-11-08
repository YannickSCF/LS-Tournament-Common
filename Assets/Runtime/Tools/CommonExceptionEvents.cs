/**
 * Author:      Yannick Santa Cruz Feuillias
 * Created:     02/11/2023
 **/

// Dependencies
using System;
using UnityEngine;
// Custom dependencies
using static YannickSCF.GeneralApp.CommonEventsDelegates;

namespace YannickSCF.LSTournaments.Common {
    public static class CommonExceptionEvents {

        public enum ImportErrorType { FileNotFound, BadExtension, EmptyFile }

        public delegate void DeserializeFieldError(AthleteInfoType infoType, string entryString);
        public delegate void ImportError(ImportErrorType errorType, string filePath);

        public static event DeserializeFieldError DeserializeFieldErrorFound;
        public static void ThrowDeserializeFieldError(AthleteInfoType infoType, string entryString) {
            Debug.LogWarning($"A {Enum.GetName(typeof(AthleteInfoType), infoType)} field with parameter '{entryString}' cannot be deserialized correctly");
            DeserializeFieldErrorFound?.Invoke(infoType, entryString);
        }

        public static event StringEventDelegate ExportErrorFound;
        public static void ThrowExportError(string filePath) {
            Debug.LogWarning($"Cannot EXPORT to file '{filePath}'...");
            ExportErrorFound?.Invoke(filePath);
        }

        public static event ImportError ImportErrorFound;
        public static void ThrowImportError(ImportErrorType errorType, string filePath) {
            Debug.LogWarning($"Cannot IMPORT to file '{filePath}' because of error {Enum.GetName(typeof(ImportErrorType), errorType)}...");
            ImportErrorFound?.Invoke(errorType, filePath);
        }
    }
}
