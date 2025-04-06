using UnityEngine;

namespace Marks.MarkGenerator
{
    /// <summary> Генерирует метки на сфере на основе данных из JSON-файла. </summary>
    public class MarkGenerator : MonoBehaviour
    {
        /// <summary> Радиус виртуальной сферы (Земли) для позиционирования меток. </summary>
        [SerializeField] private float earthUnitRadius;

        /// <summary> Родительский объект для всех создаваемых меток. </summary>
        [SerializeField] private GameObject markParrent;

        /// <summary> Префаб метки для создания экземпляров. </summary>
        [SerializeField] private GameObject mark;

        /// <summary> JSON-файл с данными о метках. </summary>
        [SerializeField] public TextAsset jsonFile;

        private MarksSer _marksFromJson;

        /// <summary> Инициализирует генерацию меток при старте. </summary>
        void Start()
        {
            _marksFromJson = JsonUtility.FromJson<MarksSer>(jsonFile.text);
            CreateMark();
        }

        /// <summary> Создает метки на основе данных из JSON. </summary>
        /// <remarks> Для каждой записи в JSON создается экземпляр метки, настраивается его позиция и поворот. </remarks>
        void CreateMark()
        {
            for (int i = 0; i < _marksFromJson.marks.Length; i++)
            {
                MarkSer markInfo = _marksFromJson.marks[i];

                GameObject newMark = Instantiate(mark, markParrent.transform);
                Mark newMarkComponent = newMark.GetComponent<Mark>();

                newMark.transform.localScale = mark.transform.localScale;

                newMark.name = _marksFromJson.marks[i].Name;
                newMarkComponent.Name = _marksFromJson.marks[i].Name;
                newMarkComponent.Info = _marksFromJson.marks[i].Info;
                newMarkComponent.SetPosition(EciPositionable.FromLongLat(markInfo.Longitude, markInfo.Latitude,
                    earthUnitRadius));

                newMark.transform.rotation =
                    Quaternion.LookRotation(markParrent.transform.position - newMark.transform.position);
            }
        }
    }
}