// Dependencies
using System.Collections.Generic;
using System.Linq;
// Custom Dependencies
using YannickSCF.LSTournaments.Common.Models;

namespace YannickSCF.LSTournaments.Common.Tools.Poule.Builder {
    public class TierPoulesBuilder : PoulesBuilder {
        public TierPoulesBuilder(int pouleMaxSize) : base(pouleMaxSize) { }

        protected override List<AthleteInfoModel> GetSpecificAthletesList(List<AthleteInfoModel> athletes) {
            List<AthleteInfoModel> result = new List<AthleteInfoModel>();

            IEnumerable<IGrouping<int, AthleteInfoModel>> tierGroups = athletes.GroupBy(x => x.Tier);

            IOrderedEnumerable<IGrouping<int, AthleteInfoModel>> ordered = tierGroups.OrderBy(x => x.Key);
            foreach (IGrouping<int, AthleteInfoModel> group in ordered) {
                List<AthleteInfoModel> groupAthletes = group.ToList();
                Randomizer.ShuffleList(groupAthletes);
                result.AddRange(groupAthletes);
            }

            return result;
        }
    }
}
