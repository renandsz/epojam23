using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using UnityEngine.SceneManagement;

public class Player3D : MonoBehaviour
{
    public float angle;

    public float lastAngle;
    public int anglePerSec = 45;

    public float currentMoveTime = 10;
    public float maxMoveTime = 2;
    public bool moving;

   // public float extraForceMax = 10;
    private Rigidbody rb;
    public int swingForceBase = 20;
    public float swingForce = 10;
    public Vector3 direction;

    public Transform sonar;

    public StudioEventEmitter ballReadyEvent, swingEvent, holeEvent,metronomeEvent;

    private float currentMetroAngle = 0;
    private int loudCount = 4;
    public int metronomeAngle = 45;

    public GameObject hole;

    public float angleBetween = 0;

    private bool beep = false;

    public float multiplier = 5;

    public MeshCollider chao;

    public bool ballReady = false;
    // Start is called before the first frame update
    private bool win = false;

    private void Awake()
    {
        
        TryGetComponent(out rb);
        rb.velocity = Vector3.down * 0.1f;
    }

    void Start()
    {
        hole = GameObject.FindWithTag("Hole");
        currentMoveTime = maxMoveTime;
        sonar.gameObject.SetActive(false);
        
        metronomeEvent.SetParameter("loud", 1f);
        swingForce = swingForceBase;
    }

    void CalcularAngulo()
    {
        
        
        Vector2 playerPos = new Vector2(transform.position.x, transform.position.z);
        Vector2 holePos = new Vector2(hole.transform.position.x, hole.transform.position.z);

        holePos = holePos - playerPos;
        
       // Vector2 hz = holePos - playerPos;

       angleBetween = Mathf.Atan2(holePos.y, holePos.x) * Mathf.Rad2Deg;
       if (angleBetween < 0) angleBetween += 360;
    }

    void LoadMenu()
    {
        GameObject.Find("Ambience").GetComponent<StudioEventEmitter>().Stop();
        SceneManager.LoadScene(0);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Hole")
        {
            chao.enabled = false;
            win = true;
            Invoke(nameof(LoadMenu),5f);

        }
    }

    void Update()
    {
        if(win) return;
        
        CalcularAngulo();
        
        if (!moving && Input.GetKey(KeyCode.Space))
        {
            if (loudCount >= 5)
            {
                loudCount = 1;
                metronomeEvent.SetParameter("loud", 1f);
            }
            
            sonar.gameObject.SetActive(true);
            angle += Time.deltaTime * anglePerSec ;
            /*swingForce += Time.deltaTime * multiplier;
            if (swingForce >= swingForceBase + extraForceMax)
            {
                swingForce = swingForceBase + extraForceMax;
            }*/
            
            
            if (angle >= 360)
            {
                angle -= 360;
                beep = false;
            }

            if (!beep && angle >= angleBetween)
            {
                beep = true;
                hole.GetComponent<StudioEventEmitter>().Play();
            }

            
            if (!metronomeEvent.IsPlaying() && currentMetroAngle <= 0)
            {

                if (loudCount is 2 or 4)
                {
                    metronomeEvent.Play();
                    metronomeEvent.SetParameter("loud", 0f);
                    
                }
                currentMetroAngle = metronomeAngle;
                loudCount++;
            }
            currentMetroAngle -= Time.deltaTime * anglePerSec ;

            direction = new Vector3(Mathf.Cos(angle*Mathf.Deg2Rad), 0.1F, Mathf.Sin(angle*Mathf.Deg2Rad));

            sonar.rotation = Quaternion.Euler(0,-angle,0);
            //sonar.Rotate(Vector3.right*90);
            
        }

        if (!moving && Input.GetKeyUp(KeyCode.Space))
        {
            loudCount = 4;
            currentMetroAngle = 0;
            swingEvent.Play();
            lastAngle = angle;
            swingForce = swingForceBase;
            angle = 0;
           // moving = true;
            rb.AddForce(direction * swingForce,ForceMode.Impulse);
            rb.AddTorque(direction* swingForce);
            sonar.gameObject.SetActive(false);
        }

        if (!ballReady && rb.velocity == Vector3.zero)
        {
            moving = false;
            ballReady = true;
            ballReadyEvent.Play();
        }
        else if (rb.velocity != Vector3.zero)
        {
            moving = true;
            ballReady = false;
        }

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

