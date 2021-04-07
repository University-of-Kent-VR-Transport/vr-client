using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameObject ovrPlayer =GameObject.Find("OVRPlayerObject");

        ovrPlayer.GetComponent<LocomotionTeleport>().enabled = true;

    }

  
}
