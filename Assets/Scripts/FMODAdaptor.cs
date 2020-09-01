using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FMODAdaptor : MonoBehaviour
{

    public string eventName;

    private void Start()
    {
        if (eventName != "")
        {
            FMODUnity.RuntimeManager.PlayOneShot(eventName, GetComponent<Transform>().position);
        }
    }
    // Start is called before the first frame update
    public void callFMODOneShot(string name)
    {
        FMODUnity.RuntimeManager.PlayOneShot(name, GetComponent<Transform>().position);

    }
}
