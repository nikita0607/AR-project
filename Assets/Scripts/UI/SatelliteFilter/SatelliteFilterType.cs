using System;
using System.Collections.Generic;
using SatelliteData.Filters;
using SatelliteData.Info;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.SatelliteFilter
{
    /// <summary> Контроллер фильтрации спутников по типу. </summary>
    public class SatelliteFilterType : MonoBehaviour
    {
        /// <summary> Контроллер информации о спутниках (Singleton). </summary>
        private SatelliteInfoController _satelliteInfoController;
        
        /// <summary> Шаблон кнопки для создания фильтров. </summary>
        [SerializeField] private Button _button;
        
        /// <summary> Контейнер для размещения кнопок фильтров. </summary>
        [SerializeField] private GameObject content;
        
        /// <summary> Текущий активный фильтр по типу спутника. </summary>
        private SatelliteHideFilter _satelliteFilter;

        private static Dictionary<SatelliteType, string> dict = new()
        {
            {SatelliteType.CUBESAT, "Кубсаты"},
            {SatelliteType.TOP100, "100 Ярчайших"},
            {SatelliteType.NOAA, "NOAA"},
            {SatelliteType.LAST30DAYS, "Запущенные в последние 30 дней"},
            {SatelliteType.WEATHER, "Метеорологические"},
            {SatelliteType.Geosynchronous, "Геостационарные"},
            {SatelliteType.SPECIFIC, "хуйня"}
        };

        /// <summary> Инициализирует контроллер и создает кнопки фильтров. </summary>
        /// <remarks>
        /// 1. Получает ссылку на SatelliteInfoController
        /// 2. Динамически создает кнопки для всех типов спутников (кроме SatelliteType.NULL)
        /// 3. Настраивает текст и обработчики кликов для кнопок
        /// </remarks>
        private void Start()
        {
            _satelliteInfoController = SatelliteInfoController.Singleton;
            
            // Создаем кнопки для всех типов спутников, кроме NULL (-1)
            for (int i = 1; i < Enum.GetNames(typeof(SatelliteType)).Length; i++)
            {
                var button = Instantiate(_button, parent: content.transform);
                int typeIndex = i;
                
                button.name = ((SatelliteType)typeIndex).ToString();
                button.GetComponentInChildren<TextMeshProUGUI>().text = dict[(SatelliteType)typeIndex];
                
                button.onClick.AddListener(() => ApplySearch((SatelliteType)typeIndex));
            }
        }

        /// <summary> Применяет фильтрацию по выбранному типу спутника. </summary>
        /// <param name="satelliteType"> Тип спутника для фильтрации. </param>
        private void ApplySearch(SatelliteType satelliteType)
        {
            Debug.Log("ApplySearch " +  satelliteType);
            
            // Удаляем предыдущий фильтр, если был установлен
            if (_satelliteFilter != null)
                _satelliteInfoController.SatelliteHideFilters -= _satelliteFilter;
            
            // Создаем и применяем новый фильтр
            _satelliteFilter = new SatelliteHideFilter(satelliteType: satelliteType);
            _satelliteInfoController.SatelliteHideFilters &= _satelliteFilter;
            
            // Пытаемся применить фильтр
            _satelliteInfoController.TryApplySatelliteFilter();
        }
    }   
}