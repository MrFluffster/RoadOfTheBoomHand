using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrameCap : MonoBehaviour
{
    // Start is called before the first frame update
    public int frameRate = 60;
    void Start()
    {
        //Set Frame rate to 30fps
        Application.targetFrameRate = frameRate;
    }

    // Update is called once per frame
    /*void Update()
    {
        
    }*/
}
