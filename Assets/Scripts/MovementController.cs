using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MovementController : MonoSingleton<MovementController>
{
    public SwerveInputSystem swerveInputSystem;
    public float swerveAmount;
    [SerializeField] private float swerveSpeed;
    [SerializeField] private float maxSwerveAmount = 1f;
    public float sidewaysLimit_x;

    public bool isActive;
    public float speed;

    public Vector3 moveDirection;
    public Vector3 lastLoc;

    public float rotationAngle;
    public float rotationSpeed;

    Vector3 defaultRotation = new Vector3(0, 0, 0);
    Vector3 endingPosition;

    public bool inSwerveMode;
    void Start()
    {
        GameManager.instance.LevelStartedEvent += LevelStartedEvent;
        swerveInputSystem = SwerveInputSystem.instance;

    }

    private void LevelStartedEvent()
    {
        isActive = true;
        inSwerveMode = true;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (isActive)
        {
            if (inSwerveMode)
            {
                MoveForward();
                MoveSideways();

                if (swerveInputSystem.MoveFactorX == 0)
                {
                    ResetAngle();
                }
                if (!swerveInputSystem._isTouching)
                {
                    ResetAngle();
                }
                //transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(-10, rotationAngle, 0), 1);

            }
            else if(!inSwerveMode)
            {
                if (swerveInputSystem._isTouching)
                {
                    CanvasManager.instance.ShowText(false);
                    MoveForward();
                    MoveSideways();

                    if (swerveInputSystem.MoveFactorX == 0)
                    {
                        ResetAngle();
                    }
                }
                else
                {
                    ResetAngle();
                }
                //transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(-10, rotationAngle, 0), 1);
                //transform.rotation = Quaternion.Euler(0, rotationAngle, 0);
            }
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(-10, 0, rotationAngle * -1), 1);
        }
        else
        {
            ResetAngle();
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(-10, rotationAngle, 0), 1);

        }
    }

    public void MoveForward()
    {
        Vector3 tempvector = new Vector3(0,0,1);
        transform.position += tempvector * speed * Time.deltaTime;
    }
    public void MoveSideways()
    {
        Vector3 pos = transform.position;

        swerveAmount = Time.deltaTime * swerveSpeed * swerveInputSystem.MoveFactorX;
        swerveAmount = Mathf.Clamp(swerveAmount, -maxSwerveAmount, maxSwerveAmount);
        //rotationAngle += swerveAmount * rotationSpeed;
        rotationAngle += Mathf.Lerp(rotationAngle, swerveAmount * rotationSpeed, 1);

        rotationAngle = Mathf.Clamp(rotationAngle, -20, 20);
        pos += swerveAmount * Vector3.right;
        pos.x = Mathf.Clamp(pos.x, -sidewaysLimit_x, sidewaysLimit_x);
        transform.position = pos;
    }
    public void ResetAngle()
    {
        rotationAngle = Mathf.Lerp(rotationAngle, 0, Time.deltaTime * 3);
    }
    public void DoEndingRotation()
    {
        transform.DOLocalRotate(defaultRotation, 1);
    }
    public void DoEndingPosition()
    {

        endingPosition = new Vector3(0, transform.position.y, transform.position.z);

        transform.DOMove(endingPosition, 1);
    }
}
