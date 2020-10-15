using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemController : MonoBehaviour
{
    public AudioClip playerPickUpSound;
    AudioSource audioSource;
    public Text itemText = null;
    public GameObject createItemObject;
    RandomCreateItem createItemScript;
    void Awake()
    {
        itemText = GetComponentInChildren<Text>();
        audioSource = GetComponent<AudioSource>();
        createItemObject = GameObject.Find("CreateItem");
        createItemScript = createItemObject.GetComponent<RandomCreateItem>();
    }

    public void PickUp()
    {
        audioSource.PlayOneShot(playerPickUpSound);
        Destroy(GetComponent<Rigidbody>());
        Destroy(GetComponent<CapsuleCollider>());
        createItemScript.PickUpItem();
        Destroy(gameObject, 1f);
    }
}
