// Dependencies
using System.Collections.Generic;
// Custom Dependencies
using YannickSCF.LSTournaments.Common.Models.Athletes;

namespace YannickSCF.LSTournaments.Common.Tools.Poule.Filler.Specific {
    public class RandomPoulesFiller : PoulesFiller {
        protected override List<AthleteInfoModel> GetListReadyToFill(List<AthleteInfoModel> athletes) {
            Randomizer.ShuffleList(athletes);
            return athletes;
        }
    }
}
