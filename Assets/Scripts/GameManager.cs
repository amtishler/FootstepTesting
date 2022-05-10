using UnityEngine;

public class GameManager : MonoBehaviour
{
    public float Doom = 0f;

    // Increases doom when called
    public void incrementDoom ()
    {
        if (Doom <= 13f)
        {
            Doom += 1f;
        }
        Debug.Log("Doom is now " + Doom);
    }
}
