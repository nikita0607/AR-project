using UnityEngine;
using TMPro;
using UI.Marks;

namespace Marks
{
    /// <summary> Класс для отображения информации о метке при клике на нее. </summary>
    public class ShowMarkinfoOnClick : MonoBehaviour
    {
        /// <summary> Текстовое поле для вывода информации о метке. </summary>
        [SerializeField] private TMP_Text textField;

        /// <summary> Панель с информацией о метке. </summary>
        [SerializeField] private GameObject infoPanel;

        /// <summary> Компонент для отображения изображения метки. </summary>
        [SerializeField] private MarkImages markImage;

        private Camera _camera;

        /// <summary> Инициализирует главную камеру при старте. </summary>
        private void Start()
        {
            _camera = Camera.main;
        }

        /// <summary> Обрабатывает клики по меткам и отображает информацию. </summary>
        /// <remarks> При клике на объект с тегом "mark" активирует панель информации и заполняет ее данными. </remarks>
        void Update()
        {
            if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
            {
                if (_camera == null) return;

                Ray ray = _camera.ScreenPointToRay(Input.GetTouch(0).position);

                if (Physics.Raycast(ray, out var hit, Mathf.Infinity))
                    if (hit.collider.CompareTag("mark"))
                    {
                        infoPanel.SetActive(true);
                        Mark mark = hit.collider.gameObject.GetComponent<Mark>();
                        markImage.SetMarkImage(mark.Name);
                        string text = mark.Name;
                        text += "\n" + mark.Info;
                        textField.text = text;
                    }
            }
        }
    }
}