using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : EciPositionable
{
    [SerializeField] private float longat;
    [SerializeField] private float lat;

    // Update is called once per frame
    void Update()
    {
        SetPosition(FromLongLat(longat, lat, 2.2f));
    }
}
