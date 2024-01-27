using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Aim : MonoBehaviour
{
    public bool CanSHoot;


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 6) CanSHoot = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == 6) CanSHoot = false;
    }
}
