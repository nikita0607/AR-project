using UnityEngine;

public class EciPositionable : MonoBehaviour
{
    public void SetPosition(float x, float y, float z) {
        SetPosition(new Vector3(x, y, z));
    }

    public void SetPosition(Vector3 position) {
        transform.localPosition = position;
    }

    public static Vector3 FromLongLat(float longitude, float latitude, float radius) {
       
        float omega = Mathf.PI/2 - Mathf.Deg2Rad * latitude;
        float phi = longitude * Mathf.Deg2Rad;

        float x = radius*Mathf.Sin(omega)*Mathf.Cos(phi);
        float y = radius*Mathf.Sin(omega)*Mathf.Sin(phi);
        float z = radius*Mathf.Cos(omega);
        
        return new Vector3(x, z, -y);
    }
}
