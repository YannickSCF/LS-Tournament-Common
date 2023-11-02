// Dependencies
using System.Collections.Generic;
using System.Linq;
using System;
using System.Globalization;
using System.IO;
using UnityEngine;
// Custom dependencies
using YannickSCF.LSTournaments.Common.Models.Athletes;
using YannickSCF.CountriesData;
using System.Text.RegularExpressions;

namespace YannickSCF.LSTournaments.Common.Tools.Importer.Deserializers {
    public static class AthletesListDeserializer {

        public enum SeparatorType { CSV, TSV };

        private const string LINE_SEPARATOR_CR = "\r";
        private const string LINE_SEPARATOR_LF = "\n";
        private const string LINE_SEPARATOR_CRLF = "\r\n";
        private const string QUOTATING_MARK = "\"";

        private const string STYLES_SEPARATOR = ",";

        private const string REGEX_COMMAS_OUT_OF_LIST = "(?!\\B\"[^\"]*),(?![^\"]*\"\\B)";
        private const string DATE_FORMAT = "dd/MM/yyyy";

        private const string SEMI_COLON_SEPARATOR = ";";
        private const string TAB_SEPARATOR = "\t";

        private static string _valueSeparator = string.Empty;
        private static string _lineSeparator = string.Empty;

        public static List<AthleteInfoType> ImportAthletesInfoFromFile(SeparatorType separatorType, string path) {
            SetSeparatorType(separatorType);

            List<AthleteInfoType> result = new List<AthleteInfoType>();

            string[] allLines = GetArrayOfEntries(path);
            Dictionary<int, AthleteInfoType> infoIndexes = GetIndexForEachValue(allLines[0]);

            foreach (KeyValuePair<int, AthleteInfoType> infoIndex in infoIndexes) {
                result.Add(infoIndex.Value);
            }

            return result;
        }
        
        public static List<AthleteInfoModel> ImportAthletesFromFile(SeparatorType separatorType, string path) {
            SetSeparatorType(separatorType);

            string[] allLines = GetArrayOfEntries(path);

            if (allLines != null) {
                // Get the headers used in CSV and the position in the line array
                Dictionary<int, AthleteInfoType> infoIndexes = GetIndexForEachValue(allLines[0]);
                // Get a list of CSV lines separated in arrays
                List<string[]> linesSeparated = GetValuesOfAllEntries(allLines);
                // Return the athletes objects list
                return ToAthleteObjectList(infoIndexes, linesSeparated);
            }

            Debug.LogError($"There was a problem with CSV format of file {path}!");
            return null;
        }

        private static void SetSeparatorType(SeparatorType separatorType) {
            switch (separatorType) {
                case SeparatorType.TSV: _valueSeparator = TAB_SEPARATOR; break;
                case SeparatorType.CSV:
                default: _valueSeparator = SEMI_COLON_SEPARATOR; break;
            }
        }

        private static Dictionary<int, AthleteInfoType> GetIndexForEachValue(string csvFirstLine) {
            Dictionary<int, AthleteInfoType> result = new Dictionary<int, AthleteInfoType>();

            string[] csvFirstLineSeparated = csvFirstLine.Split(_valueSeparator);
            if (csvFirstLineSeparated != null) {
                Array infoValues = Enum.GetValues(typeof(AthleteInfoType));
                for (int i = 0; i < infoValues.Length; ++i) {
                    AthleteInfoType currentInfo = (AthleteInfoType)infoValues.GetValue(i);
                    for (int j = 0; j < csvFirstLineSeparated.Length; ++j) {
                        // Remove envelope quotes
                        if (csvFirstLineSeparated[j].StartsWith(QUOTATING_MARK) && csvFirstLineSeparated[j].EndsWith(QUOTATING_MARK)) {
                            csvFirstLineSeparated[j] = csvFirstLineSeparated[j].Substring(1, csvFirstLineSeparated[j].Length - 2);
                        }
                        // Check if has coincidence with the AthleteInfoType
                        if (csvFirstLineSeparated[j].Equals(
                            Enum.GetName(typeof(AthleteInfoType), currentInfo),
                            StringComparison.InvariantCultureIgnoreCase)) {
                            result.Add(j, currentInfo);
                            break;
                        }
                    }
                }
            } else {
                Debug.LogError("First line is empty!");
            }

            return result;
        }

