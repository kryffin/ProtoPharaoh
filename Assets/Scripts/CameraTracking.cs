using UnityEngine;

public class CameraTracking : MonoBehaviour
{
    public Transform Player;
    public float FarLeft;
    public float FarRight;

    private void Update()
    {
        Vector3 newPosition = transform.position;
        newPosition.x = Player.position.x;
        newPosition.x = Mathf.Clamp(newPosition.x, FarLeft, FarRight);
        transform.position = newPosition;
    }
}
