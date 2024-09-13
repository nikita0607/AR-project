using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using One_Sgp4;

public class Satellite : EciPositionable
{
    [SerializeField] public String Name;
    [SerializeField] public GameObject sl;
    public Tle TLE { get; set; }
    public EpochTime timeForSatellite {get; set; }
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
        UpdatePosition();
    }

    public Coordinate GetPosition(EpochTime time = null) {
        if (time is null) time = timeForSatellite;

        try
        {
            Sgp4Data satellitePos = SatFunctions.getSatPositionAtTime(TLE, time, Sgp4.wgsConstant.WGS_84);
            return SatFunctions.calcSatSubPoint(time, satellitePos, Sgp4.wgsConstant.WGS_84);
        }
        catch (ArgumentException)
        {
            Debug.Log("Satellite " + name + " has problems" );
            return new Coordinate(0, 0);
        }
    }

    public void OnHide(SatelliteHideFilter filter) {
        if (filter.ShouldShowSatellite(this)) {
            gameObject.SetActive(true);
        } else {
            gameObject.SetActive(false);
        }
    }

    public int GetMinutesForNextPass() {
        EpochTime time = timeForSatellite;
        time.addMinutes(5);

        List<Pass> passes = SatFunctions.CalculatePasses(GetPosition(), TLE, time);
        Pass p = passes[1];
        return (int)(p.getStartEpoch().toDateTime()-timeForSatellite.toDateTime()).TotalMinutes;
    }

    public void UpdatePosition()
    {
        Coordinate cords = GetPosition();
        Vector3 newPos = FromLongLat(-(float)cords.getLongitude(), (float)cords.getLatitude(), 2.1f);

        SetPosition(newPos);

        // if (TLE.getName() == "ISS (ZARYA)")
        //     Debug.Log(cords.getLatitude() + " " + cords.getLongitude() + " " + cords.getHeight());


        // float radiusEarth = 6371f;
        // float radiusModel = 2.2f;

        // Vector3 newPos = new Vector3(-(float)pos.y / radiusEarth * radiusModel,
        //                                 (float)pos.z / radiusEarth * radiusModel,
        //                                 (float)pos.x / radiusEarth * radiusModel);

        // transform.localPosition = newPos;
    }

    // public void ValueChangeCheck()
    // {
    //     time = new EpochTime(DateTime.UtcNow.AddHours(slider.value));
    //     SetPosition();
    // }
}
