using UnityEngine;
using UnityEngine.XR.ARFoundation;

[RequireComponent(typeof(ARSessionOrigin))]
[RequireComponent(typeof(ARPlaneManager))]
[RequireComponent(typeof(ARRaycastManager))]
public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject miniWorld;
    [SerializeField] private ScanAnimationController scanAnimation;
    [SerializeField] private ARSession arSession;

    public static GameManager Instance { get; private set; }
    public ARSession ArSession { get => arSession; set => arSession = value; }
    public GameObject MiniWorld { get => miniWorld; set => miniWorld = value; }
    public ARPlaneManager PlaneManager { get => planeManager; set => planeManager = value; }
    public ARRaycastManager RaycastManager { get => raycastManager; set => raycastManager = value; }
    public ScanAnimationController ScanAnimation { get => scanAnimation; set => scanAnimation = value; }        

    private ARRaycastManager raycastManager;
    private ARPlaneManager planeManager;
    private GameManagerState currentState;        
    
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            Initialize();
        }
    }

    public void Initialize()
    {
        MiniWorld.SetActive(false);
        ScanAnimation.StopAllAnimations();
        PlaneManager = GetComponent<ARPlaneManager>();
        RaycastManager = GetComponent<ARRaycastManager>();
        SetState(new ScanState(Instance));
    }

    public void SetState(GameManagerState gameState)
    {
        if (currentState != null)
        {
            currentState.OnStateExit();
        }

        currentState = gameState;
        gameObject.name = gameState.GetType().Name;

        if (currentState != null)
        {
            currentState.OnStateEnter();
        }
    }

    private void Update()
    {
        currentState.Tick();
    }

}