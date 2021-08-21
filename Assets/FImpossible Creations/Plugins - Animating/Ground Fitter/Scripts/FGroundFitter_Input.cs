using System;
using UnityEngine;

namespace FIMSpace.GroundFitter
{
    public class FGroundFitter_Input : FGroundFitter_InputBase
    {
        public float RotationOffset = 0f;
        //protected virtual void Update()
        //{

        //    if (Input.GetKeyDown(KeyCode.Space)) TriggerJump();

        //    if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D))
        //    {
        //        if (Input.GetKey(KeyCode.LeftShift)) Sprint = true; else Sprint = false;

        //        if (Input.GetKey(KeyCode.A)) RotationOffset += -1;
        //        if (Input.GetKey(KeyCode.D)) RotationOffset += 1;
        //        if (Input.GetKey(KeyCode.S)) RotationOffset += 180;

        //        MoveVector = Vector3.forward;
        //    }
        //    else
        //    {
        //        Sprint = false;
        //        MoveVector = Vector3.zero;
        //    }
            
        //    MoveVector.Normalize();

        //    controller.Sprint = Sprint;
        //    controller.MoveVector = MoveVector;
        //    controller.RotationOffset = RotationOffset;
        //}
        public void SetControllerValue(bool Sprint, Vector3 MoveVector, float RotationOffset)
        {
            controller.Sprint = Sprint;
            controller.MoveVector = MoveVector;
            controller.RotationOffset = RotationOffset;
        }

        internal void SetControllerValue(object sprint, object moveVector, object rotationOffset)
        {
            throw new NotImplementedException();
        }
    }
}