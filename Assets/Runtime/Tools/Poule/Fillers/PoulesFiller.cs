/**
 * Author:      Yannick Santa Cruz Feuillias
 * Created:     02/10/2023
 **/

// Dependencies
using System.Collections.Generic;
using System.Linq;
// Custom Dependencies
using YannickSCF.LSTournaments.Common.Models.Poules;
using YannickSCF.LSTournaments.Common.Models.Athletes;
using YannickSCF.LSTournaments.Common.Tools.Poule.Filler.Specific;

namespace YannickSCF.LSTournaments.Common.Tools.Poule.Filler {
    public abstract class PoulesFiller {

        // STATIC METHODS
        public static PoulesFiller GetFiller(PouleFillerType builder) {
            switch (builder) {
                default:
                case PouleFillerType.Random:
                    return new RandomPoulesFiller();
                case PouleFillerType.ByRank:
                    return new RankPoulesFiller();
                case PouleFillerType.ByStyle:
                    return new StylesPoulesFiller();
                case PouleFillerType.ByTier:
                    return new TierPoulesFiller();
            }
        }
        
        // ABSTRACT METHODS
        protected abstract List<AthleteInfoModel> GetListReadyToFill(List<AthleteInfoModel> athletes);
        protected abstract Dictionary<int, List<AthleteInfoModel>>  GetFinalListReordered(Dictionary<int, List<AthleteInfoModel>> poules, PouleFillerSubtype subtype);

        // PUBLIC METHODS
        public List<PouleDataModel> FillPoules(List<string> pouleNames,
            List<AthleteInfoModel> athletes, PouleFillerSubtype subtype, int pouleMaxSize) {
            List<PouleDataModel> result = new List<PouleDataModel>();

            // Get list of athletes orderer in order to place in poules (without subtype filtering)
            List<AthleteInfoModel> orderedAthletes = GetListReadyToFill(athletes);

            // Create auxiliar dictionary to storage poules data
            Dictionary<int, List<AthleteInfoModel>> poulesData = new Dictionary<int, List<AthleteInfoModel>>();
            for (int i = 0; i < pouleNames.Count; ++i) {
                poulesData.Add(i, new List<AthleteInfoModel>());
            }
            // Build poules adding subtype filtering (if it is selected)
            poulesData = FillPoulesData(poulesData, orderedAthletes, subtype, pouleMaxSize);

            // Transform poules data in objects
            foreach (KeyValuePair<int, List<AthleteInfoModel>> pouleData in poulesData) {
                List<string> athletesIds = pouleData.Value.Select(x => x.Id).ToList();
                result.Add(new PouleDataModel(pouleNames[pouleData.Key], athletesIds));
            }

            return result;
        }

        #region Private methods
        private Dictionary<int, List<AthleteInfoModel>> FillPoulesData(
            Dictionary<int, List<AthleteInfoModel>> poulesData,
            List<AthleteInfoModel> athletes,
            PouleFillerSubtype subtype, int pouleMaxSize) {
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
                    if (TryToAddAthlete(ref pouleAthletes, athlete, subtype, pouleMaxSize)) {
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
            
            return GetFinalListReordered(poulesData, subtype);
        }

        private bool TryToAddAthlete(ref List<AthleteInfoModel> pouleAthletes,
            AthleteInfoModel athlete, PouleFillerSubtype subtypeFilter, int pouleMaxSize) {
            // First check if the poule is totally filled or not
            if (pouleAthletes.Count >= pouleMaxSize) return false;
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
