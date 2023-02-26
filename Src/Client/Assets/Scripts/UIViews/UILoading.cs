using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UILoading : MonoBehaviour
{
    public Scrollbar scrollbar;
    public Text text;
    IEnumerator Start()
    {
        yield return new WaitForSeconds(2f);
        for (float i = 0f; i < 100f;)
        {
            i += 1f;
            scrollbar.size = i / 100f;
            text.text = i + "%";
            yield return new WaitForEndOfFrame();
        }
        ViewManager.Instance.RemoveView("UILoading");
        ViewManager.Instance.CreateView("UILogin");
        yield return null;
    }
}
