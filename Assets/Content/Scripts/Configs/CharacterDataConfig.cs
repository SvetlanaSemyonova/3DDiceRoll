using UnityEngine;
using UnityEngine.Serialization;

namespace Content.Scripts.Configs
{
    [CreateAssetMenu(fileName = nameof(CharacterDataConfig),
        menuName = "Configs/" + nameof(CharacterDataConfig), order = 51)]
    public class CharacterDataConfig : ScriptableObject
    {
        [FormerlySerializedAs("ObjectId")] 
        [SerializeField] private int objectId;

        [FormerlySerializedAs("PlayerViual")] 
        [SerializeField] private GameObject playerViual;

        [FormerlySerializedAs("PlayerName")] 
        [SerializeField] private string playerName;

        [FormerlySerializedAs("PlayerCost")]
        [SerializeField] [Range(0, 100000)] private int playerCost;
        
        public GameObject VisualObject => playerViual;

        public string Name => playerName;

        public int Cost => playerCost;
        [SerializeField] private bool isBuyed = false;

        public bool IsBuyed
        {
            get => isBuyed;
            set => isBuyed = value;
        }

        public override string ToString()
        {
            return $"{base.ToString()}, {nameof(objectId)}: {objectId}, {nameof(playerName)}: {playerName}, {nameof(playerCost)}: {playerCost}, {nameof(isBuyed)}: {isBuyed}";
        }
    }
}