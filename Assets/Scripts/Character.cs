using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

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
    /// 表示名
    /// </summary>
    public string characterName;

    /// <summary>
    /// 最大HP
    /// </summary>
    public int maxHP;

    /// <summary>
    /// 現在のHP
    /// </summary>
    public int nowHP;

    /// <summary>
    /// 攻撃力
    /// </summary>
    public int atk;

    /// <summary>
    /// 防御力
    /// </summary>
    public int def;

    /// <summary>
    /// 敵ならtrue
    /// </summary>
    public bool IsEnemy;

    /// <summary>
    /// 死んで消えるのにかかる時間
    /// </summary>
    public float fadeDuration = 1f;

    public enum MovePattern
    {
        Rook,     // 飛車の動き
        Bishop,    // 角の動き
        Step3, // ３歩動ける
        Step4, // ４歩動ける
    }

    public enum AttackPattern
    {
        Normal,     // 通常
        Bow,    // 弓
    }

    public AttackPattern attackPattern;

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

        nowHP = maxHP;
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

    /// <summary>
    /// 攻撃を受ける
    /// </summary>
    public void Damage(int attack)
    {
        int damage = attack - def;
        if (damage > 0)
        {
            nowHP -= damage;
        }
        else
        {
            damage = 0;
        }

        //ダメージエフェクトを再生
        DamageViewManager.Instance.Play(damage, new Vector3(xPos, 0, zPos));


        if (nowHP <= 0)
        {
            Dead();
        }

    }

    public void Dead()
    {
        //子オブジェクトのrendererを取得
        Renderer rend = GetComponentInChildren<Renderer>();
        if (rend != null)
        {
            //色を徐々に薄くする
            Material mat = rend.material;
            mat.DOFade(0f, fadeDuration)
                .OnComplete(() => Destroy(this.gameObject)); // 完了後に破壊
        }
    }
}