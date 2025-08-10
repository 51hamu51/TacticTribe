using UnityEngine;
using TMPro;

public class DefText : MonoBehaviour
{
    /// <summary>
    /// 防御力を表示するテキスト
    /// </summary>
    [SerializeField] private TextMeshProUGUI _text;
    void Start()
    {
        _text.text = "";
    }
    void Update()
    {

    }

    /// <summary>
    /// キャラクターの防御力を表示
    /// </summary>
    /// <param name="character">キャラデータ</param>
    public void DefDisplay(Character character)
    {
        _text.text = character.def.ToString();
    }
}
