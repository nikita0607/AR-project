using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using One_Sgp4;
using TLE;
using TLE.Filters;
using UI;
using Tle = One_Sgp4.Tle;

public class SatelliteGenerator : MonoBehaviour
{
    [SerializeField] private GameObject defaultSatellite;

    [SerializeField] private GameObject satelliteParrent;
    [SerializeField] private GameObject[] specificSatellitePrefabList;
    [SerializeField] private bool _useWebDownloading = true;
    // [SerializeField] private TextAsset tleFile;
    
    private SatelliteInfoController _infoController;
    
    [SerializeField] private TleSource[] tleSources;
    [SerializeField] private DownloadController uiController;

    private void Awake() 
    {
        _infoController = GetComponent<SatelliteInfoController>();
    }

    private void Start()
    {
        StartCoroutine(LoadAllTleData());
    }
    
    private IEnumerator LoadAllTleData()
    {
        bool isSomethingLoaded = false;
        
        foreach(var source in tleSources)
        {
            Debug.Log(source.url + " " + source.satelliteType);
            uiController.ChangeText(source.satelliteType.ToString());
            
            yield return TleLoader.GetTle(
                source.url,
                source.satelliteType,
                tleList =>
                {
                    isSomethingLoaded = true;
                    ProcessTleData(tleList, source);
                },
                error => Debug.LogError(error), _useWebDownloading
            );
        }
        
        yield return null;
        
        if (isSomethingLoaded)
            uiController.OnStopDownload();
        else
            uiController.OnDownloadError();
    }

    private void ProcessTleData(List<TLE.Tle> tleList, TleSource source)
    {
        foreach(var tle in tleList)
            CreateSatellite(tle, source);
    }

    private void CreateSatellite(TLE.Tle tleData, TleSource source)
    {
        try
        {
            if (source.satelliteType != SatelliteType.SPECIFIC)
            {
                Tle parsedTle = ParserTLE.parseTle(tleData.Line1, tleData.Line2, tleData.Name);
            
                GameObject newPrefab = source.prefab;
            
                GameObject newSatellite = Instantiate(newPrefab, parent: satelliteParrent.transform);
                Satellite newSatelliteComponent = newSatellite.GetComponent<Satellite>();
                newSatellite.SetActive(false);
            
                newSatelliteComponent.Name = parsedTle.getName();
                newSatelliteComponent.Tle = parsedTle;
                newSatelliteComponent.SatelliteType = source.satelliteType;
                newSatellite.transform.localScale = newPrefab.transform.localScale;
            
                _infoController.HideSatellites += newSatelliteComponent.OnHide;
            }
            else
            {
                foreach (var specificSatellitePrefab in specificSatellitePrefabList)
                {
                    if (specificSatellitePrefab.GetComponent<Satellite>().Name != tleData.Name) continue;

                    Debug.Log(tleData.Name);
                    
                    Tle parsedTle = ParserTLE.parseTle(tleData.Line1, tleData.Line2, tleData.Name);
            
                    GameObject newPrefab = specificSatellitePrefab;
            
                    GameObject newSatellite = Instantiate(newPrefab, parent: satelliteParrent.transform);
                    Satellite newSatelliteComponent = newSatellite.GetComponent<Satellite>();
                    newSatellite.SetActive(false);
            
                    newSatelliteComponent.Name = parsedTle.getName();
                    newSatelliteComponent.Tle = parsedTle;
                    newSatelliteComponent.SatelliteType = source.satelliteType;
                    newSatellite.transform.localScale = newPrefab.transform.localScale;
            
                    _infoController.HideSatellites += newSatelliteComponent.OnHide;
                }
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"Satellite {tleData.Name}: " + e.Message);
        }
    }
    
    [Serializable]
    public class TleSource
    {
        public string url;
        public GameObject prefab;
        public SatelliteType satelliteType;

        public TleSource(string url, GameObject prefab, SatelliteType satelliteType)
        {
            this.url = url;
            this.prefab = prefab;
            this.satelliteType = satelliteType;
        }
    }
}