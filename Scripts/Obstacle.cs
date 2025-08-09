using UnityEngine;

public class Obstacle : MonoBehaviour
{
    [Header("Obstacle Settings")]
    public float moveSpeed = 10f;
    public bool isMoving = false;
    public Vector3 moveDirection = Vector3.forward;
    
    [Header("Neon Effects")]
    public Color neonColor = Color.red;
    public float pulseSpeed = 2f;
    public float pulseAmount = 0.5f;
    
    [Header("Collision")]
    public bool destroyOnCollision = true;
    public GameObject collisionEffect;
    
    private Renderer obstacleRenderer;
    private Material neonMaterial;
    private float pulseTimer = 0f;
    private Vector3 startPosition;
    
    void Start()
    {
        startPosition = transform.position;
        
        // Setup neon material
        obstacleRenderer = GetComponent<Renderer>();
        if (obstacleRenderer != null)
        {
            neonMaterial = new Material(obstacleRenderer.material);
            obstacleRenderer.material = neonMaterial;
            
            // Enable emission
            neonMaterial.EnableKeyword("_EMISSION");
            neonMaterial.SetColor("_EmissionColor", neonColor * 2f);
        }
        
        // Add neon light
        Light neonLight = GetComponentInChildren<Light>();
        if (neonLight == null)
        {
            neonLight = gameObject.AddComponent<Light>();
        }
        
        neonLight.type = LightType.Point;
        neonLight.range = 3f;
        neonLight.intensity = 1.5f;
        neonLight.color = neonColor;
    }
    
    void Update()
    {
        // Move obstacle if enabled
        if (isMoving)
        {
            transform.Translate(moveDirection * moveSpeed * Time.deltaTime);
        }
        
        // Update neon effects
        UpdateNeonEffects();
    }
    
    void UpdateNeonEffects()
    {
        if (neonMaterial != null)
        {
            pulseTimer += Time.deltaTime * pulseSpeed;
            float pulse = 1f + Mathf.Sin(pulseTimer) * pulseAmount;
            
            Color currentEmissionColor = neonColor * 2f * pulse;
            neonMaterial.SetColor("_EmissionColor", currentEmissionColor);
        }
    }
    
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Player hit obstacle
            PlayerController player = other.GetComponent<PlayerController>();
            if (player != null)
            {
                player.GameOver();
            }
            
            // Spawn collision effect
            if (collisionEffect != null)
            {
                Instantiate(collisionEffect, transform.position, Quaternion.identity);
            }
            
            // Destroy obstacle if enabled
            if (destroyOnCollision)
            {
                Destroy(gameObject);
            }
        }
    }
    
    public void SetNeonColor(Color color)
    {
        neonColor = color;
        
        if (neonMaterial != null)
        {
            neonMaterial.SetColor("_EmissionColor", color * 2f);
        }
        
        Light neonLight = GetComponentInChildren<Light>();
        if (neonLight != null)
        {
            neonLight.color = color;
        }
    }
    
    public void SetMoveSpeed(float speed)
    {
        moveSpeed = speed;
    }
    
    public void SetMoveDirection(Vector3 direction)
    {
        moveDirection = direction.normalized;
    }
}
