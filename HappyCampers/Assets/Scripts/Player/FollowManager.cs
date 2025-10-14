using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class FollowManager : MonoBehaviour
{
    public List<Camper> followers = new List<Camper>();

    public List<Vector3> trailPoints = new List<Vector3>();
    public float spacing = 1.5f; // desired distance between followers
    public int maxPoints = 10;   // max points to store

    public int StartFollow(Camper camper)
    {
        if (!followers.Contains(camper))
            followers.Add(camper); // Add camper to list of followers

        UpdateFollowerIndex();

        return camper.followIndex; // now always updated
    }

    public void StopFollow(Camper camper)
    {
        if (followers.Contains(camper))
            followers.Remove(camper); // Remove camper from list of followers

        UpdateFollowerIndex();
    }

    public void UpdateFollowerIndex()
    {
        for (int i = 0; i < followers.Count; i++)
        {
            followers[i].followIndex = i; // assign consecutive indices
        }
    }

    private void FixedUpdate()
    {
        Vector3 currentPos = transform.position;

        // Only add a new point if the player has moved enough
        if (trailPoints.Count == 0 || Vector3.Distance(currentPos, trailPoints[0]) >= spacing)
        {
            trailPoints.Insert(0, currentPos);

            // Keep the list from growing too large
            if (trailPoints.Count > maxPoints)
                trailPoints.RemoveAt(trailPoints.Count - 1);
        }
    }
}
