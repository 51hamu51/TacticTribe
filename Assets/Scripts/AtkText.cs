using UnityEngine;
using TMPro;

public class AtkText : MonoBehaviour
{
    /// <summary>
    /// 攻撃力を表示するテキスト
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
    /// キャラクターの攻撃力を表示
    /// </summary>
    /// <param name="character">キャラデータ</param>
    public void AtkDisplay(Character character)
    {
        _text.text = character.atk.ToString();
    }
}
