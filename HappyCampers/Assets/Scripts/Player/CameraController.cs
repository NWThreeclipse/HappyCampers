using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Follow Target")]
    public Transform player;

    [Header("Movement Clamps")]
    public Vector2 minPosition; // bottom-left corner
    public Vector2 maxPosition; // top-right corner

    [Header("Follow settings")]
    public float smoothSpeed = 0.1f;

    private void Update()
    {
        if (player == null) return;

        // Clamp within bounds
        float clampedX = Mathf.Clamp(player.position.x, minPosition.x, maxPosition.x);
        float clampedY = Mathf.Clamp(player.position.y, minPosition.y, maxPosition.y);

        Vector3 clampedPosition = new Vector3(clampedX, clampedY, transform.position.z);

        // Smooth follow
        transform.position = Vector3.Lerp(transform.position, clampedPosition, smoothSpeed);
    }
}
