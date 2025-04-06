namespace SatelliteData.Filters
{
    /// <summary> Интерфейс для фильтрации спутников. </summary>
    public interface IFilter 
    {
        /// <summary> Определяет, должен ли спутник отображаться. </summary>
        /// <param name="satellite"> Проверяемый спутник. </param>
        /// <returns> True если спутник должен отображаться, иначе False. </returns>
        public bool ShouldShowSatellite(Satellite satellite);

        /// <summary> Перегрузка оператора & для комбинации фильтров. </summary>
        /// <param name="a"> Первый фильтр. </param>
        /// <param name="b"> Второй фильтр. </param>
        /// <returns> Новый составной фильтр. </returns>
        /// <remarks> В текущей реализации всегда возвращает SatelliteHideFilter. </remarks>
        public static IFilter operator&(IFilter a, IFilter b) {
            return new SatelliteHideFilter();
        }
    }
}