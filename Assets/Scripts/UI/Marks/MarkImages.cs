using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Marks
{
    public class MarkImages : MonoBehaviour
    {
        [System.Serializable]
        public struct MarkImage
        {
            public string MarkName;
            public Sprite MarkImageSprite;
        }
        
        [SerializeField] private Image targetImageObject;
        [SerializeField] MarkImage[] markImages;
        [SerializeField] private Sprite defaultMarkImage;

        public void SetMarkImage(string markName)
        {
            foreach (var markImage in markImages)
                if (markImage.MarkName == markName)
                {
                    targetImageObject.sprite = markImage.MarkImageSprite;
                    return;
                }
            
            targetImageObject.sprite = defaultMarkImage;
        }
    }
}