using System.Collections.Generic;

namespace SatelliteData.Filters
{
    /// <summary> Составной фильтр для скрытия спутников, объединяющий несколько условий через логическое И. </summary>
    public class SatelliteCompositeHideFilter : IFilter
    {
        /// <summary> Список фильтров, объединенных по условию И. </summary>
        private readonly List<SatelliteHideFilter> _andFilters = new();

        /// <summary> Создает новый составной фильтр с опциональным начальным набором фильтров. </summary>
        /// <param name="andFilters"> Начальный список фильтров (может быть null). </param>
        public SatelliteCompositeHideFilter(List<SatelliteHideFilter> andFilters = null)
        {
            andFilters?.ForEach(x => _andFilters.Add(x));
        }

        /// <summary> Проверяет, должен ли спутник отображаться согласно всем фильтрам. </summary>
        /// <param name="satellite"> Проверяемый спутник. </param>
        /// <returns> True если спутник проходит все фильтры, иначе False. </returns>
        public bool ShouldShowSatellite(Satellite satellite)
        {
            if (_andFilters.Count == 0) return false;

            foreach (var filter in _andFilters)
                if (!filter.ShouldShowSatellite(satellite)) return false;

            return true;
        }


        /// <summary> Оператор объединения составного фильтра с одиночным фильтром. </summary>
        /// <returns> Новый составной фильтр, содержащий все фильтры из a плюс b. </returns>
        public static SatelliteCompositeHideFilter operator &(SatelliteCompositeHideFilter a, SatelliteHideFilter b)
        {
            var filter = new SatelliteCompositeHideFilter(a._andFilters);
            filter._andFilters.Add(b);
            return filter;
        }

        /// <summary> Оператор объединения двух составных фильтров. </summary>
        /// <returns> Новый составной фильтр, содержащий все фильтры из a и b. </returns>
        public static SatelliteCompositeHideFilter operator &(SatelliteCompositeHideFilter a,
            SatelliteCompositeHideFilter b)
        {
            var filter = new SatelliteCompositeHideFilter(a._andFilters);
            b._andFilters.ForEach(x => filter._andFilters.Add(x));
            return filter;
        }

        /// <summary> Оператор удаления фильтра из композиции. </summary>
        /// <returns> Новый составной фильтр без указанного фильтра b. </returns>
        public static SatelliteCompositeHideFilter operator -(SatelliteCompositeHideFilter a, SatelliteHideFilter b)
        {
            var filter = new SatelliteCompositeHideFilter(a._andFilters);
            filter._andFilters.RemoveAll(x => x.Equals(b));
            return filter;
        }

        /// <summary> Возвращает строковое представление фильтра. </summary>
        /// <returns> Строка с количеством вложенных фильтров. </returns>
        public override string ToString() => $"SatelliteCompositeHideFilter({_andFilters.Count})";

        /// <summary> Возвращает список всех вложенных фильтров. </summary>
        /// <returns> Копия списка фильтров. </returns>
        public List<SatelliteHideFilter> GetFilters() => _andFilters;
    }
}