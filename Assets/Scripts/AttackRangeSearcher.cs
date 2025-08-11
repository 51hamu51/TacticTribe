using UnityEngine;

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
    public void ResearchAttackableField(Character character)
    {
        switch (character.attackPattern)
        {
            case Character.AttackPattern.Normal:
                for (int i = -1; i < 2; i++)
                {
                    for (int j = -1; j < 2; j++)
                    {
                        SelectAttackFieldAtPosition(character.xPos + i, character.zPos + j);
                    }
                }
                break;

            case Character.AttackPattern.Bow:

                break;
        }
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
