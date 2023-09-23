using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Hole : MonoBehaviour
{
    
    // Start is called before the first frame update
    void Start()
    {
        GameObject[] points = GameObject.FindGameObjectsWithTag("Point");

        int i = Random.Range(0, points.Length);


        transform.position = new Vector3(points[i].transform.position.x, transform.position.y,
            points[i].transform.position.z);
        foreach (var VARIABLE in points)
        {
            VARIABLE.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            transform.GetChild(0).localPosition+= Vector3.up;
        }
    }
}
