using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 10f;
    public float jumpForce = 15f;
    public float gravity = -30f;
    public float laneDistance = 3f;
    
    [Header("Neon Effects")]
    public TrailRenderer neonTrail;
    public ParticleSystem runParticles;
    public Light neonLight;
    public Color neonColor = Color.cyan;
    
    [Header("Audio")]
    public AudioSource jumpSound;
    public AudioSource runSound;
    
    private CharacterController controller;
    private Vector3 velocity;
    private int currentLane = 1; // 0=left, 1=center, 2=right
    private bool isGrounded;
    private bool isGameOver = false;
    
    void Start()
    {
        controller = GetComponent<CharacterController>();
        
        // Setup neon effects
        if (neonTrail != null)
        {
            neonTrail.startColor = neonColor;
            neonTrail.endColor = neonColor;
        }
        
        if (neonLight != null)
        {
            neonLight.color = neonColor;
        }
        
        // Start run particles
        if (runParticles != null)
        {
            runParticles.Play();
        }
    }
    
    void Update()
    {
        if (isGameOver) return;
        
        HandleMovement();
        HandleJump();
        HandleLaneChange();
        UpdateNeonEffects();
    }
    
    void HandleMovement()
    {
        // Forward movement (automatic)
        Vector3 forwardMovement = Vector3.forward * moveSpeed;
        
        // Apply gravity
        if (controller.isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }
        
        velocity.y += gravity * Time.deltaTime;
        
        // Combine movements
        Vector3 movement = forwardMovement + velocity;
        controller.Move(movement * Time.deltaTime);
    }
    
    void HandleJump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpForce * -2f * gravity);
            
            if (jumpSound != null)
            {
                jumpSound.Play();
            }
        }
    }
    
    void HandleLaneChange()
    {
        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (currentLane > 0)
            {
                currentLane--;
            }
        }
        else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (currentLane < 2)
            {
                currentLane++;
            }
        }
        
        // Calculate target position
        float targetX = (currentLane - 1) * laneDistance;
        Vector3 targetPosition = new Vector3(targetX, transform.position.y, transform.position.z);
        
        // Smooth lane change
        transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * 5f);
    }
    
    void UpdateNeonEffects()
    {
        // Update neon trail intensity based on speed
        if (neonTrail != null)
        {
            float intensity = Mathf.Clamp01(moveSpeed / 15f);
            neonTrail.startWidth = 0.3f * intensity;
            neonTrail.endWidth = 0.1f * intensity;
        }
        
        // Update neon light intensity
        if (neonLight != null)
        {
            neonLight.intensity = 2f + Mathf.Sin(Time.time * 3f) * 0.5f;
        }
    }
    
    public void GameOver()
    {
        isGameOver = true;
        
        if (runParticles != null)
        {
            runParticles.Stop();
        }
        
        if (neonTrail != null)
        {
            neonTrail.enabled = false;
        }
    }
    
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Obstacle"))
        {
            GameOver();
        }
        else if (other.CompareTag("Collectible"))
        {
            // Handle collectible pickup
            GameManager.Instance.AddScore(10);
            Destroy(other.gameObject);
        }
    }
}
