using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using YannickSCF.LSTournaments.Common.Models;
using YannickSCF.LSTournaments.Common.Scriptables.Formulas;
using YannickSCF.LSTournaments.Common.Tools.Poule.Filler;
using YannickSCF.LSTournaments.Common.Tools.Poule.Filler.Specific;

namespace YannickSCF.LSTournaments.Common.Tools.Poule {
    public struct PouleNamingObject {
        private PouleNamingType _pouleNaming;
        private int _numOfPoules;
        private int _pouleRounds;

        public PouleNamingObject(PouleNamingType pouleNaming, int numOfPoules) {
            _pouleNaming = pouleNaming;
            _numOfPoules = numOfPoules;
            _pouleRounds = 1;
        }

        public PouleNamingObject(PouleNamingType pouleNaming, int numOfPoules, int pouleRounds) {
            _pouleNaming = pouleNaming;
            _numOfPoules = numOfPoules;
            _pouleRounds = pouleRounds;
        }

        public PouleNamingType PouleNaming { get => _pouleNaming; }
        public int NumOfPoules { get => _numOfPoules; }
        public int PouleRounds { get => _pouleRounds; }
    }

    public static class PouleUtils {
        // ENUMS
        private enum PouleSize { Three = 3, Four = 4, Five = 5, Six = 6, Seven = 7, Eight = 8, Nine = 9, Ten = 10, Eleven = 11 };
        // CONSTANTS
        private const int FIRST_LETTER_CHAR = 65;
        private const int DEFAULT_MIN_POULE_SIZE = 3;
        private const int DEFAULT_MAX_POULE_SIZE = 11;

        #region Create Poules - METHODS
        public static List<PouleInfoModel> CreatePoules(
            PouleNamingObject namingParams, List<AthleteInfoModel> athletes,
            PouleFillerType fillerType, PouleFillerSubtype fillerSubtype, int maxPouleSize) {

            // Get poules names
            List<string> names = GetPoulesNames(namingParams);
            // Get poules filler
            PoulesFiller _filler = GetFiller(fillerType);
            // Return poules filled
            return _filler.FillPoules(names, athletes, fillerSubtype, maxPouleSize);
        }

        public static List<PouleInfoModel> CreatePoules(
            PouleNamingObject namingParams, List<AthleteInfoModel> athletes,
            TournamentFormula formula, PouleFillerSubtype fillerSubtype) {

            // Get poules names
            List<string> names = GetPoulesNames(namingParams);
            // Get poules filler
            PoulesFiller _filler = GetFiller(formula.FillerType);
            // Return poules filled
            return _filler.FillPoules(names, athletes, fillerSubtype, formula.MaxPouleSize);
        }

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
        #endregion

        #region Poules names - METHODS
        public static List<string> GetPoulesNames(PouleNamingObject namingParams) {
            List<string> pouleNames = new List<string>();

            int roundSize = namingParams.NumOfPoules / namingParams.PouleRounds;
            for (int i = 0; i < namingParams.NumOfPoules; ++i) {
                string index;
                if (namingParams.PouleNaming == PouleNamingType.Numbers) {
                    index = (i + 1).ToString();
                } else {
                    if (namingParams.PouleRounds > 1) {
                        index = ((char)(FIRST_LETTER_CHAR + (i % roundSize))).ToString() + ((i / roundSize) + 1).ToString();
                    } else {
                        index = ((char)(FIRST_LETTER_CHAR + i)).ToString();
                    }
                }
                pouleNames.Add("Poule " + index);
            }

            return pouleNames;
        }

