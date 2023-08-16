using UnityEngine;
using System.Collections.Generic;

public class ClimbColliderCheck : MonoBehaviour
{
    public bool roomForPlayer;
    private int objectsInside = 0;
    Vector3 position;

    void CheckForPlayerSpace()
    {
        Collider[] overlappingColliders = Physics.OverlapBox(gameObject.transform.position, gameObject.transform.localScale * 0.5f);

        // Filtering out the ClimbableObject
        List<Collider> filteredColliders = new List<Collider>();
        foreach (Collider col in overlappingColliders)
        {
            if (col.gameObject.tag != "ClimbableObject")
            {
                filteredColliders.Add(col);
                Debug.Log("Detected overlapping object: " + col.name);
            }
        }
        overlappingColliders = filteredColliders.ToArray();

        // If there's only the cube itself, then there's room for the player.
        roomForPlayer = overlappingColliders.Length <= 1;
    }

    private void Update()
    {
        position = gameObject.transform.position;
        CheckForPlayerSpace();
    }
}
