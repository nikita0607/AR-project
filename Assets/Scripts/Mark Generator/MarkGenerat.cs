using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MarkGenerat : MonoBehaviour
{
    [SerializeField]
    private GameObject targeModel;

    [SerializeField]
    private GameObject mark;

    [SerializeField]
    public TextAsset jsonFile;
    // Start is called before the first frame update

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
            newMark.name = marksFromJson.marks[i].name;
            newMark.GetComponent<Mark>().SetPosition(markInfo.longitude, markInfo.latitude);
            newMark.transform.SetParent(targeModel.transform);
            Debug.Log(newMark.name);
        }

        Destroy(mark);
    }
}
