using UnityEngine;

public class Matryoshka : MonoBehaviour
{
    public int size;
    private Vector3 targetPos;
    [SerializeField] private Transform startPlatform;

    private void Awake()
    {
        targetPos = startPlatform.position;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
