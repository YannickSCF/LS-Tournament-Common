// Dependencies
using System.Collections.Generic;
// Custom Dependencies
using YannickSCF.LSTournaments.Common.Models;

namespace YannickSCF.LSTournaments.Common.Tools.Poule.Builder {
    public class StylesPoulesBuilder : PoulesBuilder {
        public StylesPoulesBuilder(int pouleMaxSize) : base(pouleMaxSize) { }

        protected override Dictionary<int, List<AthleteInfoModel>> BuildPoulesData(
            Dictionary<int, List<AthleteInfoModel>> poulesData,
            List<AthleteInfoModel> athletes,
            PouleBuilderSubtype subtype) {
            throw new System.NotImplementedException();
        }
    }
}
