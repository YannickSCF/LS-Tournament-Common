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

        public static readonly Color MAIN_HELP = new Color(0.286f, 0.827f, 0.161f, 0.196f);
        public static readonly Color SECONDARY_HELP = new Color(0.827f, 0.165f, 0.223f, 0.196f);

        public const string COLOR_TAG = "<color={0}>{1}</color>";
        public const string TRANSAPARENT_TAG = "<color=#00000000>{0}</color>";
        public const string SIZE_TAG_INIT = "<size={0}>";
        public const string SPRITE_TAG = "<sprite name={0}>";

        public const string DATE_FORMAT = "dd/MM/yyyy";
        public const string DATE_SEPARATOR = "/";
        public const string TIMER_FORMAT = "{0}:{1}.{3}";

    }
}
