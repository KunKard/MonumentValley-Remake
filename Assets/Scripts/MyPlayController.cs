using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;
using static MyWalkCube;

public class MyPlayController : MonoBehaviour
{
    public bool walking;

    [Space]

    public Transform currentCube;
    public Transform clickedCube;
    public bool isOnMovingGround;

    [Space]

    public List<Transform> finalPath = new List<Transform>();

    private float blend;

    public void Start()
    {
        walking = false;
        RayCastDown();
    }
    private void Update()
    {
        RayCastDown();
        //if((MyGameMgr.instance != null && MyGameMgr.instance.isRotation) || (GameMgr2.instance != null && GameMgr2.instance.isRotation)) return;
        if (Input.GetMouseButtonDown(0))
        {
            Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition); RaycastHit mouseHit;

            if (Physics.Raycast(mouseRay, out mouseHit))
            {
                if (mouseHit.transform.GetComponent<MyWalkCube>() != null)
                {
                    clickedCube = mouseHit.transform;
                    DOTween.Kill(gameObject.transform);
                    finalPath.Clear();
                    FindPath();

                    blend = transform.position.y - clickedCube.position.y > 0 ? -1 : 1;
                }
            }
        }
    }
    void FindPath()
    {
        List<Transform> nextCubes = new List<Transform>();
        List<Transform> pastCubes = new List<Transform>();

        foreach (MyWalkPath path in currentCube.GetComponent<MyWalkCube>().possiblePaths)
        {
            if (path.active)
            {
                nextCubes.Add(path.target);
                path.target.GetComponent<MyWalkCube>().previousBlock = currentCube;
            }
        }

        pastCubes.Add(currentCube);

        ExploreCube(nextCubes, pastCubes);
        BuildPath();
    }
    void ExploreCube(List<Transform> nextCubes, List<Transform> pastCubes)
    {
        Transform current = nextCubes.First();
        nextCubes.Remove(current);

        if (current == clickedCube)
        {
            return;
        }

        foreach (MyWalkPath path in current.GetComponent<MyWalkCube>().possiblePaths)
        {
            if (!pastCubes.Contains(path.target) && path.active)
            {
                nextCubes.Add(path.target);
                path.target.GetComponent<MyWalkCube>().previousBlock = current;
            }
        }

        pastCubes.Add(current);

        if (nextCubes.Any())
        {
            ExploreCube(nextCubes, pastCubes);
        }
    }
    void BuildPath()
    {
        Transform cube = clickedCube;
        while (cube != currentCube)
        {
            finalPath.Add(cube);
            if (cube.GetComponent<MyWalkCube>().previousBlock != null)
                cube = cube.GetComponent<MyWalkCube>().previousBlock;
            else
            {
                Clear();
                return;
            }
                
        }

        FollowPath();
    }
    void FollowPath()
    {
        Sequence s = DOTween.Sequence();

        walking = true;

        for (int i = finalPath.Count - 1; i >= 0; i--)
        {
            float time = finalPath[i].GetComponent<MyWalkCube>().isStair ? 1.5f : 1;

            s.Append(transform.DOMove(finalPath[i].GetComponent<MyWalkCube>().GetWalkPoint(), .2f * time).SetEase(Ease.Linear));

            if (!finalPath[i].GetComponent<MyWalkCube>().dontRotate)
                s.Join(transform.DOLookAt(finalPath[i].position, .1f, AxisConstraint.Y, Vector3.up));
        }

        s.AppendCallback(() => Clear());
    }
    void Clear()
    {
        foreach (Transform t in finalPath)
        {
            t.GetComponent<MyWalkCube>().previousBlock = null;
        }
        finalPath.Clear();
        walking = false;
    }
    void RayCastDown()
    {
        Ray playerRay = new Ray(transform.GetChild(0).position, -transform.up);
        RaycastHit playerHit;

        if (Physics.Raycast(playerRay, out playerHit))
        {
            if(MyGameMgr.instance != null && playerHit.transform == MyGameMgr.instance.btn1)
            {
                MyGameMgr.instance.NextScene();
                return;
            }
            else if (GameMgr2.instance != null)
            {
                if(playerHit.transform == GameMgr2.instance.btn1)
                {
                    GameMgr2.instance.UpRoot4();
                }
                else if(playerHit.transform == GameMgr2.instance.btn2)
                {
                    GameMgr2.instance.UpRoot2();
                }
                else if(playerHit.transform == GameMgr2.instance.btn3)
                {
                    GameMgr2.instance.GameOver();
                }
            }
            if (playerHit.transform.GetComponent<MyWalkCube>() != null)
            {
                currentCube = playerHit.transform;

                if (playerHit.transform.GetComponent<MyWalkCube>().isStair)
                {
                    DOVirtual.Float(GetBlend(), blend, .1f, SetBlend);
                }
                else
                {
                    DOVirtual.Float(GetBlend(), 0, .1f, SetBlend);
                }
            }
        }
        if (currentCube == null) return;
        if (currentCube.GetComponent<MyWalkCube>().movingGround)
        {
            isOnMovingGround = true;
        }
        else
        {
            isOnMovingGround = false;
        }
    }
    float GetBlend()
    {
        return GetComponentInChildren<Animator>().GetFloat("Blend");
    }
    void SetBlend(float x)
    {
        GetComponentInChildren<Animator>().SetFloat("Blend", x);
    }
}
