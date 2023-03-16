using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ContactManagerController : MonoBehaviour
{
    [SerializeField]
    private GameObject contactInfo;
    private TextMeshProUGUI contactInfoText;
    [SerializeField]
    private GameObject speedInput;
    private TMP_InputField speedInputText;
    [SerializeField]
    private GameObject headingInput;
    private TMP_InputField headingInputText;
    [SerializeField]
    private GameObject latInput;
    private TMP_InputField latInputText;
    [SerializeField]
    private GameObject lonInput;
    private TMP_InputField lonInputText;
    [SerializeField]
    private GameObject typeInput;
    private TMP_InputField typeInputText;
    string[] currentContactValue;
    void Start() {
        contactInfoText = contactInfo.GetComponent<TextMeshProUGUI>();
        speedInputText = speedInput.GetComponent<TMP_InputField>();
        headingInputText = headingInput.GetComponent<TMP_InputField>();
        latInputText = latInput.GetComponent<TMP_InputField>();
        lonInputText = lonInput.GetComponent<TMP_InputField>();
        typeInputText = typeInput.GetComponent<TMP_InputField>();
    }
    public void contactPressed(string[] contactValues){
        currentContactValue= contactValues;
        string fullContactInfo = "";
        fullContactInfo += "Contact: "+contactValues[0]+"\n";
        fullContactInfo += "Lat: "+contactValues[1]+"\n";
        fullContactInfo += "Lon: "+contactValues[2]+"\n";
        fullContactInfo += "Heading: "+contactValues[3]+"\n";
        fullContactInfo += "Speed: "+contactValues[4]+"\n";
        fullContactInfo += "Type: "+contactValues[5];
        contactInfoText.text = fullContactInfo;
    }
    public void sendMessageToBridge(){
        string messageToBridge="";
        messageToBridge += 
            "Contact: "+ currentContactValue[0]+ " at Lat: "+latInputText.text+", Lon: "+lonInputText.text+". Going "
            +speedInputText.text+" knots with a heading of "+headingInputText.text+" Degrees. It is a "+typeInputText.text+".";
        Debug.Log(messageToBridge);
    }
}
