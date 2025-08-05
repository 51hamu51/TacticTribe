using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    /// <summary>
    /// 初期X位置
    /// </summary>
    public int initPos_X;

    /// <summary>
    /// 初期Z位置
    /// </summary>
    public int initPos_Z;

    /// <summary>
    /// x方向の位置
    /// </summary>
    public int xPos;

    /// <summary>
    /// z方向の位置
    /// </summary>
    public int zPos;

    /// <summary>
    /// カメラ
    /// </summary>
    public GameObject MapCamera;

    public enum MovePattern
    {
        Rook,     // 飛車の動き
        Bishop,    // 角の動き
    }

    public MovePattern movePattern;

    void Start()
    {
        ///初期位置に配置
        Vector3 pos = new Vector3();
        pos.x = initPos_X;
        pos.y = 0.5f;
        pos.z = initPos_Z;
        transform.position = pos;

        xPos = initPos_X;
        zPos = initPos_Z;
    }

    void Update()
    {
    }

    /// <summary>
    /// 対象の座標へとキャラクターを移動させる
    /// </summary>
    /// <param name="targetXPos">x座標</param>
    /// <param name="targetZPos">z座標</param>
    public void MovePosition(int targetXPos, int targetZPos)
    {
        // オブジェクトを移動させる
        // 移動先座標への相対座標を取得
        Vector3 movePos = Vector3.zero;
        movePos.x = targetXPos - xPos;
        movePos.z = targetZPos - zPos;
        transform.position += movePos;

        // キャラクターデータに位置を保存
        xPos = targetXPos;
        zPos = targetZPos;
    }
}