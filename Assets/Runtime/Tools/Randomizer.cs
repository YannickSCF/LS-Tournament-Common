/**
 * Author:      Yannick Santa Cruz Feuillias
 * Created:     21/09/2023
 **/

// Dependencies
using System.Collections.Generic;
using UnityEngine;

namespace YannickSCF.LSTournaments.Common {
    public static class Randomizer {
        private static System.Random randomizer;

        public static void SetSeed(int seed) {
            randomizer = new System.Random(seed);
        }

        public static int GetRandom() {
            if (randomizer == null) {
                Debug.LogError("Randomizer is not setted yet!");
                return -1;
            }

            return randomizer.Next();
        }

        public static int GetRandom(int maxValue) {
            if (randomizer == null) {
                Debug.LogError("Randomizer is not setted yet!");
                return -1;
            }

            return randomizer.Next(maxValue);
        }

        public static int GetRandom(int minValue, int maxValue) {
            if (randomizer == null) {
                Debug.LogError("Randomizer is not setted yet!");
                return -1;
            }

            return randomizer.Next(minValue, maxValue);
        }

        public static void ShuffleList<T>(this List<T> listToSort) {
            if (randomizer == null) {
                Debug.LogError("Randomizer is not setted yet!");
                return;
            }

            int n = listToSort.Count;
            while (n > 1) {
                n--;
                int k = randomizer.Next(n + 1);
                T value = listToSort[k];
                listToSort[k] = listToSort[n];
                listToSort[n] = value;
            }
        }
    }
}
