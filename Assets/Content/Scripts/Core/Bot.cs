using System.Collections;
using Content.Scripts.Managers;
using UnityEngine;

namespace Content.Scripts.Core
{
    public class Bot : Character
    {
        private void OnValidate()
        {
            IsBot = true;
        }

        public override IEnumerator MakeBet()
        {
            if ( GameManager.Instance.currentRoundBet < currencyAmount && GameManager.Instance.currentRoundBet == 0)
            {
                var bet = Random.Range(1, currencyAmount);
                GameManager.Instance.currentRoundBet = bet;
                GameManager.Instance.totalRoundBank += bet;
                WithdrawFromCharacter(bet);
            }
            else if (GameManager.Instance.currentRoundBet < currencyAmount)
            {
                currentRoundBet = GameManager.Instance.currentRoundBet;
                GameManager.Instance.totalRoundBank += GameManager.Instance.currentRoundBet;
                WithdrawFromCharacter(GameManager.Instance.currentRoundBet);
            }
            
            ShowBetOnTheFloor(currentRoundBet);
            yield return null;
        }
    }
}
