using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimController : MonoBehaviour
{
    [SerializeField] private Transform aimTransform;
    [SerializeField] private GameObject bananaPeelObject;
    [SerializeField] private Aim aim;

    private float speed = 2f;
    private void Update()
    {
        AimMovement();
        BananaShootCheck();
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
        Instantiate(bananaPeelObject, aimTransform.position, Quaternion.identity);
    }
}
