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
        private Queue<Dialogue> dialoguesQueue;
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

        public void Init()
        {
            dialoguesQueue = new Queue<Dialogue>();
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

        private void Update() 
        {
            if(dialogueObject.activeSelf.Equals(true))
            {
                PopUpGenerator.Instance.messagesList.gameObject.SetActive(false);
            }
            else
                PopUpGenerator.Instance.messagesList.gameObject.SetActive(true);
        }

        internal void SetDialougeList(NPCManager npc, List<Dialogue> dialogueList, DialogueChoice[] choices)
        {
            SetDialogueChoices(npc, choices);
            for (int i = 0; i < dialogueList.Count; i++)
            {
                dialoguesQueue.Enqueue(dialogueList[i]);
            }

            if(dialoguesQueue.Count > 0)
                StartTalking(dialoguesQueue);
        }

        private void StartTalking(Queue<Dialogue> dialoguesQueue)
        {
            Dialogue dialogue = dialoguesQueue.Dequeue();
            StartDialogue(dialogue);
        }
        internal void StartDialogue(Dialogue dialogue)
        {
            Debug.Log("대화 시작 " + dialogue.characterName);
            dialogueObject.SetActive(true);


            GUIManager.instance.SetActiveHudWindows(false);

            //출력할 다이얼로그의 캐릭터 이름과 이미지 설정
            SetDialogue(dialogue);
            //출력할 다이얼로그의 문장 설정
            sentences.Clear();
            for (int i = 0; i < dialogue.sentences.Length; i++)
            {
                sentences.Enqueue(dialogue.sentences[i]);
            }

            //다음 문장을 보여줌.
            DisplayNextSentences();
        }
        private void SetDialogue(Dialogue dialogueList)
        {
            characterImage.sprite = dialogueList.image;
            nameText.text = dialogueList.characterName;
        }
        private void SetDialogueChoices(NPCManager npc, DialogueChoice[] choices)
        {
            if(dialogue_Choices.Count > 0)
            {
                for (int i = 0; i < dialogue_Choices.Count; i++)
                {
                    Destroy(dialogue_Choices[i]);
                }
            }
            dialogue_Choices.Clear();

            if (choices != null)
            {
                for (int i = 0; i < choices.Length; i++)
                {
                    //만약 NPC의 퀘스트가 아직안함이 아니라면 해당 선택지를 생성하지 않습니다.
                    if((choices[i].dialogChoiceType == DialogChoiceType.OpenQuest) 
                    && (npc.haveQuest.questProgress != QuestProgress.NotStarting))
                    {
                        continue;
                    }
                    DialogueChoiceBox dcb = Instantiate(choiceDialogue_Prefab, choicePanelTransform);
                    dcb.Init();
                    dcb.SetDialogChoice(choices[i].selectText, choices[i].icon, choices[i].action);
                    dialogue_Choices.Add(dcb.gameObject);
                }
            }
            else
                return;

            for (int i = 0; i < dialogue_Choices.Count; i++)
            {
                dialogue_Choices[i].gameObject.SetActive(false);
            }
        }      
        public void DisplayNextSentences()
        {
            //nextBtn의 이벤트 리스너
            
            //다음 버튼이 켜져있다면 끄기
            if (nextBtn.gameObject.activeSelf.Equals(true))
            {
                nextBtn.gameObject.SetActive(false);
            }

            //남아있는 문장의 카운트가 0이면
            //다른 다이얼로그가 아직 남아있는지 확인
            //남아 있다면, 그 다이얼로그를 시작
            //남아 있지 않다면 대화를 종료한다.

            if (sentences.Count == 0)
            {
                if(dialoguesQueue.Count > 0)
                {
                    StartTalking(dialoguesQueue);
                    return;
                }              
                EndDialogue();
                return;
            }

            //저장해뒀던 문장을 하나씩 빼와서 문장을 타이핑하는 코루틴을 실행.
            //NextBtn을 눌러야 다음 문장이 출력됨.
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
            endDialogueEvent = null;         
        }

        public void SetEndDialogueEvent(Action listener)
        {
            endDialogueEvent = listener;
        }
    }
}
