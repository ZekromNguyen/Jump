using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;
    public float smoothSpeed = 0.1f;
    public float verticalOffset = -2f;

    void Start()
    {
        if (player == null)
        {
            player = GameObject.Find("PLAYER").transform;
        }

        // Đặt camera đúng vị trí ngay từ đầu, tránh bị giật lùi
        transform.position = new Vector3(transform.position.x, player.position.y - verticalOffset, transform.position.z);
    }

    void Update()
    {
        float targetY = player.position.y - verticalOffset;

        transform.position = new Vector3(
            transform.position.x,
            Mathf.Lerp(transform.position.y, targetY, smoothSpeed),
            transform.position.z
        );
    }
}

