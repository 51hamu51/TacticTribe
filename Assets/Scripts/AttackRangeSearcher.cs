using UnityEngine;
using System.Collections.Generic;

public class AttackRangeSearcher : MonoBehaviour
{
    public MapManager mapManager;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    /// <summary>
    /// 攻撃可能なFieldを調べ、AttackOnを呼び出す
    /// </summary>
    public void ResearchAttackableField(Character character, int PosX, int PosZ)
    {
        switch (character.attackPattern)
        {
            case Character.AttackPattern.Normal:
                for (int i = -1; i < 2; i++)
                {
                    for (int j = -1; j < 2; j++)
                    {
                        SelectAttackFieldAtPosition(PosX + i, PosZ + j);
                    }
                }
                break;

            case Character.AttackPattern.Bow:
                for (int i = -2; i < 3; i++)
                {
                    for (int j = -2; j < 3; j++)
                    {
                        SelectAttackFieldAtPosition(PosX + i, PosZ + j);
                    }
                }
                break;
        }
    }

    /// <summary>
    /// 攻撃可能なFieldを調べ、そのリストを返す
    /// </summary>
    public List<Field> ResearchAttackableFieldList(Character character, int PosX, int PosZ)
    {
        List<Field> reachable = new List<Field>();
        switch (character.attackPattern)
        {
            case Character.AttackPattern.Normal:
                for (int i = -1; i < 2; i++)
                {
                    for (int j = -1; j < 2; j++)
                    {
                        if (mapManager.fieldDict.TryGetValue((PosX + i, PosZ + j), out Field field))
                        {
                            reachable.Add(field);
                        }
                    }
                }
                break;

            case Character.AttackPattern.Bow:
                for (int i = -2; i < 3; i++)
                {
                    for (int j = -2; j < 3; j++)
                    {
                        if (mapManager.fieldDict.TryGetValue((PosX + i, PosZ + j), out Field field))
                        {
                            reachable.Add(field);
                        }
                    }
                }
                break;
        }

        return reachable;
    }

    /// <summary>
    /// 指定した座標にあるFieldを攻撃状態にする
    /// </summary>
    private void SelectAttackFieldAtPosition(int x, int z)
    {
        if (mapManager.fieldDict.TryGetValue((x, z), out Field field))
        {
            field.AttackOn();
        }
    }
}