        public static List<string> GetPoulesNames(PouleNamingType pouleNaming, int numPoules, int rounds = 1) {
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
        #endregion

        #region Poules count and size - METHODS
        public static int[,] GetPoulesAndSize(int numberOfParticipants, TournamentFormula formula) {
            int[,] poulesRes;

            if (!formula.InfinitePoules) {
                poulesRes = GetPoulesAndSize(numberOfParticipants, formula.PossibleNumberOfPoules.ToArray());
                if (poulesRes != null) return poulesRes;
            }

            Vector2 minMaxPouleSize = new Vector2(formula.MinPouleSize, formula.MaxPouleSize);
            poulesRes = GetPoulesAndSize(numberOfParticipants, minMaxPouleSize);
            return poulesRes;
        }

        public static int[,] GetPoulesAndSize(int numberOfParticipants, int[] posibleNumberOfPoules) {
            int[,] poulesRes;

            if (posibleNumberOfPoules != null && posibleNumberOfPoules[0] != 0) {
                int[] numberOfPoules = new int[1] { posibleNumberOfPoules[0] };
                for (int i = DEFAULT_MIN_POULE_SIZE; i < DEFAULT_MAX_POULE_SIZE; ++i) {
                    poulesRes = CalculateNumberOfPoulesAndSizes(numberOfParticipants, numberOfPoules, new Vector2(i, i + 1));
                    if (poulesRes != null) {
                        return poulesRes;
                    }
                }
            }

            return null;
        }

        public static int[,] GetPoulesAndSize(int numberOfParticipants, Vector2 minMaxPouleSize) {
            int[,] poulesRes;
            int[] posibleNumberOfPoules;

            if (minMaxPouleSize != null && minMaxPouleSize.x >= DEFAULT_MIN_POULE_SIZE && minMaxPouleSize.y <= DEFAULT_MAX_POULE_SIZE) {
                posibleNumberOfPoules = Enumerable.Range(1, numberOfParticipants / (int)minMaxPouleSize.x).ToArray();
                poulesRes = CalculateNumberOfPoulesAndSizes(numberOfParticipants, posibleNumberOfPoules, minMaxPouleSize);
                return poulesRes;
            }

            return null;
        }

        #region Poules count and size - PRIVATE METHODS
        private static int[,] CalculateNumberOfPoulesAndSizes(int numberOfParticipants, int[] posibleNumberOfPoules, Vector2 minMaxPouleSize) {
            int[,] res = null;

            int[] simulatedPoules;
            for (int i = 0; i < posibleNumberOfPoules.Length; ++i) {
                simulatedPoules = new int[posibleNumberOfPoules[i]];
                int numberOfPools = posibleNumberOfPoules[i];
                for (int j = 0; j < numberOfPools; ++j) {
                    simulatedPoules[j] = (int)minMaxPouleSize.x;
                }

                res = GetPoulesOnArrayWithSizes(simulatedPoules, numberOfParticipants, (int)minMaxPouleSize.y, numberOfPools);
                if (res != null) break;
            }

            return res;
        }

        private static int[,] GetPoulesOnArrayWithSizes(int[] simulatedPoules, int numberOfParticipants, int maxPouleSize, int numberOfPools) {
            int[,] res = null;
            bool exitCondition = false;
            int participantsOnPoules = 0;
            do {
                participantsOnPoules = CalculateWithPoules(simulatedPoules);
                exitCondition = participantsOnPoules >= numberOfParticipants;
                if (!exitCondition) {
                    simulatedPoules = AddParticipantToSimulatedPoules(simulatedPoules);
                }
            } while (!exitCondition && participantsOnPoules < maxPouleSize * numberOfPools);

            if (participantsOnPoules == numberOfParticipants) {
                res = new int[2, 2];
                if (simulatedPoules.Max() == simulatedPoules.Min()) {
                    res[0, 0] = simulatedPoules.Count(x => x == simulatedPoules.Max());
                    res[0, 1] = simulatedPoules.Max();
                    res[1, 1] = simulatedPoules.Max() - 1;
                } else {
                    res[0, 0] = simulatedPoules.Count(x => x == simulatedPoules.Max());
                    res[0, 1] = simulatedPoules.Max();
                    res[1, 0] = simulatedPoules.Count(x => x == simulatedPoules.Min());
                    res[1, 1] = simulatedPoules.Min();
                }
                return res;
            }

            return res;
        }

        private static int CalculateWithPoules(int[] simulatedPoules) {
            int res = 0;
            for (int i = 0; i < simulatedPoules.Length; ++i) {
                res += simulatedPoules[i];
            }
            return res;
        }

        private static int[] AddParticipantToSimulatedPoules(int[] simulatedPoules) {
            if (simulatedPoules.Distinct().Count() == 1) {
                simulatedPoules[0]++;
            } else {
                int index = simulatedPoules.ToList().IndexOf(simulatedPoules.Min());
                simulatedPoules[index]++;
            }

            return simulatedPoules;
        }
        #endregion
        #endregion

        #region Poules matches - METHODS
        public static int[,] GetMatchesOrder(int count) {
            switch ((PouleSize)count) {
                case PouleSize.Three:
                    return POULE_OF_THREE;
                case PouleSize.Four:
                    return POULE_OF_FOUR;
                case PouleSize.Five:
                    return POULE_OF_FIVE;
                case PouleSize.Six:
                    return POULE_OF_SIX;
                case PouleSize.Seven:
                    return POULE_OF_SEVEN;
                case PouleSize.Eight:
                    return POULE_OF_EIGHT;
                case PouleSize.Nine:
                    return POULE_OF_NINE;
                case PouleSize.Ten:
                    return POULE_OF_TEN;
                case PouleSize.Eleven:
                    return POULE_OF_ELEVEN;
                default:
                    return null;
            }
        }

        public static int GetNumberOfMatches(int numberOfParticipants) {
            switch ((PouleSize)numberOfParticipants) {
                case PouleSize.Three:
                    return POULE_OF_THREE.GetLength(0);
                case PouleSize.Four:
                    return POULE_OF_FOUR.GetLength(0);
                case PouleSize.Five:
                    return POULE_OF_FIVE.GetLength(0);
                case PouleSize.Six:
                    return POULE_OF_SIX.GetLength(0);
                case PouleSize.Seven:
                    return POULE_OF_SEVEN.GetLength(0);
                case PouleSize.Eight:
                    return POULE_OF_EIGHT.GetLength(0);
                case PouleSize.Nine:
                    return POULE_OF_NINE.GetLength(0);
                case PouleSize.Ten:
                    return POULE_OF_TEN.GetLength(0);
                case PouleSize.Eleven:
                    return POULE_OF_ELEVEN.GetLength(0);
                default:
                    return 0;
            }
        }

        #region Poules matches - MATCHES ORDERS
        public static readonly int[,] POULE_OF_THREE = new int[3, 2] {
            { 1, 3 },
            { 2, 3 },
            { 2, 1 }
        };
        public static readonly int[,] POULE_OF_FOUR = new int[6, 2] {
            { 1, 4 },
            { 2, 3 },
            { 1, 3 },
            { 2, 4 },
            { 3, 4 },
            { 1, 2 }
        };
        public static readonly int[,] POULE_OF_FIVE = new int[10, 2] {
            { 1, 2 },
            { 3, 4 },
            { 5, 1 },
            { 2, 3 },
            { 5, 4 },
            { 1, 3 },
            { 2, 5 },
            { 4, 1 },
            { 3, 5 },
            { 4, 2 }
        };
        public static readonly int[,] POULE_OF_SIX = new int[15, 2] {
            { 1, 2 },
            { 4, 5 },
            { 2, 3 },
            { 5, 6 },
            { 3, 1 },
            { 6, 4 },
            { 2, 5 },
            { 1, 4 },
            { 5, 3 },
            { 1, 6 },
            { 4, 2 },
            { 3, 6 },
            { 5, 1 },
            { 3, 4 },
            { 6, 2 }
        };
        public static readonly int[,] POULE_OF_SEVEN = new int[21, 2] {
            { 1, 4 },
            { 2, 5 },
            { 3, 6 },
            { 7, 1 },
            { 5, 4 },
            { 2, 3 },
            { 6, 7 },
            { 5, 1 },
            { 4, 3 },
            { 6, 2 },
            { 5, 7 },
            { 3, 1 },
            { 4, 6 },
            { 7, 2 },
            { 3, 5 },
            { 1, 6 },
            { 2, 4 },
            { 7, 3 },
            { 6, 5 },
            { 1, 2 },
            { 4, 7 }
        };
        public static readonly int[,] POULE_OF_EIGHT = new int[28, 2] {
            { 2, 3 },
            { 1, 5 },
            { 7, 4 },
            { 6, 8 },
            { 1, 2 },
            { 3, 4 },
            { 5, 6 },
            { 8, 7 },
            { 4, 1 },
            { 5, 2 },
            { 8, 3 },
            { 6, 7 },
            { 4, 2 },
            { 8, 1 },
            { 7, 5 },
            { 3, 6 },
            { 2, 8 },
            { 5, 4 },
            { 6, 1 },
            { 3, 7 },
            { 4, 8 },
            { 2, 6 },
            { 3, 5 },
            { 1, 7 },
            { 4, 6 },
            { 8, 5 },
            { 7, 2 },
            { 1, 3 }
        };
        public static readonly int[,] POULE_OF_NINE = new int[36, 2] {
            { 1, 9 },
            { 2, 8 },
            { 3, 7 },
            { 4, 6 },
            { 1, 5 },
            { 2, 9 },
            { 8, 3 },
            { 7, 4 },
            { 6, 5 },
            { 1, 2 },
            { 9, 3 },
            { 8, 4 },
            { 7, 5 },
            { 6, 1 },
            { 3, 2 },
            { 9, 4 },
            { 5, 8 },
            { 7, 6 },
            { 3, 1 },
            { 2, 4 },
            { 5, 9 },
            { 8, 6 },
            { 7, 1 },
            { 4, 3 },
            { 5, 2 },
            { 6, 9 },
            { 8, 7 },
            { 4, 1 },
            { 5, 3 },
            { 6, 2 },
            { 9, 7 },
            { 1, 8 },
            { 4, 5 },
            { 3, 6 },
            { 2, 7 },
            { 9, 8 }
        };
        public static readonly int[,] POULE_OF_TEN = new int[45, 2] {
            { 1, 4 },
            { 6, 9 },
            { 2, 5 },
            { 7, 10 },
            { 3, 1 },
            { 8, 6 },
            { 4, 5 },
            { 9, 10 },
            { 2, 3 },
            { 7, 8 },
            { 5, 1 },
            { 10, 6 },
            { 4, 2 },
            { 9, 7 },
            { 5, 3 },
            { 10, 8 },
            { 1, 2 },
            { 6, 7 },
            { 3, 4 },
            { 8, 9 },
            { 5, 10 },
            { 1, 6 },
            { 2, 7 },
            { 3, 8 },
            { 4, 9 },
            { 6, 5 },
            { 10, 2 },
            { 8, 1 },
            { 7, 4 },
            { 9, 3 },
            { 2, 6 },
            { 5, 8 },
            { 4, 10 },
            { 1, 9 },
            { 3, 7 },
            { 8, 2 },
            { 6, 4 },
            { 9, 5 },
            { 10, 3 },
            { 7, 1 },
            { 4, 8 },
            { 2, 9 },
            { 3, 6 },
            { 5, 7 },
            { 1, 10 }
        };
        public static readonly int[,] POULE_OF_ELEVEN = new int[55, 2] {
            { 1, 2 },
            { 7, 8 },
            { 4, 5 },
            { 10, 11 },
            { 2, 3 },
            { 8, 9 },
            { 5, 6 },
            { 3, 1 },
            { 9, 7 },
            { 6, 4 },
            { 2, 5 },
            { 8, 11 },
            { 1, 4 },
            { 7, 10 },
            { 5, 3 },
            { 11, 9 },
            { 1, 6 },
            { 4, 2 },
            { 10, 8 },
            { 3, 6 },
            { 5, 1 },
            { 11, 7 },
            { 3, 4 },
            { 9, 10 },
            { 6, 2 },
            { 1, 7 },
            { 3, 9 },
            { 10, 4 },
            { 8, 2 },
            { 5, 11 },
            { 1, 8 },
            { 9, 2 },
            { 3, 10 },
            { 4, 11 },
            { 6, 7 },
            { 9, 1 },
            { 2, 10 },
            { 11, 3 },
            { 7, 5 },
            { 6, 8 },
            { 10, 1 },
            { 11, 2 },
            { 4, 7 },
            { 8, 5 },
            { 6, 9 },
            { 11, 1 },
            { 7, 3 },
            { 4, 8 },
            { 9, 5 },
            { 6, 10 },
            { 2, 7 },
            { 8, 3 },
            { 4, 9 },
            { 10, 5 },
            { 6, 11 }
        };
        #endregion
        #endregion
    }
}
