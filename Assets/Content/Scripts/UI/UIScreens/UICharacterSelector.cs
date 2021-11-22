using System;
using Content.Scripts.Configs;
using Content.Scripts.Core;
using Content.Scripts.Managers;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Content.Scripts.UI.UIScreens
{
    public class UICharacterSelector : MonoBehaviour
    {
        [SerializeField] private CharacterSelector characterSelector;
        [SerializeField] private Button playButton;
        [SerializeField] private TMP_Text charactersCountText;
        [SerializeField] private TMP_Text characterNameText;

        public TMP_Text CharactersCountText => charactersCountText;
        public TMP_Text CharacterNameText => characterNameText;
        

        private void Awake()
        {
            characterSelector.Initialize();
            characterSelector.OnSelectCharacterChange += SetPageStateBySelectedCharacter;
            playButton.onClick.AddListener(OnPlayButtonPress);

            characterSelector.OnSelectCharacterChange += ApplyPlayerVisual;
            characterSelector.ApplyPlayerVisual();
            SetPageStateBySelectedCharacter(characterSelector.SelectedCharacter);
        }
        
        private void ApplyPlayerVisual(CharacterDataConfig data)
        {
            GameManager.Instance.SetPlayerVisual(data.VisualObject);
        }

        private void OnPlayButtonPress()
        {
            gameObject.SetActive(false);
            characterSelector.ApplyPlayerVisual();
        }

        private void SetButtonState(bool state)
        {
            playButton.gameObject.SetActive(state);
        }

        private void SetGottenCharacterState()
        {
            playButton.gameObject.SetActive(true);
            SetButtonState(true);
        }

        private void SetPageStateBySelectedCharacter(CharacterDataConfig data)
        {
            CharacterNameText.text = data.Name;
            CharactersCountText.text = Convert.ToString(characterSelector.CurrentCharacterIndex + 1) + "/" +
                                       Convert.ToString(characterSelector.CharactersCount);
            if (data.IsBuyed)
            {
                SetGottenCharacterState();
            }
        }
    }
}