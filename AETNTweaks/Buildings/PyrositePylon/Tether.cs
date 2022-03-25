using System.Collections.Generic;
using UnityEngine;

// TODO: once settled (time? small enough motion?), stop the simulation.
// largely based on this video by Jasony: https://youtu.be/FcnvwtyxLds
public class Tether : KMonoBehaviour, ISim33ms
{
    [SerializeField]
    public Transform A;

    [SerializeField]
    public Transform B;

    Vector3 endPoint;

    [MyCmpReq]
    LineRenderer lineRenderer;

    [SerializeField]
    public float segmentLength = 0.15f;

    [SerializeField]
    public int subDivisionCount = 35;

    [SerializeField]
    public int constraintIteration = 10;

    [SerializeField]
    public float changeModifier = 0.5f;

    [SerializeField]
    public Vector2 gravity = new Vector2(0, -1f);

    public List<Segment> segments;

    internal void Settle()
    {
        // start sim
    }

    public void SetEnds(Transform start, Transform end)
    {
        if (A == start && B == end) return;

        A = start;
        B = end;

        Vector3 startPoint = A.position;

        segments = new List<Segment>();

        for (int i = 0; i < subDivisionCount; i++)
        {
            segments.Add(new Segment(startPoint));
            startPoint.y -= segmentLength;
        }

        Draw();
    }

    private void Simulate(float dt)
    {
        for (int i = 1; i < subDivisionCount; i++)
        {
            var first = segments[i];
            var velocity = first.currentPos - first.previousPos;
            first.previousPos = first.currentPos;
            first.currentPos += velocity;
            first.currentPos += gravity * dt;
            segments[i] = first;
        }

        for (int i = 0; i < constraintIteration; i++)
        {
            ApplyConstraint();
        }
    }

    private void ApplyConstraint()
    {
        var startSegment = segments[0];
        startSegment.currentPos = A.position;
        segments[0] = startSegment;

        var endSegment = segments[subDivisionCount - 1];
        endSegment.currentPos = B.position;
        segments[subDivisionCount - 1] = endSegment;

        for (int i = 0; i < subDivisionCount - 1; i++)
        {
            var first = segments[i];
            var second = segments[i + 1];

            var dist = (first.currentPos - second.currentPos).magnitude;
            var error = Mathf.Abs(dist - segmentLength);

            var changeDirection = Vector2.zero;

            if (dist > segmentLength)
            {
                changeDirection = (first.currentPos - second.currentPos).normalized;
            }
            else if (dist < segmentLength)
            {
                changeDirection = (second.currentPos - first.currentPos).normalized;
            }

            var changeAmount = changeDirection * error;

            if (i != 0)
            {
                first.currentPos -= changeAmount * changeModifier;
                segments[i] = first;

                second.currentPos += changeAmount * changeModifier;
                segments[i + 1] = second;
            }
            else
            {
                second.currentPos += changeAmount;
                segments[i + 1] = second;
            }
        }

    }

    internal void Stop()
    {
        throw new System.NotImplementedException();
    }

    private void Draw()
    {
        var positions = new Vector3[subDivisionCount];
        for (int i = 0; i < subDivisionCount; i++)
        {
            positions[i] = segments[i].currentPos;

        }

        lineRenderer.positionCount = subDivisionCount;
        lineRenderer.SetPositions(positions);
    }

    public void Sim33ms(float dt)
    {
        if (segments == null || B == null || A == null)
        {
            return;
        }

        Simulate(dt);
        Draw();
    }

    public struct Segment
    {
        public Vector2 currentPos;
        public Vector2 previousPos;

        public Segment(Vector2 pos)
        {
            currentPos = pos;
            previousPos = pos;
        }
    }
}
