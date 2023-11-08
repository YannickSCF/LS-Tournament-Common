/**
* Author:      Yannick Santa Cruz Feuillias
* Created:     08/11/2023
**/

// Dependencies
using System.Collections.Generic;
using UnityEngine;

namespace YannickSCF.LSTournaments.Common {
    public static class LSTournamentConsts {

        public static readonly List<Color> RANK_COLORS = new List<Color>() { Color.blue, Color.yellow, Color.red, Color.green, Color.magenta };

        public const string COLOR_TAG = "<color={0}>{1}</color>";
        public const string TRANSAPARENT_TAG = "<color=#00000000>{0}</color>";

        public const string DATE_FORMAT = "dd/MM/yyyy";
        public const string DATE_SEPARATOR = "/";

    }
}
