using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class PressurePlate : MonoBehaviour
{
    public List<MovablePlatform> movablePlatforms = new List<MovablePlatform>();
    private AudioSource pressSfx;

    private void Awake()
    {
        pressSfx = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !other.isTrigger)
        {
            if (other.transform.GetComponent<Robushka>().isActive)
            {
                pressSfx.Play();
                foreach (var platform in movablePlatforms)
                {
                    platform.TriggerMove();
                }
            }
        }
    }
}
