// Dependencies
using System.Collections.Generic;
// Custom Dependencies
using YannickSCF.LSTournaments.Common.Models;

namespace YannickSCF.LSTournaments.Common.Tools.Poule.Filler.Specific {
    public class RandomPoulesFiller : PoulesFiller {
        public RandomPoulesFiller(int pouleMaxSize) : base(pouleMaxSize) { }

        protected override List<AthleteInfoModel> GetListReadyToFill(List<AthleteInfoModel> athletes) {
            Randomizer.ShuffleList(athletes);
            return athletes;
        }
    }
}
