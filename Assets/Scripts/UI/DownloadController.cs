using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    /// <summary> Контроллер управления UI процесса загрузки данных. </summary>
    public class DownloadController : MonoBehaviour
    {
        /// <summary> GameObject с изображением статуса загрузки. </summary>
        [SerializeField] private GameObject sprite;
        
        /// <summary> Массив спрайтов для отображения статуса (0 - загрузка, 1 - готово). </summary>
        [SerializeField] private Sprite[] sprites;
        
        /// <summary> Текстовое поле основного статуса. </summary>
        [SerializeField] private TMP_Text text;
        
        /// <summary> Текстовое поле детализации загрузки. </summary>
        [SerializeField] private TMP_Text loadText;
        
        /// <summary> Массив кнопок, блокируемых во время загрузки. </summary>
        [SerializeField] private Button[] buttons;
        
        /// <summary> Панель отображения ошибки загрузки. </summary>
        [Header("Errors")]
        [SerializeField] private GameObject errorPanel;
        
        private Image _image;

        /// <summary> Инициализирует компонент Image для отображения статуса. </summary>
        private void Awake()
        {
            _image = sprite.GetComponent<Image>();
        }
        
        /// <summary> Настраивает начальное состояние UI. </summary>
        /// <remarks>
        /// 1. Устанавливает спрайт "загрузка"
        /// 2. Блокирует все кнопки
        /// </remarks>
        private void Start()
        {
            _image.sprite = sprites[0];
            foreach (var button in buttons)
                button.interactable = false;
        }

        /// <summary> Обновляет текст статуса загрузки. </summary>
        /// <param name="text"> Тип загружаемых спутников. </param>
        public void ChangeText(string text)
        {
            this.text.text = "Подождите пока данные о спутниках загрузятся...";
            loadText.text = $"Сейчас загружается база данных с типом спутников: {text}";
        }

        /// <summary> Активирует панель ошибки загрузки. </summary>
        public void OnDownloadError() => errorPanel.SetActive(true);

        /// <summary> Обрабатывает успешное завершение загрузки. </summary>
        /// <remarks>
        /// 1. Обновляет тексты статусов
        /// 2. Меняет спрайт на "готово"
        /// 3. Разблокирует кнопки
        /// </remarks>
        public void OnStopDownload()
        {
            text.text = "Загрузка завершена. Выберите режим работы.";
            loadText.text = "Готово";
            _image.sprite = sprites[1];
            
            foreach (var button in buttons)
                button.interactable = true;
        }
    }
}