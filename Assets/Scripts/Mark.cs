using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mark : MonoBehaviour
{
    // Start is called before the first frame update
    public string Name { get; set; }
    public string Info { get; set; }

    private float radius = 2.2f;

    void Start()
    {
        
    }

    // Update is called once per frame

    public void SetPosition(float longitude, float latitude) {
        // transform.Rotate(-latitude/10f*6f, longitude, latitude/10f*6f, Space.Self);
        float omega = Mathf.PI/2 - Mathf.Deg2Rad * latitude;
        float phi = longitude * Mathf.Deg2Rad;

        Debug.Log("Name " + Name);
        Debug.Log(""+omega+" " + phi );

        float x = radius*Mathf.Sin(omega)*Mathf.Cos(phi);
        float z = -radius*Mathf.Sin(omega)*Mathf.Sin(phi);
        float y = radius*Mathf.Cos(omega);
        Debug.Log(x + " " + z + " " + y);
        transform.localPosition = new Vector3(x, y, z);
    }
}
