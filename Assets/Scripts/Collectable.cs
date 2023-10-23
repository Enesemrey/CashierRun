using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Collectable : MonoBehaviour
{
    public float value;
    public GameObject connectedNode;
    public float followSpeed;
    public bool isCollected;
    public bool isCent;

    Vector3 baseScale;
    Vector3 basePos;
    Vector3 targetScale;
    Vector3 targetPos;

    public float followHeight;
    public float follow_x;
    public float follow_z;

    float randomTimeOffset;
    public bool atEndingState;
    public Tween tween;
    private void Start()
    {
        followHeight = 0.05f;
        followSpeed = 100;
        baseScale = transform.localScale;
        targetScale = new Vector3(baseScale.x * 1.4f, baseScale.y * 1.4f, baseScale.z * 1.4f);

        randomTimeOffset = Random.Range(-99999, 99999);
        follow_x = Random.Range(-0.3f, 0.3f);
        follow_z = Random.Range(-0.3f, 0.3f);

        if (!isCollected)
        {
            tween.SetLoops(4, LoopType.Yoyo).SetSpeedBased();
            Vector3 tempPos = new Vector3(transform.position.x, transform.position.y + 1, transform.position.z);
            tween = transform.DOMove(tempPos,1.5f).SetLoops(-1, LoopType.Yoyo);
            tween.Play();
        }
    }


    float currentMoveFactor;
    private void Update()
    {
        if (isCollected && connectedNode != null)
        {
            if (!isCent)
            {
                transform.position = new Vector3(
                Mathf.Lerp(transform.position.x, connectedNode.transform.position.x, Time.deltaTime * followSpeed),
                Mathf.Lerp(transform.position.y, connectedNode.transform.position.y + followHeight, Time.deltaTime * followSpeed),connectedNode.transform.position.z);
                transform.rotation = Quaternion.Euler(-10, transform.rotation.eulerAngles.y, MovementController.instance.rotationAngle * -1);
            }
            else if (isCent)
            {
                Vector3 temPos = new Vector3(
                    connectedNode.transform.position.x + follow_x,
                    connectedNode.transform.position.y + followHeight, 
                    connectedNode.transform.position.z + follow_z);

                currentMoveFactor = Mathf.Lerp(currentMoveFactor, SwerveInputSystem.instance.MoveFactorX, Time.deltaTime * 5);
                float currentTime = (Time.time * currentMoveFactor * 20) + randomTimeOffset;

                float xNoise = Mathf.PerlinNoise(currentTime, 0);
                xNoise = (xNoise * 2) - 1;

                float zNoise = Mathf.PerlinNoise(0, currentTime);
                zNoise = (zNoise * 2) - 1;

                if (!atEndingState)
                {
                    temPos.x += xNoise * 0.2f;
                    temPos.z += zNoise * 0.2f;
                }
                else if (atEndingState)
                {

                }

                transform.position = temPos;

                transform.rotation = Quaternion.Euler(0, 0, 90);
                transform.rotation = Quaternion.Euler(-10, transform.rotation.eulerAngles.y, (MovementController.instance.rotationAngle * -1) + 90);

            }
        }
        
    }
    public void DisableTween()
    {
        tween.Kill();
    }
    public WaitForSeconds DoScale()
    {
        IEnumerator ScaleOverTime()
        {
            if (!isCent)
            {
                transform.DOScale(targetScale, 0.15f);

                Move();

                yield return new WaitForSeconds(0.15f);

                transform.DOScale(baseScale, 0.15f);
            }
        }
        StartCoroutine(ScaleOverTime());
        return new WaitForSeconds(0.1f);
    }
    public void Move()
    {
        IEnumerator MoveOverTime()
        {
            if (!isCent)
            {
                DOTween.To(() => followHeight, x => followHeight = x, 0.2f, 0.1f);

                //targetPos = new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z);

                //transform.DOMove(targetPos, 0.08f).SetEase(Ease.InOutSine);
                yield return new WaitForSeconds(0.1f);

                DOTween.To(() => followHeight, x => followHeight = x, 0.05f, 0.1f);

                //targetPos = new Vector3(transform.position.x, targetPos.y - 0.5f, transform.position.z);

                //transform.DOMove(targetPos, 0.08f).SetEase(Ease.InOutSine);

            }
        }
        StartCoroutine(MoveOverTime());
    }
}
