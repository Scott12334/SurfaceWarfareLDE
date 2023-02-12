using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class GameController : MonoBehaviour
{
    [SerializeField]
    private GameObject CICSendMessage;
    private TextMeshProUGUI inputText;
    //Position Inputs
    //LAT
    [SerializeField]
    private GameObject LatInput;
    private TextMeshProUGUI latText;
    //LON
    [SerializeField]
    private GameObject LongInput;
    private TextMeshProUGUI longText;
    //SPEED
    [SerializeField]
    private GameObject SpeedInput;
    private TextMeshProUGUI speedText;
    //HEADING
    [SerializeField]
    private GameObject HeadingInput;
    private TMP_InputField headingText;
    //COMPASS
    [SerializeField]
    private GameObject compassPointer;
    private float heading=0;
    //SHIP
    [SerializeField]
    private GameObject currentShip;
    // Start is called before the first frame update
    void Start()
    {
        setTexts();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void sendPositionUpdate(){
        string positionString ="";
        positionString += "Ship 1 is at Latitude: "+latText.text;
        positionString += ", Longitutde: "+longText.text;
        positionString += ". With a speed of "+speedText.text;
        positionString += " knots. With a heading of "+headingText.text+" degrees";
        currentShip.transform.eulerAngles= new Vector3(0,0,180-float.Parse(headingText.text.Trim()));
        compassPointer.transform.eulerAngles= new Vector3(0,0,-1*float.Parse(headingText.text.Trim()));
        Debug.Log(positionString);
    }
    public void sendCICMessage(){
        Debug.Log(inputText.text);
    }

    private void setTexts(){
        inputText= CICSendMessage.GetComponent<TextMeshProUGUI>();
        inputText.enableWordWrapping = true;

        latText= LatInput.GetComponent<TextMeshProUGUI>();
        longText= LongInput.GetComponent<TextMeshProUGUI>();
        speedText= SpeedInput.GetComponent<TextMeshProUGUI>();
        headingText= HeadingInput.GetComponent<TMP_InputField>();
    }
}
