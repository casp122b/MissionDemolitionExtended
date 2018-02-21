using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal: MonoBehaviour
{
    public static bool goalMet = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Projectile")
        {
            Goal.goalMet = true;
            Color c = GetComponent<MeshRenderer>().materials[0].color;
            c.a = 1.0f;
            GetComponent<MeshRenderer>().materials[0].color = c;
        }
    }

}