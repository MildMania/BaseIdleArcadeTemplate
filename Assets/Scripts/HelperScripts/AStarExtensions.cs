using Pathfinding;
using UnityEngine;

public static class AStarExtensions
{
    public static bool IsPathPossibleFrom(
        this Vector3 from,
        Vector3 to)
    {
        GraphNode node1 = AstarPath.active.GetNearest(from, NNConstraint.Default).node;
        GraphNode node2 = AstarPath.active.GetNearest(to, NNConstraint.Default).node;

        return PathUtilities.IsPathPossible(node1, node2);
    }

    public static bool IsOnGraph(
        Vector3 target,
        float distTreshold,
        out Vector3 position,
        bool isWalkable = true,
        bool checkXZ = true)
    {
        position = default;

        NNConstraint nnConstraint = NNConstraint.Default;
        nnConstraint.distanceXZ = true;
        NNInfo nodeInfo = AstarPath.active.GetNearest(target, nnConstraint);

        if (isWalkable && !nodeInfo.node.Walkable)
            return false;

        Vector3 nearestPos = nodeInfo.position;

        if (checkXZ)
            target.y = nearestPos.y;

        float distance = Vector3.Distance(target, nearestPos);

        if (distance < distTreshold)
        {
            position = nearestPos;
            return true;
        }
        else
            return false;
    }

    public static bool IsOnGraph(
        this Seeker seeker,
        float distTreshold,
        out Vector3 position,
        bool checkXZ = true)
    {
        position = default;

        Vector3 nearestPos = AstarPath.active.GetNearest(seeker.transform.position).position;

        Vector3 checkPos = nearestPos;

        if (checkXZ)
            checkPos.y = seeker.transform.position.y;

        float distance = Vector3.Distance(seeker.transform.position, checkPos);
        if (distance < distTreshold)
        {
            position = nearestPos;
            return true;
        }
        else
            return false;
    }

    public static Vector3 RandomPosition(
        this Seeker seeker,
        float maxRadius,
        float minRadius = 0)
    {
        float radius = Random.Range(minRadius, maxRadius);

        Vector3 randPosition = Random.onUnitSphere * radius;
        randPosition += seeker.transform.position;
        randPosition.y = seeker.transform.position.y;

        Vector3 nearestPos = AstarPath.active.GetNearest(randPosition).position;

        return nearestPos;
    }
}