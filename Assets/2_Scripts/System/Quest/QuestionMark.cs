using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace SG
{
    public class QuestionMark : MonoBehaviour
    {
        public Sprite questMark;
        public Sprite checkMark;
        public Sprite exclamMark;

        public SpriteRenderer spriteRenderer;

        public void Init(){
            spriteRenderer = GetComponent<SpriteRenderer>();
        }
    }
}

