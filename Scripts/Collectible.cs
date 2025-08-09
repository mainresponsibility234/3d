using UnityEngine;

public class Collectible : MonoBehaviour
{
    [Header("Collectible Settings")]
    public int pointValue = 10;
    public bool isRotating = true;
    public float rotationSpeed = 90f;
    public float bobSpeed = 2f;
    public float bobHeight = 0.5f;
    
    [Header("Neon Effects")]
    public Color neonColor = Color.yellow;
    public float pulseSpeed = 3f;
    public float pulseAmount = 0.5f;
    
    [Header("Collection Effects")]
    public GameObject collectionEffect;
    public AudioClip collectionSound;
    
    private Renderer collectibleRenderer;
    private Material neonMaterial;
    private float pulseTimer = 0f;
    private Vector3 startPosition;
    private AudioSource audioSource;
    
    void Start()
    {
        startPosition = transform.position;
        
        // Setup neon material
        collectibleRenderer = GetComponent<Renderer>();
        if (collectibleRenderer != null)
        {
            neonMaterial = new Material(collectibleRenderer.material);
            collectibleRenderer.material = neonMaterial;
            
            // Enable emission
            neonMaterial.EnableKeyword("_EMISSION");
            neonMaterial.SetColor("_EmissionColor", neonColor * 3f);
        }
        
        // Add neon light
        Light neonLight = GetComponentInChildren<Light>();
        if (neonLight == null)
        {
            neonLight = gameObject.AddComponent<Light>();
        }
        
        neonLight.type = LightType.Point;
        neonLight.range = 2f;
        neonLight.intensity = 2f;
        neonLight.color = neonColor;
        
        // Setup audio source
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        
        audioSource.clip = collectionSound;
        audioSource.volume = 0.5f;
    }
    
    void Update()
    {
        // Rotate collectible
        if (isRotating)
        {
            transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
        }
        
        // Bob up and down
        float bobOffset = Mathf.Sin(Time.time * bobSpeed) * bobHeight;
        transform.position = startPosition + Vector3.up * bobOffset;
        
        // Update neon effects
        UpdateNeonEffects();
    }
    
    void UpdateNeonEffects()
    {
        if (neonMaterial != null)
        {
            pulseTimer += Time.deltaTime * pulseSpeed;
            float pulse = 1f + Mathf.Sin(pulseTimer) * pulseAmount;
            
            Color currentEmissionColor = neonColor * 3f * pulse;
            neonMaterial.SetColor("_EmissionColor", currentEmissionColor);
        }
    }
    
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Add score
            if (GameManager.Instance != null)
            {
                GameManager.Instance.AddScore(pointValue);
            }
            
            // Play collection sound
            if (audioSource != null && collectionSound != null)
            {
                audioSource.Play();
            }
            
            // Spawn collection effect
            if (collectionEffect != null)
            {
                Instantiate(collectionEffect, transform.position, Quaternion.identity);
            }
            
            // Destroy collectible
            Destroy(gameObject);
        }
    }
    
    public void SetNeonColor(Color color)
    {
        neonColor = color;
        
        if (neonMaterial != null)
        {
            neonMaterial.SetColor("_EmissionColor", color * 3f);
        }
        
        Light neonLight = GetComponentInChildren<Light>();
        if (neonLight != null)
        {
            neonLight.color = color;
        }
    }
    
    public void SetPointValue(int points)
    {
        pointValue = points;
    }
    
    public void SetRotationSpeed(float speed)
    {
        rotationSpeed = speed;
    }
    
    public void SetBobSpeed(float speed)
    {
        bobSpeed = speed;
    }
    
    public void SetBobHeight(float height)
    {
        bobHeight = height;
    }
}
