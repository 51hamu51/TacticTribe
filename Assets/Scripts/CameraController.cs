using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float dragSpeed = 2f; // ドラッグの感度
    private Vector3 lastMousePos;

    void Update()
    {
        // 左クリック or タッチしたままドラッグ
        if (Input.GetMouseButtonDown(0))
        {
            lastMousePos = Input.mousePosition;
        }

        if (Input.GetMouseButton(0))
        {
            Vector3 delta = Input.mousePosition - lastMousePos;

            // カメラを移動（XZ平面）
            transform.Translate(-delta.x * dragSpeed * Time.deltaTime, 0, -delta.y * dragSpeed * Time.deltaTime, Space.World);

            lastMousePos = Input.mousePosition;
        }
    }
}
