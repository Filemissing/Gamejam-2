using TMPro;
using UnityEngine;

[RequireComponent(typeof(TMP_Text))]
public class ScoreText : MonoBehaviour
{
    TMP_Text text;
    private void Awake()
    {
        text = GetComponent<TMP_Text>();
    }

    public string defaultText = "Score: ";

    private void Update()
    {
        text.text = defaultText + GameManager.instance.score;
    }
}
