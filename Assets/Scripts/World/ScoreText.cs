using TMPro;
using UnityEngine;

public class ScoreText : MonoBehaviour
{
    [Header("Refs")]
    public TextMeshProUGUI text;
    public Animator animator;

    public void SetScore(int value)
    {
        text.text = value.ToString();
    }

    public void Highlight()
    {
        animator.SetTrigger("highlight");
    }
}