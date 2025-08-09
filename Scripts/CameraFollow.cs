using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("Follow Settings")]
    public Transform target;
    public Vector3 offset = new Vector3(0f, 5f, -10f);
    public float smoothSpeed = 5f;
    public float lookAheadDistance = 5f;
    
    [Header("Neon Effects")]
    public Light cameraLight;
    public Color neonColor = Color.cyan;
    public float lightIntensity = 1f;
    
    [Header("Camera Shake")]
    public float shakeIntensity = 0.1f;
    public float shakeDuration = 0.2f;
    
    private Vector3 desiredPosition;
    private Vector3 smoothedPosition;
    private Vector3 originalPosition;
    private bool isShaking = false;
    private float shakeTimer = 0f;
    
    void Start()
    {
        if (target == null)
        {
            target = GameObject.FindGameObjectWithTag("Player").transform;
        }
        
        originalPosition = transform.position;
        
        // Setup camera neon light
        if (cameraLight != null)
        {
            cameraLight.color = neonColor;
            cameraLight.intensity = lightIntensity;
        }
    }
    
    void LateUpdate()
    {
        if (target == null) return;
        
        // Calculate desired position with look ahead
        Vector3 lookAheadPos = target.position + target.forward * lookAheadDistance;
        desiredPosition = lookAheadPos + offset;
        
        // Smooth follow
        smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
        
        // Apply camera shake if active
        if (isShaking)
        {
            shakeTimer -= Time.deltaTime;
            if (shakeTimer <= 0f)
            {
                isShaking = false;
            }
            else
            {
                Vector3 shakeOffset = Random.insideUnitSphere * shakeIntensity;
                smoothedPosition += shakeOffset;
            }
        }
        
        transform.position = smoothedPosition;
        
        // Look at target
        Vector3 lookAtPosition = target.position + Vector3.up * 2f;
        transform.LookAt(lookAtPosition);
        
        // Update neon effects
        UpdateNeonEffects();
    }
    
    void UpdateNeonEffects()
    {
        if (cameraLight != null)
        {
            // Animate light intensity
            float intensity = lightIntensity + Mathf.Sin(Time.time * 2f) * 0.2f;
            cameraLight.intensity = intensity;
            
            // Animate light color
            Color animatedColor = neonColor;
            animatedColor.r += Mathf.Sin(Time.time * 1.5f) * 0.1f;
            animatedColor.g += Mathf.Sin(Time.time * 2f) * 0.1f;
            animatedColor.b += Mathf.Sin(Time.time * 2.5f) * 0.1f;
            cameraLight.color = animatedColor;
        }
    }
    
    public void ShakeCamera()
    {
        isShaking = true;
        shakeTimer = shakeDuration;
    }
    
    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }
    
    public void SetNeonColor(Color color)
    {
        neonColor = color;
        if (cameraLight != null)
        {
            cameraLight.color = color;
        }
    }
}
