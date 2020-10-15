using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteArc : MonoBehaviour
{
    //キャラクターオブジェクト
    GameObject playerObj = null;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (playerObj == null)
        {
            if (playerObj != null)
                return;
            var deleteArcs = GameObject.FindGameObjectsWithTag("Arc");
            foreach (var deleteArc in deleteArcs)
            {
                Destroy(deleteArc);
            }
            playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj == null)
                return;
        }
    }
}
