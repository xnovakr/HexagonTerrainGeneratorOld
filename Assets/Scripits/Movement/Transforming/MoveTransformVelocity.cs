using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTransformVelocity : MonoBehaviour, IMoveVelocity
{
    [SerializeField] public float moveSpeed;

    private Vector3 velocityVector;

    private void Update()
    {
        transform.position += velocityVector * moveSpeed * Time.deltaTime;
    }

    public void SetVelocity(Vector3 velocityVector)
    {
        this.velocityVector = velocityVector;
    }

    public void SetVelocityX(int velocityVectorX)
    {
        this.velocityVector.x = velocityVectorX;
    }
    public void SetVelocityY(int velocityVectorY)
    {
        this.velocityVector.y = velocityVectorY;
    }
    public void SetVelocityZ(int velocityVectorZ)
    {
        this.velocityVector.z = velocityVectorZ;
    }
    public Vector3 GetVelocity()
    {
        return velocityVector;
    }
}
