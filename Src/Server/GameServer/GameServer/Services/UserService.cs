using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
using Network;
using SkillBridge.Message;

namespace GameServer.Services
{
    class UserService : Singleton<UserService>
    {
        public UserService()
        {
            MessageDistributer<NetConnection<NetSession>>.Instance.Subscribe<UserLoginRequest>(this.OnLogin);
        }

        public void Init()
        {
        }

        public void Start()
        {

        }

        void OnLogin(NetConnection<NetSession> sender, UserLoginRequest request)
        {
            Console.WriteLine("UserName = {0}  PassWord  = {1}", request.User, request.Passward);

            //SkillBridge.Message.NetMessage netMessage = new SkillBridge.Message.NetMessage();
            //netMessage.Request = new SkillBridge.Message.NetMessageRequest();
            //netMessage.Request.userLogin = request;
            //MessageDistributer<NetConnection<NetSession>>.Instance.
        }
    }
}
