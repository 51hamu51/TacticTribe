using UnityEngine;

public class Field : MonoBehaviour
{
    /// <summary>
    /// 選択エフェクト
    /// </summary>
    public GameObject ChoiceObject;

    /// <summary>
    /// 移動可能エフェクト
    /// </summary>
    public GameObject MoveObject;

    /// <summary>
    /// 攻撃可能エフェクト
    /// </summary>
    public GameObject AttackableObject;

    /// <summary>
    /// x方向の位置
    /// </summary>
    public int xPos;

    /// <summary>
    /// z方向の位置
    /// </summary>
    public int zPos;

    /// <summary>
    /// このターンで移動可能かどうか
    /// </summary>
    public bool IsReachable;

    /// <summary>
    /// 現在攻撃可能かどうか
    /// </summary>
    public bool IsAttackable;

    /// <summary>
    /// その場所にキャラクターが存在できないならtrue
    /// </summary>
    public bool IsProhibit;

    public void Start()
    {
        ChoiceObject.SetActive(false);
        MoveObject.SetActive(false);
        AttackableObject.SetActive(false);
        IsReachable = false;
        IsAttackable = false;
    }

    /// <summary>
    /// 選ばれたとき
    /// </summary>
    public void ChoiceOn()
    {
        ChoiceObject.SetActive(true);
    }

    /// <summary>
    /// 移動可能なとき
    /// </summary>
    public void MoveOn()
    {
        IsReachable = true;
        MoveObject.SetActive(true);
    }

    /// <summary>
    /// 攻撃可能なとき
    /// </summary>
    public void AttackOn()
    {
        IsAttackable = true;
        AttackableObject.SetActive(true);
    }



    /// <summary>
    /// 選択が外されたとき
    /// </summary>
    public void ChoiceOff()
    {
        IsReachable = false;
        IsAttackable = false;
        ChoiceObject.SetActive(false);
        MoveObject.SetActive(false);
        AttackableObject.SetActive(false);
    }
}
