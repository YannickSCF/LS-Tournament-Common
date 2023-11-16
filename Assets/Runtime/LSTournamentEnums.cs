/**
 * Author:      Yannick Santa Cruz Feuillias
 * Created:     21/09/2023
 **/

// Dependencies
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization.Settings;

namespace YannickSCF.LSTournaments.Common {

    public enum TournamentType { Unrated, School, Academy, National, International }

    // -------------- Athlete Info Enumerators --------------

    public enum AthleteInfoType { Country, Surname, Name, Academy, School, Rank, Styles, Tier, SaberColor, BirthDate, StartDate };
    public enum AthleteInfoStatus { Active, Hide, Disable};
    public enum RankType { Novizio, Iniziato, Accademico, Cavaliere, MaestroDiScuola }
    public enum SubRankType { Long, Dual, Staff }
    public enum StyleType {
        Form1, Form2, CourseY,
        Form3Long, Form4Long, Form5Long,
        Form3Dual, Form4Dual, Form5Dual,
        Form3Staff, Form4Staff, Form5Staff,
        Form6, Form7
    }
    public enum FullNameType { SurnameName, NameSurname }
    public enum TournamentPhase { Poules, EliminationPhase }

    // -------------- Poules Builder Enumerators --------------

    public enum PoulesBy { NumberOfPoules, MaxPoulesSize }
    public enum PouleNamingType { Letters, Numbers }
    public enum PouleFillerType { TBD, Random, ByRank, ByStyle, ByTier }
    public enum PouleFillerSubtype { None, Country, Academy, School }

    // -------------- Scoring Sorters Enumerators --------------

    public enum ScoringType { TBD, LS, Fencing }

    // -------------- Elimination Phase Enumerators --------------

    public enum ClassificationType { TBD, GeneralRanking, BetweenPoules }
    public enum EliminationRound { T128 = 256, T64 = 128, T32 = 64, T16 = 32, T8 = 16, T4 = 8, T2 = 4 }

    // -------------- Rankings Enumerators --------------

    public enum StyleRankingType { TBD, Style }
    public enum WarRankingType { TBD, LS, Fencing, Style }
    public enum MixedRankingType { TBD, PrefStyle, PrefWar }

    public static class LSTournamentEnums {

        public static string GetEnumsLocalization<T>(T enumValue) {
            if (typeof(T).BaseType != typeof(Enum)) {
                Debug.LogWarning("The value introduced must be an enum");
                return null;
            }

            return LocalizationSettings.StringDatabase.GetLocalizedString("Common Enums", typeof(T).Name + "." + enumValue.ToString());
        }

        public static List<string> GetEnumsLocalizations<T>(List<T> enumValues = null) {
            List<string> typeOptions = new List<string>();

            if (typeof(T).BaseType != typeof(Enum)) {
                Debug.LogWarning("The value introduced must be an enum");
                return null;
            }

            if (enumValues == null) {
                enumValues = new List<T>();
                Array types = Enum.GetValues(typeof(T));
                foreach (Enum type in types) {
                    enumValues.Add((T)(object)type);
                }
            }

            foreach (T enumValue in enumValues) {
                string localized = LocalizationSettings.StringDatabase.GetLocalizedString("Common Enums", typeof(T).Name + "." + enumValue.ToString());
                typeOptions.Add(localized);
            }

            return typeOptions;
        }

        public static Dictionary<AthleteInfoType, AthleteInfoStatus> GetAthleteInfoBase(Dictionary<AthleteInfoType, AthleteInfoStatus> current, TournamentType tournamentType) {
            if (tournamentType == TournamentType.School ||
                tournamentType == TournamentType.Academy ||
                tournamentType == TournamentType.National) {
                current[AthleteInfoType.Country] = AthleteInfoStatus.Hide;
            } else if (current[AthleteInfoType.Country] != AthleteInfoStatus.Disable) {
                current[AthleteInfoType.Country] = AthleteInfoStatus.Active;
            }

            if (tournamentType == TournamentType.School ||
                tournamentType == TournamentType.Academy) {
                current[AthleteInfoType.Academy] = AthleteInfoStatus.Hide;
            } else if (current[AthleteInfoType.Academy] != AthleteInfoStatus.Disable) {
                current[AthleteInfoType.Academy] = AthleteInfoStatus.Active;
            }

            if (tournamentType == TournamentType.School) {
                current[AthleteInfoType.School] = AthleteInfoStatus.Hide;
            } else if (current[AthleteInfoType.School] != AthleteInfoStatus.Disable) {
                current[AthleteInfoType.School] = AthleteInfoStatus.Active;
            }

            return current;
        }
    }
}
