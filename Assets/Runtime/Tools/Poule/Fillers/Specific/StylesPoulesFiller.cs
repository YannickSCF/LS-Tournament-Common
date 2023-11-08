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
    public class StylesPoulesFiller : PoulesFiller {
        protected override Dictionary<int, List<AthleteInfoModel>> GetFinalListReordered(Dictionary<int, List<AthleteInfoModel>> poules, PouleFillerSubtype subtype) {
            for (int i = 0; i < poules.Count; ++i) {
                switch (subtype) {
                    case PouleFillerSubtype.Country: poules[i] = poules[i].OrderBy(x => x.Styles.Count).ThenByDescending(x => x.Country).ToList(); break;
                    case PouleFillerSubtype.Academy: poules[i] = poules[i].OrderBy(x => x.Styles.Count).ThenByDescending(x => x.Academy).ToList(); break;
                    case PouleFillerSubtype.School: poules[i] = poules[i].OrderBy(x => x.Styles.Count).ThenByDescending(x => x.School).ToList(); break;
                    case PouleFillerSubtype.None:
                    default: break;
                }
            }

            return poules;
        }

        protected override List<AthleteInfoModel> GetListReadyToFill(List<AthleteInfoModel> athletes) {
            List<AthleteInfoModel> result = new List<AthleteInfoModel>();

            IEnumerable<IGrouping<int, AthleteInfoModel>> stylesGroups = athletes.GroupBy(x => x.Styles.Count);

            IOrderedEnumerable<IGrouping<int, AthleteInfoModel>> ordered = stylesGroups.OrderBy(x => x.Key);
            foreach (IGrouping<int, AthleteInfoModel> group in ordered.Reverse().ToList()) {
                List<AthleteInfoModel> groupAthletes = group.ToList();
                Randomizer.ShuffleList(groupAthletes);
                result.AddRange(groupAthletes);
            }

            return result;
        }
    }
}
