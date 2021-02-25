using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSettings : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //
        Debug.Log("current screen aspect ratio: " + GetComponent<Camera>().aspect);

        //the lazy way
        GetComponent<Camera>().aspect = 16f / 9f;
        //GetComponent<Camera>().aspect = 1920f / 1080f;
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
