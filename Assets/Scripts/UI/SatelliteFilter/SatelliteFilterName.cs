using System;
using SatelliteData.Filters;
using SatelliteData.Info;
using TMPro;
using UnityEngine;

namespace UI.SatelliteFilter
{
    /// <summary> Контроллер фильтрации спутников по названию. </summary>
    public class SatelliteFilterName : MonoBehaviour
    {
        /// <summary> Поле ввода для поиска по названию спутника. </summary>
        [SerializeField] private TMP_InputField inputField;

        public static Action<string> OnNameFilterApply;

        /// <summary> Контроллер информации о спутниках (Singleton). </summary>
        private SatelliteInfoController _satelliteInfoController;
        
        /// <summary> Текущий активный фильтр по названию. </summary>
        private SatelliteHideFilter _satelliteHideFilter;

        /// <summary> Получить объект контроллера спутников. </summary>
        private void Start()
        {
            _satelliteInfoController = SatelliteInfoController.Singleton;
        }

        /// <summary> Применяет фильтрацию по введенному названию. </summary>
        /// <remarks>  Фильтр активируется только при вводе 3+ символов.
        /// Удаляет предыдущий фильтр перед установкой нового. </remarks>
        public void ApplySearch()
        {
            // Удаляем предыдущий фильтр, если был установлен
            if (_satelliteHideFilter != null)
                _satelliteInfoController.SatelliteHideFilters -= _satelliteHideFilter;

            // Применяем новый фильтр только если введено 3+ символа
            if (inputField.text.Length >= 3)
            {
                _satelliteHideFilter = new SatelliteHideFilter(nameFilter: inputField.text);
                _satelliteInfoController.SatelliteHideFilters &= _satelliteHideFilter;
            }
            
            OnNameFilterApply?.Invoke(inputField.text);
            _satelliteInfoController.TryApplySatelliteFilter();
        }
    }   
}