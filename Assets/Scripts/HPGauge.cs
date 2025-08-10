using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HPGauge : MonoBehaviour
{
    /// <summary>ゲージのバー表示のImage</summary>
    [SerializeField] private Image _bar;

    /// <summary>HP量を表示するテキスト</summary>
    [SerializeField] private TextMeshProUGUI _text;

    void Start()
    {

    }

    void Update()
    {

    }

    /// <summary>
    /// HPを指定して表示を更新
    /// </summary>
    public void SetHP(Character character)
    {
        var hpRate = (float)character.nowHP / character.maxHP;
        // 残りHPの割合でバーの表示幅を更新
        _bar.fillAmount = hpRate;

        _text.text = string.Format("{0}/{1}", character.nowHP, character.maxHP);
    }
}
