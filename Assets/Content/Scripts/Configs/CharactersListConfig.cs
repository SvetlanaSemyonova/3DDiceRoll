using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace Content.Scripts.Configs
{
    [CreateAssetMenu(fileName = "New Characters List Config", menuName = "Configs/" + nameof(CharactersListConfig), order = 51)]
    public class CharactersListConfig : ScriptableObject
    {
        [SerializeField] private List<CharacterDataConfig> charactersList = new List<CharacterDataConfig>();

        public List<CharacterDataConfig> Characters => charactersList;


        private void OnValidate()
        {
            charactersList.Sort((first, second) => { return first.Cost.CompareTo(second.Cost); });
            
            for (var i = 0; i < charactersList.Count; i++)
            {
                if (charactersList[i].Cost <= 0)
                {
                    charactersList[i].IsBuyed = true;
                }
                if (charactersList[i] != null)
                {
                    var field = charactersList[i].GetType().GetField("ObjectId",
                        BindingFlags.NonPublic | BindingFlags.Instance);
                    if (field != null)
                    {
                        field.SetValue(charactersList[i], i);
                    }
                }
            }
        }
    }
}