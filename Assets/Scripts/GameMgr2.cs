using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameMgr2 : MonoBehaviour
{
    public static GameMgr2 instance;
    public void Awake()
    {
        instance = this;
        isRotation = false;
    }

    public Transform root2;
    public Transform root4;
    public bool isRotation;
    public MyPlayController playController;
    public List<MyCondition> conditions;
    public int count = 0;
    public int floor = 0;

    [Space]

    public Transform btn1;
    public Transform btn2;
    public Transform btn3;
    public bool btn1On = false;
    public bool btn2On = false;
    public bool btn3On = false;

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (playController.walking) return;
            rotateRoot2();
        }
    }

    public void rotateRoot2()
    {
        if (isRotation) return;
        // - ������Ŧ��ת
            root2.DOComplete();

            isRotation = true;
            // ������������
            Sequence seq = DOTween.Sequence();

            // ��һ����������ת����
            seq.Append(root2.DORotate(new Vector3(0, 90, 0), 0.6f, RotateMode.WorldAxisAdd)
                .SetEase(Ease.OutBack));

            seq.InsertCallback(0.3f, () => {
                MeshRenderer[] allRenderers = root2.GetComponentsInChildren<MeshRenderer>(true);

                //foreach (MeshRenderer r in allRenderers)
                //{
                //    r.gameObject.layer = 8;
                //}
            });

            // ��ת��ɺ����
            seq.AppendCallback(() => {
                isRotation = false;
                ChangeCondition(floor * 4 + (count + 1));
                count = (count + 1) % 4;
                ChangeCondition(floor * 4 + (count + 1));
            });
        
    }
    public void ChangeFloor()
    {
        root2.DOLocalMove(new Vector3(2, 4, -7), .2f).SetEase(Ease.Linear);

        ChangeCondition(floor * 4 + (count + 1));
        floor = 1;
        ChangeCondition(floor * 4 + (count + 1));
        

        MeshRenderer[] allRenderers = root2.GetComponentsInChildren<MeshRenderer>(true);

        foreach (MeshRenderer r in allRenderers)
        {
            r.gameObject.layer = 7;
        }
    }
    public void ChangeCondition(int id)
    {
        ConditionManager.ChangeCondition(conditions, id);
    }

    public void UpRoot4()
    {
        if (btn1On) return;
        root4.DOLocalMove(new Vector3(0, 0, 0), .5f).SetEase(Ease.Linear);
        btn1On = true;
    }
    public void UpRoot2()
    {
        if (btn2On) return;
        ChangeFloor();
        btn2On = true;
    }
    public Text gameoverTxt;
    public void GameOver()
    {
        if (btn3On) return;
        Debug.Log("Game Over");
        gameoverTxt.enabled = true;
        btn3On = true;
    }
}
