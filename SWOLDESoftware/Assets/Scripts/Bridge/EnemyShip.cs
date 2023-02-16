using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShip : MonoBehaviour
{
    //Target Screen
    [SerializeField]
    private GameObject targetScreenGame;
    [SerializeField]
    private GameObject targetScreenCanvas;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnMouseDown() {
        Debug.Log("Object clicked!");
        targetScreenGame.SetActive(true);
        targetScreenCanvas.SetActive(true);
    }
}
