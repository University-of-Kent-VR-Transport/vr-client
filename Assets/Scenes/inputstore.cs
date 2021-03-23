using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inputstore : MonoBehaviour
{

    public static Inputstore InputStore1;

    public InputField inputField;

    public string area;

    // Start is called before the first frame update
    void Awake()
    {
        if (InputStore1 == null)
        {
            InputStore1 = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
    }
    

    // Update is called once per frame
    void Update()
    {
        if(area != inputField.text){area = inputField.text;}
    }
}

