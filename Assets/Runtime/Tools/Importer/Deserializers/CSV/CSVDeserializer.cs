using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using YannickSCF.CountriesData;
using YannickSCF.LSTournaments.Common.Models;

namespace YannickSCF.LSTournaments.Common.Tools.Importers {
    public class CSVDeserializer : IDeserializer {

        public List<PouleInfoModel> GetPoulesFromFile(string path) {
            throw new NotImplementedException();
        }

        #region Methods to deserialize list of athletes
        public List<AthleteInfoModel> ImportAthletesFromFile(string path) {
            Debug.Log("Athletes by CSV: " + path);
            string jsonText = File.ReadAllText(path, System.Text.Encoding.UTF8);
            jsonText = jsonText.Replace("\r", string.Empty);
            string[] allLines = jsonText.Split('\n');

            // Get the headers used in CSV and the position in the line array
            Dictionary<int, AthleteInfoType> infoIndexes = GetIndexForEachValue(allLines[0]);
            // Get a list of CSV lines separated in arrays
            List<string[]> linesSeparated = GetContentLinesSeparated(allLines);
            // Return the athletes objects list
            return ToAthleteObjectList(infoIndexes, linesSeparated);
        }

        private Dictionary<int, AthleteInfoType> GetIndexForEachValue(string csvFirstLine) {
            Dictionary<int, AthleteInfoType> result = new Dictionary<int, AthleteInfoType>();

            string[] csvFirstLineSeparated = SeparateCSVLine(csvFirstLine);
            if (csvFirstLineSeparated != null) {
                Array infoValues = Enum.GetValues(typeof(AthleteInfoType));
                for (int i = 0; i < infoValues.Length; ++i) {
                    AthleteInfoType currentInfo = (AthleteInfoType)infoValues.GetValue(i);
                    for (int j = 0; j < csvFirstLineSeparated.Length; ++j) {
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

        private List<string[]> GetContentLinesSeparated(string[] allLines) {
            List<string[]> res = new List<string[]>();

            for (int i = 1; i < allLines.Length; ++i) {
                string[] separated = SeparateCSVLine(allLines[i]);
                if (separated != null && !string.IsNullOrEmpty(separated[0])) {
                    res.Add(separated);
                }
            }

            return res;
        }

        private string[] SeparateCSVLine(string csvLine) {
            return csvLine.Split(";");
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

            return res;
        }

        private AthleteInfoModel AddInfoToAthleteModel(AthleteInfoModel toFill, AthleteInfoType infoType, string info) {

            switch (infoType) {
                case AthleteInfoType.Country:
                    toFill.Country = info;
                    break;
                case AthleteInfoType.Surname:
                    toFill.Surname = info;
                    break;
                case AthleteInfoType.Name:
                    toFill.Name = info;
                    break;
                case AthleteInfoType.Academy:
                    toFill.Academy = info;
                    break;
                case AthleteInfoType.School:
                    toFill.School = info;
                    break;
                case AthleteInfoType.Rank:
                    toFill.Rank = ManageRank(info);
                    break;
                case AthleteInfoType.Styles:
                    toFill.Styles = ManageStyles(info);
                    break;
                case AthleteInfoType.Tier:
                    toFill.Tier = int.Parse(info);
                    break;
                case AthleteInfoType.SaberColor:
                    //toFill.SaberColor = info;
                    break;
                case AthleteInfoType.BirthDate:
                    //toFill.BirthDate = info;
                    break;
                case AthleteInfoType.StartDate:
                    //toFill.StartDate = info;
                    break;
            }

            return toFill;
        }

        private RankType ManageRank(string rankStr) {
            rankStr = rankStr.Replace("\r", "");

            string[] rankNames = Enum.GetNames(typeof(RankType));
            if (rankNames.Contains(rankStr)) {
                return (RankType)Enum.Parse(typeof(RankType), rankStr);
            }

            throw new Exception("ERROR: No coincidence for Rank '" + rankStr + "'. Please, review your CSV");
        }

        private List<StyleType> ManageStyles(string stylesStrArr) {
            List<StyleType> res = new List<StyleType>();
            string[] styleNames = Enum.GetNames(typeof(StyleType));

            stylesStrArr = stylesStrArr.Replace(" ", "");
            stylesStrArr = stylesStrArr.Replace("\r", "");

            string[] stylesStr = stylesStrArr.Split(',');

            foreach (string style in stylesStr) {
                if (styleNames.Contains(style)) {
                    res.Add((StyleType)Enum.Parse(typeof(StyleType), style));
                } else {
                    throw new Exception("ERROR: No coincidence for Style '" + style + "'. Please, review your CSV");
                }
            }

            return res;
        }

        private string ManageCountry(string countryStr) {
            countryStr = countryStr.Replace("\r", "");

            if (CountriesDataUtils.IsCountryNameInList(countryStr)) {
                return CountriesDataUtils.GetCodeByName(countryStr);
            }

            if (CountriesDataUtils.IsCountryCodeInList(countryStr)) {
                return countryStr;
            }

            throw new Exception("ERROR: No coincidence for Country '" + countryStr + "'. Please, review your CSV");
        }
        #endregion

    }
}
