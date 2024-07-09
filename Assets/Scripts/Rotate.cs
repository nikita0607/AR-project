using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    void SendMessage(string message) {
        Debug.Log(message);
        transform.Rotate(10, 0, 0, Space.Self);
    }
}
