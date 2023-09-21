// Dependencies
using System;
using System.Collections.Generic;
using System.Linq;
// Custom Dependencies
using YannickSCF.LSTournaments.Common.Models;

namespace YannickSCF.LSTournaments.Common.Tools.Poule.Builder {
    public class RandomPoulesBuilder : PoulesBuilder {

        public RandomPoulesBuilder(int pouleMaxSize) : base(pouleMaxSize) { }

        protected override Dictionary<int, List<AthleteInfoModel>> BuildPoulesData(
            Dictionary<int, List<AthleteInfoModel>> poulesData,
            List<AthleteInfoModel> athletes,
            PouleBuilderSubtype subtype) {

            for (int i = 0; i < athletes.Count; ++i) {
                AthleteInfoModel athlete = athletes[i];

                int auxCounter = 0;
                int athletePouleIndex = GetPouleInitialIndex(poulesData);
                while (athletePouleIndex < poulesData.Count && auxCounter < poulesData.Count) {
                    List<AthleteInfoModel> pouleAthletes = poulesData[athletePouleIndex];
                    if (TryToAddAthlete(ref pouleAthletes, athlete, subtype)) {
                        poulesData[athletePouleIndex] = pouleAthletes;
                        break;
                    }

                    athletePouleIndex++;
                    if (athletePouleIndex >= poulesData.Count) {
                        athletePouleIndex = 0;
                    }

                    ++auxCounter;
                    if (auxCounter >= poulesData.Count) {
                        poulesData[GetPouleInitialIndex(poulesData)].Add(athlete);
                    }
                }
            }

            return poulesData;
        }
    }
}
