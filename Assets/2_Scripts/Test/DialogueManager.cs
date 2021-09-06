using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace SG
{
    public class DialogueManager : MonoBehaviour
    {
        private Queue<Sentences> sentences;
        public GameObject dialogueObject;
        public TMP_Text nameText;
        public TMP_Text dialogueText;
        public Image characterImage;
        public Button nextBtn;
        public Transform choicePanelTransform;
        public DialogueChoiceBox choiceDialogue_Prefab;

        private List<GameObject> dialogue_Choices;

        public bool isTyping = false;

        private Action endDialogueEvent;
        [Tooltip("yield return new Waitforseconds = \"this\" * Time.deltaTime")]
        [SerializeField] private float typeLetterSpeed = 2f;

        private void Awake()
        {
            sentences = new Queue<Sentences>();
            dialogue_Choices = new List<GameObject>();

            if (dialogueObject != null)
            {
                nameText = UtilHelper.Find<TMP_Text>(dialogueObject.transform, "Dialogue/CharacterName");
                dialogueText = UtilHelper.Find<TMP_Text>(dialogueObject.transform, "Dialogue/DialougeText");
                characterImage = UtilHelper.Find<Image>(dialogueObject.transform, "CharacterImage");
                nextBtn = UtilHelper.Find<Button>(dialogueObject.transform, "Dialogue/NextButton");
                if (nextBtn != null)
                    nextBtn.onClick.AddListener(DisplayNextSentences);
            }

            dialogueObject.SetActive(false);
        }
        internal void StartDialogue(Dialogue dialogue, DialogueChoice[] choices)
        {
            Debug.Log("대화 시작 " + dialogue.characterName);
            dialogueObject.SetActive(true);
            SetDialogue(dialogue);
            SetDialogueChoices(choices);
            GUIManager.instance.SetActiveHudWindows(false);

            sentences.Clear();
            for (int i = 0; i < dialogue.sentences.Length; i++)
            {
                sentences.Enqueue(dialogue.sentences[i]);
            }

            DisplayNextSentences();
        }
        private void SetDialogue(Dialogue dialogue)
        {
            characterImage.sprite = dialogue.image;
            nameText.text = dialogue.characterName;
        }
        private void SetDialogueChoices(DialogueChoice[] choices)
        {
            if(dialogue_Choices.Count > 0)
            {
                for (int i = 0; i < dialogue_Choices.Count; i++)
                {
                    Destroy(dialogue_Choices[i]);
                }
            }
            dialogue_Choices.Clear();

            for (int i = 0; i < choices.Length; i++)
            {
                GameObject go = Instantiate(choiceDialogue_Prefab, choicePanelTransform).gameObject;
                go.GetComponent<DialogueChoiceBox>().SetDialogChoice(choices[i].selectText, choices[i].icon, choices[i].action);
                dialogue_Choices.Add(go);
            }

            for (int i = 0; i < dialogue_Choices.Count; i++)
            {
                dialogue_Choices[i].gameObject.SetActive(false);
            }
        }

        public void DisplayNextSentences()
        {
            if (nextBtn.gameObject.activeSelf.Equals(true))
            {
                nextBtn.gameObject.SetActive(false);
            }

            if (sentences.Count == 0)
            {
                EndDialogue();
                return;
            }

            Sentences sentence = sentences.Dequeue();
            StopAllCoroutines();
            StartCoroutine(TypeSentence(sentence));
        }

        IEnumerator TypeSentence(Sentences sentence)
        {
            isTyping = true;

            dialogueText.text = "";
            foreach (char letter in sentence.sentence.ToCharArray())
            {
                dialogueText.text += letter;
                yield return new WaitForSeconds(typeLetterSpeed * Time.deltaTime);

                if (CheckSkipDialogue(sentence))
                {
                    break;
                }
            }

            if(sentence.dialogType == DialogType.Select)
            {
                OpenChoicePanel();
                isTyping = false;
                yield break;
            }
            else
            {
                nextBtn.gameObject.SetActive(true);
                isTyping = false;
            }
        }

        private bool CheckSkipDialogue(Sentences sentence)
        {
            if (Mouse.current.leftButton.isPressed || 
                Keyboard.current.enterKey.isPressed ||
                Keyboard.current.spaceKey.isPressed)
            {
                dialogueText.text = sentence.sentence;
                return true;
            }

            return false;
        }

        private void OpenChoicePanel()
        {
            for (int i = 0; i < dialogue_Choices.Count; i++)
            {
                dialogue_Choices[i].gameObject.SetActive(true);
            }
        }

        public void EndDialogue()
        {
            dialogueObject.SetActive(false);
            GUIManager.instance.SetActiveHudWindows(true);
            endDialogueEvent?.Invoke();
        }

        public void SetEndDialogueEvent(Action listener)
        {
            endDialogueEvent = listener;
        }
    }
}
