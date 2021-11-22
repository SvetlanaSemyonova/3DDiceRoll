using System;
using System.Collections.Generic;
using Content.Scripts.Configs;
using Content.Scripts.Managers;
using Content.Scripts.UI;
using UnityEngine;

namespace Content.Scripts.Core
{
    public class CharacterSelector : MonoBehaviour
    {
        public Action<CharacterDataConfig> OnSelectCharacterChange;

        [SerializeField] private int currentCharacterIndex;
        [SerializeField] private float selectionDragSpeed = 2;
        [SerializeField] private bool isDrag = true;
        [SerializeField] private Vector3 characterIdentation;
        [SerializeField] private Transform selector;
        [SerializeField] private CharactersListConfig charactersList;
        [SerializeField] private CharacterSelectorInput inputScript;

        private string SaveKey = "CurrentCharacterIndex";
        private List<GameObject> _localCharactersArray = new List<GameObject>();


        public int CharactersCount
        {
            get => charactersList.Characters.Count;
        }

        public int CurrentCharacterIndex
        {
            get => currentCharacterIndex;
            set
            {
                if (value.Equals(currentCharacterIndex))
                    return;

                currentCharacterIndex = value;
                OnSelectCharacterChange?.Invoke(charactersList.Characters[currentCharacterIndex]);
            }
        }

        private void Update()
        {
            for (var i = 0; i < _localCharactersArray.Count; i++)
            {
                if (i == CurrentCharacterIndex)
                {
                    var activeCharacterScale = _localCharactersArray[i].transform.localScale;
                    _localCharactersArray[i].transform.localScale = Vector3.Lerp(activeCharacterScale,
                        new Vector3(3, 3, 3), Time.deltaTime * 5);
                    continue;
                }

                var localCharacterScale = _localCharactersArray[i].transform.localScale;
                _localCharactersArray[i].transform.localScale = Vector3.Lerp(localCharacterScale,
                    new Vector3(2.0f, 2.0f, 2.0f), Time.deltaTime * 5);
            }

            CurrentCharacterIndex =
                Mathf.Clamp(Mathf.Abs(Convert.ToInt32(selector.localPosition.x / characterIdentation.x)), 0,
                    charactersList.Characters.Count - 1);

            if (!isDrag)
            {
                var localPosition = selector.localPosition;
                var toLerp = new Vector3(-CurrentCharacterIndex * characterIdentation.x, localPosition.y,
                    localPosition.z);
                localPosition = Vector3.Lerp(localPosition, toLerp, Time.deltaTime * 2);
                selector.localPosition = localPosition;
            }
        }

        private void OnDestroy()
        {
            inputScript.onPointerDrag -= MoveSelection;
            inputScript.onEndDrag -= EndDrag;
        }

        public void Initialize()
        {
            if (PlayerPrefs.HasKey(SaveKey))
            {
                CurrentCharacterIndex = PlayerPrefs.GetInt(SaveKey);
            }

            ApplyPlayerVisual();
            inputScript.onPointerDrag += MoveSelection;
            inputScript.onEndDrag += EndDrag;
            Initialize(charactersList.Characters, CurrentCharacterIndex);
            isDrag = true;
            OnSelectCharacterChange?.Invoke(charactersList.Characters[CurrentCharacterIndex]);
        }


        public CharacterDataConfig SelectedCharacter
        {
            get => charactersList.Characters[CurrentCharacterIndex];
        }


        private void Initialize(List<CharacterDataConfig> characters, int startChararacterIndex)
        {
            if (startChararacterIndex < characters.Count || startChararacterIndex <= 0)
            {
                _localCharactersArray = new List<GameObject>();
                for (var i = 0; i < characters.Count; i++)
                {
                    var visual = Instantiate(characters[i].VisualObject, selector);
                    var buffObject = new GameObject()
                    {
                        name = "Character"
                    };

                    visual.transform.parent = buffObject.transform;
                    visual.transform.localPosition = new Vector3();
                    buffObject.transform.parent = selector;
                    buffObject.transform.localPosition = new Vector3(0, 0, -1.3f);

                    _localCharactersArray.Add(buffObject);
                    ResetObject(_localCharactersArray[i].gameObject);

                    _localCharactersArray[i].transform.localPosition = new Vector3(characterIdentation.x * i,
                        characterIdentation.y, characterIdentation.z);
                }

                var localPosition = selector.localPosition;
                localPosition = new Vector3(characterIdentation.x * -startChararacterIndex,
                    localPosition.y, localPosition.z);
                selector.localPosition = localPosition;
                return;
            }

            throw new Exception("Invalid argument in Initialize Character selector!");
        }


        private void MoveSelection(float inputValue)
        {
            isDrag = true;

            CurrentCharacterIndex =
                Mathf.Clamp(Mathf.Abs(Convert.ToInt32(selector.localPosition.x / characterIdentation.x)), 0,
                    charactersList.Characters.Count - 1);

            if (-(selector.localPosition.x / characterIdentation.x) <= 0)
            {
                inputValue = Mathf.Max(0, inputValue);
            }

            if (-(selector.localPosition.x / characterIdentation.x) +
                (1 % (selector.localPosition.x / characterIdentation.x)) >= _localCharactersArray.Count)
            {
                inputValue = Mathf.Min(0, inputValue);
            }

            var position = selector.position;
            position = new Vector3(position.x - (inputValue * Time.deltaTime * selectionDragSpeed),
                position.y, position.z);

            selector.position = position;
        }


        private void ResetObject(GameObject targer)
        {
            //TODO need in remaking
            targer.transform.localPosition = new Vector3(0, 0, 0);
            var rotation = new Quaternion
            {
                eulerAngles = new Vector3(0, 180, 0)
            };
            targer.transform.localRotation = rotation;
            targer.transform.localScale = new Vector3(2.4f, 2.4f, 2.4f);
        }


        private void EndDrag()
        {
            isDrag = false;
        }

        public void ApplyPlayerVisual()
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.SetPlayerVisual(charactersList.Characters[CurrentCharacterIndex]
                    .VisualObject);
            }

            PlayerPrefs.SetInt(SaveKey, CurrentCharacterIndex);
        }
    }
}
