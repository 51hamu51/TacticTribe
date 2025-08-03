using UnityEngine;

public class Field : MonoBehaviour
{
    public GameObject ChoiceObject;

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
