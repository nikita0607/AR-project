using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ShowMarkinfoOnClick : MonoBehaviour
{
    [SerializeField] private TMP_Text _textField;
    [SerializeField] private GameObject _infoPanel;


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
                    _textField.text = hit.collider.gameObject.GetComponent<Mark>().Info;
                    return;
                }
            }
        }
    }


}
