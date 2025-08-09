using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class NeonMaterial : MonoBehaviour
{
    [Header("Neon Settings")]
    public Color neonColor = Color.cyan;
    public float emissionIntensity = 2f;
    public float pulseSpeed = 2f;
    public float pulseAmount = 0.5f;
    
    [Header("Advanced Settings")]
    public bool useRandomColor = false;
    public Color[] randomColors = { Color.red, Color.blue, Color.green, Color.yellow, Color.magenta, Color.cyan };
    
    private Renderer objectRenderer;
    private Material neonMaterial;
    private Color originalColor;
    private float pulseTimer = 0f;
    
    void Start()
    {
        objectRenderer = GetComponent<Renderer>();
        
        if (objectRenderer != null)
        {
            // Create a new material instance
            neonMaterial = new Material(objectRenderer.material);
            objectRenderer.material = neonMaterial;
            
            // Set initial color
            if (useRandomColor && randomColors.Length > 0)
            {
                neonColor = randomColors[Random.Range(0, randomColors.Length)];
            }
            
            originalColor = neonColor;
            
            // Enable emission
            neonMaterial.EnableKeyword("_EMISSION");
            neonMaterial.SetColor("_EmissionColor", neonColor * emissionIntensity);
        }
    }
    
    void Update()
    {
        if (neonMaterial != null)
        {
            // Pulse effect
            pulseTimer += Time.deltaTime * pulseSpeed;
            float pulse = 1f + Mathf.Sin(pulseTimer) * pulseAmount;
            
            // Update emission color
            Color currentEmissionColor = originalColor * emissionIntensity * pulse;
            neonMaterial.SetColor("_EmissionColor", currentEmissionColor);
        }
    }
    
    public void SetNeonColor(Color newColor)
    {
        neonColor = newColor;
        originalColor = newColor;
        
        if (neonMaterial != null)
        {
            neonMaterial.SetColor("_EmissionColor", newColor * emissionIntensity);
        }
    }
    
    public void SetEmissionIntensity(float intensity)
    {
        emissionIntensity = intensity;
        
        if (neonMaterial != null)
        {
            neonMaterial.SetColor("_EmissionColor", originalColor * intensity);
        }
    }
    
    public void SetPulseSpeed(float speed)
    {
        pulseSpeed = speed;
    }
    
    public void SetPulseAmount(float amount)
    {
        pulseAmount = amount;
    }
    
    void OnDestroy()
    {
        // Clean up material instance
        if (neonMaterial != null)
        {
            DestroyImmediate(neonMaterial);
        }
    }
}
