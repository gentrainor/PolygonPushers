using UnityEngine;

public class TurnGear : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public float torque = 45f;
    public GameObject gear;
    public float offset = 0f;

    void Start()
    {
        gear = this.gameObject;
        gameObject.transform.Rotate(0, 0, offset);
    }

    // Update is called once per frame
    void Update()
    {
        gear.transform.Rotate(0, 0, torque * Time.deltaTime);
    }
}
