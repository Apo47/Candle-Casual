using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform target;  // Mumun Transform bile�eni
    [SerializeField] private float distance = 5f;  // Kameran�n mumdan olan sabit mesafesi

    void Update()
    {
        if (target != null)
        {
            // Kameran�n yeni pozisyonunu belirle
            Vector3 newPosition = new Vector3(transform.position.x, transform.position.y, target.position.z - distance);

            // Kameray� yeni pozisyona ta��
            transform.position = newPosition;
        }
    }
}
