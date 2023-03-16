using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Contact : MonoBehaviour
{
    [SerializeField]
    private GameObject contactCICManager;
    private ContactManagerController controller;
    [SerializeField]
    private string[] contactinfo;
    // Start is called before the first frame update
    void Start()
    {
        contactCICManager = GameObject.Find("GameController");
        controller = contactCICManager.GetComponent<ContactManagerController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnMouseDown() {
        Debug.Log("Object clicked!");
        controller.contactPressed(contactinfo);
    }
    public void setContactValue(string[] inputs, string[] latLon){
        contactinfo = new string[6];
        contactinfo[0] = inputs[0];
        contactinfo[1] = latLon[0];
        contactinfo[2] = latLon[1];
        contactinfo[3] = inputs[2];
        contactinfo[4] = inputs[3];
        contactinfo[5] = inputs[4];
    }
}
