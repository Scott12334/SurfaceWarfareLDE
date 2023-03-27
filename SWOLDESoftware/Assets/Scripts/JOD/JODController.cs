using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JODController : MonoBehaviour
{
    [SerializeField]
    private GameObject[] dials;
    private Dial[] dialScripts;
    bool debug=true;
    // Start is called before the first frame update
    void Start()
    {
        dialScripts= new Dial[dials.Length];
        for(int i=0; i<dials.Length; i++){
            dialScripts[i]=dials[i].GetComponent<Dial>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(debug){
            for(int i=0; i<dialScripts.Length; i++){
            switch(i){
                    case 0:
                    dialScripts[i].sendMessage("This is a Message from CIC");
                    dialScripts[i].sendMessage("This is a Message from CIC");
                    dialScripts[i].sendMessage("This is a Message from CIC");
                    break;
                }
            }
            debug=false;
        }
    }
    public void recieveMessage(string message){
        string[] inputs = message.Split(",");
        if(inputs[1] == "2"){
            //STRINGs from COMO
            if(inputs[3] == "1"){
                dialScripts[0].sendMessage(inputs[4]);
            }
        }
        else if(inputs[1] == "3"){
            //Firing Solutions from CIC
            string firingSolutionUpdate = "Firing Solution for Contact ";
            firingSolutionUpdate+= inputs[3]+ ":"+"\n";
            if(int.Parse(inputs[4]) > 0){firingSolutionUpdate+= "Fire 5-Inch Gun"+"\n";}
            if(int.Parse(inputs[5]) > 0){firingSolutionUpdate+= "Fire "+inputs[5]+ " SAM(s)"+"\n";}
            if(int.Parse(inputs[6]) > 0){firingSolutionUpdate+= "Fire "+inputs[6]+ " SSM(s)"+"\n";}
            if(int.Parse(inputs[7]) > 0){firingSolutionUpdate+= "Fire KID"+"\n";}
            dialScripts[0].sendMessage(firingSolutionUpdate);
        }
        else if(inputs[1] == "4"){
            //Contact Updates from CIC
            string[] latLon= new string[2];
                    for(int i=0; i<inputs.Length; i++){
                        if(inputs[i].Contains(":")){
                            latLon = inputs[i].Split(":");
                        }
                    }
            dialScripts[0].sendMessage("Contact "+inputs[3] +" is now at Latitude: "+ latLon[0]+ " and Longitude: "+latLon[1]+". Heading at "+inputs[5]+" Degrees with a speed of "+inputs[6]+" knots. It is a "+inputs[7]);
        }
    }
}
