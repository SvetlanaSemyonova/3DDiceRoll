using System.Collections;
using System.Collections.Generic;
using Content.Scripts.UI;
using UnityEngine;

namespace Content.Scripts.Core
{
    public abstract class Character : MonoBehaviour
    {
        [SerializeField] private UICharacterView characterView;
        [SerializeField] private Transform betTransformRoot;
        [SerializeField] private GameObject player3DVisual;
        [SerializeField] private List<GameObject> gameBetViews = new List<GameObject>(5);
        [SerializeField] private float spawnChipsScatter = 0.1f;
        [SerializeField] private int chipsRelativeCoefficient = 50;
        
        public int tripleNumber = 0;
        public int setPointNumber = 0;
        public int currencyAmount;
        public bool IsBot = false;
        public ComboInfo currentCombo;
        [HideInInspector] public int currentRoundBet;

        public void AddCurrencyToCharacterBalance(int amount)
        {
            currencyAmount += amount;
            if (characterView != null)
            {
                characterView.UpdateCurrencyAmount(currencyAmount);
            }
        }

        public void SetPlayerVisual(GameObject playerVisual)
        {
            if (player3DVisual != null)
            {
                Destroy(player3DVisual);
                player3DVisual = Instantiate(playerVisual, gameObject.transform);
            }
        }

        public void WithdrawFromCharacter(int amount)
        {
            currencyAmount -= amount;
            if (characterView != null)
            {
                characterView.UpdateCurrencyAmount(currencyAmount);
            }
        }

        public void SetCharacterBalance(int newBalanceAmount)
        {
            currencyAmount = newBalanceAmount;
            
            if (characterView != null)
            {
                characterView.UpdateCurrencyAmount(newBalanceAmount);
            }
        }

        public void SetRollsResult(ComboInfo comboInfo)
        {
            currentCombo = comboInfo;
            
            if (characterView != null)
            {
                characterView.SetRollsResult(comboInfo);
            }
        }

        protected void ShowBetOnTheFloor(int betAmount)
        {
            var chipsCount = Mathf.CeilToInt(betAmount / chipsRelativeCoefficient);
            
            for (var i = 0; i < chipsCount; i++)
            {
                var chip = Instantiate(gameBetViews[Random.Range(0, gameBetViews.Count)], betTransformRoot);
                var scatterVector = new Vector3(Random.Range(-spawnChipsScatter, spawnChipsScatter),
                    Random.Range(-spawnChipsScatter, spawnChipsScatter),
                    Random.Range(-spawnChipsScatter, spawnChipsScatter));
                chip.transform.position += scatterVector;
            }
        }


        public virtual IEnumerator MakeBet()
        {
            yield return null;
        }

        public void SetEmptyCombo()
        {
            characterView.SetEmptyResults();
        }
    }
}