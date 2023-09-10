using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class LabelControl : MonoBehaviour
{
    public static LabelControl Instance;
    private void Start() {
        Instance = this;
    }
    public void setLabel(){
        GameObject label = GameObject.Find("ShipLabel");
        TextMeshProUGUI labelText = label.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        string ship = "";
        switch(PlayerPrefs.GetInt("Ship")){
            case 0:
            ship = "Sampson";
            break;

            case 1:
            ship = "Gettysburg";
            break;

            case 2:
            ship = "Chosin";
            break;

            case 3:
            ship = "Kidd";
            break;

            case 4:
            ship = "Galagher";
            break;

            case 5:
            ship = "Lake Erie";
            break;

            case 6:
            ship = "Staff";
            break;

            case 7:
            ship = "DESCON";
            break;
        }
        string screen = "";
        switch(PlayerPrefs.GetInt("Screen")){
            case 0:
            screen = "Bridge";
            break;

            case 1:
            screen = "JOD";
            break;

            case 2:
            screen = "COMO";
            break;

            case 3:
            screen = "CIC";
            break;

            case 4:
            screen = "ContactManager";
            break;

            case 5:
            screen = "Staff";
            break;

            case 6:
            screen = "DESCON";
            break;
        }
        labelText.text = ship + "<br>" + screen;
    }
}
