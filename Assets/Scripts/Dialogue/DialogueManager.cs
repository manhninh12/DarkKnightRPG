using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using Ink.Runtime;
using TMPro;

// Phiên bản đơn giản hóa tạm thời để project không bị lỗi
public class DialogueManager : MonoBehaviour
{
    [Header("UI Stuff")]
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private TextMeshProUGUI speakerNameText;
    [SerializeField] private TextMeshProUGUI dialogueText;

    private Story currentStory;

    public bool dialogueIsPlaying { get; private set; }

    private static DialogueManager instance;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("Đã có Instance của DialogueManager");
        }
        instance = this;
    }
    public static DialogueManager GetInstance()
    {
        return instance;
    }
    private void Start()
    {
        dialogueIsPlaying = false;
        dialoguePanel.SetActive(false);
    }
    private void Update()
    {
        if(!dialogueIsPlaying)
        {
            return;
        }
        //handle continuing to the next line
        if (InputManager.GetInstance().GetSubmitPressed())
        {
            ContinueStory();
        }
    }

    public void EnterDialogueMode(TextAsset inkJSON)
    {
     currentStory = new Story(inkJSON.text);
     dialogueIsPlaying = true;
     dialoguePanel.SetActive(true);

     ContinueStory();
    }
    private void ExitDialogueMode()
    {
        dialogueIsPlaying = false;
        dialoguePanel.SetActive(false);
        dialogueText.text = "";
    }
    private void ContinueStory()
    {
        if(currentStory.canContinue)
        {
            dialogueText.text = currentStory.Continue();
        }
        else
        {
            ExitDialogueMode();
        }
    }
}
