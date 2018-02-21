using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCam: MonoBehaviour
{

    public static FollowCam s;
    public GameObject poi;
    public float easing = 0.05f;
    public Vector2 minXY;
    private float camZ;

    private void Awake()
    {
        s = this;
        camZ = this.transform.position.z;
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        Vector3 destination;
        if (poi == null)
            destination = Vector3.zero;
        else
        {
            destination = poi.transform.position;
            if (poi.tag == "Projectile")
            {
                if (poi.GetComponent<Rigidbody>().IsSleeping())
                {
                    poi = null;
                    return;
                }
            }
        }
        destination.x = Mathf.Max(minXY.x, destination.x);
        destination.y = Mathf.Max(minXY.y, destination.y);
        destination.z = camZ;
        destination = Vector3.Lerp(transform.position, destination, easing);
        transform.position = destination;
        GetComponent<Camera>().orthographicSize = destination.y + 10;
    }
}