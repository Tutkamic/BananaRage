using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using DG.Tweening;

public class NPCController : MonoBehaviour
{
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Sprite spriteUp;
    [SerializeField] private Sprite spriteDown;
    [SerializeField] private Sprite spriteRight;
    private float distanceTreshold = 0.2f;
    private float speed = 1.5f;
    private Vector3 randomDestination;
    private bool isSlipped;

    private void Awake()
    {
        agent.speed = speed;
    }
    private void SetRandomDestination()
    {
        randomDestination = Random.insideUnitSphere * 5f;
        randomDestination = new Vector3(randomDestination.x, randomDestination.y, 0);
        agent.SetDestination(randomDestination);

        CheckSprite();
    }
    private void CheckSprite()
    {
        spriteRenderer.flipX = false;
        Vector3 normalized = (randomDestination - transform.position).normalized;
        float angle = Vector3.Angle(normalized, transform.up);

        if (angle < 45) spriteRenderer.sprite = spriteUp;
        else if (angle > 45 && angle < 135 && normalized.x > 0) spriteRenderer.sprite = spriteRight;
        else if (angle > 135) spriteRenderer.sprite = spriteDown;
        else
        {
            spriteRenderer.sprite = spriteRight;
            spriteRenderer.flipX = true;
        }

    }
    private void Update()
    {
        if (isSlipped) return;
        if (agent.remainingDistance <= distanceTreshold) SetRandomDestination();
    }

    public void OnBananaSlip()
    {
        isSlipped = true;
        agent.isStopped = true;
        SlipAnimation();
    }

    private void SlipAnimation()
    {
        Vector3 rotation = spriteRenderer.sprite == spriteRight ? new Vector3(0, 0, 90) : new Vector3(0, 0, -90);
        Sequence sequence = DOTween.Sequence();
        sequence.Append(gameObject.transform.DOLocalRotate(rotation, 1.5f)).OnComplete(OnSlipAnimationComplete).SetEase(Ease.OutExpo);
    }

    private void OnSlipAnimationComplete()
    {
        StartCoroutine(HideWithDelay());
    }

    IEnumerator HideWithDelay()
    {
        yield return new WaitForSeconds(1);
        gameObject.SetActive(false);
    }
}
