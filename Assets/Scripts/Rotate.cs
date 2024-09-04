using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour
{
    void SendMessage(string message) {
        // Debug.Log("123");
        transform.Rotate(10, 0, 0, Space.Self);
    }
}
