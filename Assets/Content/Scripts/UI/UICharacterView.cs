using Content.Scripts.Core;
using TMPro;
using UnityEngine;

namespace Content.Scripts.UI
{
    public class UICharacterView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI currencyAmount;
        [SerializeField] private TextMeshProUGUI firstRollResult;
        [SerializeField] private TextMeshProUGUI secondRollResult;
        [SerializeField] private TextMeshProUGUI thirdRollResult;

        public void UpdateCurrencyAmount(int newAmount)
        {
            currencyAmount.text = newAmount.ToString();
        }

        public void SetRollsResult(ComboInfo comboInfo)
        {
            firstRollResult.text = comboInfo.firstDiceResult.ToString();
            secondRollResult.text = comboInfo.secondDiceResult.ToString();
            thirdRollResult.text = comboInfo.thirdDiceResult.ToString();
        }

        public void SetEmptyResults()
        {
            firstRollResult.text = "";
            secondRollResult.text = "";
            thirdRollResult.text = "";
        }
    }
}
