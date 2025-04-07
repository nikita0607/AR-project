namespace SatelliteData.Info
{
    /// <summary> Сериализуемый класс данных спутника для хранения и передачи информации. </summary>
    [System.Serializable]
    public class SatelliteSer
    {
        /// <summary> Наименование спутника. </summary>
        public string name;

        /// <summary> Дополнительная информация о спутнике. </summary>
        public string info;
    }
}