using System;
using System.Collections.Generic;
using System.IO;
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
    private WebParser _webParser;

    private void Awake() {
        _infoController = GetComponent<SatelliteInfoController>();
        _webParser = GetComponent<WebParser>();
    }

    private void Start()
    {
        _webParser.LoadTleData(
            tleList => {
                ProcessTleData(tleList);
            },
            error => {
                Debug.LogError(error);
                LoadFromLocalFile();
            }
        );
    }

    private void ProcessTleData(List<TLE.Tle> tleList)
    {
        Debug.Log($"Загружено {tleList.Count} спутников");
        int i = 0;
        foreach (var tle in tleList)
            CreateSatellite(tle);
    }

    private void LoadFromLocalFile()
    {
        string filePath = "Assets/Resources/TLE_data/data.txt";
        
        if(!File.Exists(filePath))
        {
            Debug.LogError("Локальный файл данных не найден");
            return;
        }

        try
        {
            string rawData = File.ReadAllText(filePath);
            
            List<string> lines = new List<string>(rawData.Split('\n'))
                .Select(line => line.Trim())
                .Where(line => !string.IsNullOrEmpty(line))
                .ToList();
            
            if(lines.Count % 3 != 0)
            {
                Debug.LogError("Некорректный формат локального файла");
                return;
            }
    
            List<TLE.Tle> tleList = Enumerable
                .Range(0, lines.Count / 3)
                .Select(i => new TLE.Tle(
                    name: lines[i * 3],
                    line1: lines[i * 3 + 1],
                    line2: lines[i * 3 + 2]))
                .ToList();
            
            ProcessTleData(tleList);
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Ошибка чтения файла: {e.Message}");
        }
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
            Debug.LogError(e.Message);
        }
    }
}