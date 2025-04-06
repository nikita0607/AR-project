namespace SatelliteData.Info
{
    /// <summary> Сериализуемый класс данных спутника для хранения и передачи информации. </summary>
    [System.Serializable]
    public class SatelliteSer
    {
        /// <summary> Наименование спутника. </summary>
        public string Name;

        /// <summary> Дополнительная информация о спутнике. </summary>
        public string Info;
    }
}