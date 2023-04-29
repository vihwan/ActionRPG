using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace SA
{
    public class Helper : MonoBehaviour
    {
        [Range(0, 1)]
        public float vertical;

        public string animName;
        public bool playAnim;
        public bool enableRM;

        private Animator ani;
        // Start is called before the first frame update
        void Start()
        {
            ani = GetComponent<Animator>();
        }

        // Update is called once per frame
        void Update()
        {
            enableRM = !ani.GetBool("canMove");
            ani.applyRootMotion = enableRM;

            if (enableRM)
                return;

            if (playAnim)
            {
                vertical = 0f;
                ani.CrossFade(animName, 0.2f);
               // ani.SetBool("canMove", false);
               // enableRM = true;
                playAnim = false;
            }

            ani.SetFloat("Vertical", vertical);
        }
    }
}
