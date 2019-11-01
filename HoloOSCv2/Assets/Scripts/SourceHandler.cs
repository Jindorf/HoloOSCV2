﻿using Microsoft.MixedReality.Toolkit.Utilities.Solvers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SourceHandler : MonoBehaviour
{
    public GameObject source;
    public GameObject shell;
    ArrayList sources = new ArrayList();
    float sourceRadius;
    float shellRadius;


    OSCOutput output;
    GameObject handler;
    const string inputChannelNumber = "/MultiEncoder/inputSetting";


    [SerializeField]
    private float numberOfObjects = 5;

    public void Start() {
       sourceRadius = source.GetComponent<SphereCollider>().radius;
       shellRadius = shell.GetComponent<SphereCollider>().radius;
        InstantiateObjects();
        handler = GameObject.FindGameObjectWithTag("OSCHandler");
        output = handler.GetComponent<OSCOutput>();
    }

    //Insantiates numberOfObjects Sources evenly around shell
    public void InstantiateObjects() {
        //Instantiate Prefab at Runtime
        Vector3 spawnPos = shell.transform.position;
        for (int i = 1; i <= numberOfObjects; i++) {
            float theta = (2 * Mathf.PI / numberOfObjects) * i;
            // scale has to be equal in all directions or algorithm has to be changed accordingly
            spawnPos.x = Mathf.Cos(theta) * shellRadius * shell.transform.localScale.x;
            spawnPos.z = Mathf.Sin(theta) * shellRadius * shell.transform.localScale.x;
            GameObject src = Instantiate(source, spawnPos, Quaternion.identity);
            src.name = "Source" + sources.Count;
            src.transform.parent = this.transform;
            src.GetComponent<SolverHandler>().TransformOverride = src.transform.parent;
            src.GetComponent<SourceObject>().SetID(sources.Count);
            sources.Add(src);
        }
    }

    public void DeleteSourceObject() { 
        //Destroy() the targeted Source 
    }

    public void UpdateSources() {
        string[] data = new string[2];

        data[0] = inputChannelNumber;
        data[1] = numberOfObjects.ToString();
        output.SendMessage("SendOSCMessageToClient", data);

        for (int i=0; i< numberOfObjects; i++ ) {
            GameObject src = sources[i] as GameObject;
            src.GetComponent<SourceObject>().sendMessageToOSCHandler();
        }
    }


}
