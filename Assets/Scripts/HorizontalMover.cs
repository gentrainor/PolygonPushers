using UnityEngine;

public class HorizontalMover : MonoBehaviour
{
    public float speed = 2f;      
    public float distance = 3f;   

    private Vector3 startPos;

    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        transform.position = startPos + new Vector3(0, 0, Mathf.Sin(Time.time * speed) * distance);
    }
}
