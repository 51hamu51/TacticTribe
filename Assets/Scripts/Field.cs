using UnityEngine;

public class Field : MonoBehaviour
{
    public GameObject ChoiceObject;

    /// <summary>
    /// x方向の位置
    /// </summary>
    public int xPos;

    /// <summary>
    /// z方向の位置
    /// </summary>
    public int zPos;

    public void Start()
    {
        ChoiceObject.SetActive(false);
    }

    /// <summary>
    /// 選ばれたとき
    /// </summary>
    public void ChoiceOn()
    {
        ChoiceObject.SetActive(true);
    }


    /// <summary>
    /// 選択が外されたとき
    /// </summary>
    public void ChoiceOff()
    {
        ChoiceObject.SetActive(false);
    }
}
