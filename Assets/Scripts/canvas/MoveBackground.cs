using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveBackground : MonoBehaviour
{


    //speed at which the background moves
    //current x position of the background
    private float x;
    //destination point where the background will reset
    //original x position of the background
    public float PontoOriginal;




    // Use this for initialization
    void Start()
    {
        //PontoOriginal = transform.position.x;
    }

    // Update is called once per frame
    void Update()
    {

        //get the current x position of the background
        x = transform.position.x;
        //update the x position based on the speed and time
        x += speed * Time.deltaTime;
        //update the position of the background
        transform.position = new Vector3(x, transform.position.y, transform.position.z);


        //if the background has reached the destination point
        if (x <= PontoDeDestino)
        {

            Debug.Log("hhhh");
            //reset the x position of the background to its original position
            x = PontoOriginal;
            //update the position of the background to the original position
            transform.position = new Vector3(x, transform.position.y, transform.position.z);
        }


    }
}

