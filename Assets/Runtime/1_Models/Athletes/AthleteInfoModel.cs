/**
 * Author:      Yannick Santa Cruz Feuillias
 * Created:     21/09/2023
 **/

// Dependencies
using System;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

namespace YannickSCF.LSTournaments.Common.Models.Athletes {
    [Serializable]
    public class AthleteInfoModel {

        [SerializeField] private string _id;
        // Personal Information
        [SerializeField] private string _name;
        [SerializeField] private string _surname;
        [SerializeField] private string _birthDate;
        // Representation Information
        [SerializeField] private string _country;
        [SerializeField] private string _academy;
        [SerializeField] private string _school;
        // Ludosport Information
        [SerializeField] private RankType _rank;
        [SerializeField] private List<StyleType> _styles;
        [SerializeField] private Color _saberColor;
        [SerializeField] private string _startDate;
        // OTHER
        [SerializeField] private int _tier;

        #region Properties
        public string Id { get => _id; }
        // Personal Information
        public string Name { get => _name; set => _name = value; }
        public string Surname { get => _surname; set => _surname = value; }
        public DateTime BirthDate {
            get {
                DateTime.TryParseExact(_birthDate, LSTournamentConsts.DATE_FORMAT, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime res);
                
                return res;
            }
            set {
                _birthDate = value.ToString(LSTournamentConsts.DATE_FORMAT, CultureInfo.InvariantCulture);
            }
        }
        // Representation Information
        public string Country { get => _country; set => _country = value; }
        public string Academy { get => _academy; set => _academy = value; }
        public string School { get => _school; set => _school = value; }
        // Ludosport Information
        public RankType Rank { get => _rank; set => _rank = value; }
        public List<StyleType> Styles { get => _styles; set => _styles = value; }
        public Color SaberColor { get => _saberColor; set => _saberColor = value; }
        public DateTime StartDate {
            get {
                DateTime.TryParseExact(_startDate, LSTournamentConsts.DATE_FORMAT, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime res);

                return res;
            }
            set {
                _startDate = value.ToString(LSTournamentConsts.DATE_FORMAT, CultureInfo.InvariantCulture);
            }
        }
        // OTHER
        public int Tier { get => _tier; set => _tier = value; }
        #endregion

        #region Constructors
        public AthleteInfoModel() {
            _id = Guid.NewGuid().ToString();
            _styles = new List<StyleType>();
            _styles.Add(StyleType.Form1);
        }

        public AthleteInfoModel(
            string name, string surname, DateTime birthDate,
            string country, string academy, string school,
            RankType rank, List<StyleType> styles, Color saberColor,
            DateTime startDate, int tier) {
            _id = Guid.NewGuid().ToString();
            _name = name;
            _surname = surname;
            _birthDate = birthDate.ToString(LSTournamentConsts.DATE_FORMAT, CultureInfo.InvariantCulture);
            _country = country;
            _academy = academy;
            _school = school;
            _rank = rank;
            _styles = styles;
            _saberColor = saberColor;
            _startDate = startDate.ToString(LSTournamentConsts.DATE_FORMAT, CultureInfo.InvariantCulture);
            _tier = tier;

            if (!styles.Contains(StyleType.Form1)) {
                _styles.Add(StyleType.Form1);
            }
        }

        public AthleteInfoModel(string id,
            string name, string surname, DateTime birthDate,
            string country, string academy, string school,
            RankType rank, List<StyleType> styles, Color saberColor,
            DateTime startDate, int tier) {
            _id = id;
            _name = name;
            _surname = surname;
            _birthDate = birthDate.ToString(LSTournamentConsts.DATE_FORMAT, CultureInfo.InvariantCulture);
            _country = country;
            _academy = academy;
            _school = school;
            _rank = rank;
            _styles = styles;
            _saberColor = saberColor;
            _startDate = startDate.ToString(LSTournamentConsts.DATE_FORMAT, CultureInfo.InvariantCulture);
            _tier = tier;

            if (!styles.Contains(StyleType.Form1)) {
                _styles.Add(StyleType.Form1);
            }
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
            return DateTime.Now.Year - BirthDate.Year;
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
