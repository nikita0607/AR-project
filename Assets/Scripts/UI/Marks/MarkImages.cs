using UnityEngine;
using UnityEngine.UI;

namespace UI.Marks
{
    /// <summary> Компонент для управления изображениями меток/маркеров. </summary>
    public class MarkImages : MonoBehaviour
    {
        /// <summary> Структура для хранения связки имени метки и соответствующего изображения. </summary>
        [System.Serializable]
        public struct MarkImage
        {
            /// <summary> Название метки для сопоставления. </summary>
            public string MarkName;
            
            /// <summary> Изображение спрайта для данной метки. </summary>
            public Sprite MarkImageSprite;
        }
        
        /// <summary> Целевой объект Image для отображения выбранного спрайта. </summary>
        [SerializeField] private Image targetImageObject;
        
        /// <summary> Массив возможных изображений меток. </summary>
        [SerializeField] private MarkImage[] markImages;
        
        /// <summary> Изображение по умолчанию, если метка не найдена. </summary>
        [SerializeField] private Sprite defaultMarkImage;

        /// <summary> Устанавливает изображение метки по её названию. </summary>
        /// <param name="markName"> Название искомой метки. </param>
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