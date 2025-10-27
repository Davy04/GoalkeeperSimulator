using UnityEngine;

public class Goalkeeper : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 10f;
    [SerializeField] private float limitX = 3.5f;
    [SerializeField] private float limitY = 2f;
    [SerializeField] private bool moveVertically = true;

    private Camera mainCamera;
    private Vector3 targetPosition;

    private void Start()
    {
        gameObject.tag = "Player"; // garanta a tag
        mainCamera = Camera.main;
        targetPosition = transform.position;
    }

    private void Update()
    {
        Vector3 mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0f;

        targetPosition = moveVertically
            ? new Vector3(mousePos.x, mousePos.y, 0f)
            : new Vector3(mousePos.x, transform.position.y, 0f);

        targetPosition.x = Mathf.Clamp(targetPosition.x, -limitX, limitX);
        targetPosition.y = Mathf.Clamp(targetPosition.y, -limitY, limitY);

        transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * moveSpeed);
    }
}