using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    /// <summary>
    /// 前回選択したブロック
    /// </summary>
    private Field preField;

    public CharacterManager characterManager;

    /// <summary>
    /// 全てのキャラクターのリスト
    /// </summary>
    public List<Character> allCharacters;

    /// <summary>
    /// 今のフェーズ
    /// </summary>
    private Phase nowPhase;

    /// <summary>
    /// 選択中のキャラ
    /// </summary>
    private Character selectingChara;

    public MapManager mapManager;

    public MoveRangeSearcher moveRangeSearcher;

    public StatusDisplayManager statusDisplayManager;

    public ButtonManager buttonManager;

    public AttackRangeSearcher attackRangeSearcher;

    public enum Phase
    {
        MyTurn_Start,     // 自分のターン開始
        MyTurn_Moving,    // 移動先選択中
        MyTurn_Command,   // 移動後のコマンド選択中
        MyTurn_Attack,  //攻撃先選択中
    }

    void Start()
    {
        // 全キャラクターをCharacterManagerに登録
        foreach (var ch in allCharacters)
        {
            characterManager.RegisterCharacter(ch);
        }

        nowPhase = Phase.MyTurn_Start;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            GetMapBlockByTapPos();
        }
    }

    /// <summary>
    /// タップした場所にあるオブジェクトを見つけ、選択処理などを開始する
    /// </summary>
    private void GetMapBlockByTapPos()
    {
        GameObject targetObject = null;

        // タップした方向にカメラからRayを飛ばす
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit = new RaycastHit();
        if (Physics.Raycast(ray, out hit))
        {
            targetObject = hit.collider.gameObject;
        }

        if (targetObject != null)
        {
            Field field = targetObject.GetComponent<Field>();
            if (field != null)
            {
                SelectBlock(field);
            }
        }
    }

    /// <summary>
    /// 指定したブロックを選択状態にする処理
    /// </summary>
    /// <param name="targetMapBlock">対象のブロックデータ</param>
    private void SelectBlock(Field targetBlock)
    {
        switch (nowPhase)
        {
            case Phase.MyTurn_Start:
                if (preField != null)
                {
                    preField.ChoiceOff();
                }
                Debug.Log("ブロックがタップされました。\nブロックの座標：" + targetBlock.transform.position);
                targetBlock.ChoiceOn();
                preField = targetBlock;

                //選択したマスにキャラがいたら次のフェーズへ
                var charaData = characterManager.GetCharacterAtPosition(targetBlock.xPos, targetBlock.zPos);
                if (charaData != null && !charaData.IsEnemy)
                {
                    selectingChara = charaData;
                    moveRangeSearcher.ResearchReachableField(selectingChara);
                    statusDisplayManager.ShowStatus(selectingChara);
                    nowPhase = Phase.MyTurn_Moving;
                }
                else if (charaData != null)//敵ならステータスの表示のみ行う
                {
                    selectingChara = charaData;
                    statusDisplayManager.ShowStatus(selectingChara);
                }
                break;

            case Phase.MyTurn_Moving:
                if (targetBlock.IsReachable)
                {
                    moveRangeSearcher.MoveCharacterTo(selectingChara, targetBlock.xPos, targetBlock.zPos);//指定座標まで移動
                    mapManager.AllChoiceOff();
                }
                break;

            case Phase.MyTurn_Command:

                break;

            case Phase.MyTurn_Attack:
                if (targetBlock.IsAttackable)
                {
                    Attack(targetBlock);
                }
                break;
        }
    }

    /// <summary>
    /// 攻撃コマンドボタンが押されたとき
    /// </summary>
    public void AttackCommand()
    {
        buttonManager.HideCommandButtons();
        attackRangeSearcher.ResearchAttackableField(selectingChara);
        nowPhase = Phase.MyTurn_Attack;
    }

    /// <summary>
    /// 待機コマンドボタンが押されたとき
    /// </summary>
    public void WaitCommand()
    {
        buttonManager.HideCommandButtons();
        nowPhase = Phase.MyTurn_Start;
    }

    /// <summary>
    /// 攻撃
    /// </summary>
    public void Attack(Field targetBlock)
    {
        mapManager.AllChoiceOff();

        //選択したマスに敵キャラがいたら攻撃
        var charaData = characterManager.GetCharacterAtPosition(targetBlock.xPos, targetBlock.zPos);
        if (charaData != null && charaData.IsEnemy)
        {
            // 攻撃モーション再生
            Animator animator = selectingChara.GetComponent<Animator>();
            if (animator != null)
            {
                animator.SetTrigger("Attack");
                // 攻撃モーション終了後にダメージ
                StartCoroutine(WaitForAttackEnd(animator, "Attack", charaData, selectingChara.atk));
            }
            else
            {
                // Animator がない場合は即ダメージ
                charaData.Damage(selectingChara.atk);
            }
        }

        nowPhase = Phase.MyTurn_Start;
    }

    /// <summary>
    /// 指定アニメーション終了まで待ってからダメージ
    /// </summary>
    private IEnumerator WaitForAttackEnd(Animator animator, string stateName, Character target, int damage)
    {
        // Animator が Attack ステートに遷移するまで待つ
        while (!animator.GetCurrentAnimatorStateInfo(0).IsName(stateName))
        {
            yield return null;
        }

        // アニメーションの終了を待つ
        AnimatorStateInfo state = animator.GetCurrentAnimatorStateInfo(0);
        while (state.normalizedTime < 1f)
        {
            yield return null;
            state = animator.GetCurrentAnimatorStateInfo(0);
        }

        // ダメージを与える
        target.Damage(damage);
    }
}