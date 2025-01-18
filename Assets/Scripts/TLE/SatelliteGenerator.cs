using System.Collections.Generic;
using UnityEngine;

using One_Sgp4;
using UnityEditor;
using System.Linq;
using System;
using UnityEngine.Serialization;

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

    void Start()
    {
        List<string> tleStrings = tleFile.text.Split("\n").Select(x => x.Trim()).ToList();

        List<Tle> tleList = Enumerable.Range(0, tleStrings.Count/3).Select(i => {
            return ParserTLE.parseTle(tleStrings[i*3+1], tleStrings[i*3+2], tleStrings[i*3]);
        }).ToList();

        foreach (Tle tle in tleList)
        {

            GameObject prefab = defaultSatellite;

            foreach (GameObject pref in satellitePrefabList)
            {
                Debug.Log(pref.GetComponent<Satellite>().Name + " " + tle.getName());
                if (pref.GetComponent<Satellite>().Name == tle.getName())
                {
                    prefab = pref;
                    break;
                }
            }

            GameObject newSatellite = Instantiate(prefab, parent: satelliteParrent.transform);
            Satellite newSatelliteComponent = newSatellite.GetComponent<Satellite>();
            
            newSatellite.SetActive(true);

            newSatelliteComponent.TLE = tle;
            newSatelliteComponent.Name = tle.getName();
            
            newSatellite.transform.localScale = prefab.transform.localScale;

            _infoController.HideSatellites += newSatellite.GetComponent<Satellite>().OnHide;
        }
    }
}