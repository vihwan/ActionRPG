using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SG
{
    public enum SetType
    {
        Jason, //이아손
        Zhongli, //종려
        Fragnance //향기
    }
    
    public class SetItem : ScriptableObject
    {
        public SetType setType;
        public string setName;
        public string setEffect_One;
        public string setEffect_Two;

        public SetType SetType
        {
            get => setType;
            private set
            {
                setType = value;
                switch (SetType)
                {
                    case SetType.Jason:
                        setName = "이아손의 차별";
                        setEffect_One = "이아손은 자신보다 헤라클레스를 더 소중히 여깁니다.";
                        setEffect_Two = "때로는 우수하고 냉철한 지휘력을 보여주기도 합니다.";
                        break;

                    case SetType.Zhongli:
                        setName = "종려의 건망증";
                        setEffect_One = "\"아쉽게도 까먹었어.\"";
                        setEffect_Two = "그래서 뭘 까먹었다는 걸까요?";
                        break;

                    case SetType.Fragnance:
                        setName = "해동의 향기";
                        setEffect_One = "무엇을 해동한다는 걸까요?";
                        setEffect_Two = "해동하는데 향기가 나긴 하는 것인지, 과학적으로도 의문입니다.";
                        break;
                }
            }
        }
    }
}

