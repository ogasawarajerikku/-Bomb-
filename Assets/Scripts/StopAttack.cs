using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopAttack : MonoBehaviour
{
    GameObject playerObject = null;
    PlayerController playerScript = null;

   
    private void LateUpdate()
    {
        if(playerObject==null)
        {
            playerObject = GameObject.FindGameObjectWithTag("Player");
            if (playerObject == null)
                return;
            playerScript = playerObject.GetComponent<PlayerController>();
        }
    }
    public void PointerEnter()
    {
        playerScript.SetCanAttack(false);
    }

    public void PointerExit()
    {
        playerScript.SetCanAttack(true);
    }
}
