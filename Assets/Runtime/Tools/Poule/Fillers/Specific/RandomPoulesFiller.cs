/**
 * Author:      Yannick Santa Cruz Feuillias
 * Created:     21/09/2023
 **/

// Dependencies
using System.Collections.Generic;
using System.Linq;
// Custom Dependencies
using YannickSCF.LSTournaments.Common.Models.Athletes;

namespace YannickSCF.LSTournaments.Common.Tools.Poule.Filler.Specific {
    public class RandomPoulesFiller : PoulesFiller {
        protected override Dictionary<int, List<AthleteInfoModel>> GetFinalListReordered(Dictionary<int, List<AthleteInfoModel>> poules, PouleFillerSubtype subtype) {
            for (int i = 0; i < poules.Count; ++i) {
                switch (subtype) {
                    case PouleFillerSubtype.Country: poules[i] = poules[i].OrderBy(x => x.Country).ToList(); break;
                    case PouleFillerSubtype.Academy: poules[i] = poules[i].OrderBy(x => x.Academy).ToList(); break;
                    case PouleFillerSubtype.School: poules[i] = poules[i].OrderBy(x => x.School).ToList(); break;
                    case PouleFillerSubtype.None:
                    default: break;
                }
            }

            return poules;
        }

        protected override List<AthleteInfoModel> GetListReadyToFill(List<AthleteInfoModel> athletes) {
            Randomizer.ShuffleList(athletes);
            return athletes;
        }
    }
}
