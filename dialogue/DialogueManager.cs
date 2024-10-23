using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Ink.Runtime;
using UnityEngine.EventSystems;

public class DialogueManager : MonoBehaviour
{
    private static DialogueManager instance;

    [Header("dialogue ui")]
    [SerializeField]
    private GameObject dialogue_box;
    [SerializeField]
    private Text dialogue_text;
    [SerializeField]
    private Text character_name_text;
    [SerializeField]
    private Animator portrait_anim;

    private Story current_story;

    private bool dialogue_isPlaying = false;

    [Header("choices ui")]
    [SerializeField]
    private GameObject[] choices;

    private Text[] choices_text;

    private void Awake()
    {
        instance = this;
    }

    public static DialogueManager GetInstance()
    {
        return instance;
    }

    private void Start()
    {
        dialogue_isPlaying = false;
        dialogue_box.SetActive(false);

        choices_text = new Text[choices.Length];

        int choice_index = 0;

        foreach (GameObject choice in choices)
        {
            choices_text[choice_index] = choice.GetComponentInChildren<Text>();
            choice_index++;
        }
    }

    private void Update()
    {
        if (!dialogue_isPlaying)
        {
            return;
        }

        if (current_story.currentChoices.Count == 0 && Input.GetMouseButtonDown(0))
        {
            ContinueStory();
        }
    }
    public void EnterDialogue(TextAsset inkJSON)
    {
        current_story = new Story(inkJSON.text);
        dialogue_isPlaying = true;
        dialogue_box.SetActive(true);

        ContinueStory();
    }

    private void ExitDialogue()
    {
        dialogue_isPlaying = false;
        dialogue_box.SetActive(false);
        dialogue_text.text = string.Empty;
    }

    private void ContinueStory()
    {
        if (current_story.canContinue)
        {
            dialogue_text.text = current_story.Continue();

            DisplayChoices();
            HandleTags(current_story.currentTags);
        }
        else
        {
            ExitDialogue();
        }
    }

    void HandleTags(List<string> current_tags)
    {
        foreach (string tag in current_tags)
        {
            string[] split_tag = tag.Split(':');


            string tag_key = split_tag[0].Trim();
            string tag_value = split_tag[1].Trim();

            switch (tag_key)
            {
                case "character":
                    character_name_text.text = tag_value;
                    break;
                case "sprite":
                    portrait_anim.Play(tag_value);
                    break;
                case "layout":
                    break;
            }

        }
    }

    private void DisplayChoices()
    {
        List<Choice> current_choices = current_story.currentChoices;

        int index = 0;

        foreach (Choice choice in current_choices)
        {
            choices[index].gameObject.SetActive(true);
            choices_text[index].text = choice.text;
            index++;
        }

        for (int i = index; i < choices.Length; i++)
        {
            choices[i].gameObject.SetActive(false);
        }

        StartCoroutine(SelectFirstChoice());
    } 

    private IEnumerator SelectFirstChoice()
    {
        EventSystem.current.SetSelectedGameObject(null);
        yield return new WaitForEndOfFrame();
        EventSystem.current.SetSelectedGameObject(choices[0].gameObject);
    }

    public void MakeChoice(int choice_index)
    {
        current_story.ChooseChoiceIndex(choice_index);
        ContinueStory();
    }
}
