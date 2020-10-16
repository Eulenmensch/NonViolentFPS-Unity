using UnityEngine;
using UnityEngine.AI;

public static class NavMeshExtensions
{
    public static Vector3 RandomPosition(this NavMeshAgent agent, float radius)
    {
        var randDirection = Random.insideUnitSphere * radius;
        randDirection += agent.transform.position;
        NavMeshHit navHit;
        NavMesh.SamplePosition( randDirection, out navHit, radius, -1 );
        return navHit.position;
    }
}