/**
 * Author:      Yannick Santa Cruz Feuillias
 * Created:     10/11/2023
 **/

// Dependencies
using System;
using System.Collections.Generic;
using UnityEngine;

namespace YannickSCF.LSTournaments.Common.Scriptables.Settings.AthleteTables {
    [CreateAssetMenu(fileName = "Athlete Info Table Settings", menuName = "YannickSCF/LS Tournaments/Settings/New Athlete Info Table Settings")]
    public class AthleteInfoTableSettings : ScriptableObject {

        [SerializeField] private float _rowHeight = 40f;

        [SerializeField] private bool _countryCanExpand;
        [SerializeField] private bool _countryIsVisible;
        [SerializeField, Range(1, 100)] private float _countrySize;

        // ------------------------------------------------------------

        [SerializeField] private bool _surnameCanExpand;
        [SerializeField] private bool _surnameIsVisible;
        [SerializeField, Range(1, 100)] private float _surnameSize;

        // ------------------------------------------------------------

        [SerializeField] private bool _nameCanExpand;
        [SerializeField] private bool _nameIsVisible;
        [SerializeField, Range(1, 100)] private float _nameSize;

        // ------------------------------------------------------------

        [SerializeField] private bool _academyCanExpand;
        [SerializeField] private bool _academyIsVisible;
        [SerializeField, Range(1, 100)] private float _academySize;

        // ------------------------------------------------------------

        [SerializeField] private bool _schoolCanExpand;
        [SerializeField] private bool _schoolIsVisible;
        [SerializeField, Range(1, 100)] private float _schoolSize;

        // ------------------------------------------------------------

        [SerializeField] private bool _rankCanExpand;
        [SerializeField] private bool _rankIsVisible;
        [SerializeField, Range(1, 100)] private float _rankSize;

        // ------------------------------------------------------------

        [SerializeField] private bool _stylesCanExpand;
        [SerializeField] private bool _stylesIsVisible;
        [SerializeField, Range(1, 100)] private float _stylesSize;

        // ------------------------------------------------------------

        [SerializeField] private bool _tierCanExpand;
        [SerializeField] private bool _tierIsVisible;
        [SerializeField, Range(1, 100)] private float _tierSize;

        // ------------------------------------------------------------

        [SerializeField] private bool _saberColorCanExpand;
        [SerializeField] private bool _saberColorIsVisible;
        [SerializeField, Range(1, 100)] private float _saberColorSize;

        // ------------------------------------------------------------

        [SerializeField] private bool _birthDateCanExpand;
        [SerializeField] private bool _birthDateIsVisible;
        [SerializeField, Range(1, 100)] private float _birthDateSize;

        // ------------------------------------------------------------

        [SerializeField] private bool _startDateCanExpand;
        [SerializeField] private bool _startDateIsVisible;
        [SerializeField, Range(1, 100)] private float _startDateSize;

        // ------------------------------------------------------------

        public float RowHeight { get => _rowHeight; }

        public bool GetCanExpand(AthleteInfoType infoType) {
            switch (infoType) {
                case AthleteInfoType.Country: return _countryCanExpand;
                case AthleteInfoType.Surname: return _surnameCanExpand;
                case AthleteInfoType.Name: return _nameCanExpand;
                case AthleteInfoType.Academy: return _academyCanExpand;
                case AthleteInfoType.School: return _schoolCanExpand;
                case AthleteInfoType.Rank: return _rankCanExpand;
                case AthleteInfoType.Styles: return _stylesCanExpand;
                case AthleteInfoType.Tier: return _tierCanExpand;
                case AthleteInfoType.SaberColor: return _saberColorCanExpand;
                case AthleteInfoType.BirthDate: return _birthDateCanExpand;
                case AthleteInfoType.StartDate: return _startDateCanExpand;
                default: return false;
            }
        }

        public bool GetIsVisible(AthleteInfoType infoType) {
            switch (infoType) {
                case AthleteInfoType.Country: return _countryIsVisible;
                case AthleteInfoType.Surname: return _surnameIsVisible;
                case AthleteInfoType.Name: return _nameIsVisible;
                case AthleteInfoType.Academy: return _academyIsVisible;
                case AthleteInfoType.School: return _schoolIsVisible;
                case AthleteInfoType.Rank: return _rankIsVisible;
                case AthleteInfoType.Styles: return _stylesIsVisible;
                case AthleteInfoType.Tier: return _tierIsVisible;
                case AthleteInfoType.SaberColor: return _saberColorIsVisible;
                case AthleteInfoType.BirthDate: return _birthDateIsVisible;
                case AthleteInfoType.StartDate: return _startDateIsVisible;
                default: return false;
            }
        }

        public float GetSize(AthleteInfoType infoType) {
            switch (infoType) {
                case AthleteInfoType.Country: return _countrySize;
                case AthleteInfoType.Surname: return _surnameSize;
                case AthleteInfoType.Name: return _nameSize;
                case AthleteInfoType.Academy: return _academySize;
                case AthleteInfoType.School: return _schoolSize;
                case AthleteInfoType.Rank: return _rankSize;
                case AthleteInfoType.Styles: return _stylesSize;
                case AthleteInfoType.Tier: return _tierSize;
                case AthleteInfoType.SaberColor: return _saberColorSize;
                case AthleteInfoType.BirthDate: return _birthDateSize;
                case AthleteInfoType.StartDate: return _startDateSize;
                default: return 0;
            }
        }

        public List<AthleteInfoType> GetAllCanExpandInfo() {
            List<AthleteInfoType> canExpandInfo = new List<AthleteInfoType>();
            Array infoArray = Enum.GetValues(typeof(AthleteInfoType));
            foreach (Enum info in infoArray) {
                if (GetCanExpand((AthleteInfoType)info)) {
                    canExpandInfo.Add((AthleteInfoType)info);
                }
            }

            return canExpandInfo;
        }

        public List<AthleteInfoType> GetAllIsNotVisible() {
            List<AthleteInfoType> isNotVisibleInfo = new List<AthleteInfoType>();
            Array infoArray = Enum.GetValues(typeof(AthleteInfoType));
            foreach (Enum info in infoArray) {
                if (!GetIsVisible((AthleteInfoType)info)) {
                    isNotVisibleInfo.Add((AthleteInfoType)info);
                }
            }

            return isNotVisibleInfo;
        }
    }
}
