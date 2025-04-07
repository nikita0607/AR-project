using UnityEngine.Serialization;

namespace Marks.MarkGenerator
{
    /// <summary> Класс для хранения данных о метке. </summary>
    [System.Serializable]
    public class MarkSer
    {
        /// <summary> Название метки. </summary>
        public string name;

        /// <summary> Дополнительная информация о метке. </summary>
        public string info;

        /// <summary> Географическая долгота в градусах. </summary>
        public float longitude;

        /// <summary> Географическая широта в градусах. </summary>
        public float latitude;
    }
}