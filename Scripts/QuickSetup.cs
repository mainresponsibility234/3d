using UnityEngine;
using UnityEngine.UI;

public class QuickSetup : MonoBehaviour
{
    [Header("Setup Options")]
    public bool setupPlayer = true;
    public bool setupCamera = true;
    public bool setupGameManager = true;
    public bool setupObstacleGenerator = true;
    public bool setupNeonEnvironment = true;
    public bool setupUI = true;
    
    [Header("Player Settings")]
    public Vector3 playerStartPosition = new Vector3(0, 1, 0);
    public float playerSpeed = 10f;
    public float jumpForce = 15f;
    
    [Header("Camera Settings")]
    public Vector3 cameraOffset = new Vector3(0, 5, -10);
    public float cameraSmoothSpeed = 5f;
    
    [Header("Neon Colors")]
    public Color[] neonColors = { Color.cyan, Color.magenta, Color.yellow, Color.red, Color.blue, Color.green };
    
    void Start()
    {
        if (setupPlayer) SetupPlayer();
        if (setupCamera) SetupCamera();
        if (setupGameManager) SetupGameManager();
        if (setupObstacleGenerator) SetupObstacleGenerator();
        if (setupNeonEnvironment) SetupNeonEnvironment();
        if (setupUI) SetupUI();
    }
    
    void SetupPlayer()
    {
        // Create Player GameObject
        GameObject player = new GameObject("Player");
        player.transform.position = playerStartPosition;
        player.tag = "Player";
        
        // Add CharacterController
        CharacterController controller = player.AddComponent<CharacterController>();
        controller.height = 2f;
        controller.radius = 0.5f;
        
        // Add PlayerController script
        PlayerController playerController = player.AddComponent<PlayerController>();
        playerController.moveSpeed = playerSpeed;
        playerController.jumpForce = jumpForce;
        
        // Create player model (capsule)
        GameObject playerModel = GameObject.CreatePrimitive(PrimitiveType.Capsule);
        playerModel.transform.SetParent(player.transform);
        playerModel.transform.localPosition = Vector3.zero;
        playerModel.name = "PlayerModel";
        
        // Add neon material
        NeonMaterial neonMat = playerModel.AddComponent<NeonMaterial>();
        neonMat.neonColor = neonColors[0];
        
        // Add trail renderer
        TrailRenderer trail = playerModel.AddComponent<TrailRenderer>();
        trail.startColor = neonColors[0];
        trail.endColor = neonColors[0];
        trail.startWidth = 0.3f;
        trail.endWidth = 0.1f;
        trail.time = 0.5f;
        
        // Add neon light
        Light neonLight = playerModel.AddComponent<Light>();
        neonLight.type = LightType.Point;
        neonLight.range = 3f;
        neonLight.intensity = 2f;
        neonLight.color = neonColors[0];
        
        Debug.Log("Player setup complete!");
    }
    
    void SetupCamera()
    {
        Camera mainCamera = Camera.main;
        if (mainCamera != null)
        {
            CameraFollow cameraFollow = mainCamera.gameObject.AddComponent<CameraFollow>();
            cameraFollow.offset = cameraOffset;
            cameraFollow.smoothSpeed = cameraSmoothSpeed;
            
            // Add camera light
            Light cameraLight = mainCamera.gameObject.AddComponent<Light>();
            cameraLight.type = LightType.Spot;
            cameraLight.range = 20f;
            cameraLight.spotAngle = 30f;
            cameraLight.intensity = 1f;
            cameraLight.color = neonColors[1];
            
            cameraFollow.cameraLight = cameraLight;
            
            Debug.Log("Camera setup complete!");
        }
    }
    
    void SetupGameManager()
    {
        GameObject gameManager = new GameObject("GameManager");
        GameManager gm = gameManager.AddComponent<GameManager>();
        
        Debug.Log("GameManager setup complete!");
    }
    
    void SetupObstacleGenerator()
    {
        GameObject obstacleGenerator = new GameObject("ObstacleGenerator");
        ObstacleGenerator og = obstacleGenerator.AddComponent<ObstacleGenerator>();
        
        Debug.Log("ObstacleGenerator setup complete!");
    }
    
    void SetupNeonEnvironment()
    {
        GameObject neonEnvironment = new GameObject("NeonEnvironment");
        NeonEnvironment ne = neonEnvironment.AddComponent<NeonEnvironment>();
        ne.neonColorPalette = neonColors;
        
        // Add some ambient lights
        for (int i = 0; i < 5; i++)
        {
            GameObject light = new GameObject("NeonLight_" + i);
            light.transform.SetParent(neonEnvironment.transform);
            light.transform.position = new Vector3(Random.Range(-20f, 20f), Random.Range(5f, 15f), Random.Range(-20f, 20f));
            
            Light neonLight = light.AddComponent<Light>();
            neonLight.type = LightType.Point;
            neonLight.range = Random.Range(5f, 15f);
            neonLight.intensity = Random.Range(1f, 3f);
            neonLight.color = neonColors[Random.Range(0, neonColors.Length)];
        }
        
        Debug.Log("NeonEnvironment setup complete!");
    }
    
    void SetupUI()
    {
        // Create Canvas
        GameObject canvas = new GameObject("Canvas");
        Canvas canvasComponent = canvas.AddComponent<Canvas>();
        canvasComponent.renderMode = RenderMode.ScreenSpaceOverlay;
        
        CanvasScaler scaler = canvas.AddComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(1920, 1080);
        
        canvas.AddComponent<GraphicRaycaster>();
        
        // Create UI Panels
        CreateUIPanel(canvas, "MainMenuPanel");
        CreateUIPanel(canvas, "GamePanel");
        CreateUIPanel(canvas, "GameOverPanel");
        CreateUIPanel(canvas, "PausePanel");
        
        // Add UIManager
        UIManager uiManager = canvas.AddComponent<UIManager>();
        
        Debug.Log("UI setup complete!");
    }
    
    void CreateUIPanel(GameObject parent, string panelName)
    {
        GameObject panel = new GameObject(panelName);
        panel.transform.SetParent(parent.transform, false);
        
        RectTransform rectTransform = panel.AddComponent<RectTransform>();
        rectTransform.anchorMin = Vector2.zero;
        rectTransform.anchorMax = Vector2.one;
        rectTransform.offsetMin = Vector2.zero;
        rectTransform.offsetMax = Vector2.zero;
        
        Image panelImage = panel.AddComponent<Image>();
        panelImage.color = new Color(0, 0, 0, 0.8f);
    }
}
