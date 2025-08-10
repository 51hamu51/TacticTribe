using UnityEngine;

public class StatusDisplayManager : MonoBehaviour
{
    /// <summary>
    /// キャラクター名を表示するテキスト
    /// </summary>
    public NameText nameText;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    /// <summary>
    /// キャラのステータスを表示
    /// </summary>
    /// <param name="character">キャラデータ</param>
    public void ShowStatus(Character character)
    {

        nameText.NameDisplay(character);
    }
}
