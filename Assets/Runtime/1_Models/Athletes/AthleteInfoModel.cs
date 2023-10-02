using System;
using System.Collections.Generic;
using UnityEngine;

namespace YannickSCF.LSTournaments.Common.Models.Athletes {
    [Serializable]
    public class AthleteInfoModel {

        [SerializeField] private string _id;
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
        public string Id { get => _id; }
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
            _id = Guid.NewGuid().ToString();
            _styles = new List<StyleType>();
        }

        public AthleteInfoModel(
            string name, string surname, DateTime birthDate,
            string country, string academy, string school,
            RankType rank, List<StyleType> styles, Color saberColor,
            DateTime startDate, int tier) {
            _id = Guid.NewGuid().ToString();
            _name = name;
            _surname = surname;
            _birthDate = birthDate;
            _country = country;
            _academy = academy;
            _school = school;
            _rank = rank;
            _styles = styles;
            _saberColor = saberColor;
            _startDate = startDate;
            _tier = tier;
        }

        public AthleteInfoModel(string id,
            string name, string surname, DateTime birthDate,
            string country, string academy, string school,
            RankType rank, List<StyleType> styles, Color saberColor,
            DateTime startDate, int tier) {
            _id = id;
            _name = name;
            _surname = surname;
            _birthDate = birthDate;
            _country = country;
            _academy = academy;
            _school = school;
            _rank = rank;
            _styles = styles;
            _saberColor = saberColor;
            _startDate = startDate;
            _tier = tier;
        }
        #endregion

        #region Public methods
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
