using UnityEngine;

public class MarkGenerat : MonoBehaviour
{

    [SerializeField] private float earthUnitRadius;
    [SerializeField] private GameObject markParrent;

    [SerializeField] private GameObject mark;

    [SerializeField] public TextAsset jsonFile;



    private MarksSer marksFromJson;

    void Start()
    {
        marksFromJson = JsonUtility.FromJson<MarksSer>(jsonFile.text);
        CreateMark();
    }

    // Update is called once per frame
    void CreateMark() {
        for (int i = 0; i < marksFromJson.marks.Length; i++)
        {
            MarkSer markInfo = marksFromJson.marks[i];

            GameObject newMark = Instantiate(mark);
            Mark newMarkComponent = newMark.GetComponent<Mark>();

            newMark.transform.SetParent(markParrent.transform);
            newMark.transform.localScale = mark.transform.localScale;
            newMark.transform.rotation = mark.transform.rotation;

            newMarkComponent.Name = marksFromJson.marks[i].name;
            newMarkComponent.Info = marksFromJson.marks[i].info;
            newMarkComponent.SetPosition(EciPositionable.FromLongLat(markInfo.longitude, markInfo.latitude, earthUnitRadius));

            // Debug.Log(newMark.name);
        }

       // Destroy(mark);
    }
}
