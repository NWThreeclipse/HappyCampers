using SuperTiled2Unity;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public static CameraController instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // The cursor or any target to follow
    public Transform target;
    // Smoothing factor for following target
    public float smoothSpeed = 5f;
    // Minimum X/Y bounds for camera position
    public Vector2 panLimitMin = new Vector2(-10f, -10f);
    // Maximum X/Y bounds for camera position
    public Vector2 panLimitMax = new Vector2(10f, 10f);

    // Speed of zooming (mouse, keyboard, or gamepad)
    public float zoomSpeed = 2f;
    // Minimum zoom level (orthographic size)
    public float minZoom = 3f;
    // Maximum zoom level (orthographic size)
    public float maxZoom = 10f;

    private Camera cam;
    // Tracks if mouse panning is currently active
    private bool isPanning = false;
    // Last recorded mouse position for panning
    private Vector3 lastMousePosition;
    // Prevents panning/following when enabled (toggle with L)
    // private bool isPanLocked = false; // ⬅ Pan lock toggle
    private bool isPanLocked = true; // ⬅ Pan lock toggle
    Vector3 mapCenter;

    public GameObject map;

    private float outsideLeft;
    private float outsideRight;
    private float outsideBottom;
    private float outsideTop;

    private bool panUp = false;
    private bool panDown = false;
    private bool panLeft = false;
    private bool panRight = false;

    public GameObject player;

    void Start()
    {
        cam = Camera.main;
    }

    void Update()
    {

         Camera cam = Camera.main;
        float camHeight = cam.orthographicSize * 2;                  // world units vertically
        float camWidth = camHeight * cam.aspect;                     // world units horizontally
        Vector2 camSize = new Vector2(camWidth, camHeight);


        IsPlayerNearCameraEdge(player.transform);


        Vector2 panDirection = Vector2.zero;

        if (panUp) panDirection = Vector2.up;
        else if (panDown) panDirection = Vector2.down;
        else if (panLeft) panDirection = Vector2.left;
        else if (panRight) panDirection = Vector2.right;

        if (panDirection != Vector2.zero)
        {
            Vector3 target = PanByDirection(panDirection, zoomSpeed);

            // Stop panning if camera reached the target
            if ((Vector2)transform.position == (Vector2)target)
            {
                panUp = panDown = panLeft = panRight = false;
            }
        }
        

    }

    private void HandlePanGamepad()
    {
        float panX = Input.GetAxis("Horizontal");
        float panY = Input.GetAxis("Vertical");

        Vector3 pan = new Vector3(panX, panY, 0f);

        // Only apply if joystick is moved beyond a tiny threshold
        if (pan.sqrMagnitude > 0.01f)
        {
            // transform.position = ClampPosition(transform.position + pan * zoomSpeed * Time.deltaTime * 5f);
            transform.position = transform.position + pan * zoomSpeed * Time.deltaTime * 5f;
        }
    }

    private Vector3 PanByDirection(Vector2 direction, float speed)
    {
        // direction = normalized (e.g., Vector2.up, Vector2.left)
        // speed = units per second

        Vector3 move = new Vector3(direction.x, direction.y, 0f) * speed * Time.deltaTime;

        float camHeight = Camera.main.orthographicSize * 2f;
        float camWidth = camHeight * Camera.main.aspect;

        Debug.Log($"Camera Width: {camWidth}, Height: {camHeight}");

        Vector3 targetPos = transform.position;



        // transform.position = ClampPosition(transform.position + move);
        // --- DEBUG DIRECTION ---
        string dirName = "";

        if (direction == Vector2.up)
        {
            dirName = "UP";
            targetPos = new Vector3(transform.position.x, player.transform.position.y + (camHeight/2 * 0.8f), transform.position.z);
          
        }
        else if (direction == Vector2.down)
        {
            dirName = "DOWN";
            targetPos = new Vector3(transform.position.x, player.transform.position.y - camHeight/2 * 0.8f, transform.position.z);
        }
        else if (direction == Vector2.left)
        {
            dirName = "LEFT";
            targetPos = new Vector3(player.transform.position.x - camWidth/2 * 0.8f, transform.position.y, transform.position.z);
        }
        else if (direction == Vector2.right)
        {
            dirName = "RIGHT";
            targetPos = new Vector3(player.transform.position.x + camWidth/2 * 0.8f, transform.position.y, transform.position.z);
        }
        else
            dirName = direction.ToString(); // fallback for diagonal or custom dirs


        transform.position = Vector3.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);

        Debug.Log($"Panning {dirName} | Direction: {direction} | Pos: {transform.position} | New Pos: {targetPos}");

        return targetPos;
    }



    bool IsPlayerNearCameraEdge(Transform player, float threshold = 0.05f)
    {
        // Convert player world position to viewport position (0 to 1)
        // (0,0) = bottom-left of screen, (1,1) = top-right
        Vector3 viewportPos = Camera.main.WorldToViewportPoint(player.position);

        bool nearLeft   = viewportPos.x <= threshold;
        bool nearRight  = viewportPos.x >= 1f - threshold;
        bool nearBottom = viewportPos.y <= threshold;
        bool nearTop    = viewportPos.y >= 1f - threshold;

            if (nearLeft)
                // PanByDirection(Vector2.left, zoomSpeed);
                panLeft = true;
            else if (nearRight)
                panRight = true;
            // PanByDirection(Vector2.right, zoomSpeed);

            if (nearBottom)
                // PanByDirection(Vector2.down, zoomSpeed);
                panDown = true;
            else if (nearTop)
                // PanByDirection(Vector2.up, zoomSpeed);
                panUp = true;

        return nearLeft || nearRight || nearBottom || nearTop;
    }





}

