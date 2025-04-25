using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManagerMenu : MonoBehaviour
{
    public static UIManagerMenu instance;
    [SerializeField] private TextMeshProUGUI message;
    [SerializeField] private GameObject btn;

    void Awake()
    {
        instance = this;
    }
    public void SetMessage(bool active, string content = "")
    {
        message.gameObject.SetActive(active);
        if (active) message.text = content;
    }
    public void CloseBtn()
    {
        btn.SetActive(false);
    }
}