        #region Methods to format Separated Value files
        private static string[] GetArrayOfEntries(string path) {
            // Read CSV text from file
            string fileText = File.ReadAllText(path, System.Text.Encoding.UTF8);
            // Format CSV text to a common standard
            fileText = FormatFileText(fileText);
            // Separate lines
            string[] allLines = fileText.Split(_lineSeparator);

            // Si el resultado NO es null, tiene AL MENOS 1 entrada y esa primera entrada NO está vacía
            return allLines != null && allLines.Length > 0 && !string.IsNullOrEmpty(allLines[0]) ?
                // Devolvemos el resultado, si no cumple alguna de estas condiciones devolvemos NULL
                allLines : null;
        }

        private static string FormatFileText(string fileText) {
            // Format line separators
            fileText = FormatLineSeparator(fileText);
            // Format values separators
            fileText = FormatValuesSeparator(fileText);
            return fileText;
        }

        private static string FormatLineSeparator(string csvText) {
            int crCount = csvText.Split(LINE_SEPARATOR_CR).Length;
            int lfCount = csvText.Split(LINE_SEPARATOR_LF).Length;
            int crlfCount = csvText.Split(LINE_SEPARATOR_CRLF).Length;

            // If every count are the same, then all lines are separated by CRLF
            if (crCount == lfCount && crCount == crlfCount) {
                _lineSeparator = LINE_SEPARATOR_CRLF;
            } else {// ... if the counts are different, we will standardize to LF
                csvText = csvText.Replace(LINE_SEPARATOR_CRLF, LINE_SEPARATOR_LF);
                csvText = csvText.Replace(LINE_SEPARATOR_CR, LINE_SEPARATOR_LF);
                _lineSeparator = LINE_SEPARATOR_LF;
            }

            return csvText;
        }

        private static string FormatValuesSeparator(string csvText) {
            string firstLine = csvText.Split(_lineSeparator)[0];

            if (!string.IsNullOrEmpty(firstLine) && _valueSeparator == SEMI_COLON_SEPARATOR) {
                // If there are commas in header out of quotation marks...
                Match searchCommasOutOfQuotation = Regex.Match(firstLine, REGEX_COMMAS_OUT_OF_LIST);
                if (searchCommasOutOfQuotation.Success) {
                    // ... replace all commas out of quotation marks by _valueSeparator
                    csvText = Regex.Replace(csvText, REGEX_COMMAS_OUT_OF_LIST, _valueSeparator);
                }
            }
            return csvText;
        }

        private static List<string[]> GetValuesOfAllEntries(string[] allLines) {
            List<string[]> res = new List<string[]>();

            for (int i = 1; i < allLines.Length; ++i) {
                string[] separated = allLines[i].Split(_valueSeparator);
                if (separated != null && !string.IsNullOrEmpty(separated[0])) {
                    // Remove envelope quotes
                    for (int j = 0; j < separated.Length; ++j) {
                        if (separated[j].StartsWith(QUOTATING_MARK) && separated[j].EndsWith(QUOTATING_MARK)) {
                            separated[j] = separated[j].Substring(1, separated[j].Length - 2);
                        }
                    }

                    res.Add(separated);
                }
            }

            return res;
        }
        #endregion

        #region Methods to parse athletes info
        private static List<AthleteInfoModel> ToAthleteObjectList(Dictionary<int, AthleteInfoType> infoIndexes, List<string[]> athletesInfo) {
            List<AthleteInfoModel> res = new List<AthleteInfoModel>();

            foreach (string[] athleteInfo in athletesInfo) {
                AthleteInfoModel athlete = new AthleteInfoModel();

                foreach (KeyValuePair<int, AthleteInfoType> infoIndex in infoIndexes) {
                    athlete = AddInfoToAthleteModel(athlete, infoIndex.Value, athleteInfo[infoIndex.Key]);
                }

                res.Add(athlete);
            }

            return res.Count == 0 ? null : res;
        }

