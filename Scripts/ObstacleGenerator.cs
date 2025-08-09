using UnityEngine;
using System.Collections.Generic;

public class ObstacleGenerator : MonoBehaviour
{
    [Header("Obstacle Settings")]
    public GameObject[] obstaclePrefabs;
    public GameObject[] collectiblePrefabs;
    public float spawnDistance = 50f;
    public float minSpawnInterval = 2f;
    public float maxSpawnInterval = 5f;
    public float laneWidth = 3f;
    
    [Header("Neon Effects")]
    public Material neonObstacleMaterial;
    public Color[] neonColors = { Color.red, Color.blue, Color.green, Color.yellow, Color.magenta, Color.cyan };
    
    [Header("Difficulty")]
    public float difficultyIncreaseRate = 0.1f;
    public float maxDifficulty = 2f;
    
    private Transform player;
    private float lastSpawnZ = 0f;
    private float nextSpawnTime = 0f;
    private float currentDifficulty = 1f;
    private List<GameObject> activeObstacles = new List<GameObject>();
    
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        
        if (player == null)
        {
            Debug.LogError("Player not found! Make sure to tag your player with 'Player'");
        }
    }
    
    void Update()
    {
        if (player == null || !GameManager.Instance.IsGameActive()) return;
        
        // Increase difficulty over time
        currentDifficulty = Mathf.Min(maxDifficulty, 1f + (Time.time * difficultyIncreaseRate));
        
        // Check if we need to spawn new obstacles
        if (player.position.z + spawnDistance > lastSpawnZ)
        {
            SpawnObstacles();
        }
        
        // Clean up old obstacles
        CleanupObstacles();
    }
    
    void SpawnObstacles()
    {
        float spawnZ = lastSpawnZ + Random.Range(minSpawnInterval, maxSpawnInterval);
        lastSpawnZ = spawnZ;
        
        // Determine number of obstacles based on difficulty
        int obstacleCount = Mathf.RoundToInt(Random.Range(1, 3) * currentDifficulty);
        
        for (int i = 0; i < obstacleCount; i++)
        {
            SpawnObstacle(spawnZ + (i * 5f));
        }
        
        // Spawn collectibles
        if (Random.Range(0f, 1f) < 0.3f)
        {
            SpawnCollectible(spawnZ + Random.Range(10f, 20f));
        }
    }
    
    void SpawnObstacle(float zPosition)
    {
        if (obstaclePrefabs.Length == 0) return;
        
        // Choose random obstacle
        GameObject obstaclePrefab = obstaclePrefabs[Random.Range(0, obstaclePrefabs.Length)];
        
        // Choose random lane
        int lane = Random.Range(0, 3);
        float xPosition = (lane - 1) * laneWidth;
        
        Vector3 spawnPosition = new Vector3(xPosition, 0f, zPosition);
        GameObject obstacle = Instantiate(obstaclePrefab, spawnPosition, Quaternion.identity);
        
        // Apply neon effects
        ApplyNeonEffects(obstacle);
        
        activeObstacles.Add(obstacle);
    }
    
    void SpawnCollectible(float zPosition)
    {
        if (collectiblePrefabs.Length == 0) return;
        
        GameObject collectiblePrefab = collectiblePrefabs[Random.Range(0, collectiblePrefabs.Length)];
        
        // Choose random lane
        int lane = Random.Range(0, 3);
        float xPosition = (lane - 1) * laneWidth;
        
        Vector3 spawnPosition = new Vector3(xPosition, 1f, zPosition);
        GameObject collectible = Instantiate(collectiblePrefab, spawnPosition, Quaternion.identity);
        
        // Apply neon effects to collectible
        ApplyNeonEffects(collectible);
        
        activeObstacles.Add(collectible);
    }
    
    void ApplyNeonEffects(GameObject obj)
    {
        // Apply neon material
        Renderer[] renderers = obj.GetComponentsInChildren<Renderer>();
        foreach (Renderer renderer in renderers)
        {
            if (neonObstacleMaterial != null)
            {
                Material neonMat = new Material(neonObstacleMaterial);
                neonMat.SetColor("_EmissionColor", neonColors[Random.Range(0, neonColors.Length)]);
                neonMat.EnableKeyword("_EMISSION");
                renderer.material = neonMat;
            }
        }
        
        // Add neon light
        Light neonLight = obj.GetComponentInChildren<Light>();
        if (neonLight == null)
        {
            neonLight = obj.AddComponent<Light>();
        }
        
        neonLight.type = LightType.Point;
        neonLight.range = 5f;
        neonLight.intensity = 2f;
        neonLight.color = neonColors[Random.Range(0, neonColors.Length)];
        
        // Add particle effect
        ParticleSystem particles = obj.GetComponentInChildren<ParticleSystem>();
        if (particles == null)
        {
            particles = obj.AddComponent<ParticleSystem>();
        }
        
        var main = particles.main;
        main.startColor = neonLight.color;
        main.startLifetime = 1f;
        main.startSpeed = 2f;
        main.maxParticles = 20;
        
        var emission = particles.emission;
        emission.rateOverTime = 10f;
        
        var shape = particles.shape;
        shape.shapeType = ParticleSystemShapeType.Sphere;
        shape.radius = 0.5f;
    }
    
    void CleanupObstacles()
    {
        for (int i = activeObstacles.Count - 1; i >= 0; i--)
        {
            if (activeObstacles[i] == null)
            {
                activeObstacles.RemoveAt(i);
            }
            else if (activeObstacles[i].transform.position.z < player.position.z - 20f)
            {
                Destroy(activeObstacles[i]);
                activeObstacles.RemoveAt(i);
            }
        }
    }
    
    public void ClearAllObstacles()
    {
        foreach (GameObject obstacle in activeObstacles)
        {
            if (obstacle != null)
            {
                Destroy(obstacle);
            }
        }
        activeObstacles.Clear();
    }
}
