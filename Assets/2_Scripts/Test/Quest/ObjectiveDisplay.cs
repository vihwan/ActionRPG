using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SG
{
    public class ObjectiveDisplay : MonoBehaviour
    {

        public Image Icon;
        public TMP_Text titleText;
        public TMP_Text countText;

        // Start is called before the first frame update
        public void Initailize()
        {
            Icon = UtilHelper.Find<Image>(transform, "Icon");
            titleText = UtilHelper.Find<TMP_Text>(transform, "Title");
            countText = UtilHelper.Find<TMP_Text>(transform, "Count");
        }
        public void SetObjectiveDisplay(QuestObjective questObjective)
        {
            titleText.text = questObjective.title;
            countText.text = questObjective.progress * 100 + "%";
        }
        public void UpdateCountText(QuestObjective questObjective)
        {
            countText.text = questObjective.progress * 100 + "%";
        }
    }
}
