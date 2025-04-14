using System;
using SatelliteData.Filters;
using UI;
using UnityEngine;
using UnityEngine.EventSystems;

namespace SatelliteData.Info
{
    /// <summary> Контроллер для управления информацией о спутниках и их фильтрацией. </summary>
    public class SatelliteInfoController : MonoBehaviour
    {
        [SerializeField] private SatelliteInfo satelliteInfo;

        /// <summary> Событие для скрытия спутников по указанному фильтру. </summary>
        public Action<IFilter> HideSatellites;

        /// <summary> Статический экземпляр для реализации Singleton. </summary>
        public static SatelliteInfoController Singleton;

        /// <summary> Составной фильтр для скрытия спутников. </summary>
        public SatelliteCompositeHideFilter SatelliteHideFilters;

        /// <summary> Флаг, определяющий возможность применения фильтра. </summary>
        private bool _canApplyFilter = true;

        /// <summary> Ссылка на главную камеру для обработки ввода. </summary>
        private Camera _camera;

        /// <summary> Подписывается на событие скрытия при активации. </summary>
        private void OnEnable() => satelliteInfo.OnHide += ApplySatelliteFilter;

        /// <summary> Отписывается от события скрытия при деактивации. </summary>
        private void OnDisable() => satelliteInfo.OnHide -= ApplySatelliteFilter;

        /// <summary> Применяет текущий фильтр спутников. </summary>
        private void ApplySatelliteFilter()
        {
            HideSatellites?.Invoke(SatelliteHideFilters);
            _canApplyFilter = true;
        }

        /// <summary> Пытается применить фильтр спутников, если это разрешено. </summary>
        /// <returns> True если фильтр был применен, иначе False. </returns>
        public void TryApplySatelliteFilter()
        {
            if (_canApplyFilter) ApplySatelliteFilter();
        }

        private void Start()
        {
            _camera = Camera.main;
            if (Singleton)
                Debug.LogWarning("More than one SatelliteInfoController found!");
            else
                Singleton = this;

            SatelliteHideFilters = new SatelliteCompositeHideFilter();
        }

        /// <summary> Обрабатывает клики по спутникам и управление фильтрами. </summary>
        private void Update()
        {
            if (_camera && Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
            {
                if (EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
                    return;
                
                Ray ray = _camera.ScreenPointToRay(Input.GetTouch(0).position);

                if (Physics.Raycast(ray, out var hit, Mathf.Infinity))
                    if (hit.collider.CompareTag("satellite"))
                    {
                        Satellite satellite = hit.collider.gameObject.GetComponent<Satellite>();
                        SatelliteHideFilter filter = new SatelliteHideFilter(noradID: satellite.LoadedTle.getNoradID());
                        HideSatellites?.Invoke(filter);
                        satelliteInfo.SetInfo(satellite);
                        satelliteInfo.Show();

                        _canApplyFilter = false;
                    }
            }
        }
    }
}