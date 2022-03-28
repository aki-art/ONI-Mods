using FUtility;
using KSerialization;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TODO: once settled (time? small enough motion?), stop the simulation.
// largely based on this video by Jasony: https://youtu.be/FcnvwtyxLds
[SerializationConfig(MemberSerialization.OptIn)]
public class Tether : KMonoBehaviour//, ISim33ms
{
    [SerializeField]
    public Transform A;

    [SerializeField]
    public Transform B;

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

    [MyCmpReq]
    private LineRenderer lineRenderer;

    internal void Settle(float dt)
    {
        if (segments != null && B != null && A != null)
        {
            Simulate(dt);
            Draw();
        }
    }

    public void Reset()
    {
        Vector3 startPoint = A.position;

        segments = null;
        segments = new List<Segment>();

        for (int i = 0; i < subDivisionCount; i++)
        {
            segments.Add(new Segment(startPoint));
            startPoint.y -= segmentLength;
        }

        Draw();
    }

    public void SetEnds(Transform start, Transform end, bool forceReset = false)
    {
        Log.Debuglog($"SETTING NEW {start.position} {end.position}");
        if (!forceReset && A == start && B == end) return;

        A = start;
        B = end;

        Reset();
    }

    public void Simulate(float dt)
    {
        for (int i = 1; i < subDivisionCount; i++)
        {
            Segment first = segments[i];
            Vector2 velocity = first.currentPos - first.previousPos;
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
        Segment startSegment = segments[0];
        startSegment.currentPos = A.position;
        segments[0] = startSegment;

        Segment endSegment = segments[subDivisionCount - 1];
        endSegment.currentPos = B.position;
        segments[subDivisionCount - 1] = endSegment;

        for (int i = 0; i < subDivisionCount - 1; i++)
        {
            Segment first = segments[i];
            Segment second = segments[i + 1];

            float dist = (first.currentPos - second.currentPos).magnitude;
            float error = Mathf.Abs(dist - segmentLength);

            Vector2 changeDirection = Vector2.zero;

            if (dist > segmentLength)
            {
                changeDirection = (first.currentPos - second.currentPos).normalized;
            }
            else if (dist < segmentLength)
            {
                changeDirection = (second.currentPos - first.currentPos).normalized;
            }

            Vector2 changeAmount = changeDirection * error;


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

    public void Draw()
    {
        Vector3[] positions = new Vector3[subDivisionCount];
        for (int i = 0; i < subDivisionCount; i++)
        {
            positions[i] = segments[i].currentPos;

        }

        lineRenderer.positionCount = subDivisionCount;
        lineRenderer.SetPositions(positions);
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
