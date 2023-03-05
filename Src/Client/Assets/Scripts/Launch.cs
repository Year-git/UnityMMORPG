using Services;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Launch : MonoBehaviour
{
    void Awake()
    {
        DontDestroyOnLoad(gameObject);

        HotAssetUpdate();
        StartCoroutine(InitGameLogic());
    }

    public void HotAssetUpdate()
    {
    }

    IEnumerator InitGameLogic()
    {
        ViewManager.Instance.CreateView("UILoading");

        log4net.Config.XmlConfigurator.ConfigureAndWatch(new System.IO.FileInfo("log4net.xml"));
        UnityLogger.Init();
        Common.Log.Init("Unity");
        Common.Log.Info("LoadingManager start");

        yield return DataManager.Instance.LoadData();

        //Init basic services
        //MapService.Instance.Init();
        UserService.Instance.Init();
        yield return null;

    }
}
