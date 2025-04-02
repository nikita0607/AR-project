using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using One_Sgp4;
using TLE;
using Tle = One_Sgp4.Tle;

public class SatelliteGenerator : MonoBehaviour
{
    [SerializeField] private GameObject defaultSatellite;

    [SerializeField] private GameObject satelliteParrent;
    [SerializeField] private GameObject[] satellitePrefabList;
    [SerializeField] private TextAsset tleFile;
    
    private SatelliteInfoController _infoController;
    
    [SerializeField] private TleSource[] tleSources; // Новое поле для источников
    private Dictionary<string, GameObject> urlToPrefabs = new();

    private void Awake() {
        _infoController = GetComponent<SatelliteInfoController>();
    }

    private void Start()
    {
        urlToPrefabs = tleSources.ToDictionary(source => source.url, source => source.prefab);
        StartCoroutine(LoadAllTleData());
    }
    
    private IEnumerator LoadAllTleData()
    {
        foreach(var source in tleSources)
        {
            Debug.Log(source.url);
            yield return TleLoader.GetTle(
                source.url,
                tleList => ProcessTleData(tleList, source.prefab),
                error => Debug.LogError(error)
            );
        }
    }

    private void ProcessTleData(List<TLE.Tle> tleList, GameObject prefab)
    {
        foreach(var tle in tleList)
            CreateSatellite(tle, prefab);
    }

    private void CreateSatellite(TLE.Tle tleData, GameObject prefab)
    {
        try
        {
            Tle parsedTle = ParserTLE.parseTle(tleData.Line1, tleData.Line2, tleData.Name);
            
            GameObject newPrefab = prefab;
            foreach (GameObject pref in satellitePrefabList)
            {
                if (pref.GetComponent<Satellite>().Name == parsedTle.getName())
                {
                    newPrefab = pref;
                    break;
                }
            }
            
            GameObject newSatellite = Instantiate(newPrefab, parent: satelliteParrent.transform);
            Satellite newSatelliteComponent = newSatellite.GetComponent<Satellite>();
            newSatellite.SetActive(true);
            
            newSatelliteComponent.Name = parsedTle.getName();
            newSatelliteComponent.TLE = parsedTle;
            newSatellite.transform.localScale = newPrefab.transform.localScale;
            
            _infoController.HideSatellites += newSatelliteComponent.OnHide;
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
    }
}