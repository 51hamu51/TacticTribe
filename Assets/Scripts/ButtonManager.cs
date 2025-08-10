using UnityEngine;

public class ButtonManager : MonoBehaviour
{
    public GameObject attackButton;
    public GameObject waitButton;
    void Start()
    {
        HideCommandButtons();
    }

    void Update()
    {

    }

    /// <summary>
    /// コマンドボタンを隠す
    /// </summary>
    public void HideCommandButtons()
    {
        attackButton.SetActive(false);
        waitButton.SetActive(false);
    }

    /// <summary>
    /// コマンドボタンを表示
    /// </summary>
    public void ShowCommandButtons()
    {
        attackButton.SetActive(true);
        waitButton.SetActive(true);
    }
}
