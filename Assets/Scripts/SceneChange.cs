using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//制作者　小笠原ジェリック
public class SceneChange : MonoBehaviour
{
    public AudioClip sound1;
    AudioSource audioSource;
    public string nextScene = null;
    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }
   
    public void MoveToNextScene()
    {
        audioSource.PlayOneShot(sound1);
        SceneManager.LoadScene(nextScene);
    }
}
