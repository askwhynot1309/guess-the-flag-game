using UnityEngine;

public class FootDetector : MonoBehaviour
{
    public bool IsFootOver { get; private set; }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Foot"))
        {
            IsFootOver = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Foot"))
        {
            IsFootOver = false;
        }
    }
}
