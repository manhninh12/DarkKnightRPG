using UnityEngine;

public class TriggerDebug : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("ENTERED " + name + " by " + other.name + " tag=" + other.tag);
    }
}