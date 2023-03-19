using Services;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Launch : MonoBehaviour
{
    void Awake()
    {
        DontDestroyOnLoad(gameObject);
        StartCoroutine(InitGameLogic());
    }

    IEnumerator InitGameLogic()
    {
        log4net.Config.XmlConfigurator.ConfigureAndWatch(new System.IO.FileInfo("log4net.xml"));

        ViewManager.Instance.CreateView("UILoading");
        UnityLogger.Init();
        Common.Log.Init("Unity");
        Common.Log.Info("LoadingManager start");

        yield return DataManager.Instance.LoadData();

        UserService.Instance.Init();
        MapService.Instance.Init();
        CharacterManager.Instance.Init();
        yield return null;

    }
}
