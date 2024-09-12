using System;
using UnityEngine;

public class SatelliteInfoController : MonoBehaviour
{
    [SerializeField] private SatelliteInfo _satelliteInfo;
    public Action<SatelliteHideFilter> HideSatellites;

    
    public void ShowAllSatellites() {
        HideSatellites(new SatelliteHideFilter());   
    }

    void Update()
    {
        
        if(Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                if(hit.collider.tag == "satellite")
                {
                    Satellite satellite = hit.collider.gameObject.GetComponent<Satellite>();
                    SatelliteHideFilter filter = new SatelliteHideFilter(name: satellite.name);
                    HideSatellites(filter);
                    _satelliteInfo.SetInfo(satellite);
                    _satelliteInfo.Show();
                    return;
                }
            }
        }
    }
}
