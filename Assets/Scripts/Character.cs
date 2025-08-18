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
    /// 初期y位置
    /// </summary>
    public float initPos_Y;

    /// <summary>
    /// 初期Z位置
    /// </summary>
    public int initPos_Z;

    /// <summary>
    /// x方向の位置
    /// </summary>
    public int xPos;

    /// <summary>
    /// y方向の位置
    /// </summary>
    public float yPos;

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
    /// 2Dならtrue
    /// </summary>
    public bool Is2D;

    /// <summary>
    /// 死んで消えるのにかかる時間
    /// </summary>
    public float fadeDuration = 2f;

    public float tiltAngle = -30f; // 背中側に傾ける角度

    public Animator animator;

    public CharacterManager characterManager;

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
        pos.y = initPos_Y;
        pos.z = initPos_Z;
        transform.position = pos;

        if (Is2D)
        {
            this.transform.eulerAngles = new Vector3(tiltAngle, 0, 0);
        }
        else
        {
            this.transform.eulerAngles = new Vector3(tiltAngle, 180, 0);
        }

        xPos = initPos_X;
        yPos = initPos_Y;
        zPos = initPos_Z;

        nowHP = maxHP;
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

            animator.SetTrigger("Damage");
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
        animator.SetTrigger("Dead");
        characterManager.UnregisterCharacter(this);
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