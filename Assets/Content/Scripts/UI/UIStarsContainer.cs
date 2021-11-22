using System.Collections.Generic;
using Content.Scripts.Managers;
using Content.Scripts.Utilities;
using UnityEngine;
using UnityEngine.UI;

namespace Content.Scripts.UI
{
    public class UIStarsContainer : MonoBehaviour
    {
        [SerializeField] private List<Image> starImages = new List<Image>();
        [SerializeField] private Sprite activeStarSprite;
        [SerializeField] private Sprite disabledStarSprite;


        private void OnEnable()
        {
            SelectStarsCount(0);
        }

        public void SelectStarsCount(int starNumber)
        {
            if (PlayerData.Instance.settings.Value.isSoundEnable)
            {
                SoundManager.Instance.PlayEffect(AudioDefault.Switch);
            }
            
            for (var i = 0; i < starImages.Count; i++)
            {
                var starImage = starImages[i];
                starImage.sprite = i < starNumber ? activeStarSprite : disabledStarSprite;
                
            }
        }
    }
}