using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwerveInputSystem : MonoSingleton<SwerveInputSystem>
{
    [SerializeField] private float lastFramePositionX;
    [SerializeField] private float _moveFactorX;
    public float initialPositionX;

    public bool isStarted = false;


    public float MoveFactorX => _moveFactorX;
    public float FirstPosition => initialPositionX;
    public float LastPosition => lastFramePositionX;
    public bool _isTouching => isTouching;
    public bool IsStarted => isStarted;


    public bool isTouching = false;


    private Vector3 position;
    private float width;
    private float height;


    protected override void Awake()
    {
        base.Awake();

        width = (float)Screen.width / 2.0f;
        height = (float)Screen.height / 2.0f;

        position = new Vector3(0.0f, 0.0f, 0.0f);

    }
    private void Update()
    {
        if (GameManager.instance.isLevelActive)
        {
            if (Input.GetMouseButtonDown(0))
            {
                initialPositionX = NitroUtilities.GetScreenRelativeTouchPos().x;
                lastFramePositionX = NitroUtilities.GetScreenRelativeTouchPos().x;

                isStarted = true;
                isTouching = true;


            }
            else if (Input.GetMouseButton(0))
            {
                _moveFactorX = NitroUtilities.GetScreenRelativeTouchPos().x - lastFramePositionX;
                lastFramePositionX = NitroUtilities.GetScreenRelativeTouchPos().x;

            }
            else if (Input.GetMouseButtonUp(0))
            {
                _moveFactorX = 0f;
                initialPositionX = 0f;
                lastFramePositionX = 0f;
                isTouching = false;
            }
        }

    }
}
