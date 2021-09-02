using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Text;

namespace SG
{
    public class SetTemplate : MonoBehaviour
    {
        public TMP_Text setName;
        public TMP_Text setExplain;

        public StringBuilder sb = new StringBuilder();

        private void Start()
        {
            setName = UtilHelper.Find<TMP_Text>(transform, "SetName");
            setExplain = UtilHelper.Find<TMP_Text>(transform, "SetExplain");
        }

        public void SetEffect(SetItem setItem)
        {
            setName.text = setItem.setName;
            SetExplainText(setExplain, setItem);
        }

        private void AddStatusText(string effectProperty)
        {
            if (sb.Length > 0)
                sb.AppendLine();

            sb.Append("2세트 : ");
            sb.Append(effectProperty);
        }
        private void SetExplainText(TMP_Text tMP_Text, SetItem setItem)
        {
            sb.Length = 0;
            AddStatusText(setItem.setEffect_One);
            AddStatusText(setItem.setEffect_Two);

            tMP_Text.text = sb.ToString();
        }
    }
}
