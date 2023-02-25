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

        SkillBridge.Message.NetMessage netMessage = new SkillBridge.Message.NetMessage();
        netMessage.Request = new SkillBridge.Message.NetMessageRequest();
        netMessage.Request.userLogin = new SkillBridge.Message.UserLoginRequest();
        netMessage.Request.userLogin.User = "Year";
        netMessage.Request.userLogin.Passward = "123";
        Network.NetClient.Instance.SendMessage(netMessage);
    }
}
