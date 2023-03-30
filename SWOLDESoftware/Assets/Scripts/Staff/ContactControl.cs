using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class ContactControl : MonoBehaviour
{
    private List<ContactManager> contacts;
    [SerializeField]
    private GameObject[] newInputs;
    private TMP_InputField[] newInputsTexts;
    private string[] attributes;
    private int currentAttribute;
    private TMP_InputField contactNumberInput, attributeInput;
    private TextMeshProUGUI contactInfoText, attributeSelectionPlaceHolderText;
    private ContactManager currentContact;
    [SerializeField]
    private GameObject carrier, placementControl, locationMarker;
    private PlacementCalc placementCalc;
    private bool firstFrame;
    private int currentMin;
    // Start is called before the first frame update
    void Start()
    {
        contacts = new List<ContactManager>();
        attributes = new string[5]{"LAT","LON","HEADING","SPEED","TYPE"};
        currentContact = null;
        newInputsTexts = new TMP_InputField[newInputs.Length];
        placementCalc = placementControl.GetComponent<PlacementCalc>();  
        for(int i=0 ; i<newInputs.Length; i++){
            newInputsTexts[i] = newInputs[i].GetComponent<TMP_InputField>();
        }
        firstFrame = true;
    }
    void Update(){
        if(firstFrame){
            placementCalc.setCurrentLoc(new string[]{"7 30'0\"", "134 30'0\""});
            contacts.Add(new ContactManager("Lincoln",235,20,"7 42'15\":135 03'00\"","Friendly"));
            firstFrame = false;
        }
    }
    public void startTheClock(){
        StartCoroutine(intervals());
    }
    IEnumerator intervals(){
        while(true){
            addCurrentContacts();
            updateContacts();
            currentMin++;
            yield return new WaitForSeconds(300f);
        }
    }
    public void newContact(){
        string latLon = newInputsTexts[1].text+":"+newInputsTexts[2].text;
        string message = GameObject.Find("MessageHandler").GetComponent<MessageHandler>().header();
        message += "0,"+newInputsTexts[0].text+",";
        message += latLon+","+newInputsTexts[3].text+",";
        message += newInputsTexts[4].text+","+newInputsTexts[5].text;
        GameObject.Find("SimController").GetComponent<StartScreenControl>().sendMessage(message);
        //string contactNumber, int bearing, float speed, string initLoc, string type
        contacts.Add(new ContactManager(newInputsTexts[0].text,int.Parse(newInputsTexts[3].text),int.Parse(newInputsTexts[4].text), latLon, newInputsTexts[5].text));
        for(int i=0 ; i<newInputs.Length; i++){
            newInputsTexts[i].text="";
        }
    }
    void updateContacts(){
        for(int i=0; i<contacts.Count; i++){
            contacts[i].updatePos();
            if(i == 0){
                string newLocation = contacts[i].getPosition();
                string[] latLon = newLocation.Split(":");
                carrier.transform.position = placementCalc.calcWorldPos(latLon, locationMarker.transform.position);
                carrier.transform.eulerAngles= new Vector3(0,0,90-contacts[i].getBearing());
            }
            string message = GameObject.Find("MessageHandler").GetComponent<MessageHandler>().header();
            message += contacts[i].getMessage();
            GameObject.Find("SimController").GetComponent<StartScreenControl>().sendMessage(message);
        }
    }
    void addCurrentContacts(){
        switch(currentMin){
            //Minute 0
            case 0:
            contacts.Add(new ContactManager("001",65,20,"7 35'45\":135 00'45\"","Unknown"));
            contacts.Add(new ContactManager("002",10,15,"7 38'45\":135 08'00\"","Merchant"));
            contacts.Add(new ContactManager("003",10,15,"7 42'15\":135 08'30\"","Tanker"));
            contacts.Add(new ContactManager("004",60,10,"7 45'00\":134 56'30\"","Cargo"));
            contacts.Add(new ContactManager("005",25,15,"7 41'45\":134 55'00\"","Fishing"));
            contacts.Add(new ContactManager("006",300,25,"7 38'30\":134 58'15\"","Wig"));
            contacts.Add(new ContactManager("007",60,20,"7 35'45\":135 00'30\"","Cargo"));
            contacts.Add(new ContactManager("008",59,12,"7 44'30\":134 57'00\"","Unknown"));
            contacts.Add(new ContactManager("009",12,30,"7 40'15\":135 06'30\"","Merchant"));
            contacts.Add(new ContactManager("010",20,10,"7 45'00\":135 01'00\"","Cargo"));
            contacts.Add(new ContactManager("101",90,150,"7 45'00\":134 50'00\"","Cessna 172"));
            contacts.Add(new ContactManager("102",70,250,"7 31'45\":134 41'15\"","757 Passenger"));
            contacts.Add(new ContactManager("103",85,250,"7 42'00\":134 53'45\"","757 Passenger"));
            contacts.Add(new ContactManager("104",82,250,"7 39'30\":134 51'45\"","EDGE 540 (RedBull)"));
            contacts.Add(new ContactManager("121",66,200,"7 34'34\":134 44'30\"","Unknown"));
            break;

            //Minute 5
            case 1:
            contacts.Add(new ContactManager("011",220,17,"7 45'30\":135 05'30\"","Unknown"));
            contacts.Add(new ContactManager("012",42,15,"7 35'15\":134 02'45\"","Merchant"));
            contacts.Add(new ContactManager("013",45,15,"7 36'45\":135 04'30\"","Cargo"));
            contacts.Add(new ContactManager("014",42,15,"7 36'30\":135 03'15\"","Trawling"));
            contacts.Add(new ContactManager("015",45,20,"7 39'45\":134 55'15\"","Unknown"));
            contacts.Add(new ContactManager("016",15,25,"7 41'15\":134 55'00\"","Merchant"));
            contacts.Add(new ContactManager("017",45,20,"7 40'30\":134 53'45\"","Unknown"));
            contacts.Add(new ContactManager("105",94,180,"7 38'15\":134 47'45\"","Private Jet"));
            contacts.Add(new ContactManager("106",95,280,"7 39'30\":134 47'45\"","AirBus A380"));
            break;

            //Minute 10
            case 2:
            contacts.Add(new ContactManager("018",85,20,"7 31'15\":134 54'00\"","Research Vessel"));
            contacts.Add(new ContactManager("019",7,30,"7 33'00\":134 53'30\"","Unknown"));
            contacts.Add(new ContactManager("020",75,20,"7 41'45\":134 52'30\"","Cargo"));
            contacts.Add(new ContactManager("107",87,300,"7 38'00\":134 40'15\"","AirBus A320"));
            contacts.Add(new ContactManager("108",238,200,"7 45'30\":135 09'15\"","Unknown"));
            break;

            //Minute 15
            case 3:
            contacts.Add(new ContactManager("021",57,20,"7 41'30\":134 51'45\"","Cargo"));
            contacts.Add(new ContactManager("022",42,15,"7 30'45\":134 58'45\"","Unknown"));
            contacts.Add(new ContactManager("023",42,15,"7 29'00\":134 59'00\"","Fishing"));
            contacts.Add(new ContactManager("024",142,35,"7 31'45\":134 52'30\"","Speed Boat"));
            contacts.Add(new ContactManager("025",184,30,"7 39'00\":134 51'00\"","Schooner"));
            break;

            //Minute 20
            case 4:
            contacts.Add(new ContactManager("026",18,25,"7 31'15\":134 47'15\"","Cruise"));
            contacts.Add(new ContactManager("027",42,15,"7 32'30\":134 49'30\"","Unknown"));
            break;

            //Minute 25
            case 5:
            contacts.Add(new ContactManager("028",40,20,"7 24'30\":134 50'30\"","Unknown"));
            contacts.Add(new ContactManager("029",40,20,"7 23'45\":134 49'45\"","Unknown"));
            contacts.Add(new ContactManager("030",40,20,"7 25'15\":134 51'00\"","Unknown"));
            contacts.Add(new ContactManager("109",148,220,"7 34'45\":134 43'45\"","Taiwanese Cargo"));
            break;

            //Minute 30
            case 6:
            contacts.Add(new ContactManager("031",60,20,"7 24'15\":134 52'00\"","Fishing"));
            contacts.Add(new ContactManager("032",103,25,"7 34'00\":134 47'00\"","Unknown"));
            contacts.Add(new ContactManager("033",235,35,"7 36'15\":134 52'00\"","Speed Boat"));
            break;

            //Minute 35
            case 7:
            break;

            //Minute 40
            case 8:
            contacts.Add(new ContactManager("039",1,25,"7 22'30\":134 41'45\"","Express (Mail) Ship"));
            contacts.Add(new ContactManager("040",0,20,"7 22'00\":134 40'00\"","Cargo"));
            contacts.Add(new ContactManager("110",60,800,"7 16'30\":134 36'30\"","Unknown"));
            contacts.Add(new ContactManager("111",200,300,"7 40'00\":134 53'00\"","Unknown"));
            break;

            //Minute 45
            case 9:
            contacts.Add(new ContactManager("034",277,30,"7 17'00\":134 51'30\"","Unknown"));
            contacts.Add(new ContactManager("035",277,30,"7 16'15\":134 51'15\"","Unknown"));
            contacts.Add(new ContactManager("036",277,30,"7 16'30\":134 52'45\"","Unknown"));
            break;

            //Minte 50
            case 10:
            contacts.Add(new ContactManager("037",277,30,"7 17'45\":134 50'30\"","Unknown"));
            contacts.Add(new ContactManager("038",277,30,"7 16'00\":134 50'00\"","Unknown"));
            break;
        }
    }
}
