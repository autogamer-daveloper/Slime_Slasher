using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class SelectAnswerToDialogue : MonoBehaviour
{
    [SerializeField] private ButtonAndAnswer[] answers;
    private AudioSource src;
    private AudioClip click;

    private void Start()
    {
        foreach (ButtonAndAnswer item in answers) { item.answer.SetActive(false);}

        for (int i = 0; i < answers.Length; i++)
        {
            int index = i;
            answers[index].action = () => { answers[index].answer.SetActive(true); };
            answers[index].button.onClick.AddListener(answers[index].action);
        }
    }

    private void OnDestroy() { foreach (var answer in answers) {answer.button.onClick.RemoveListener(answer.action);}}
}

[System.Serializable]
internal class ButtonAndAnswer
{
    public Button button;
    public GameObject answer;
    public UnityAction action;
}
