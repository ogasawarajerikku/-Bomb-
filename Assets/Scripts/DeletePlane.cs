
using UnityEngine;
using UnityEngine.SceneManagement;
//制作者　小笠原ジェリック
public class DeletePlane : MonoBehaviour
{
    string nowScene = null;
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Player")
        {
            nowScene = SceneManager.GetActiveScene().name;
            SceneManager.LoadScene(nowScene);
        }
        else
            Destroy(other.gameObject);
    }
}
