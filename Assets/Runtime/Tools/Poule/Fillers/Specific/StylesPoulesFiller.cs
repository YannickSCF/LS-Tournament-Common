// Dependencies
using System.Collections.Generic;
using System.Linq;
// Custom Dependencies
using YannickSCF.LSTournaments.Common.Models;

namespace YannickSCF.LSTournaments.Common.Tools.Poule.Filler.Specific {
    public class StylesPoulesFiller : PoulesFiller {
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
