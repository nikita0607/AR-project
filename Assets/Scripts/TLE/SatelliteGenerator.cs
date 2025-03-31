using System;
using System.Collections.Generic;
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

    private void Awake() {
        _infoController = GetComponent<SatelliteInfoController>();
    }

    private void Start()
    {
        StartCoroutine(TleLoader.GetTle(ProcessTleData));
    }

    private void ProcessTleData(List<TLE.Tle> tleList)
    {
        Debug.Log($"Загружено {tleList.Count} спутников");
        int i = 0;
        foreach (var tle in tleList)
            CreateSatellite(tle);
    }

    private void CreateSatellite(TLE.Tle tleData)
    {
        try
        {
            Tle parsedTle = ParserTLE.parseTle(tleData.Line1, tleData.Line2, tleData.Name);
            GameObject prefab = defaultSatellite;
            foreach (GameObject pref in satellitePrefabList)
            {
                if (pref.GetComponent<Satellite>().Name == parsedTle.getName())
                {
                    prefab = pref;
                    break;
                }
            }
        
            GameObject newSatellite = Instantiate(prefab, parent: satelliteParrent.transform);
            Satellite newSatelliteComponent = newSatellite.GetComponent<Satellite>();
        
            newSatellite.SetActive(true);
            newSatelliteComponent.TLE = parsedTle;
            newSatelliteComponent.Name = parsedTle.getName();
            newSatellite.transform.localScale = prefab.transform.localScale;

            _infoController.HideSatellites += newSatelliteComponent.OnHide;
        }
        catch (Exception e)
        {
            Debug.LogError($"Satellite {tleData.Name}: " + e.Message);
        }
    }
}