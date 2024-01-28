using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BananaPeel : MonoBehaviour
{
    [SerializeField] private CapsuleCollider capsuleCollider;
    [SerializeField] private SpriteRenderer sprite;
    [SerializeField] private Transform peelFromAboveTransform;


    private void Start()
    {
        Sequence sequence = DOTween.Sequence();
        sequence.Append(peelFromAboveTransform.DOLocalMoveY(0, 1)).OnComplete(ActivateBanana).SetEase(Ease.InCubic);
    }

    private void ActivateBanana()
    {
        peelFromAboveTransform.gameObject.SetActive(false);
        sprite.enabled = true;
        capsuleCollider.enabled = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out INpcBananaCOliision npc))
        {
            Destroy(gameObject);
            npc.BananaSlip();
        }
    }
}
