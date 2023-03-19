using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class StartScreenControl : MonoBehaviour
{
    private int ship;
    [SerializeField]
    private GameObject shipSelection, screenSelection;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    public void shipSelected(int shipId){
        this.ship = shipId;
        //STAFF
        if(shipId == 5){
            SceneManager.LoadScene(shipId);
        }
        //DESCON
        else if(shipId == 6){
            SceneManager.LoadScene(shipId);
        }
        else{
            PlayerPrefs.SetInt("shipId", shipId);
            shipSelection.SetActive(false);
            screenSelection.SetActive(true);
        }
    }
    public void screenSelected(int screenID){
        SceneManager.LoadScene(screenID);
    }
}
