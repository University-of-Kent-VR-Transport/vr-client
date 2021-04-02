using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public BusController busController;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(UpdateMap());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator UpdateMap()
    {
        busController.UpdateBuses();

        yield return new WaitForSeconds(5);
        StartCoroutine(UpdateMap());
    }
}
