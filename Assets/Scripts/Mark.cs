using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mark : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began) {
            Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit)) {
                Debug.Log(hit.collider.gameObject.GetComponent<Mark>().name);
            }
        }
    }

    public void SetPosition(float longitude, float latitude) {
        transform.Rotate(0, longitude, latitude);
    }
}
