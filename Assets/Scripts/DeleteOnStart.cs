using UnityEngine;

public class DeleteOnStart : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Destroy(this.gameObject, 0.1f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
