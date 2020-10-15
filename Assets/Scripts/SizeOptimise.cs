using UnityEngine;

public class SizeOptimise : MonoBehaviour
{
    GameObject canvas;
    public bool horizontal = true;
    public bool vertical = true;

    void Start()
    {
        canvas = transform.root.gameObject;
        float width = gameObject.GetComponent<RectTransform>().sizeDelta.x;
        float height = gameObject.GetComponent<RectTransform>().sizeDelta.y;
        if (horizontal)
        {
            width = canvas.GetComponent<RectTransform>().sizeDelta.x;
        }
        if (vertical)
        {
            height = canvas.GetComponent<RectTransform>().sizeDelta.y;
        }

        gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(width, height);
        //gameObject.GetComponent<RectTransform>().localPosition = new Vector3(0, 0, 0);
    }
}