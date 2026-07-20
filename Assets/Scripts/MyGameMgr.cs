using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MyGameMgr : MonoBehaviour
{
    public static MyGameMgr instance;
    public void Awake()
    {
        instance = this;
        ifRoot2 = false;
        isRotation = false;
    }

    public Transform root2;
    public bool ifRoot2;
    public bool isRotation;
    public MyPlayController playController;
    public List<MyCondition> conditions;

    [Space]

    public Transform btn1;
    public bool btn1On = false;

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
        // - 触发枢纽旋转
        if (ifRoot2)
        {
            root2.DOComplete();

            isRotation = true;
            // 创建动画序列
            Sequence seq = DOTween.Sequence();

            // 第一步：添加旋转动画
            seq.Append(root2.DORotate(new Vector3(-90, 0, 0), 0.6f, RotateMode.WorldAxisAdd)
                .SetEase(Ease.OutBack));

            seq.InsertCallback(0.3f, () => {
                MeshRenderer[] allRenderers = root2.GetComponentsInChildren<MeshRenderer>(true);

                foreach (MeshRenderer r in allRenderers)
                {
                    r.gameObject.layer = 8;
                }
            });

            // 旋转完成后调用
            seq.AppendCallback(() => {
                isRotation = false;
                ifRoot2 = false;
            });

            
        }
        else
        {
            root2.DOComplete();

            isRotation = true;
            // 创建动画序列
            Sequence seq = DOTween.Sequence();

            // 第一步：添加旋转动画
            seq.Append(root2.DORotate(new Vector3(90, 0, 0), 0.6f, RotateMode.WorldAxisAdd)
                .SetEase(Ease.OutBack));

            seq.InsertCallback(0.2f, () => {
                MeshRenderer[] allRenderers = root2.GetComponentsInChildren<MeshRenderer>(true);

                foreach (MeshRenderer r in allRenderers)
                {
                    r.gameObject.layer = 0;
                }
            });

            // 旋转完成后调用
            seq.AppendCallback(() => {
                isRotation = false;
                ifRoot2 = true;
            });
        }
        ChangeCondition(0);
    }
    public void ChangeCondition(int id)
    {
        MyCondition condition = conditions[id];
        for(int i = 0;i <= condition.targets.Count - 1;i++)
        {
            condition.walkCubes[i].ChangActive(condition.targets[i]);
        }
    }
    public void NextScene()
    {
        if (btn1On) return;
        SceneManager.LoadScene("CloneScene2");
        btn1On = true;
    }
}
[System.Serializable]
public class MyCondition
{
    public int conditionID;
    public List<MyWalkCube> walkCubes;
    public List<int> targets;
}
