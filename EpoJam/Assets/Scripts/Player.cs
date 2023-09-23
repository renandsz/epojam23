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

    public float speed = 10;
    public float minSpeed = 3;
    
    // Start is called before the first frame update
    void Start()
    {
        currentMoveTime = maxMoveTime;
        speed = minSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        if (!moving && Input.GetKey(KeyCode.Space))
        {
            angle += Time.deltaTime * anglePerSec ;
            speed += Time.deltaTime * minSpeed;
            if (angle >= 360) angle -= 360;            
            
            transform.rotation = Quaternion.Euler(0,0,angle);
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            lastAngle = angle;
            speed = minSpeed;
            angle = 0;

            moving = true;
        }

        if (moving)
        {
            currentMoveTime -= Time.deltaTime;
            transform.position += transform.right * (speed * Time.deltaTime);
            if (currentMoveTime <= 0)
            {
                currentMoveTime = maxMoveTime;
                moving = false;
                
            }
           
        }
        
    }
}
