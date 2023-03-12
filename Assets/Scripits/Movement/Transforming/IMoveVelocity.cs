using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMoveVelocity
{
    void SetVelocity(Vector3 velocityVector);
    void SetVelocityX(int velocityVectorX);
    void SetVelocityY(int velocityVectorY);
    void SetVelocityZ(int velocityVectorZ);
    Vector3 GetVelocity();
}
