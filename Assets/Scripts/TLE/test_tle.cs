using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

using One_Sgp4;

public class test_tle : MonoBehaviour
{
    [SerializeField]
    private GameObject satellite;

    [SerializeField]
    private GameObject satelliteParrent;
    void Start()
    {
        List<Tle> tleList = ParserTLE.ParseFile("Assets/Resources/TLE_data/data.txt");

        foreach (Tle tle in tleList)
        {
            GameObject newSatellite = Instantiate(satellite);
            Satellite newSatelliteComponent = newSatellite.GetComponent<Satellite>();
            if (tle == null)
            {
                Debug.Log("NULL!");
            }
           
            newSatelliteComponent.TLE = tle;
            newSatellite.transform.SetParent(satelliteParrent.transform);
        }

        // //Calculate Satellite Position and Speed
        // One_Sgp4.Sgp4 sgp4Propagator = new Sgp4(tleISS, Sgp4.wgsConstant.WGS_84);
        // //set calculation parameters StartTime, EndTime and caclulation steps in minutes
        // sgp4Propagator.runSgp4Cal(startTime, stopTime, 1 / 30.0); // 1/60 => caclulate sat points every 2 seconds
        // List<One_Sgp4.Sgp4Data> resultDataList = new List<Sgp4Data>();
        // //Return Results containing satellite Position x,y,z (ECI-Coordinates in Km) and Velocity x_d, y_d, z_d (ECI-Coordinates km/s) 
        // resultDataList = sgp4Propagator.getResults();

        // startTime = new EpochTime(DateTime.Now);
        // //Coordinate of an observer on Ground lat, long, height(in meters)
        // One_Sgp4.Coordinate observer = new Coordinate(35.00, 18, 0);
        // //Convert to ECI coordinate system
        // One_Sgp4.Point3d eci = observer.toECI(0.0);
        // Debug.Log("Sidereal Time: " + startTime.getLocalSiderealTime());
        // Debug.Log(String.Format("X: {0}, Y: {1}, Z: {2} ", eci.x, eci.y, eci.z));

        // //Get Local SiderealTime for Observer
        // double localSiderealTime = startTime.getLocalSiderealTime(observer.getLongitude());

        // //Calculate if Satellite is Visible for a certain Observer on ground at certain timePoint
        // bool satelliteIsVisible = One_Sgp4.SatFunctions.isSatVisible(observer, 0.0, startTime, resultDataList[0]);

        // //Calculate Sperical Coordinates from an Observer to Satellite
        // //returns 3D-Point with range(km), azimuth(radians), elevation(radians) to the Satellite
        // One_Sgp4.Point3d spherical = One_Sgp4.SatFunctions.calcSphericalCoordinate(observer, startTime, resultDataList[0]);

    }
}