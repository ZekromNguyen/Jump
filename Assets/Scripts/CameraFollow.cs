using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;
    public float roomHeight = 9f; // Chiều cao của từng phòng (phù hợp với tỷ lệ màn hình)
    private Vector3 targetPosition;

    void Start()
    {
        if (player == null)
        {
            player = GameObject.Find("PLAYER").transform;
        }

        // Đặt camera ở vị trí ban đầu
        targetPosition = GetRoomPosition(player.position);
        Camera.main.transform.position = targetPosition;
    }

    void Update()
    {
        // Nếu nhân vật ra khỏi giới hạn theo chiều dọc, chuyển camera lên/xuống
        if (IsOutOfRoom(player.position))
        {
            targetPosition = GetRoomPosition(player.position);
            Camera.main.transform.position = targetPosition;
        }
    }

    // Tính toán vị trí phòng mới (chỉ thay đổi theo trục Y)
    private Vector3 GetRoomPosition(Vector3 referencePos)
    {
        int roomIndex = Mathf.RoundToInt(referencePos.y / roomHeight); // Chuyển vị trí Y về số nguyên
        float newY = roomIndex * roomHeight; // Xác định vị trí mới của phòng

        return new Vector3(Camera.main.transform.position.x, newY, Camera.main.transform.position.z);
    }





    // Kiểm tra nhân vật có ra khỏi phòng theo chiều dọc không
    private bool IsOutOfRoom(Vector3 playerPos)
    {
        float roomCenterY = Camera.main.transform.position.y;
        float halfHeight = roomHeight / 2f;

        return playerPos.y >= roomCenterY + halfHeight || playerPos.y <= roomCenterY - halfHeight;
    }


}
