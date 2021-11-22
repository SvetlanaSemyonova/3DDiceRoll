using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Content.Scripts.Core;
using Content.Scripts.Utilities;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;


namespace Content.Scripts.Managers
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance;
        
        [SerializeField] private GamesRules gamesRules;
        [SerializeField] private List<Character> characters = new List<Character>(5);
        [SerializeField] private CameraHandler cameraHandler;
        [SerializeField] private float moveCameraTime = 1f;
        [SerializeField] private float botGenerateComboDelay = 0.2f;
        [SerializeField] private SwipeDetector _swipeDetector;
        [SerializeField] private List<Dice> diceList = new List<Dice>(3);
        [SerializeField] private List<Transform> dicePositions = new List<Transform>(3);
        [SerializeField] private float forceCoefficient = 10f;
        [SerializeField] [Range(0, 5000)] private int animationsDelayTime = 2000;


        [HideInInspector] public int currentRoundBet;
        [HideInInspector] public int totalRoundBank;

        private List<Character> _charactersInCurrentRound = new List<Character>(5);
        private bool _hasWinnerInCurrentRound = false;
        private int _taskDelayCheckTime = 10;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
        }

        private void Start()
        {
            foreach (var character in characters)
            {
                character.SetCharacterBalance(PlayerData.Instance.startGameCurrencyAmount);
            }
        }


        public async void StartGame()
        {
            ResetGameState();
            MoveCameraToNextPos();
            await Task.Delay(TimeSpan.FromSeconds(moveCameraTime));

            StartCoroutine(DefineRoundBets());
        }

        private void ResetGameState()
        {
            cameraHandler.SetCurrentCameraPosIndex(0);
            _hasWinnerInCurrentRound = false;
            foreach (var character in characters)
            {
                character.tripleNumber = 0;
                character.setPointNumber = 0;
                character.currentRoundBet = 0;
                totalRoundBank = 0;
                currentRoundBet = 0;
                character.SetEmptyCombo();
            }

            _charactersInCurrentRound.Clear();
            _charactersInCurrentRound.AddRange(characters);
        }

        private IEnumerator DefineRoundBets()
        {
            foreach (var character in characters)
            {
                if (character.IsBot)
                {
                    yield return StartCoroutine(character.MakeBet());
                }
                else
                {
                    yield return StartCoroutine(character.MakeBet());
                }

                if (character.currentRoundBet == 0)
                {
                    _charactersInCurrentRound.Remove(character);
                }
            }

            if (_charactersInCurrentRound.Count == 1)
            {
                CharacterWin(_charactersInCurrentRound.First());
            }
            else
            {
                StartRound();
            }
        }


        private async void StartRound()
        {
            foreach (var character in _charactersInCurrentRound)
            {
                if (_hasWinnerInCurrentRound)
                {
                    return;
                }

                if (character.IsBot)
                {
                    await BotMakeTurn(character);
                }
                else
                {
                    await PlayerMakeTurn(character);
                }
            }

            if (!_hasWinnerInCurrentRound)
            {
                CompareNumbers();
            }
        }


        private void CompareNumbers()
        {
            var charactersResults = new List<int>();

            foreach (var character in _charactersInCurrentRound)
            {
                charactersResults.Add(character.tripleNumber * GamesRules.TripleComboScoreMultiplier +
                                      character.setPointNumber);
            }

            var maxResult = charactersResults.Max();
            CharacterWin(_charactersInCurrentRound[charactersResults.IndexOf(maxResult)]);
        }

        private async Task PlayerMakeTurn(Character currentPlayer)
        {
            MoveCameraToNextPos();

            await Task.Delay(TimeSpan.FromSeconds(moveCameraTime));
            await AwaitUserSwipe(currentPlayer);

            MoveCameraToNextPos();
        }

        private async Task AwaitUserSwipe(Character currentPlayer)
        {
            for (var i = 0; i < diceList.Count; i++)
            {
                diceList[i].transform.position = dicePositions[i].position;
                diceList[i].gameObject.SetActive(true);
            }

            while (!_swipeDetector.SwipeUp)
            {
                await Task.Delay(_taskDelayCheckTime);
            }

            foreach (var dice in diceList)
            {
                //TODO use force vector
                dice.diceRigidbody.isKinematic = false;
                var forceVector = new Vector3(_swipeDetector.SwipeDelta.x * forceCoefficient, 0,
                    _swipeDetector.SwipeDelta.y * forceCoefficient);
                dice.diceRigidbody.AddForce(new Vector3(Random.Range(-10f, 10f), Random.Range(130f, 140f),
                    Random.Range(120f, 130f)));
            }

            await Task.Delay(animationsDelayTime);

            while (!DicesMovingEnd())
            {
                await Task.Delay(_taskDelayCheckTime);
            }

            currentPlayer.currentCombo = GetDicesComboInfo();
            currentPlayer.SetRollsResult(currentPlayer.currentCombo);

            if (gamesRules.IsPermanentlyWinCombo(currentPlayer.currentCombo))
            {
                CharacterWin(currentPlayer);
                return;
            }

            if (gamesRules.IsPermanentlyLoseCombo(currentPlayer.currentCombo))
            {
                CharacterLose(currentPlayer);
                return;
            }

            if (gamesRules.IsTripleCombo(currentPlayer.currentCombo))
            {
                currentPlayer.tripleNumber = currentPlayer.currentCombo.firstDiceResult;
                return;
            }

            if (gamesRules.IsSetPointCombo(currentPlayer.currentCombo))
            {
                currentPlayer.setPointNumber = currentPlayer.currentCombo.GetSetPointNumber();
            }

            MoveCameraToNextPos();

            await Task.Delay(animationsDelayTime);

            foreach (var dice in diceList)
            {
                dice.diceRigidbody.isKinematic = true;
                dice.gameObject.SetActive(false);
            }

            if (gamesRules.IsTripleCombo(currentPlayer.currentCombo) == false &&
                gamesRules.IsSetPointCombo(currentPlayer.currentCombo) == false)
            {
                MoveCameraToBackPos();
                await Task.Delay(animationsDelayTime);
                await AwaitUserSwipe(currentPlayer);
            }
        }

        private ComboInfo GetDicesComboInfo()
        {
            return new ComboInfo(diceList[0].GetCurrentNumber(), diceList[1].GetCurrentNumber(),
                diceList[2].GetCurrentNumber());
        }

        private bool DicesMovingEnd()
        {
            for (var i = 0; i < diceList.Count; i++)
            {
                if (diceList[i].diceRigidbody.velocity.magnitude > GameConstants.DICE_CHECK_VELOCITY_ACCURACY)
                {
                    return false;
                }
            }

            return true;
        }

        private async Task BotMakeTurn(Character currentBot)
        {
            await Task.Delay(TimeSpan.FromSeconds(botGenerateComboDelay));
            var currentCombo = GetRandomCombo();
            currentBot.SetRollsResult(currentCombo);
            if (gamesRules.IsPermanentlyWinCombo(currentCombo))
            {
                CharacterWin(currentBot);
                return;
            }

            if (gamesRules.IsPermanentlyLoseCombo(currentCombo))
            {
                CharacterLose(currentBot);
                return;
            }

            if (gamesRules.IsTripleCombo(currentCombo))
            {
                currentBot.tripleNumber = currentCombo.firstDiceResult;
                return;
            }

            if (gamesRules.IsSetPointCombo(currentCombo))
            {
                currentBot.setPointNumber = currentCombo.GetSetPointNumber();
            }

            if (gamesRules.IsTripleCombo(currentCombo) == false && gamesRules.IsSetPointCombo(currentCombo) == false)
            {
                await BotMakeTurn(currentBot);
            }
        }

        private ComboInfo GetRandomCombo()
        {
            var comboInfo = new ComboInfo(Random.Range(1, 7), Random.Range(1, 7), Random.Range(1, 7));
            return comboInfo;
        }

        private void CharacterWin(Character winCharacter)
        {
            _hasWinnerInCurrentRound = true;
            winCharacter.AddCurrencyToCharacterBalance(totalRoundBank);
            EndTurn();
        }

        private void CharacterLose(Character loosedCharacter)
        {
            loosedCharacter.tripleNumber = -1;
            loosedCharacter.setPointNumber = -1;
        }

        private void EndTurn()
        {
            PanelManager.Instance.ShowWinPanel();
        }

        public void SetPlayerVisual(GameObject visualObject)
        {
            foreach (var character in characters.Where(character => !character.IsBot))
            {
                character.SetPlayerVisual(visualObject);
                return;
            }
        }

        private void MoveCameraToNextPos()
        {
            cameraHandler.gameObject.transform.DORotate(cameraHandler.GetNextPoint().rotation.eulerAngles,
                moveCameraTime);
            cameraHandler.gameObject.transform.DOMove(cameraHandler.GetNextPoint().position, moveCameraTime)
                .OnComplete(() => { cameraHandler.IncreaseCurrentCameraPosIndex(); });
        }

        private void MoveCameraToBackPos()
        {
            cameraHandler.gameObject.transform.DORotate(cameraHandler.GetLastPoint().rotation.eulerAngles,
                moveCameraTime);
            cameraHandler.gameObject.transform.DOMove(cameraHandler.GetLastPoint().position, moveCameraTime)
                .OnComplete(() => { cameraHandler.DecreaseCurrentCameraPosIndex(); });
        }
    }
}