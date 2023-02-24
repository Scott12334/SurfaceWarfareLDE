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
}
