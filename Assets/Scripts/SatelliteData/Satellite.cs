using UnityEngine;
using System;
using One_Sgp4;
using SatelliteData.Filters;
using SatelliteData.Info;
using Utilities;

namespace SatelliteData
{
    public class Satellite : GCSPositionable
    {
        /// <summary> Название спутника. </summary>
        [SerializeField] public String Name;
        
        [SerializeField] public GameObject Pointer;

        /// <summary> TLE-данные для спутника. </summary>
        public Tle LoadedTle { get; set; }

        /// <summary> Время для спутника. </summary>
        public EpochTime TimeForSatellite { get; set; }

        /// <summary> Тип спутника. </summary>
        public SatelliteType SatelliteType { get; set; }
        
        /// <summary> Родительский объект. </summary>
        public GameObject Parent { get; set; }

        /// <summary> Менеджер времени. </summary>
        private TimeManager timeManager;

        /// <summary> Позиция на экране. </summary>
        private Vector3 _screenPos;
        
        /// <summary> Камера. </summary>
        private Camera _camera;
        

        /// <summary> Задание стартовых данных. </summary>
        private void Start()
        {
            transform.rotation = Quaternion.identity;
            gameObject.name = LoadedTle.getName();

            timeManager = GameObject.FindWithTag("TimeManager").GetComponent<TimeManager>();
            TimeForSatellite = timeManager.GetTime();
            _camera = Camera.allCameras[1];
        }

        private void Update()
        {
            if (LoadedTle == null) Destroy(gameObject);

            TimeForSatellite = timeManager.GetTime();
            UpdatePosition();
            
            _screenPos = _camera.WorldToScreenPoint(transform.position);
            Pointer.transform.position = _screenPos;
        }

        private void OnEnable()
        {
            Pointer.SetActive(false);
        }

        private void OnDisable()
        {
            Pointer.SetActive(false);
        }

        /// <summary> Получить позицию. </summary>
        /// <param name="time"> Заданный момент времени. </param>
        /// <returns> Позиция спутника в заданный момент времени. </returns>
        public Coordinate GetPosition(EpochTime time = null)
        {
            if (time is null) time = TimeForSatellite;

            try
            {
                Sgp4Data satellitePos = SatFunctions.getSatPositionAtTime(LoadedTle, time, Sgp4.wgsConstant.WGS_84);
                return SatFunctions.calcSatSubPoint(time, satellitePos, Sgp4.wgsConstant.WGS_84);
            }
            catch (ArgumentException)
            {
                // Debug.Log("Satellite " + name + " has problems" );
                return new Coordinate(0, 0);
            }
        }

        /// <summary> Действия при скрытии спутника. </summary>
        /// <param name="filter"> Фильтр для показа/скрытия спутника. </param>
        public void OnHide(IFilter filter)
        {
            gameObject.SetActive(filter.ShouldShowSatellite(this));
        }

        /// <summary> При применении фильтра на имя. </summary>
        /// <param name="lookingName"> Искомое имя. </param>
        public void OnNameFilterApplied(string lookingName)
        {
            if (lookingName.Length >= 3 && Name.Contains(lookingName))
                Pointer.SetActive(true);
            else
                Pointer.SetActive(false);
        }

        /// <summary> Поулчить высоту. </summary>
        /// <returns> Текущая высота спутника, относительно виртуальной модели Земли. </returns>
        public float GetHeight() => EarthParametrs.RealToVirtualDistance((float)GetPosition().getHeight() / 4);

        /// <summary> Обновить позицию. </summary>
        public void UpdatePosition()
        {
            Coordinate cords = GetPosition();
            Vector3 newPos = FromLongLat(-(float)cords.getLongitude(), (float)cords.getLatitude(),
                EarthParametrs.VirtualEarthRadius + GetHeight());
            
            gameObject.transform.rotation =
                Quaternion.LookRotation(Parent.transform.position - gameObject.transform.position);

            SetPosition(newPos);
        }
    }
}