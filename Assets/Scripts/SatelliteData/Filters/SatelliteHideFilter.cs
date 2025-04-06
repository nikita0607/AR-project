using System.Collections.Generic;

namespace SatelliteData.Filters
{
    /// <summary> Фильтр для скрытия спутников по различным критериям. </summary>
    public class SatelliteHideFilter : IFilter
    {
        /// <summary> Минимальный год запуска для фильтрации. Если null - фильтр по году не применяется. </summary>
        private readonly int? _minYearFilter;
        
        /// <summary> Строка для поиска в названии спутника. Если null - фильтр по имени не применяется. </summary>
        private readonly string _nameFilter;
        
        /// <summary> Тип спутника для фильтрации. Если SatelliteType.NULL - фильтр по типу не применяется. </summary>
        private readonly SatelliteType _satelliteType;
        
        /// <summary> NORAD ID спутника для точного совпадения. Если null - фильтр по NORAD ID не применяется. </summary>
        private readonly string _noradID;

        /// <summary> Создает новый фильтр спутников с указанными параметрами. </summary>
        /// <param name="minYearFilter"> Минимальный год запуска (опционально). </param>
        /// <param name="nameFilter"> Строка для фильтрации по имени (опционально). </param>
        /// <param name="noradID"> NORAD ID для точного совпадения (опционально). </param>
        /// <param name="satelliteType"> Тип спутника для фильтрации. </param>
        public SatelliteHideFilter(int? minYearFilter = null, string nameFilter = null, string noradID = null,
            SatelliteType satelliteType = SatelliteType.NULL)
        {
            _minYearFilter = minYearFilter;
            _nameFilter = nameFilter;
            _satelliteType = satelliteType;
            _noradID = noradID;
        }

        /// <summary> Проверяет, соответствует ли спутник критериям фильтра. </summary>
        /// <param name="satellite"> Проверяемый спутник. </param>
        /// <returns> True если спутник соответствует всем критериям, иначе False. </returns>
        public bool ShouldShowSatellite(Satellite satellite)
        {
            if (_noradID != null && satellite.LoadedTle.getNoradID() != _noradID) return false;
            if (_minYearFilter.HasValue && satellite.LoadedTle.getStartYear() < _minYearFilter.Value) return false;
            if (_nameFilter != null && !satellite.LoadedTle.getName().Contains(_nameFilter)) return false;
            if (_satelliteType != SatelliteType.NULL && satellite.SatelliteType != _satelliteType) return false;

            return true;
        }

        /// <summary> Объединяет два фильтра через логическое И. </summary>
        /// <returns> Новый составной фильтр, содержащий оба условия. </returns>
        public static SatelliteCompositeHideFilter operator &(SatelliteHideFilter a, SatelliteHideFilter b)
        {
            return new SatelliteCompositeHideFilter(new List<SatelliteHideFilter> { a, b });
        }

        /// <summary> Возвращает строковое представление фильтра. </summary>
        /// <returns> Строка с основными параметрами фильтра. </returns>
        public override string ToString() => $"Filter({_nameFilter}, {_satelliteType})";

        /// <summary> Сравнивает фильтры по их параметрам. </summary>
        /// <param name="obj"> Объект для сравнения. </param>
        /// <returns> True если фильтры идентичны, иначе False. </returns>
        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            return ToString() == obj.ToString();
        }

        /// <summary> Возвращает хэш-код фильтра. </summary>
        /// <returns> Хэш-код, вычисленный на основе параметров фильтра. </returns>
        public override int GetHashCode() => _nameFilter.GetHashCode() + _satelliteType.GetHashCode();
    }
}