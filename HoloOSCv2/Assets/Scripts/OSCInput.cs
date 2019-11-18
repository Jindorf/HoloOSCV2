using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OSCInput : MonoBehaviour
{
    [SerializeField]
    private OscIn oscIn;

    const string address1 = "/MultiEncoder/";
    // Start is called before the first frame update
    void Start()
    {
        bool response = oscIn.Open(8001);
        Debug.Log("Opening OSCin: " + response);
    }

    void OnEnable() {
        // You can "map" messages to methods in two ways:

        // 1) For messages with a single argument, route the value using the type specific map methods.
        oscIn.MapFloat(address1, OnTest1);

        // 2) For messages with multiple arguments, route the message using the Map method.
    }

    void OnTest1(float value) {
        Debug.Log("Received " + address1 + " " + value + "\n");
    }

}
