namespace Marks.MarkGenerator
{
    /// <summary> Класс для хранения данных о метке. </summary>
    [System.Serializable]
    public class MarkSer
    {
        /// <summary> Название метки. </summary>
        public string Name;

        /// <summary> Дополнительная информация о метке. </summary>
        public string Info;

        /// <summary> Географическая долгота в градусах. </summary>
        public float Longitude;

        /// <summary> Географическая широта в градусах. </summary>
        public float Latitude;
    }
}