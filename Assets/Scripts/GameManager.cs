using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    /// <summary>
    /// 前回選択したブロック
    /// </summary>
    private Field preField;

    void Start()
    {

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
        if (preField != null)
        {
            preField.ChoiceOff();
        }
        Debug.Log("ブロックがタップされました。\nブロックの座標：" + targetBlock.transform.position);
        targetBlock.ChoiceOn();
        preField = targetBlock;
    }
}