using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Launch : MonoBehaviour
{
    void Awake()
    {
        DontDestroyOnLoad(gameObject);

        Network.NetClient.Instance.Init("127.0.0.1", 8000);
        Network.NetClient.Instance.Connect();

        //SkillBridge.Message.NetMessage netMessage = new SkillBridge.Message.NetMessage();
        //netMessage.Request = new SkillBridge.Message.NetMessageRequest();
        //netMessage.Request.userLogin = new SkillBridge.Message.UserLoginRequest();
        //netMessage.Request.userLogin.User = "Year";
        //netMessage.Request.userLogin.Passward = "123";
        //Network.NetClient.Instance.SendMessage(netMessage);

        StartCoroutine(HotAssetUpdate());
        InitGameLogic();
    }

    IEnumerator HotAssetUpdate()
    {
        ViewManager.Instance.CreateView("UILoading");

        log4net.Config.XmlConfigurator.ConfigureAndWatch(new System.IO.FileInfo("log4net.xml"));
        UnityLogger.Init();
        Common.Log.Init("Unity");
        Common.Log.Info("LoadingManager start");

        yield return DataManager.Instance.LoadData();

        //Init basic services
        //MapService.Instance.Init();
        //UserService.Instance.Init();
        yield return null;
    }

    public void InitGameLogic()
    {

    }
}
