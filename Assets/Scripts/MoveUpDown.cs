using UnityEngine;

public class MoveUpDown : MonoBehaviour
{
    [Header("Motion")]
    public float amplitude = 2f;     
    public float speed = 1f;         
    public Vector3 axis = Vector3.up; 
    public float startPhase = 0f;   

    [Header("Space")]
    public bool relativeToParent = false; 

    private Vector3 basePos;

    void Start()
    {
        basePos = relativeToParent && transform.parent != null
            ? transform.parent.TransformPoint(transform.localPosition)
            : transform.position;

    }

    void Update()
    {
        float y = Mathf.Sin(Time.time * speed + startPhase) * amplitude;
        Vector3 offset = axis.normalized * y;

        if (relativeToParent && transform.parent != null)
        {
            Vector3 targetWorld = basePos + offset;
            transform.position = targetWorld;
        }
        else
        {
            transform.position = basePos + offset;
        }
    }
}

