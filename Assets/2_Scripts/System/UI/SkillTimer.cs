using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace SG
{
    public class SkillTimer : MonoBehaviour
    {
        [SerializeField] private Animation Animation;
        private TMP_Text tmp_text;

        private float time = 1f;     //업데이트가 될 총 시간값
        private float elapsedTime = 0f; //업데이트 경과 시간
        private int interval = 1;    // 시간 주기로 타이머 애니메이션을 실행시키기 위한 변수
        private int remain = 0;      // 
        private bool update = false;
        private float intervalElapsedTime = 0f;

        // Start is called before the first frame update
        public void Init()
        {
            Animation = GetComponent<Animation>();
            tmp_text = GetComponent<TMP_Text>();
        }

        public void Execute(int targetTime, int interval = 1)
        {
            update = true;
            time = targetTime;
            remain = targetTime;
            this.interval = interval;
            intervalElapsedTime = 0f;

            //텍스트를 활성화하고, 텍스트의 값을 변경한 상태에서 플레이합니다.
            gameObject.SetActive(true);
            tmp_text.text = remain.ToString();
            Animation.Play();
        }

        // Update is called once per frame
        void Update()
        {
            if (!update)
                return;

            intervalElapsedTime += Time.deltaTime;
            // intervalElapsedTime = Mathf.Clamp(intervalElapsedTime,0f,interval);
            if (intervalElapsedTime >= interval)
            {
                intervalElapsedTime = 0;
                remain -= interval;
                tmp_text.text = remain.ToString();

                Animation.Play();
            }

            elapsedTime += Time.deltaTime / time;
            // 업데이트가 모두 끝난 상태라면, 텍스트를 비활성화합니다.
            if (elapsedTime >= 1.0f)
            {
                update = false;
                elapsedTime = 0f;
                gameObject.SetActive(false);
            }
        }
    }
}
