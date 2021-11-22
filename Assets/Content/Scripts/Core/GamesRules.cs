using System.Collections.Generic;
using UnityEngine;

namespace Content.Scripts.Core
{
    public interface IGameRules
    {
        public bool IsPermanentlyWinCombo(ComboInfo comboInfo);

        public bool IsPermanentlyLoseCombo(ComboInfo comboInfo);

        public bool IsSetPointCombo(ComboInfo comboInfot);
    }
    
    public class GamesRules : MonoBehaviour, IGameRules
    {
        public static int TripleComboScoreMultiplier => 100;
        public bool IsPermanentlyWinCombo(ComboInfo comboInfo)
        {
            return comboInfo.HasNumber(4) && comboInfo.HasNumber(5) && comboInfo.HasNumber(6);
        }

        public bool IsPermanentlyLoseCombo(ComboInfo comboInfo)
        {
            return comboInfo.HasNumber(1) && comboInfo.HasNumber(2) && comboInfo.HasNumber(3);
        }

        public bool IsSetPointCombo(ComboInfo comboInfo)
        {
            if (comboInfo.IsSetPointCombo())
            {
                return true;
            }
            return false;
        }

        public bool IsTripleCombo(ComboInfo comboInfo)
        {
            return comboInfo.IsTriple();
        }
    }
}
