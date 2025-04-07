using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using One_Sgp4;
using SatelliteData.Filters;
using SatelliteData.Info;
using UI;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace SatelliteData
{
    /// <summary> Генератор спутников на основе TLE-данных. </summary>
    public class SatelliteGenerator : MonoBehaviour
    {
        /// <summary> Префаб спутника по умолчанию. </summary>
        [SerializeField] private GameObject[] defaultSatellites;
        
        /// <summary> Родительский объект для всех создаваемых спутников. </summary>
        [SerializeField] private GameObject satelliteParrent;
        
        /// <summary> Список префабов специальных спутников. </summary>
        [SerializeField] private GameObject[] specificSatellitePrefabList;
        
        /// <summary> Флаг использования загрузки из интернета. </summary>
        [SerializeField] private bool useWebDownloading = true;
        
        /// <summary> Источники TLE-данных. </summary>
        [SerializeField] private TleSource[] tleSources;
        
        /// <summary> Контроллер UI для отображения процесса загрузки. </summary>
        [SerializeField] private DownloadController uiController;
        
        private SatelliteInfoController _infoController;

        /// <summary> Получает необходимые компоненты. </summary>
        private void Awake()
        {
            _infoController = GetComponent<SatelliteInfoController>();
        }

        /// <summary> Запускает корутину. </summary>
        private void Start()
        {
            StartCoroutine(LoadAllTleData());
        }

        /// <summary> Загружает все TLE-данные из указанных источников. </summary>
        private IEnumerator LoadAllTleData()
        {
            bool isSomethingLoaded = false;

            foreach (var source in tleSources)
            {
                uiController.ChangeText(source.SatelliteType.ToString());

                yield return TleLoader.GetTle(
                    source.Url,
                    source.SatelliteType,
                    tleList =>
                    {
                        isSomethingLoaded = true;
                        ProcessTleData(tleList, source);
                    },
                    error => Debug.LogError(error), useWebDownloading
                );
            }

            yield return null;

            if (isSomethingLoaded)
                uiController.OnStopDownload();
            else
                uiController.OnDownloadError();
        }

        /// <summary> Обрабатывает загруженные TLE-данные. </summary>
        /// <param name="tleList"> Список загруженных TLE-данных. </param>
        /// <param name="source"> Источник данных. </param>
        private void ProcessTleData(List<LoadedTle> tleList, TleSource source)
        {
            foreach (var tle in tleList)
                CreateSatellite(tle, source);
        }

        /// <summary> Создает объект спутника на основе TLE-данных. </summary>
        /// <param name="loadedTleData"> Загруженные TLE-данные. </param>
        /// <param name="source"> Источник данных. </param>
        private void CreateSatellite(LoadedTle loadedTleData, TleSource source)
        {
            try
            {
                if (source.SatelliteType != SatelliteType.SPECIFIC) 
                    CreateStandardSatellite(loadedTleData, source);
                else 
                    CreateSpecificSatellite(loadedTleData);
            }
            catch (Exception e)
            {
                Debug.LogError($"Satellite {loadedTleData.Name}: " + e.Message);
            }
        }

        /// <summary> Создает стандартный спутник. </summary>
        private void CreateStandardSatellite(LoadedTle loadedTleData, TleSource source)
        {
            Tle parsedLoadedTle = ParserTLE.parseTle(loadedTleData.Line1, loadedTleData.Line2, loadedTleData.Name);

            GameObject prefab;
            if (source.Prefab == null)
                prefab = defaultSatellites[Random.Range(0, defaultSatellites.Length)];
            else
                prefab = source.Prefab;
            
            GameObject newSatellite = Instantiate(prefab, parent: satelliteParrent.transform);
            Satellite newSatelliteComponent = newSatellite.GetComponent<Satellite>();
            newSatellite.SetActive(false);

            ConfigureSatellite(newSatellite, newSatelliteComponent, parsedLoadedTle, source.SatelliteType, prefab);
        }

        /// <summary> Создает специальный спутник. </summary>
        private void CreateSpecificSatellite(LoadedTle loadedTleData)
        {
            foreach (var specificSatellitePrefab in specificSatellitePrefabList)
            {
                if (specificSatellitePrefab.GetComponent<Satellite>().Name != loadedTleData.Name) continue;

                Tle parsedLoadedTle = ParserTLE.parseTle(loadedTleData.Line1, loadedTleData.Line2, loadedTleData.Name);
                GameObject newSatellite = Instantiate(specificSatellitePrefab, parent: satelliteParrent.transform);
                Satellite newSatelliteComponent = newSatellite.GetComponent<Satellite>();
                newSatellite.SetActive(false);

                ConfigureSatellite(newSatellite, newSatelliteComponent, parsedLoadedTle, SatelliteType.SPECIFIC, specificSatellitePrefab);
            }
        }

        /// <summary> Настраивает параметры спутника. </summary>
        private void ConfigureSatellite(GameObject satelliteObj, Satellite satelliteComponent, Tle tleData, 
            SatelliteType type, GameObject prefab)
        {
            satelliteComponent.Name = tleData.getName();
            satelliteComponent.LoadedTle = tleData;
            satelliteComponent.SatelliteType = type;
            satelliteComponent.Parent = satelliteParrent;
            satelliteObj.transform.localScale = prefab.transform.localScale;
            _infoController.HideSatellites += satelliteComponent.OnHide;
        }

        /// <summary> Класс-контейнер для источника TLE-данных. </summary>
        [Serializable]
        public class TleSource
        {
            /// <summary> URL для загрузки TLE-данных. </summary>
            public string Url;
            
            /// <summary> Префаб для создания спутников этого типа. </summary>
            public GameObject Prefab;
            
            /// <summary> Тип спутников. </summary>
            public SatelliteType SatelliteType;

            /// <summary> Создает новый источник TLE-данных. </summary>
            public TleSource(string url, GameObject prefab, SatelliteType satelliteType)
            {
                Url = url;
                Prefab = prefab;
                SatelliteType = satelliteType;
            }
        }
    }
}