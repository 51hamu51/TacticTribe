using UnityEngine;

public class StatusDisplayManager : MonoBehaviour
{
    /// <summary>
    /// キャラクター名を表示するテキスト
    /// </summary>
    public NameText nameText;

    /// <summary>
    /// キャラクターの攻撃力を表示するテキスト
    /// </summary>
    public AtkText atkText;

    /// <summary>
    /// キャラクターの防御力を表示するテキスト
    /// </summary>
    public DefText defText;

    /// <summary>
    /// HPゲージ
    /// </summary>
    public HPGauge hpGauge;

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
        atkText.AtkDisplay(character);
        defText.DefDisplay(character);
        hpGauge.SetHP(character);
        nameText.NameDisplay(character);
    }
}
