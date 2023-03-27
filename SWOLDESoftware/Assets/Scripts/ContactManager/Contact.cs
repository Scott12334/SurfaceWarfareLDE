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
    private string[] storedLatLong;
    // Start is called before the first frame update
    void Start()
    {
        contactCICManager = GameObject.Find("ContactController");
        controller = contactCICManager.GetComponent<ContactManagerController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnMouseDown() {
        controller.contactPressed(contactinfo);
    }
    public void setContactName(string contactName){
        contactinfo[0] = contactName;
    }
    public string[] getLatLon(){
        return storedLatLong;
    }
    public void setContactValue(string[] inputs, string[] latLon, bool isFriendly, string shipName){
        if(!isFriendly){
            storedLatLong = latLon;
            contactinfo = new string[6];
            contactinfo[0] = inputs[3];
            contactinfo[1] = latLon[0];
            contactinfo[2] = latLon[1];
            contactinfo[3] = inputs[5];
            contactinfo[4] = inputs[6];
            if(inputs.Length == 8){
                contactinfo[5] = inputs[7];
            }
            else{
                contactinfo[5] = "Friendly";
            }
        }
        else{
            storedLatLong = latLon;
            contactinfo = new string[6];
            contactinfo[0] = shipName;
            contactinfo[1] = latLon[0];
            contactinfo[2] = latLon[1];
            contactinfo[3] = inputs[4];
            contactinfo[4] = inputs[5];
            contactinfo[5] = "Friendly";
        }
    }
}
