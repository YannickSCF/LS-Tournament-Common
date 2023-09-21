
namespace YannickSCF.LSTournaments.Common {

    // -------------- Athlete Info Enumerators --------------

    public enum AthleteInfoType { Name, Surname, BirthDate, Country, Academy, School, Rank, Styles, SaberColor, StartDate, Tier };
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

    // -------------- Poules Builder Enumerators --------------

    public enum PouleNamingType { Letters, Numbers }
    public enum PouleBuilderType { Random, ByRank, ByStyle, ByTier }
    public enum PouleBuilderSubtype { None, Country, Academy, School }
}
