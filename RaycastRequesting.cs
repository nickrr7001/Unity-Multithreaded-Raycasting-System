using System.Collections.Generic;
using Unity.Jobs;
using Unity.Collections;
using UnityEngine;
    public class RaycastRequesting : MonoBehaviour
    {
    private static List<RaycastRequest> requests = new List<RaycastRequest>();
    private static RaycastHit[] hits;
    public static RaycastHit getResponse(int i)
    {
        return hits[i];
    }
    public static int NewRequest(RaycastRequest request)
    {
        requests.Add(request);
        return requests.Count-1;
    }
    private void LateUpdate()
    {
        NativeArray<RaycastHit> responses = new NativeArray<RaycastHit>(requests.Count, Allocator.TempJob);
        var commands = new NativeArray<RaycastCommand>(requests.Count, Allocator.TempJob);
        for (int i = 0; i < requests.Count; i++)
        {
            commands[i] = new RaycastCommand(requests[i].CastPoint,
                requests[i].CastDirection,requests[i].distance,requests[i].mask);

        }
        JobHandle handle = RaycastCommand.ScheduleBatch(commands, responses, 1, default(JobHandle));
        handle.Complete();
        hits = responses.ToArray();
        responses.Dispose();
        requests = new List<RaycastRequest>();
        commands.Dispose();
    }
 }
public struct RaycastRequest
{
    public Vector3 CastPoint;
    public Vector3 CastDirection;
    public LayerMask mask;
    public float distance;
    public RaycastRequest(Vector3 _point, Vector3 _dir, LayerMask m, float d)
    {
        CastPoint = _point;
        CastDirection = _dir;
        mask = m;
        distance = d;
    }
}
