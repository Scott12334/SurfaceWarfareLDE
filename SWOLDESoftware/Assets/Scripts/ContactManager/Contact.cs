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
    // Start is called before the first frame update
    void Start()
    {
        contactCICManager = GameObject.Find("GameController");
        controller = contactCICManager.GetComponent<ContactManagerController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnMouseDown() {
        Debug.Log("Object clicked!");
        controller.contactPressed(contactinfo);
    }
}
