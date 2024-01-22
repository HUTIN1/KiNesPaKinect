using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class ShowText : MonoBehaviour
{

    public TMP_Text textElement;
    public Serveur client;
    // Start is called before the first frame update
    void Start()
    {
        client = client = GameObject.Find("CameraHolder").GetComponent<Serveur>();
    }

    // Update is called once per frame
    void Update()
    {
        textElement.text = client.PrintAll();
    }
}
