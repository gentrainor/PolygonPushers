using UnityEngine;

public class RotateZ : MonoBehaviour
{
    public GameObject target;
    public float torque = 15f;
    public float initialRotation;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        target = this.gameObject;
        target.transform.Rotate(0, 0, initialRotation);
    }

    // Update is called once per frame
    void Update()
    {
        target.transform.Rotate(0, 0, torque * Time.deltaTime);
    }
}
