using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YannickSCF.LSTournaments.Common.Scriptables.Data;
using YannickSCF.LSTournaments.Common.Scriptables.Formulas;

namespace YannickSCF.LSTournaments.Common.Controllers {
    public class ConfiguratorController : MonoBehaviour {

        [Header("Tournament Formulas")]
        [SerializeField] private List<TournamentFormula> _allTournamentFormulas;
        [SerializeField] private TournamentFormula _customFormula;

        [Header("Tournament Data")]
        [SerializeField] private TournamentData _data;

        #region Mono
        private void Awake() {
            TournamentFormulaUtils.SetTournamentFormulas(_allTournamentFormulas, _customFormula);
        }
        #endregion
    }
}
