using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class ContactControl : MonoBehaviour
{
    private List<ContactManager> contacts;
    [SerializeField]
    private GameObject contactSelection, attributeSelection, contactInfoDisplay, attributeSelectionPlaceHolder;
    [SerializeField]
    private GameObject[] newInputs;
    private TMP_InputField[] newInputsTexts;
    private string[] attributes;
    private int currentAttribute;
    private TMP_InputField contactNumberInput, attributeInput;
    private TextMeshProUGUI contactInfoText, attributeSelectionPlaceHolderText;
    private ContactManager currentContact;
    // Start is called before the first frame update
    void Start()
    {
        contacts = new List<ContactManager>();
        contacts.Add(new ContactManager("045",45,15,"35'0\",35'0\"", "Cargo Plane"));
        contactNumberInput = contactSelection.GetComponent<TMP_InputField>();
        attributeInput = attributeSelection.GetComponent<TMP_InputField>();
        contactInfoText = contactInfoDisplay.GetComponent<TextMeshProUGUI>();
        attributeSelectionPlaceHolderText = attributeSelectionPlaceHolder.GetComponent<TextMeshProUGUI>();
        attributes = new string[5]{"LAT","LON","HEADING","SPEED","TYPE"};
        currentContact = null;
        newInputsTexts = new TMP_InputField[newInputs.Length];
        for(int i=0 ; i<newInputs.Length; i++){
            newInputsTexts[i] = newInputs[i].GetComponent<TMP_InputField>();
        }
    }
    public void newContact(){
        string latLon = newInputsTexts[1].text+","+newInputsTexts[2].text;
        contacts.Add(new ContactManager(newInputsTexts[0].text,int.Parse(newInputsTexts[3].text),int.Parse(newInputsTexts[4].text), latLon, newInputsTexts[5].text));
        for(int i=0 ; i<newInputs.Length; i++){
            newInputsTexts[i].text="";
        }
    }
    public void enterContactNumber(){
        string contactNumberSelected = contactNumberInput.text;
        currentContact = null;
        for(int i=0; i<contacts.Count; i++){
            if(contacts[i].getContactNumber() == contactNumberSelected){
                currentContact = contacts[i]; 
                break;
            }
        }
        if(currentContact != null){
            string contactInfo="";
            for(int i=0; i<currentContact.getContactInfo().Length; i++){
                contactInfo += currentContact.getContactInfo()[i] +"\n";
            }
            contactInfoText.text=contactInfo;
            currentAttribute =0;
            attributeSelectionPlaceHolderText.text = "ENTER "+attributes[currentAttribute];
        }
    }
    public void cycleCurrentAttribute(int change){
        if(currentContact != null){
            if(change == -1 && currentAttribute == 0){
            currentAttribute = attributes.Length-1;
            }
            else if(change == 1 && currentAttribute == attributes.Length-1){
                currentAttribute = 0;
            }
            else{
                currentAttribute += change;
            }
            attributeSelectionPlaceHolderText.text = "ENTER "+attributes[currentAttribute];
            attributeInput.text="";
        }
    }
    public void setAttribute(){
        if(currentContact !=null){
            switch(currentAttribute){
            case 0:
                float newLat = ConvertDmsToDecimal(attributeInput.text);
                currentContact.setPosition(newLat, 0);
            break;
            case 1:
                float newLon = ConvertDmsToDecimal(attributeInput.text);
                currentContact.setPosition(newLon, 1);
            break;
            case 2:
                currentContact.setHeading(int.Parse(attributeInput.text));
            break;
            case 3:
                currentContact.setSpeed(int.Parse(attributeInput.text));
            break;
            case 4:
                currentContact.setType(attributeInput.text);
            break;
            }
            string contactInfo="";
            for(int i=0; i<currentContact.getContactInfo().Length; i++){
                contactInfo += currentContact.getContactInfo()[i] +"\n";
            }
            contactInfoText.text=contactInfo;
            attributeInput.text="";
        }
    }
    void updateContacts(){
        for(int i=0; i<contacts.Count; i++){
            contacts[i].updatePos();
        }
    }
    private float ConvertDmsToDecimal(string dms)
    {
        string[] parts = dms.Split('\'', '\"');
        float minutes = float.Parse(parts[0]);
        float seconds = float.Parse(parts[1]);
        float decimalDegrees = minutes + (seconds / 60);
        return decimalDegrees;
    }
}
