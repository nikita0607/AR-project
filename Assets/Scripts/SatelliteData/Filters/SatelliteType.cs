namespace SatelliteData.Filters
{
    /// <summary> Перечисление типов спутников для фильтрации. </summary>
    public enum SatelliteType
    {
        /// <summary> Специальное значение, обозначающее отсутствие фильтрации по типу. </summary>
        NULL = -1,
        
        /// <summary> Кубсаты - малые спутники стандартизированных размеров. </summary>
        CUBESAT = 1,
        
        /// <summary> Топ 100 наиболее значимых/известных спутников. </summary>
        TOP100 = 2,
        
        /// <summary> Спутники системы Starlink. </summary>
        NOAA = 3,
        
        /// <summary> Спутники, запущенные в последние 30 дней. </summary>
        LAST30DAYS = 4,
        
        /// <summary> Метеорологические спутники. </summary>
        WEATHER = 5,
        
        /// <summary> Геосинхронные спутники. </summary>
        Geosynchronous = 6,
        
        /// <summary> Специальные/особые спутники. </summary>
        SPECIFIC = 7,
    }
}