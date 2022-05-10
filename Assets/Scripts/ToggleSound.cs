using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleSound : MonoBehaviour
{
    [SerializeField] private FMODUnity.StudioEventEmitter emitter;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (emitter.IsPlaying())
        {
            emitter.Stop();
        }
        else
        {
            emitter.Play();
        }
    }
}
