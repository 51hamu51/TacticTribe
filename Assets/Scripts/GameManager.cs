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

    public enum Phase
    {
        MyTurn_Start,     // 自分のターン開始
        MyTurn_Moving,    // 移動先選択中
        MyTurn_Command,   // 移動後のコマンド選択中
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
            SelectBlock(targetObject.GetComponent<Field>());
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
                if (charaData != null)
                {
                    selectingChara = charaData;
                    mapManager.ResearchReachableField(selectingChara);
                    nowPhase = Phase.MyTurn_Moving;
                }
                break;

            case Phase.MyTurn_Moving:
                if (targetBlock.IsReachable)
                {
                    selectingChara.MovePosition(targetBlock.xPos, targetBlock.zPos);
                    mapManager.AllChoiceOff();
                    nowPhase = Phase.MyTurn_Start;
                }
                break;

            case Phase.MyTurn_Command:

                break;
        }
    }
}