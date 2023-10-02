// Dependencies
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;
// Custom dependencies
using YannickSCF.CountriesData;
using YannickSCF.LSTournaments.Common.Models.Poules;
using YannickSCF.LSTournaments.Common.Models.Athletes;

namespace YannickSCF.LSTournaments.Common.Tools.Importer.Deserializers {
    public class CSVDeserializer : IDeserializer {
        private const string LINE_SEPARATOR_CR = "\r";
        private const string LINE_SEPARATOR_LF = "\n";
        private const string LINE_SEPARATOR_CRLF = "\r\n";

        private const string COMMA_SEPARATOR = ",";
        private const string SEMI_COLON_SEPARATOR = ";";
        private const string REGEX_COMMAS_OUT_OF_LIST = "(?!\\B\"[^\"]*),(?![^\"]*\"\\B)";

        private const string QUOTATING_MARK = "\"";

        private string _valueSeparator = string.Empty;
        private string _lineSeparator = string.Empty;

        public List<PouleDataModel> GetPoulesFromFile(string path) {
            string[] allLines = GetArrayOfEntries(path);

            return null;
        }

        #region Methods to deserialize list of athletes
        public List<AthleteInfoModel> ImportAthletesFromFile(string path) {
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

        private Dictionary<int, AthleteInfoType> GetIndexForEachValue(string csvFirstLine) {
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

        private List<AthleteInfoModel> ToAthleteObjectList(Dictionary<int, AthleteInfoType> infoIndexes, List<string[]> athletesInfo) {
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

        private AthleteInfoModel AddInfoToAthleteModel(AthleteInfoModel toFill, AthleteInfoType infoType, string info) {
            switch (infoType) {
                case AthleteInfoType.Country: toFill.Country = ManageCountry(info); break;
                case AthleteInfoType.Surname: toFill.Surname = info; break;
                case AthleteInfoType.Name: toFill.Name = info; break;
                case AthleteInfoType.Academy: toFill.Academy = info; break;
                case AthleteInfoType.School: toFill.School = info; break;
                case AthleteInfoType.Rank: toFill.Rank = ManageRank(info); break;
                case AthleteInfoType.Styles: toFill.Styles = ManageStyles(info); break;
                case AthleteInfoType.Tier: toFill.Tier = int.Parse(info); break;
                case AthleteInfoType.SaberColor:
                    if (ColorUtility.TryParseHtmlString(info, out Color colorParsed)) {
                        toFill.SaberColor = colorParsed;
                    }
                    break;
                case AthleteInfoType.BirthDate:
                    toFill.BirthDate = DateTime.ParseExact(info, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    break;
                case AthleteInfoType.StartDate:
                    toFill.StartDate = DateTime.ParseExact(info, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    break;
            }

            return toFill;
        }

        private RankType ManageRank(string rankStr) {
            string[] rankNames = Enum.GetNames(typeof(RankType));
            if (rankNames.Contains(rankStr)) {
                return (RankType)Enum.Parse(typeof(RankType), rankStr);
            }

            // TODO Evitar esto. Lanzar un warning (con popup) y devolver El valor de Novizio
            throw new Exception("ERROR: No coincidence for Rank '" + rankStr + "'. Please, review your CSV");
        }

        private List<StyleType> ManageStyles(string stylesValuesString) {
            List<StyleType> res = new List<StyleType>();
            string[] styleNames = Enum.GetNames(typeof(StyleType));

            string[] stylesValues = stylesValuesString.Split(COMMA_SEPARATOR);

            foreach (string style in stylesValues) {
                if (styleNames.Contains(style.Trim())) {
                    res.Add((StyleType)Enum.Parse(typeof(StyleType), style.Trim()));
                } else {
                    // TODO Evitar esto. Lanzar un warning (con popup) y devolver una lista con Forma 1 unicamente
                    throw new Exception("ERROR: No coincidence for Style '" + style + "'. Please, review your CSV");
                }
            }

            return res;
        }

        private string ManageCountry(string countryStr) {
            if (CountriesDataUtils.IsCountryNameInList(countryStr)) {
                return CountriesDataUtils.GetCodeByName(countryStr);
            }

            if (CountriesDataUtils.IsCountryCodeInList(countryStr)) {
                return countryStr;
            }

            throw new Exception("ERROR: No coincidence for Country '" + countryStr + "'. Please, review your CSV");
        }
        #endregion

        private string[] GetArrayOfEntries(string path) {
            // Read CSV text from file
            string csvText = File.ReadAllText(path, System.Text.Encoding.UTF8);
            // Format CSV text to a common standard
            csvText = FormatCSVText(csvText);
            // Separate lines
            string[] allLines = csvText.Split(_lineSeparator);

            // Si el resultado NO es null, tiene AL MENOS 1 entrada y esa primera entrada NO está vacía
            return allLines != null && allLines.Length > 0 && !string.IsNullOrEmpty(allLines[0]) ?
                // Devolvemos el resultado, si no cumple alguna de estas condiciones devolvemos NULL
                allLines : null;
        }

        private string FormatCSVText(string csvText) {
            // Format line separators
            csvText = FormatLineSeparator(csvText);
            // Format values separators
            csvText = FormatValuesSeparator(csvText);
            return csvText;
        }

        private string FormatLineSeparator(string csvText) {
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

        private string FormatValuesSeparator(string csvText) {
            string firstLine = csvText.Split(_lineSeparator)[0];
            _valueSeparator = SEMI_COLON_SEPARATOR;

            if (!string.IsNullOrEmpty(firstLine)) {
                // If there are commas in header out of quotation marks...
                Match searchCommasOutOfQuotation = Regex.Match(firstLine, REGEX_COMMAS_OUT_OF_LIST);
                if (searchCommasOutOfQuotation.Success) {
                    // ... replace all commas out of quotation marks by _valueSeparator
                    csvText = Regex.Replace(csvText, REGEX_COMMAS_OUT_OF_LIST, _valueSeparator);
                }
            }
            return csvText;
        }

        private List<string[]> GetValuesOfAllEntries(string[] allLines) {
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
    }
}
