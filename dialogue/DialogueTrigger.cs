using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    [SerializeField]
    private TextAsset inkJSON;

    private void OnTriggerStay2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {

            if (Input.GetKey(KeyCode.E))
            {
                DialogueManager.GetInstance().EnterDialogue(inkJSON);
            }
        } 
    }
}
