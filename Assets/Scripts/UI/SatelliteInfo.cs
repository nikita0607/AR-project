using System;
using One_Sgp4;
using SatelliteData;
using SatelliteData.Info;
using TMPro;
using UnityEngine;
using Utilities;

namespace UI
{
    /// <summary> Контроллер отображения информации о спутнике. </summary>
    public class SatelliteInfo : MonoBehaviour
    {
        /// <summary> Событие, вызываемое при скрытии панели информации. </summary>
        public Action OnHide;

        /// <summary> Текстовое поле для отображения информации о спутнике. </summary>
        [SerializeField] private TMP_Text textField;
        
        /// <summary> Родительский объект для отрисовки линии траектории. </summary>
        [SerializeField] private GameObject lineRedererParrent;
        
        /// <summary> Префаб для визуализации точек траектории. </summary>
        [SerializeField] private GameObject trailPrefab;
        
        /// <summary> JSON-файл с дополнительной информацией о спутниках. </summary>
        [SerializeField] private TextAsset satteliteJson;

        /// <summary> Действие, выполняемое при скрытии (очистка траектории). </summary>
        private Action _onHide;
        
        /// <summary> Текущий отображаемый спутник. </summary>
        private Satellite _currentSatellite;

        /// <summary> Подписывается на событие изменения времени при активации. </summary>
        private void OnEnable()
        {
            TimeManager.Instance.OnMinuteChanged += UpdateInfo;
        }

        /// <summary> Отписывается от события изменения времени при деактивации. </summary>
        private void OnDisable()
        {
            TimeManager.Instance.OnMinuteChanged -= UpdateInfo;
        }

        /// <summary> Устанавливает информацию о спутнике и строит его траекторию. </summary>
        /// <param name="satellite"> Целевой спутник для отображения. </param>
        public void SetInfo(Satellite satellite)
        {
            _currentSatellite = satellite;

            Coordinate cords = satellite.GetPosition();
            string text = $"  Название: {satellite.LoadedTle.getName()}\n" +
                         $"  NORAD-ID: {satellite.LoadedTle.getNoradID()}\n" +
                         $"  Координаты:\n\tширота: {cords.getLatitude():f2}\n\tдолгота: {cords.getLongitude():f2}\n \n";

            if (SatellitesSerializer.GetSatellitesSer(satteliteJson).ContainsKey(satellite.Name))
                text += $"{SatellitesSerializer.GetSatellitesSer(satteliteJson)[satellite.gameObject.name].info}\n";

            textField.text = text;
            BuildSatelliteTrail(satellite);
        }

        /// <summary> Строит траекторию движения спутника на 94 минуты вперед. </summary>
        private void BuildSatelliteTrail(Satellite satellite)
        {
            satellite.TimeForSatellite = new EpochTime(TimeManager.Instance.GetTime());

            for (int i = 0; i < 94; i++)
            {
                Coordinate satCords = satellite.GetPosition();
                CreateTrailPoint(satCords, satellite);
                satellite.TimeForSatellite.addMinutes(1);
                satellite.UpdatePosition();
            }

            satellite.TimeForSatellite = TimeManager.Instance.GetTime();
        }

        /// <summary> Создает точку траектории спутника. </summary>
        private void CreateTrailPoint(Coordinate coords, Satellite satellite)
        {
            GameObject trail = Instantiate(trailPrefab, Vector3.zero, Quaternion.identity, lineRedererParrent.transform);
            trail.transform.localPosition = EciPositionable.FromLongLat(
                -(float)coords.getLongitude(),
                (float)coords.getLatitude(),
                EarthParametrs.VirtualEarthRadius + satellite.GetHeight());
            _onHide += () => Destroy(trail);
        }

        /// <summary> Обновляет информацию при изменении времени. </summary>
        private void UpdateInfo()
        {
            if (!_currentSatellite) return;
            _onHide?.Invoke();
            _onHide = null;
            SetInfo(_currentSatellite);
            Show();
        }

        /// <summary> Активирует панель информации. </summary>
        public void Show() => gameObject.SetActive(true);

        /// <summary> Деактивирует панель и очищает ресурсы. </summary>
        public void Hide()
        {
            gameObject.SetActive(false);
            _currentSatellite = null;
            _onHide?.Invoke();
            _onHide = null;
            OnHide?.Invoke();
        }
    }
}