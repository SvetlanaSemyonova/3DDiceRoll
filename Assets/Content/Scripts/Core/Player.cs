using System.Collections;
using Content.Scripts.Managers;
using Content.Scripts.UI;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Content.Scripts.Core
{
    public class Player : Character
    {
        [SerializeField] private UIBetModel betModel;
        [SerializeField] private float awaitingUserActionTime = 30f;

        private void Start()
        {
            betModel.OnAcceptButtonClick += CheckInputBet;
            betModel.OnCloseButtonClick += CloseBetModelButtonClickHandler;
        }

        public override IEnumerator MakeBet()
        {
            var awaitBetTime = 0f;
            betModel.gameObject.SetActive(true);
            
            
            while (awaitBetTime<awaitingUserActionTime && currentRoundBet <= 0)
            {
                awaitBetTime += Time.deltaTime;
                yield return null;
            }

            if (currentRoundBet <= 0)
            {
                var currentPlayerBet = Random.Range(1, currencyAmount);
                Debug.Log("Player not enter the bet. Set a random value" + currentPlayerBet);

                currentRoundBet = currentPlayerBet;
            }
            ShowBetOnTheFloor( currentRoundBet);
            WithdrawFromCharacter(currentRoundBet);
            
            GameManager.Instance.currentRoundBet = currentRoundBet;
            GameManager.Instance.totalRoundBank += currentRoundBet;
        }

        private void OnDestroy()
        {
            betModel.OnAcceptButtonClick -= CheckInputBet;
            betModel.OnCloseButtonClick -= CloseBetModelButtonClickHandler;
        }

        private void CloseBetModelButtonClickHandler()
        {
            Debug.Log("player canceled the bet , skip this round");
        }

        private void CheckInputBet(int betAmount)
        {
            if (betAmount <= 0 || betAmount>currencyAmount)
            {
                return;
            }

            currentRoundBet = betAmount;
            
            if (Debug.isDebugBuild)
            {
                Debug.LogWarning($"Set player's bet: {betAmount}");
            }
            
            betModel.gameObject.SetActive(false);
        }
    }
}
