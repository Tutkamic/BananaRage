using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimController : MonoBehaviour
{
    [SerializeField] private Transform aimTransform;
    [SerializeField] private GameObject bananaPeelObject;
    [SerializeField] private Aim aim;

    private float speed = 2f;
    private float tresholdPosY = 4.55f;
    private float tresholdPosX = 8.50f;
    private void Update()
    {
        AimMovement();
        BananaShootCheck();
        BananaMovementControll();
    }

    private void BananaMovementControll()
    {
        if (aimTransform.position.x > tresholdPosX) aimTransform.position = new Vector3(tresholdPosX, aimTransform.position.y, aimTransform.position.z);
        else if (aimTransform.position.x < -tresholdPosX) aimTransform.position = new Vector3(-tresholdPosX, aimTransform.position.y, aimTransform.position.z);

        if (aimTransform.position.y > tresholdPosY) aimTransform.position = new Vector3(aimTransform.position.x, tresholdPosY, aimTransform.position.z);
        else if (aimTransform.position.y < -tresholdPosY) aimTransform.position = new Vector3(aimTransform.position.x, -tresholdPosY, aimTransform.position.z);
    }

    private void AimMovement()
    {
        if (Input.GetKey(KeyCode.RightArrow)) aimTransform.Translate(Vector3.right * Time.deltaTime * speed);
        if (Input.GetKey(KeyCode.UpArrow)) aimTransform.Translate(Vector3.up * Time.deltaTime * speed);
        if (Input.GetKey(KeyCode.LeftArrow)) aimTransform.Translate(Vector3.left * Time.deltaTime * speed);
        if (Input.GetKey(KeyCode.DownArrow)) aimTransform.Translate(Vector3.down * Time.deltaTime * speed);
    }
    private void BananaShootCheck()
    {
        if (Input.GetKeyDown(KeyCode.Space)) BananaShoot();

    }
    private void BananaShoot()
    {
        if (!aim.CanSHoot || GameController.Instance.BananaAmount <= 0) return;
        AimShotAnimation();
        GameController.Instance.BananaThrow();
        Instantiate(bananaPeelObject, aimTransform.position, Quaternion.identity);
    }

    private void AimShotAnimation()
    {
        Sequence sequence = DOTween.Sequence();
        sequence.Append(aimTransform.transform.DOPunchScale(new Vector3(0.04f, 0.04f, 0.04f), 0.4f, 2));
    }
}
