using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;

public class MovePositionNodes : MonoBehaviour
{
    private static float DISTANCE = .1f;
    private BlockNodes[,] mapNodes;
    private int3[] nodeField;
    private Node lastNode;
    private Vector3 currentNodePosition;
    private Animator animator;
    private int i;
    private Node currentNode;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        i = -1;
    }

    public void SetMovePosition(int3[] nodeField, int3 endPos)
    {
        GetComponent<IMoveVelocity>().SetVelocity(new Vector3(0, 0, 0));//stop current movement
        mapNodes = GameObject.Find("WorldHandler").GetComponent<WorldGenerationHandler>().mapNodes;
        this.nodeField = nodeField;
        this.lastNode = mapNodes[endPos.y,endPos.x].GetNode(endPos.z);
        i = nodeField.Length - 1;
        bool drawPath = true;
        if (drawPath)
        {
            for (int j = 0; j < i; j++)
            {
                GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                Node node = mapNodes[nodeField[j].x, nodeField[j].y].GetNode(nodeField[j].z);
                Vector3 pos = node.position;
                pos.z = -5;
                sphere.transform.position = pos;
                sphere.transform.localScale = new Vector3(.1f, .1f, .1f);
                sphere.transform.name = nodeField[j].z + " " + j;
            }
        }// if path exist then draw it with primitive spheres
    }
    //private void LateUpdate()
    //{
    //    if (position != null && Vector3.Distance(new Vector3(transform.position.x, transform.position.y, 0), new Vector3(position.x, position.y, 0)) < .3f)
    //    {
    //        GetComponent<IMoveVelocity>().SetVelocity(new Vector3(0, 0));
    //    }
    //}
    //public void MoveTo(Vector3 position)
    //{
    //    this.position = position;
    //    Vector3 moveDir = (position - transform.position).normalized;
    //    Debug.Log(moveDir);
    //    GetComponent<IMoveVelocity>().SetVelocity(moveDir);
    //}
    private void Update()
    {
        if (i >= 0 && nodeField != null)
        {

            currentNode = mapNodes[nodeField[i].x, nodeField[i].y].GetNode(nodeField[i].z);
            currentNodePosition = currentNode.position;
            currentNodePosition.z = transform.position.z; // neutralize Z movement for now

            if (transform.position.x - currentNodePosition.x < .5f) GetComponent<IMoveVelocity>().SetVelocityX(0);
            if (transform.position.y - currentNodePosition.y < .5f) GetComponent<IMoveVelocity>().SetVelocityY(0);
            if (!animator.GetBool("isWalking"))
            {
                animator.SetBool("isWalking", true);
                Debug.Log("Start walking anim");
            }// activate walking animation if its not active

            if (Vector3.Distance(transform.position, currentNodePosition) < DISTANCE)
            {
                //Debug.Log("ciel " + i);
                GetComponent<IMoveVelocity>().SetVelocity(new Vector3(0, 0, 0));
                i--;
            }// if node is reached decrement i by 1
            else
            {
                if (!GetComponent<IMoveVelocity>().GetVelocity().Equals(new Vector3(0,0,0)))
                {

                }// if object is moving (velocity dont equal zero)
                else
                {
                    //currentNode = mapNodes[nodeField[i].x, nodeField[i].y].GetNode(nodeField[i].z);
                    //currentNodePosition = currentNode.position;
                    //Debug.Log("currPos: " + transform.position);
                    //Debug.Log("currNodePos: " + currentNodePosition);
                    //Vector3 moveDir = new Vector3(transform.position.y - currentNodePosition.x, 0, 0);
                    Vector3 moveDir = (/*lastNode.position*/currentNodePosition - transform.position)/*.normalized*/;
                    Debug.DrawRay(transform.position, moveDir, Color.green, 300f);
                    moveDir = moveDir.normalized;
                    GetComponent<IMoveVelocity>().SetVelocity(moveDir);
                    //Debug.Log("moveDir: " + moveDir);
                    //i = -1;
                }// if object is stationary
            }// if position havent reached target yet

            if (i < 0)
            {
                if (animator.GetBool("isWalking"))
                {
                    Debug.Log("Stop walking anim");
                    animator.SetBool("isWalking", false);
                }// activate walking animation if its not active
                //                nodeField = null;
                //                i = -1;
                //                GetComponent<IMoveVelocity>().SetVelocity(new Vector3(0, 0, 0));
                //                //Debug.Log("Dorazily ste do ciela, prosim vystupte.");
                //                animator.SetBool("isWalking", false);
            }// when position reaches end start IDLE animation, stop moving and set i to -1


        }//do wihile there is nodes to go to

    }
}