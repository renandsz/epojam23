using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player3D : MonoBehaviour
{
    public float angle;

    public float lastAngle;
    public int anglePerSec = 45;

    public float currentMoveTime = 10;
    public float maxMoveTime = 2;
    public bool moving;

    public float speed = 10;
    public float minSpeed = 3;
    private Rigidbody rb;
    public int swingForce = 10;
    public Vector3 direction;

    public Transform sonar;
    // Start is called before the first frame update
    void Start()
    {
        currentMoveTime = maxMoveTime;
        speed = minSpeed;
        TryGetComponent(out rb);
        sonar.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (!moving && Input.GetKey(KeyCode.Space))
        {
            sonar.gameObject.SetActive(true);
            angle += Time.deltaTime * anglePerSec ;
            speed += Time.deltaTime * minSpeed;
            if (angle >= 360) angle -= 360;

            direction = new Vector3(Mathf.Cos(angle*Mathf.Deg2Rad), 0, Mathf.Sin(angle*Mathf.Deg2Rad));

            sonar.rotation = Quaternion.Euler(0,-angle,0);
            //sonar.Rotate(Vector3.right*90);
        }

        if (!moving && Input.GetKeyUp(KeyCode.Space))
        {
            lastAngle = angle;
            speed = minSpeed;
            angle = 0;
           // moving = true;
            rb.AddForce(direction * swingForce,ForceMode.Impulse);
            sonar.gameObject.SetActive(false);
        }

        moving = (rb.velocity != Vector3.zero);

        /*if (moving)
        {
            currentMoveTime -= Time.deltaTime;
            if (currentMoveTime <= 0)
            {
                currentMoveTime = maxMoveTime;
                moving = false;
            }
        }*/
        
    }
}

