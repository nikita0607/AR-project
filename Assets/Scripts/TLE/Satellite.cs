using System.Collections.Generic;
using UnityEngine;
using System;

using One_Sgp4;
using Unity.VisualScripting;

public class Satellite : MonoBehaviour
{
    public Tle TLE { get; set; }
    private float scale = 0.1f;
    private float timeToUpdate = 5.0f;
    private float timeSinceLastUpdate = 0.0f;

    private void Start()
    {
        transform.rotation = Quaternion.identity;
        transform.localScale = new Vector3(scale, scale, scale);

        gameObject.name = TLE.getName();

        if (TLE.getName() == "ISS (ZARYA)")
        {
            gameObject.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        }
    }

    private void Update()
    {
        timeSinceLastUpdate += Time.deltaTime;
        if (timeSinceLastUpdate >= timeToUpdate)
        {
            Debug.Log("Update");
            if (TLE != null)
                SetPosition();
            else
                Destroy(gameObject);
            timeSinceLastUpdate = 0.0f;
        }
    }

    public void SetPosition()
    {
        EpochTime time = new EpochTime(DateTime.UtcNow);
        Sgp4Data satellitePos = One_Sgp4.SatFunctions.getSatPositionAtTime(TLE, time, Sgp4.wgsConstant.WGS_84);
        Point3d pos = satellitePos.getPositionData();

        float radiusEarth = 6371f;
        float radiusModel = 2.1f;

        Vector3 newPos = new Vector3(-(float)pos.y / radiusEarth * radiusModel,
                                        (float)pos.x / radiusEarth * radiusModel,
                                        (float)pos.z / radiusEarth * radiusModel);

        if (gameObject.name == "ISS (ZARYA)")
        {
            Debug.Log(newPos);
        }

        transform.localPosition = newPos;
    }
}
