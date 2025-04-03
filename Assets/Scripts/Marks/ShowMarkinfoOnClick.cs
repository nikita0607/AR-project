using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UI.Marks;

public class ShowMarkinfoOnClick : MonoBehaviour
{
    [SerializeField] private TMP_Text _textField;
    [SerializeField] private GameObject _infoPanel;
    [SerializeField] private MarkImages markImage;


    void Update() {

        if(Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                if(hit.collider.tag == "mark")
                {
                    _infoPanel.SetActive(true);
                    Mark mark = hit.collider.gameObject.GetComponent<Mark>();
                    markImage.SetMarkImage(mark.Name);
                    string text = mark.Name;
                    text += "\n" + mark.Info;
                    _textField.text = text;
                }
            }
        }
    }


}
