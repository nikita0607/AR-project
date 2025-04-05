using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class DownloadController :  MonoBehaviour
    {
        [SerializeField] private GameObject  _sprite;
        [SerializeField] private Sprite[]  _sprites;
        
        [SerializeField] private TMP_Text _text;
        [SerializeField] private TMP_Text loadText;
        
        [SerializeField] private Button[] _buttons;
        
        private Image _image;

        private void Awake()
        {
            _image = _sprite.GetComponent<Image>();
        }
        
        private void Start()
        {
            _image.sprite = _sprites[0];

            foreach (var button in _buttons)
                button.interactable = false;
        }

        public void ChangeText(string text)
        {
            _text.text = "Подождите пока данные о спутниках загрузятся...";
            loadText.text =  $"Сейчас загружается база данных с типом спутников: {text}";
        }

        public void OnStopDownload()
        {
            _text.text = "Загрузка завершена. Выберите режим работы.";
            loadText.text = "Готово";
            _image.sprite = _sprites[1];
            
            foreach (var button in _buttons)
                button.interactable = true;
        }
    }
}