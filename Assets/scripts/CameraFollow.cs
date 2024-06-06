using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform target;  // Mumun Transform bileþeni
    [SerializeField] private float distance = 5f;  // Kameranýn mumdan olan sabit mesafesi

    void Update()
    {
        if (target != null)
        {
            // Kameranýn yeni pozisyonunu belirle
            Vector3 newPosition = new Vector3(transform.position.x, transform.position.y, target.position.z - distance);

            // Kamerayý yeni pozisyona taþý
            transform.position = newPosition;
        }
    }
}
