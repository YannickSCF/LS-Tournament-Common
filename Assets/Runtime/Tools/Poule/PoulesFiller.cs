// Dependencies
using System.Collections.Generic;
using System.Linq;
// Custom Dependencies
using YannickSCF.LSTournaments.Common.Models;
using YannickSCF.LSTournaments.Common.Tools.Poule.Builder;

namespace YannickSCF.LSTournaments.Common.Tools.Poule {
    public abstract class PoulesFiller {
        // CONSTANTS
        private const int FIRST_LETTER_CHAR = 65;
        // VARIABLES
        protected int _PouleMaxSize;
        // CONSTRUCTORS
        public PoulesFiller(int pouleMaxSize) { _PouleMaxSize = pouleMaxSize; }
        // STATIC
        public static PoulesFiller GetFiller(PouleFillerType builder, int pouleMaxSize) {
            switch (builder) {
                default:
                case PouleFillerType.Random:
                    return new RandomPoulesFiller(pouleMaxSize);
                case PouleFillerType.ByRank:
                    return new RankPoulesFiller(pouleMaxSize);
                case PouleFillerType.ByStyle:
                    return new StylesPoulesFiller(pouleMaxSize);
                case PouleFillerType.ByTier:
                    return new TierPoulesFiller(pouleMaxSize);
            }
        }
        // ABSTRACT METHODS
        protected abstract List<AthleteInfoModel> GetListReadyToFill(List<AthleteInfoModel> athletes);

        #region Public methods
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

        public List<PouleInfoModel> FillPoules(List<string> pouleNames,
            List<AthleteInfoModel> athletes, PouleFillerSubtype subtype) {
            List<PouleInfoModel> result = new List<PouleInfoModel>();

            // Get list of athletes orderer in order to place in poules (without subtype filtering)
            List<AthleteInfoModel> orderedAthletes = GetListReadyToFill(athletes);

            // Create auxiliar dictionary to storage poules data
            Dictionary<int, List<AthleteInfoModel>> poulesData = new Dictionary<int, List<AthleteInfoModel>>();
            for (int i = 0; i < pouleNames.Count; ++i) {
                poulesData.Add(i, new List<AthleteInfoModel>());
            }
            // Build poules adding subtype filtering (if it is selected)
            poulesData = FillPoulesData(poulesData, orderedAthletes, subtype);

            // Transform poules data in objects
            foreach (KeyValuePair<int, List<AthleteInfoModel>> pouleData in poulesData) {
                result.Add(new PouleInfoModel(pouleNames[pouleData.Key], pouleData.Value));
            }

            return result;
        }
        #endregion

        #region Private methods
        private Dictionary<int, List<AthleteInfoModel>> FillPoulesData(
            Dictionary<int, List<AthleteInfoModel>> poulesData,
            List<AthleteInfoModel> athletes,
            PouleFillerSubtype subtype) {
            // Loop all athletes to add them to poules
            for (int i = 0; i < athletes.Count; ++i) {
                AthleteInfoModel athlete = athletes[i];

                // Variable to know if all poules have been tried
                int pouleLoopCounter = 0;
                // Initial poule where begin (usually the smaller in size and index)
                int athletePouleIndex = GetPouleInitialIndex(poulesData);
                while (athletePouleIndex < poulesData.Count && pouleLoopCounter < poulesData.Count) {
                    List<AthleteInfoModel> pouleAthletes = poulesData[athletePouleIndex];
                    // Try to add the athlete to the poule
                    if (TryToAddAthlete(ref pouleAthletes, athlete, subtype)) {
                        poulesData[athletePouleIndex] = pouleAthletes;
                        break;
                    }
                    // If it wasn't possible to add athlete try in the next poule
                    athletePouleIndex++;
                    if (athletePouleIndex >= poulesData.Count) {
                        athletePouleIndex = 0;
                    }
                    // Always taking care to avoid an infinite loop and exit when a round is done
                    ++pouleLoopCounter;
                    if (pouleLoopCounter >= poulesData.Count) {
                        poulesData[GetPouleInitialIndex(poulesData)].Add(athlete);
                    }
                }
            }

            return poulesData;
        }

        private bool TryToAddAthlete(ref List<AthleteInfoModel> pouleAthletes,
            AthleteInfoModel athlete, PouleFillerSubtype subtypeFilter) {
            // First check if the poule is totally filled or not
            if (pouleAthletes.Count >= _PouleMaxSize) return false;
            // Then chekc the subtype filter to avoid repetitions in some characteristics
            switch (subtypeFilter) {
                case PouleFillerSubtype.Country:
                    if (!pouleAthletes.Any(x => x.Country == athlete.Country)) {
                        pouleAthletes.Add(athlete);
                        return true;
                    }
                    break;
                case PouleFillerSubtype.Academy:
                    if (!pouleAthletes.Any(x => x.Academy == athlete.Academy)) {
                        pouleAthletes.Add(athlete);
                        return true;
                    }
                    break;
                case PouleFillerSubtype.School:
                    if (!pouleAthletes.Any(x => x.School == athlete.School)) {
                        pouleAthletes.Add(athlete);
                        return true;
                    }
                    break;
                default:
                    pouleAthletes.Add(athlete);
                    return true;
            }

            return false;
        }

        private int GetPouleInitialIndex(Dictionary<int, List<AthleteInfoModel>> poulesData) {

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
        #endregion
    }
}
