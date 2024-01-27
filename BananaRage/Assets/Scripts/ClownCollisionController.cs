using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ClownCollisionController : MonoBehaviour, INpcBananaCOliision
{
    [SerializeField] private NPCController npcController;
    public void BananaSlip()
    {
        npcController.OnBananaSlip();
        GameController.Instance.ClownSlip();

    }
}
