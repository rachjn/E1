
using UnityEngine;

public class Coin : MonoBehaviour
{
    [SerializeField] private int value;
    private bool hasTriggered;
    // Start is called before the first frame update
    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.CompareTag("Player") && !hasTriggered) {
            hasTriggered = true;
            Destroy(gameObject);
        }
    }
}
