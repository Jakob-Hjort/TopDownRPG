using UnityEngine;
using UnityEngine.UI;

public class FloatingText
{
    public bool active;
    public GameObject go;
    public Text txt;
    public Vector3 motion;
    public float duration;
    public float lastShown;

    public void Show()
    {
        active = true;
        lastShown = Time.time;
        go.SetActive(active);
    }

    public void Hide()
    {
        active = false;
        go.SetActive(active);
    }

    public void UpdateFloatingText()
    {
        if (!active)
            return;
    

        //      10    -     7     >    2 ( hvis tiden lige nu (10) minus tiden vi startede med at vise (10) (10-7 = 3) er større end tiden den skal vise altså 2, skjules den. 
        if (Time.time - lastShown > duration)
        
            Hide();

        go.transform.position += motion * Time.deltaTime;

    }
    
}
