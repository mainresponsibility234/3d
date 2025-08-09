using UnityEngine;

public class NeonEnvironment : MonoBehaviour
{
    [Header("Environment Lighting")]
    public Light[] neonLights;
    public Material[] neonMaterials;
    public Color[] neonColorPalette = { Color.cyan, Color.magenta, Color.yellow, Color.red, Color.blue, Color.green };
    
    [Header("Particle Effects")]
    public ParticleSystem[] backgroundParticles;
    public ParticleSystem[] ambientParticles;
    
    [Header("Atmosphere Settings")]
    public float lightPulseSpeed = 1f;
    public float materialPulseSpeed = 2f;
    public float particleIntensity = 1f;
    
    [Header("Audio")]
    public AudioSource ambientAudio;
    public AudioClip[] ambientSounds;
    
    private float lightPulseTimer = 0f;
    private float materialPulseTimer = 0f;
    
    void Start()
    {
        SetupNeonEnvironment();
        SetupParticleEffects();
        SetupAmbientAudio();
    }
    
    void Update()
    {
        UpdateNeonEffects();
        UpdateParticleEffects();
    }
    
    void SetupNeonEnvironment()
    {
        // Setup neon lights
        foreach (Light light in neonLights)
        {
            if (light != null)
            {
                light.color = neonColorPalette[Random.Range(0, neonColorPalette.Length)];
                light.intensity = Random.Range(1f, 3f);
                light.type = LightType.Point;
                light.range = Random.Range(5f, 15f);
            }
        }
        
        // Setup neon materials
        foreach (Material material in neonMaterials)
        {
            if (material != null)
            {
                Color neonColor = neonColorPalette[Random.Range(0, neonColorPalette.Length)];
                material.SetColor("_EmissionColor", neonColor * 2f);
                material.EnableKeyword("_EMISSION");
            }
        }
    }
    
    void SetupParticleEffects()
    {
        // Setup background particles
        foreach (ParticleSystem particles in backgroundParticles)
        {
            if (particles != null)
            {
                var main = particles.main;
                main.startColor = neonColorPalette[Random.Range(0, neonColorPalette.Length)];
                main.startLifetime = Random.Range(2f, 5f);
                main.startSpeed = Random.Range(1f, 3f);
                
                var emission = particles.emission;
                emission.rateOverTime = 20f * particleIntensity;
                
                particles.Play();
            }
        }
        
        // Setup ambient particles
        foreach (ParticleSystem particles in ambientParticles)
        {
            if (particles != null)
            {
                var main = particles.main;
                main.startColor = neonColorPalette[Random.Range(0, neonColorPalette.Length)];
                main.startLifetime = Random.Range(1f, 3f);
                main.startSpeed = Random.Range(0.5f, 1.5f);
                
                var emission = particles.emission;
                emission.rateOverTime = 10f * particleIntensity;
                
                particles.Play();
            }
        }
    }
    
    void SetupAmbientAudio()
    {
        if (ambientAudio != null && ambientSounds.Length > 0)
        {
            ambientAudio.clip = ambientSounds[Random.Range(0, ambientSounds.Length)];
            ambientAudio.loop = true;
            ambientAudio.volume = 0.3f;
            ambientAudio.Play();
        }
    }
    
    void UpdateNeonEffects()
    {
        lightPulseTimer += Time.deltaTime * lightPulseSpeed;
        materialPulseTimer += Time.deltaTime * materialPulseSpeed;
        
        // Update neon lights
        foreach (Light light in neonLights)
        {
            if (light != null)
            {
                float pulse = 1f + Mathf.Sin(lightPulseTimer + light.transform.position.x) * 0.3f;
                light.intensity = 2f * pulse;
            }
        }
        
        // Update neon materials
        foreach (Material material in neonMaterials)
        {
            if (material != null)
            {
                Color emissionColor = material.GetColor("_EmissionColor");
                float pulse = 1f + Mathf.Sin(materialPulseTimer) * 0.2f;
                material.SetColor("_EmissionColor", emissionColor * pulse);
            }
        }
    }
    
    void UpdateParticleEffects()
    {
        // Update particle intensities based on game speed
        float gameSpeed = GameManager.Instance != null ? GameManager.Instance.GetGameSpeed() : 10f;
        float speedMultiplier = Mathf.Clamp01(gameSpeed / 20f);
        
        foreach (ParticleSystem particles in backgroundParticles)
        {
            if (particles != null)
            {
                var emission = particles.emission;
                emission.rateOverTime = 20f * particleIntensity * (1f + speedMultiplier);
            }
        }
    }
    
    public void SetNeonColorPalette(Color[] newPalette)
    {
        neonColorPalette = newPalette;
        SetupNeonEnvironment();
    }
    
    public void SetParticleIntensity(float intensity)
    {
        particleIntensity = intensity;
        SetupParticleEffects();
    }
    
    public void SetLightPulseSpeed(float speed)
    {
        lightPulseSpeed = speed;
    }
    
    public void SetMaterialPulseSpeed(float speed)
    {
        materialPulseSpeed = speed;
    }
}
