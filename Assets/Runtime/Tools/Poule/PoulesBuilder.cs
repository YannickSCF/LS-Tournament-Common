// Dependencies
using System.Collections.Generic;
using System.Linq;
// Custom Dependencies
using YannickSCF.LSTournaments.Common.Models;
using YannickSCF.LSTournaments.Common.Tools.Poule.Builder;

namespace YannickSCF.LSTournaments.Common.Tools.Poule {
    public abstract class PoulesBuilder {
        private const int FIRST_LETTER_CHAR = 65;

        protected int _PouleMaxSize;

        protected PoulesBuilder(int pouleMaxSize) {
            _PouleMaxSize = pouleMaxSize;
        }

        public static PoulesBuilder GetBuilder(PouleBuilderType builder, int pouleMaxSize) {
            switch (builder) {
                default:
                case PouleBuilderType.Random:
                    return new RandomPoulesBuilder(pouleMaxSize);
                case PouleBuilderType.ByRank:
                    return new RankPoulesBuilder(pouleMaxSize);
                case PouleBuilderType.ByStyle:
                    return new StylesPoulesBuilder(pouleMaxSize);
                case PouleBuilderType.ByTier:
                    return new TierPoulesBuilder(pouleMaxSize);
            }
        }

        public List<string> GetPoulesNames(PouleNamingType pouleNaming, int numPoules, int rounds = 1) {
            List<string> pouleNames = new List<string>();

            int roundSize = numPoules / rounds;
            for (int i = 0; i < numPoules; ++i) {
                string index;
                if (pouleNaming == PouleNamingType.Numbers) {
                    index = (i + 1).ToString();
                } else {
                    if (rounds > 1) {
                        index = ((char)(FIRST_LETTER_CHAR + (i % roundSize))).ToString() + ((i / roundSize) + 1).ToString();
                    } else {
                        index = ((char)(FIRST_LETTER_CHAR + i)).ToString();
                    }
                }
                pouleNames.Add("Poule " + index);
            }

            return pouleNames;
        }

        public List<PouleInfoModel> BuildPoules(List<string> pouleNames,
            List<AthleteInfoModel> athletes, PouleBuilderSubtype subtype) {
            List<PouleInfoModel> result = new List<PouleInfoModel>();

            Dictionary<int, List<AthleteInfoModel>> poulesData = new Dictionary<int, List<AthleteInfoModel>>();
            for (int i = 0; i < pouleNames.Count; ++i) {
                poulesData.Add(i, new List<AthleteInfoModel>());
            }

            poulesData = BuildPoulesData(poulesData, athletes, subtype);

            foreach (KeyValuePair<int, List<AthleteInfoModel>> pouleData in poulesData) {
                result.Add(new PouleInfoModel(pouleNames[pouleData.Key], pouleData.Value));
            }

            return result;
        }

        protected abstract Dictionary<int, List<AthleteInfoModel>> BuildPoulesData(
            Dictionary<int, List<AthleteInfoModel>> poulesData,
            List<AthleteInfoModel> athletes,
            PouleBuilderSubtype subtype);

        protected bool TryToAddAthlete(ref List<AthleteInfoModel> pouleAthletes,
            AthleteInfoModel athlete, PouleBuilderSubtype subtypeFilter) {

            if (pouleAthletes.Count >= _PouleMaxSize) return false;

            switch (subtypeFilter) {
                case PouleBuilderSubtype.Country:
                    if (!pouleAthletes.Any(x => x.Country == athlete.Country)) {
                        pouleAthletes.Add(athlete);
                        return true;
                    }
                    break;
                case PouleBuilderSubtype.Academy:
                    if (!pouleAthletes.Any(x => x.Academy == athlete.Academy)) {
                        pouleAthletes.Add(athlete);
                        return true;
                    }
                    break;
                case PouleBuilderSubtype.School:
                    if (!pouleAthletes.Any(x => x.School == athlete.School)) {
                        pouleAthletes.Add(athlete);
                        return true;
                    }
                    break;
            }

            return false;
        }

        protected int GetPouleInitialIndex(Dictionary<int, List<AthleteInfoModel>> poulesData) {

            int minPouleSize = poulesData[0].Count;
            List<int> smallerPoules = new List<int>() { 0 };
            for (int i = 1; i < poulesData.Count; ++i) {
                if (poulesData[i].Count == minPouleSize) {
                    smallerPoules.Add(i);
                } else if (poulesData[i].Count < minPouleSize) {
                    smallerPoules.Clear();
                    smallerPoules.Add(i);
                    minPouleSize = poulesData[i].Count;
                }
            }

            return smallerPoules.Min();
        }
    }
}