        private static AthleteInfoModel AddInfoToAthleteModel(AthleteInfoModel toFill, AthleteInfoType infoType, string info) {
            switch (infoType) {
                case AthleteInfoType.Country: toFill.Country = ManageCountry(info); break;
                case AthleteInfoType.Surname: toFill.Surname = info; break;
                case AthleteInfoType.Name: toFill.Name = info; break;
                case AthleteInfoType.Academy: toFill.Academy = info; break;
                case AthleteInfoType.School: toFill.School = info; break;
                case AthleteInfoType.Rank: toFill.Rank = ManageRank(info); break;
                case AthleteInfoType.Styles: toFill.Styles = ManageStyles(info); break;
                case AthleteInfoType.Tier:
                    if (int.TryParse(info, out int parsedNumber)) {
                        toFill.Tier = parsedNumber;
                    } else {
                        CommonExceptionEvents.ThrowDeserializeFieldError(AthleteInfoType.Tier, info);
                    }
                    break;
                case AthleteInfoType.SaberColor:
                    if (ColorUtility.TryParseHtmlString(info, out Color colorParsed)) {
                        toFill.SaberColor = colorParsed;
                    } else {
                        CommonExceptionEvents.ThrowDeserializeFieldError(AthleteInfoType.SaberColor, info);
                    }
                    break;
                case AthleteInfoType.BirthDate:
                    try {
                        toFill.BirthDate = DateTime.ParseExact(info, DATE_FORMAT, CultureInfo.InvariantCulture);
                    } catch (Exception e) {
                        Debug.LogWarning(e.StackTrace);
                        CommonExceptionEvents.ThrowDeserializeFieldError(AthleteInfoType.BirthDate, info);
                    }
                    break;
                case AthleteInfoType.StartDate:
                    try {
                        toFill.StartDate = DateTime.ParseExact(info, DATE_FORMAT, CultureInfo.InvariantCulture);
                    } catch (Exception e) {
                        Debug.LogWarning(e.StackTrace);
                        CommonExceptionEvents.ThrowDeserializeFieldError(AthleteInfoType.StartDate, info);
                    }
                    break;
            }

            return toFill;
        }

        private static RankType ManageRank(string rankStr) {
            string[] rankNames = Enum.GetNames(typeof(RankType));
            if (rankNames.Contains(rankStr)) {
                return (RankType)Enum.Parse(typeof(RankType), rankStr);
            }

            CommonExceptionEvents.ThrowDeserializeFieldError(AthleteInfoType.Rank, rankStr);
            return RankType.Novizio;
        }

        private static List<StyleType> ManageStyles(string stylesValuesString) {
            List<StyleType> res = new List<StyleType>();
            string[] styleNames = Enum.GetNames(typeof(StyleType));

            string[] stylesValues = stylesValuesString.Split(STYLES_SEPARATOR);

            foreach (string style in stylesValues) {
                if (styleNames.Contains(style.Trim())) {
                    res.Add((StyleType)Enum.Parse(typeof(StyleType), style.Trim()));
                } else {
                    CommonExceptionEvents.ThrowDeserializeFieldError(AthleteInfoType.Styles, style);
                }
            }

            return res;
        }

        private static string ManageCountry(string countryStr) {
            if (CountriesDataUtils.IsCountryNameInList(countryStr)) {
                return CountriesDataUtils.GetCodeByName(countryStr);
            }

            if (CountriesDataUtils.IsCountryCodeInList(countryStr)) {
                return countryStr;
            }

            CommonExceptionEvents.ThrowDeserializeFieldError(AthleteInfoType.Country, countryStr);
            return string.Empty;
        }
        #endregion
    }
}
