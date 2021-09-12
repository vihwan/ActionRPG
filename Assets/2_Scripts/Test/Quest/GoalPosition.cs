using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace SG
{
    public class GoalPosition : MonoBehaviour
    {
        public string goalName;

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                other.GetComponent<PlayerQuestInventory>().SetReachGoalPosition(this);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                other.GetComponent<PlayerQuestInventory>().SetReachGoalPosition(null);
            }
        }
    }
}
