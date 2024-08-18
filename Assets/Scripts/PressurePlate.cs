using System.Collections.Generic;
using UnityEngine;

public class Lever : MonoBehaviour
{
    public List<MovablePlatform> movablePlatforms = new List<MovablePlatform>();

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (other.transform.GetComponent<Robushka>().isActive)
            {
                foreach (var platform in movablePlatforms)
                {
                    platform.TriggerMove();
                }
            }
        }
    }
}
