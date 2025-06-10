using UnityEngine;

public class VRPlayer : MonoBehaviour
{
    public Transform mainCamera;
    // public float lookDownAngle = 15.0f;
    // public float lookUpAngle = 15.0f;
    public float lookAngleThreshold = 15.0f;
    public float speed = 2.0f;
    public bool moveForward;

    private CharacterController cc;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        cc = GetComponent<CharacterController>();

    }

    // Update is called once per frame
    void Update()
    {
        //         if (mainCamera.eulerAngles.x >= lookDownAngle && mainCamera.eulerAngles.x < 60.0f)
        //         {
        //             moveForward = true; // the stae changes
        //         }
        //         else
        //         {
        //             moveForward = false;
        //         }
        // 
        //         if (moveForward)
        //         {
        //         Vector3 forward = mainCamera.forward;
        //         forward.Normalize(); // 归一化方向向量
        // 
        //         cc.Move(forward * speed * Time.deltaTime);
        //         }
        // Map angles from 0~360 to -180~180
        float pitch = mainCamera.eulerAngles.x;
        if (pitch > 180f) pitch -= 360f;

        // If the head is raised or lowered beyond the threshold, flight is triggered
        if (pitch >= lookAngleThreshold || pitch <= -lookAngleThreshold)
        {
            moveForward = true;
        }
        else
        {
            moveForward = false;
        }

        if (moveForward)
        {
            Vector3 forward = mainCamera.forward;
            forward.Normalize();
            cc.Move(forward * speed * Time.deltaTime);
        }
    }
}