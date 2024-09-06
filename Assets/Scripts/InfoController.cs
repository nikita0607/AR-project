using UnityEngine;

public class InfoController : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {   
        gameObject.SetActive(false);
    }

    new public void SendMessage(string message) {
        gameObject.SetActive(false);
    }

}
