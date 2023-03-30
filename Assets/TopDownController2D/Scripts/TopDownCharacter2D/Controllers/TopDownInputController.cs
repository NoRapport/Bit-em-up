﻿using UnityEngine;
using UnityEngine.InputSystem;

namespace TopDownCharacter2D.Controllers
{
    /// <summary>
    ///     This class encapsulate all the input processing for a player using Unity's new input system
    /// </summary>
    public class TopDownInputController : TopDownCharacterController
    {

    // Auto lock objects
        public GameObject[] targetList;
        public GameObject targetLocked;
    //

//        private Camera _camera;
//
//        protected override void Awake()
//        {
//            base.Awake();
//            _camera = Camera.main;
//        }

    // Auto lock functions
        void FixedUpdate()
        {
            targetList = GameObject.FindGameObjectsWithTag("Target");
            targetLocked = TargetLocked();

            if (targetList.Length != 0) {
              Vector2 newAim = targetLocked.transform.position - transform.position;
              LookEvent.Invoke(newAim);
            }
        }

        public GameObject TargetLocked()
        {
          float lastTargetDist = 100000; //add your max range here
          GameObject closestObject = null;

          for (int i = 0; i < targetList.Length; i++)  //list of gameObjects to search through
          {
            float dist = Vector3.Distance(targetList[ i ].transform.position, transform.position);

            if (dist < lastTargetDist)
            {
              lastTargetDist = dist;
              closestObject = targetList[ i ];
            }
          }
        return closestObject;
        }
    //


        #region Methods called by unity input events

        /// <summary>
        ///     Method called when the user input a movement
        /// </summary>
        /// <param name="value"> The value of the input </param>
        public void OnMove(InputValue value)
        {
            Vector2 moveInput = value.Get<Vector2>().normalized;
            OnMoveEvent.Invoke(moveInput);
        }

//        /// <summary>
//        ///     Method called when the user enter a look input
//        /// </summary>
//        /// <param name="value"> The value of the input </param>
//        public void OnLook(InputValue value)
//        {
//            Vector2 newAim = value.Get<Vector2>();
//            if (!(newAim.normalized == newAim))
//            {
//                Vector2 worldPos = _camera.ScreenToWorldPoint(newAim);
//                newAim = (worldPos - (Vector2) transform.position).normalized;
//            }
//
//            if (newAim.magnitude >= .9f)
//            {
//                LookEvent.Invoke(newAim);
//            }
//        }

        /// <summary>
        ///     Method called when the user enter a fire input
        /// </summary>
        /// <param name="value"> The value of the input </param>
        public void OnFire(InputValue value)
        {
            IsAttacking = value.isPressed;
            Debug.Log(IsAttacking);
        }

        #endregion
    }
}
