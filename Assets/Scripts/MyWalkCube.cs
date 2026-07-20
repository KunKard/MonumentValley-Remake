using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyWalkCube : MonoBehaviour
{
    [SerializeField]
    public List<MyWalkPath> possiblePaths = new List<MyWalkPath>();

    [Space]

    public Transform previousBlock;

    [Space]

    public bool isStair = false;
    public bool isButton = false;
    public bool movingGround = false;
    public bool dontRotate;

    [Space]

    public float walkPointOffset = .5f;
    public float stairOffset = .4f;

    public Vector3 GetWalkPoint()
    {
        float stair = isStair ? stairOffset : 0;
        return transform.position + transform.up * walkPointOffset - transform.up * stair;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.gray;
        float stair = isStair ? .4f : 0;
        Gizmos.DrawSphere(GetWalkPoint(), .1f);

        if (possiblePaths == null)
            return;

        foreach (MyWalkPath p in possiblePaths)
        {
            if (p.target == null)
                return;
            Gizmos.color = p.active ? Color.black : Color.clear;
            Gizmos.DrawLine(GetWalkPoint(), p.target.GetComponent<MyWalkCube>().GetWalkPoint());
        }
    }

    [Serializable]
    public class MyWalkPath
    {
        public Transform target;
        public bool active = true;
    }
    public void ChangActive(int targetNum)
    {
        MyWalkPath path = possiblePaths[targetNum];
        if(path.active) path.active = false;
        else path.active = true;
    }
}
