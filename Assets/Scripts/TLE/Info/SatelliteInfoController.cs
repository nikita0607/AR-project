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

        var fil = new SatelliteCompositeHideFilter();
        fil &= new SatelliteHideFilter(nameFilter: "HUI");
        fil -= new SatelliteHideFilter(nameFilter: "HUI");
        
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
                    _satelliteInfo.SetInfo(satellite);
                    _satelliteInfo.Show();
                    return;
                }
            }
        }
    }
}
