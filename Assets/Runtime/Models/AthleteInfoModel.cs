using System;
using System.Collections.Generic;
using UnityEngine;

namespace YannickSCF.LSTournaments.Common.Models {

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

    [Serializable]
    public class AthleteInfoModel {
        // Personal Information
        [SerializeField] private string _name;
        [SerializeField] private string _surname;
        [SerializeField] private DateTime _birthDate;
        // Representation Information
        [SerializeField] private string _country;
        [SerializeField] private string _academy;
        [SerializeField] private string _school;
        // Ludosport Information
        [SerializeField] private RankType _rank;
        [SerializeField] private List<StyleType> _styles;
        [SerializeField] private Color _saberColor;
        [SerializeField] private DateTime _startDate;
        // OTHER
        [SerializeField] private int _tier;
        
        #region Properties
        // Personal Information
        public string Name { get => _name; set => _name = value; }
        public string Surname { get => _surname; set => _surname = value; }
        public DateTime BirthDate { get => _birthDate; set => _birthDate = value.Date; }
        // Representation Information
        public string Country { get => _country; set => _country = value; }
        public string Academy { get => _academy; set => _academy = value; }
        public string School { get => _school; set => _school = value; }
        // Ludosport Information
        public RankType Rank { get => _rank; set => _rank = value; }
        public List<StyleType> Styles { get => _styles; set => _styles = value; }
        public Color SaberColor { get => _saberColor; set => _saberColor = value; }
        public DateTime StartDate { get => _startDate; set => _startDate = value.Date; }
        // OTHER
        public int Tier { get => _tier; set => _tier = value; }
        #endregion

        #region Constructors
        public AthleteInfoModel() {
            _styles = new List<StyleType>();
        }
        #endregion

        #region Public methods
        public string GetAthleteId() {
            return _country + "-" + _school + "-" + GetAge() + "-" + _surname + "_" + _name + "-" + _startDate.ToString("ddMMyyyy");
        }

        public string GetFullName(FullNameType howToGiveName = FullNameType.SurnameName) {
            switch(howToGiveName) {
                default:
                case FullNameType.SurnameName:
                    return _surname + ", " + _name;
                case FullNameType.NameSurname:
                    return _name + " " + _surname;
            }
        }

        public int GetAge() {
            return DateTime.Now.Year - _birthDate.Year;
        }

        public List<SubRankType> GetSubRankTypes() {
            if ((int)_rank < (int)RankType.Cavaliere) return null;

            List<SubRankType> subranks = new List<SubRankType>();
            if (_styles.Contains(StyleType.Form3Long) &&
                _styles.Contains(StyleType.Form4Long) && 
                _styles.Contains(StyleType.Form5Long)) {
                subranks.Add(SubRankType.Long);
            }

            if (_styles.Contains(StyleType.Form3Dual) &&
                _styles.Contains(StyleType.Form4Dual) && 
                _styles.Contains(StyleType.Form5Dual)) {
                subranks.Add(SubRankType.Dual);
            }
            
            if (_styles.Contains(StyleType.Form3Staff) &&
                _styles.Contains(StyleType.Form4Staff) && 
                _styles.Contains(StyleType.Form5Staff)) {
                subranks.Add(SubRankType.Staff);
            }

            if(subranks.Count == 0) return null;
            
            return subranks;
        }
        #endregion
    }
}
