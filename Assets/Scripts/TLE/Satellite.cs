using UnityEngine;
using System;

using One_Sgp4;

public class Satellite : MonoBehaviour
{
    [SerializeField] public String Name;
    [SerializeField] public GameObject sl;
    public Tle TLE { get; set; }
    private EpochTime timeForSatellite;
    private TimeManager timeManager;

    private void Start()
    {
        transform.rotation = Quaternion.identity;
        gameObject.name = TLE.getName();

        timeManager = transform.parent.GetComponent<TimeManager>();
        timeForSatellite = timeManager.GetTime();

        // slider = GameObject.Find("Slider").GetComponent<Slider>();
        // slider.onValueChanged.AddListener(delegate { ValueChangeCheck(); });
    }

    private void Update()
    {
        if (TLE == null) Destroy(gameObject);

        timeForSatellite = timeManager.GetTime();
        SetPosition();
    }

    public void SetPosition()
    {
        Sgp4Data satellitePos = SatFunctions.getSatPositionAtTime(TLE, timeForSatellite, Sgp4.wgsConstant.WGS_84);
        Point3d pos = satellitePos.getPositionData();
        Coordinate cords = SatFunctions.calcSatSubPoint(timeForSatellite, satellitePos, Sgp4.wgsConstant.WGS_84);

        if (TLE.getName() == "ISS (ZARYA)")
            Debug.Log(cords.getLatitude() + " " + cords.getLongitude() + " " + cords.getHeight());


        float radiusEarth = 6371f;
        float radiusModel = 2.2f;

        Vector3 newPos = new Vector3(-(float)pos.y / radiusEarth * radiusModel,
                                        (float)pos.z / radiusEarth * radiusModel,
                                        (float)pos.x / radiusEarth * radiusModel);

        transform.localPosition = newPos;
    }

    // public void ValueChangeCheck()
    // {
    //     time = new EpochTime(DateTime.UtcNow.AddHours(slider.value));
    //     SetPosition();
    // }
}
