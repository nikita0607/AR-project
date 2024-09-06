using System.Collections.Generic;
using UnityEngine;
using System;

using One_Sgp4;

public class Satellite : MonoBehaviour
{
    [SerializeField] public String Name;

    public Tle TLE { get; set; }
    private float timeToUpdate = 5.0f;
    private float timeSinceLastUpdate = 0.0f;

    private void Start()
    {
        transform.rotation = Quaternion.identity;
        gameObject.name = TLE.getName();
    }

    private void Update()
    {
        if (TLE == null) Destroy(gameObject);

        timeSinceLastUpdate += Time.deltaTime;
        if (timeSinceLastUpdate >= timeToUpdate)
        {
            SetPosition();
            timeSinceLastUpdate = 0.0f;
        }
    }

    public void SetPosition()
    {
        EpochTime time = new EpochTime(DateTime.UtcNow);
        Sgp4Data satellitePos = SatFunctions.getSatPositionAtTime(TLE, time, Sgp4.wgsConstant.WGS_84);
        Point3d pos = satellitePos.getPositionData();

        float radiusEarth = 6371f;
        float radiusModel = 2.2f;

        Vector3 newPos = new Vector3(-(float)pos.y / radiusEarth * radiusModel,
                                        (float)pos.z / radiusEarth * radiusModel,
                                        (float)pos.x / radiusEarth * radiusModel);

        transform.localPosition = newPos;
    }
}
