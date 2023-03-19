using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ViewManager : MonoSingleton<ViewManager>
{
    private Dictionary<string, GameObject> viewDictionary = new Dictionary<string, GameObject>();

    public void CreateView(string viewName)
    {
        if (viewDictionary.ContainsKey(viewName))
        {
            return;
        }
        Object obj = Resloader.LoadResources<Object>("UI/" + viewName);
        Debug.LogFormat("CreateView:{0}", viewName);
        GameObject go = Instantiate(obj) as GameObject;
        RectTransform rectTransform = go.GetComponent<RectTransform>();
        rectTransform.SetParent(transform);
        rectTransform.offsetMax = Vector2.zero;
        rectTransform.offsetMin = Vector2.zero;
        go.name = viewName;
        go.transform.localPosition = Vector3.zero;
        go.transform.localScale = Vector3.one;

        viewDictionary.Add(viewName, go);
    }

    public void RemoveView(string viewName)
    {
        GameObject view;
        if (!viewDictionary.TryGetValue(viewName, out view))
        {
            return;
        }
        Destroy(view);
    }
}
