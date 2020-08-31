using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FMODAdaptor : MonoBehaviour
{
    // Start is called before the first frame update
    public void callFMODOneShot(string name)
    {
        FMODUnity.RuntimeManager.PlayOneShot(name, GetComponent<Transform>().position);

    }
}
