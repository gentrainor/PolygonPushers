using UnityEngine;

public class GoalVisualEffect : MonoBehaviour
{
    public float rotationSpeed = 60f; 
    public float hoverHeight = 0.2f;  
    public float hoverSpeed = 2f;      
    public Color emissionColor = Color.yellow;
    public float emissionBase = 1.0f;    
    public float emissionPulse = 0.5f;  

    private Vector3 _startPos;
    private Material _mat;

    void Start()
    {
        _startPos = transform.position;

        var renderer = GetComponent<MeshRenderer>();
        if (renderer != null)
        {
            _mat = renderer.material;
            _mat.EnableKeyword("_EMISSION");
        }
    }

    void Update()
    {
        transform.Rotate(0f, rotationSpeed * Time.deltaTime, 0f);

        float offset = Mathf.Sin(Time.time * hoverSpeed) * hoverHeight;
        transform.position = _startPos + new Vector3(0f, offset, 0f);

        if (_mat != null)
        {
            float intensity = emissionBase + Mathf.Sin(Time.time * 2f) * emissionPulse;
            _mat.SetColor("_EmissionColor", emissionColor * intensity);
        }
    }
}
