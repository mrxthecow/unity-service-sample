using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class UIManagerGame : MonoBehaviour
{
    public static UIManagerGame instance;
    [SerializeField] private TextMeshProUGUI message;
    [SerializeField] private GameObject btn;

    void Awake()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
