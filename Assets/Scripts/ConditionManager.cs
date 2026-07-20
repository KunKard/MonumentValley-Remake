using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 管理关卡条件切换的公共逻辑
/// </summary>
public static class ConditionManager
{
    /// <summary>
    /// 根据 id 切换指定条件中的路径开关状态
    /// </summary>
    public static void ChangeCondition(List<MyCondition> conditions, int id)
    {
        MyCondition condition = conditions[id];
        for (int i = 0; i <= condition.targets.Count - 1; i++)
        {
            condition.walkCubes[i].ChangActive(condition.targets[i]);
        }
    }
}

[System.Serializable]
public class MyCondition
{
    public int conditionID;
    public List<MyWalkCube> walkCubes;
    public List<int> targets;
}
