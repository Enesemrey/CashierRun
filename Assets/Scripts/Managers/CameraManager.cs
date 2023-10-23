using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraManager : MonoSingleton<CameraManager>
{
    public enum CamType
    {
        Menu, Game, Win, Fail, Ending
    }

    public Camera mainCam;
    public CinemachineVirtualCamera menuCam;
    public CinemachineVirtualCamera gameCam;
    public CinemachineVirtualCamera winCam;
    public CinemachineVirtualCamera failCam;
    public CinemachineVirtualCamera endingCam;

    CinemachineVirtualCamera[] vcamArr;

    protected override void Awake()
    {
        base.Awake();

        vcamArr = new CinemachineVirtualCamera[System.Enum.GetNames(typeof(CamType)).Length];

        vcamArr[(int)CamType.Menu] = menuCam;
        vcamArr[(int)CamType.Game] = gameCam;
        vcamArr[(int)CamType.Win] = winCam;
        vcamArr[(int)CamType.Fail] = failCam;
        vcamArr[(int)CamType.Ending] = endingCam;
    }

    private void Start()
    {
        GameManager.instance.LevelStartedEvent += (() => { SetCam(CamType.Game); });
        GameManager.instance.LevelSuccessEvent += (() => { SetCam(CamType.Win); });
        GameManager.instance.LevelFailedEvent += (() => { SetCam(CamType.Fail); });
    }
    public CamType GetEndingCam()
    {
        return CamType.Ending;
    }
    public void SetCam(CamType camType)
    {
        for (int i = 0; i < vcamArr.Length; i++)
        {
            if (i == (int)camType)
            {
                vcamArr[i].Priority = 50;
            }

            else
            {
                vcamArr[i].Priority = 0;
            }
        }
    }

    public CinemachineVirtualCamera GetCam(CamType camType)
    {
        return vcamArr[(int)camType];
    }
}
