﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoliceAI : MonoBehaviour
{

    #region variables
    public List<Transform> waypoints;
    public int currentWaypoint = 0;
    float speed = 3f;
    public int mode = 0;
    float cooldown = 0;
    public GameObject player;
    bool seePlayer = false;
    public GameObject Vision;
    #endregion

    void Start()
    {
        gameObject.GetComponent<Unit>().PathEnd += OnPathEnd;
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(!seePlayer)cooldown += Time.deltaTime;
        #region Waypoints (mode 0)
        if (mode == 0)
        {
            if (Vector3.Distance(transform.position, waypoints[currentWaypoint].position) < 0.1f) currentWaypoint++;
            if (currentWaypoint >= waypoints.Capacity) currentWaypoint = 0;

            var dir = waypoints[currentWaypoint].position - transform.position;
            var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            Vision.transform.rotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);
            transform.position = Vector3.MoveTowards(transform.position, waypoints[currentWaypoint].position, speed * Time.deltaTime);
        }

        #endregion

        if (mode == 1 && cooldown > 10) 
        {
            mode = 2;
            gameObject.GetComponent<Unit>().target = waypoints[currentWaypoint];
            gameObject.GetComponent<Unit>().PathFindToTarget();
        }
    }
    public void OnPathEnd()
    {
        Destroy(gameObject.GetComponent<Unit>().target.gameObject);
        if (mode == 1)
        {
            seePlayer = false;
        }
        else if (mode == 2)
        {
            mode = 0;
        }
    }
    public void Chase()
    {
         cooldown = 0;
         GameObject temp = new GameObject();
         Destroy(temp, 10);
         temp.transform.position = player.transform.position;
         gameObject.GetComponent<Unit>().target = temp.transform;
         gameObject.GetComponent<Unit>().PathFindToTarget();
         seePlayer = true;
         mode = 1;
    }

}