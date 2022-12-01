using System;
using UnityEngine;
using System.Collections;

public class MoveModule : MonoBehaviour
{
    public Transform target;
    public Vector3 targetPos;
    public Vector3 beforeMovePos;
    float speed = 2;
    public Vector3[] path;
    int targetIndex;
    public Action<Vector3, float, float, bool> OnCompleteMove;
    public Action<Vector3, float, float, bool> OnBeforeMove;

    public Vector3 targetPosConvert()
    {
        if (target)
        {
            var position = target.position;
            targetPos = new Vector3(position.x, 0, position.y);
        }

        targetPos = new Vector3(targetPos.x, 0, targetPos.y);

        return targetPos;
    }

    public void Move()
    {
        beforeMovePos = Vector3.ClampMagnitude(transform.position, 10);
        StartCoroutine(BeforeMove(beforeMovePos, 32, 32));
        PathRequestManager.RequestPath(transform.position, targetPosConvert(), OnPathFound);
    }

    public void Move(Vector3 targetPos)
    {
        this.targetPos = targetPos;
        var position = transform.position;
        beforeMovePos = Vector3.ClampMagnitude(position, 10);
        OnCompleteMove = GridManager.Instance.UpdateGrid;
        OnBeforeMove = GridManager.Instance.UpdateGrid;
        StartCoroutine(BeforeMove(beforeMovePos, 32, 32));
        PathRequestManager.RequestPath(position, targetPosConvert(), OnPathFound);
    }

    public void OnPathFound(Vector3[] newPath, bool pathSuccessful)
    {
        if (pathSuccessful)
        {
            path = newPath;
            targetIndex = 0;
            StopCoroutine("FollowPath");
            StartCoroutine("FollowPath");
        }
    }

    IEnumerator FollowPath()
    {
        Vector3 currentWaypoint = path[0];
        while (true)
        {
            if (transform.position == currentWaypoint)
            {
                targetIndex++;
                if (targetIndex >= path.Length)
                {
                    CompleteMove();
                    yield break;
                }

                currentWaypoint = path[targetIndex];
            }

            transform.position = Vector3.MoveTowards(transform.position, currentWaypoint, speed * Time.deltaTime);
            yield return null;
        }
    }

    private IEnumerator BeforeMove(Vector3 pos, float height, float width)
    {
        yield return new WaitForSeconds(0.25f);
        OnBeforeMove.Invoke(pos, height, width, true);
        yield return null;
    }

    public void CompleteMove()
    {
        OnCompleteMove.Invoke(transform.position, 32, 32, false);
    }

    public void OnDrawGizmos()
    {
        if (path != null)
        {
            for (int i = targetIndex; i < path.Length; i++)
            {
                Gizmos.color = Color.black;
                Gizmos.DrawCube(path[i], Vector3.one);

                if (i == targetIndex)
                {
                    Gizmos.DrawLine(transform.position, path[i]);
                }
                else
                {
                    Gizmos.DrawLine(path[i - 1], path[i]);
                }
            }
        }
    }
}