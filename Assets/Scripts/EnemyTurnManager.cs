using UnityEngine;
using System.Collections.Generic;

public class EnemyTurnManager : MonoBehaviour
{
    public CharacterManager characterManager;

    public MoveRangeSearcher moveRangeSearcher;

    public AttackRangeSearcher attackRangeSearcher;

    public GameManager gameManager;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }


    /// <summary>
    /// 敵のターンがスタートしたときに呼ばれる
    /// </summary>
    public void EnemyTurnStart()
    {
        Debug.Log("EnemyStart");

        var reachableBlocks = new List<Field>();
        var attackableBlocks = new List<Field>();

        // 生存中の敵キャラクターのリストを作成する
        var enemyCharas = new List<Character>(); // 敵キャラクターリスト
        foreach (Character charaData in characterManager.characters)
        {// 全生存キャラクターから敵フラグの立っているキャラクターをリストに追加
            if (charaData.IsEnemy)
                enemyCharas.Add(charaData);
        }


        // 攻撃可能な敵キャラクター１体を見つけるまで処理
        foreach (Character enemyData in enemyCharas)
        {
            // 移動可能な場所リストを取得する
            reachableBlocks.Clear();
            reachableBlocks = moveRangeSearcher.ResearchReachableFieldList(enemyData);

            // それぞれの移動可能な場所ごとの処理
            foreach (Field block in reachableBlocks)
            {
                // 攻撃可能な場所リストを取得する
                attackableBlocks.Clear();
                attackableBlocks = attackRangeSearcher.ResearchAttackableFieldList(enemyData, block.xPos, block.zPos);

                // それぞれの攻撃可能な場所ごとの処理
                foreach (Field attackBlock in attackableBlocks)
                {
                    // 攻撃できる相手キャラクター(プレイヤー側のキャラクター)を探す
                    Character targetChara = characterManager.GetCharacterAtPosition(attackBlock.xPos, attackBlock.zPos);
                    if (targetChara != null && !targetChara.IsEnemy)
                    {// 相手キャラクターが存在する
                        Debug.Log("target : " + targetChara.characterName);
                        // 敵キャラクター移動処理
                        moveRangeSearcher.MoveCharacterTo(enemyData, block.xPos, block.zPos, () =>
                        {// 敵キャラクター攻撃処理
                            gameManager.selectingChara = enemyData;
                            gameManager.Attack(attackBlock);
                        });

                        // 移動場所・攻撃場所リストをクリアする
                        reachableBlocks.Clear();
                        attackableBlocks.Clear();
                        return;
                    }
                }
            }
        }

        // (攻撃可能な相手が見つからなかった場合何もせずターン終了)
        // 移動場所・攻撃場所リストをクリアする
        reachableBlocks.Clear();
        attackableBlocks.Clear();

        Debug.Log("EnemyCantMove");

        gameManager.MoveToMyTurn();
    }
}
