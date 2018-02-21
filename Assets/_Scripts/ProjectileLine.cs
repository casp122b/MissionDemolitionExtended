using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileLine: MonoBehaviour
{

    public static ProjectileLine S;
    public float minDist = 0.1f;

    private LineRenderer line;
    private GameObject poi;
    private List<Vector3> points;

    private void Awake()
    {
        S = this;
        line = GetComponent<LineRenderer>();
        line.enabled = false;
        points = new List<Vector3>();
    }

    public GameObject Poi
    {
        get
        {
            return poi;
        }
        set
        {
            poi = value;
            if (poi != null)
            {
                line.enabled = false;
                points = new List<Vector3>();
                AddPoint();
            }
        }
    }

    public void Clear()
    {
        poi = null;
        line.enabled = false;
        points = new List<Vector3>();
    }

    private void AddPoint()
    {
        Vector3 pt = poi.transform.position;
        if (points.Count > 0 && (pt - lastPoint).magnitude < minDist)
        {
            return;
        }
        if (points.Count == 0)
        {
            Vector3 launchPos = SlingShot.S.launchPoint.transform.position;
            Vector3 launchPosDiff = pt - launchPos;
            points.Add(pt + launchPosDiff);
            points.Add(pt);
            line.positionCount = points.Count;
            line.SetPosition(0, points[0]);
            line.SetPosition(1, points[1]);
            line.enabled = true;
        }
        else
        {
            points.Add(pt);
            line.positionCount = points.Count;
            line.SetPosition(points.Count - 1, lastPoint);
            line.enabled = true;
        }
    }

    public Vector3 lastPoint
    {
        get
        {
            if (points == null)
                return Vector3.zero;
            else
                return points[points.Count - 1];
        }
    }

    private void FixedUpdate()
    {
        if (poi == null)
        {
            if (FollowCam.s.poi != null)
            {
                if (FollowCam.s.poi.tag == "Projectile")
                {
                    Poi = FollowCam.s.poi;
                }
                else
                    return;
            }
            else
                return;
        }
        AddPoint();
        if (poi.GetComponent<Rigidbody>().IsSleeping())
        {
            poi = null;
        }
    }

}