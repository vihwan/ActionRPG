using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SG
{
    public class InteractDoor : Interactable
    {

        [Header("GameObject")]
        public GameObject leftDoor;
        public GameObject rightDoor;
        public GameObject[] etcObject;

        [Header("Target Vector")]
        public Vector3 leftDoorOpenRotation;
        public Vector3 rightDoorOpenRotation;

        public bool isOpen;

        public override void Interact(PlayerManager playerManager)
        {
            if (canInteract.Equals(false))
                return;

            canInteract = false;

            // Vector3 rotationDirection = transform.position - playerManager.transform.position;
            // rotationDirection.y = 0;
            // rotationDirection.Normalize();

            // Quaternion tr = Quaternion.LookRotation(rotationDirection);
            // Quaternion targetRotation = Quaternion.Slerp(playerManager.transform.rotation, tr, 300 * Time.deltaTime);
            // playerManager.transform.rotation = targetRotation;

            playerManager.OpenInteractionToVelocityZero();
            playerManager.InteractableUI.SetActiveInteractUI(false);

            for (int i = 0; i < etcObject.Length; i++)
            {
                Destroy(etcObject[i]);
            }
            StartCoroutine(OpenDoor());
        }

        private IEnumerator OpenDoor()
        {
            while (leftDoor.transform.eulerAngles.y < leftDoorOpenRotation.y)
            {
                leftDoor.transform.rotation = Quaternion.Slerp(leftDoor.transform.rotation, Quaternion.Euler(leftDoorOpenRotation), Time.deltaTime);
                rightDoor.transform.rotation = Quaternion.Slerp(rightDoor.transform.rotation, Quaternion.Euler(rightDoorOpenRotation), Time.deltaTime);
                yield return null;

                // if (leftDoor.transform.eulerAngles.y >= leftDoorOpenRotation.y  || 
                //     rightDoor.transform.eulerAngles.y <= rightDoorOpenRotation.y)
                // {
                //     isOpen = true;
                //     Debug.Log("문 전부 열림");
                //     yield break;
                // }
            }
        }
    }
}
