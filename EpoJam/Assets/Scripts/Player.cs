using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float angle;

    public float lastAngle;
    public int anglePerSec = 45;

    public float currentMoveTime = 10;
    public float maxMoveTime = 2;
    public bool moving;

    
    // Start is called before the first frame update
    void Start()
    {
        currentMoveTime = maxMoveTime;
    }

    // Update is called once per frame
    void Update()
    {
        if (!moving && Input.GetKey(KeyCode.Space))
        {
            angle += Time.deltaTime * anglePerSec ;
            if (angle >= 360) angle -= 360;            
            
            transform.rotation = Quaternion.Euler(0,0,angle);
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            lastAngle = angle;
            angle = 0;

            moving = true;
        }

        if (moving)
        {
            currentMoveTime -= Time.deltaTime;
            if (currentMoveTime <= 0)
            {
                currentMoveTime = maxMoveTime;
                moving = false;
            }
            transform.Translate(transform.right * Time.deltaTime);
        }
        
    }
}
