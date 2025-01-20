using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SatellitesSer
{
    public SatelliteSer[] satellites;
    private static Dictionary<string, SatelliteSer> _satellites = new Dictionary<string, SatelliteSer>();

    public static Dictionary<string, SatelliteSer> GetSatellitesSer(TextAsset textAsset) {
        if (_satellites.Count == 0) {
            SatellitesSer satells = JsonUtility.FromJson<SatellitesSer>(textAsset.text);
            foreach (var sat in satells.satellites) {
                _satellites.Add(sat.name, sat);
            }
        }

        return _satellites;
    }
}
