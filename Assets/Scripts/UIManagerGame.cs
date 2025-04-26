using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class UIManagerGame : MonoBehaviour
{
    public static UIManagerGame instance;
    [SerializeField] private TextMeshProUGUI message;
    [SerializeField] private GameObject[] btns;
    [SerializeField] private Sprite[] sprites;
    public enum side { you, opponent }
    [SerializeField] private Image yourChoice;
    [SerializeField] private Image opponentChoice;
    [SerializeField] private Image hint;

    void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        yourChoice.enabled = false;
        opponentChoice.enabled = false;
    }
    // Update is called once per frame
    void Update()
    {

    }
    public void CloseBtn()
    {
        foreach(GameObject btn in btns)
        {
            btn.SetActive(false);
        }
    }
    public void SetSprite(side side, int choice)
    {
        if (side == side.you)
        {
            yourChoice.enabled = true;
            yourChoice.sprite = sprites[choice];
        }
        else
        {
            opponentChoice.enabled = true;
            opponentChoice.sprite = sprites[choice];
        }
    }
    public void SetResultMessage(int result)
    {
        string resultMessage = "";
        switch (result)
        {
            case 0:
                resultMessage = "Draw";
                break;
            case 1:
                resultMessage = "You Win";
                break;
            case 2:
                resultMessage = "You Lose";
                break;

        }
        message.text = resultMessage;
    }
    public void ShowLeaveHint()
    {
        hint.enabled = true;
    }
}
