using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

using One_Sgp4;
using System.Linq;

public class test_tle : MonoBehaviour
{
    [SerializeField] private GameObject _defaultSatellite;

    [SerializeField] private GameObject satelliteParrent;
    [SerializeField] private GameObject[] _satellitePrefabList;
    void Start()
    {
        List<Tle> tleList = ParserTLE.ParseFile("Assets/Resources/TLE_data/data.txt");

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
        }
    }
}