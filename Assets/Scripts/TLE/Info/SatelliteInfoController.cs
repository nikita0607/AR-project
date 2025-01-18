using System;
using UnityEngine;

public class SatelliteInfoController : MonoBehaviour
{
    [SerializeField] private SatelliteInfo satelliteInfo;
    
    public Action<SatelliteHideFilter> HideSatellites;
    public static SatelliteInfoController Singleton;
    
    public void ShowAllSatellites() {
        HideSatellites(new SatelliteHideFilter());   
    }

    private void Start()
    {
        if (Singleton)
            Debug.LogWarning("More than one SatelliteInfoController found!");
        else
            Singleton = this;
    }

    void Update()
    {
        var fil = new SatelliteCompositeHideFilter();
        fil &= new SatelliteHideFilter(nameFilter: "Test");
        fil -= new SatelliteHideFilter(nameFilter: "Test");
        Debug.Log(fil);
        
        if(Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                if(hit.collider.tag == "satellite")
                {
                    Satellite satellite = hit.collider.gameObject.GetComponent<Satellite>();
                    SatelliteHideFilter filter = new SatelliteHideFilter(nameFilter: satellite.name);
                    HideSatellites(filter);
                    satelliteInfo.SetInfo(satellite);
                    satelliteInfo.Show();
                    return;
                }
            }
        }
    }
}
