using System.Collections.Generic;
using UnityEngine;

using One_Sgp4;
using UnityEditor;
using System.Linq;
using System;

public class SatelliteGenerator : MonoBehaviour
{
    [SerializeField] private GameObject _defaultSatellite;

    [SerializeField] private GameObject satelliteParrent;
    [SerializeField] private GameObject[] _satellitePrefabList;
    [SerializeField] private TextAsset tleFile;
    
    private SatelliteInfoController _infoConroller;

    private void Awake() {
        _infoConroller = GetComponent<SatelliteInfoController>();
    }

    void Start()
    {
        List<string> tleStrings = tleFile.text.Split("\n").Select(x => x.Trim()).ToList();

        List<Tle> tleList = Enumerable.Range(0, tleStrings.Count/3).Select(i => {
            return ParserTLE.parseTle(tleStrings[i*3+1], tleStrings[i*3+2], tleStrings[i*3]);
        }).ToList();

        foreach (Tle tle in tleList)
        {

            GameObject prefab = _defaultSatellite;

            foreach (GameObject pref in _satellitePrefabList)
            {
                Debug.Log(pref.GetComponent<Satellite>().Name + " " + tle.getName());
                if (pref.GetComponent<Satellite>().Name == tle.getName())
                {
                    prefab = pref;
                    break;
                }
            }

            GameObject newSatellite = Instantiate(prefab);
            newSatellite.SetActive(true);
            Satellite newSatelliteComponent = newSatellite.GetComponent<Satellite>();

            newSatelliteComponent.TLE = tle;
            newSatellite.transform.SetParent(satelliteParrent.transform);
            newSatellite.transform.localScale = prefab.transform.localScale;

            _infoConroller.HideSatellites += newSatellite.GetComponent<Satellite>().OnHide;
        }
    }
}