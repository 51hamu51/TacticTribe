using UnityEngine;
using TMPro;

public class NameText : MonoBehaviour
{
    /// <summary>
    /// 名前を表示するテキスト
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
    /// キャラクターの名前を表示
    /// </summary>
    /// <param name="character">キャラデータ</param>
    public void NameDisplay(Character character)
    {
        _text.text = character.characterName;
    }
}
