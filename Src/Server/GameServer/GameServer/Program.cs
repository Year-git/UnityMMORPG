using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SkillBridge.Message;
using ProtoBuf;
using System.IO;
using Common;
using System.Threading;

namespace GameServer
{
    class Program
    {
        static void Main(string[] args)
        {
            // log4net : C#环境下广泛使用的日志记录库
            FileInfo fi = new System.IO.FileInfo("log4net.xml");
            log4net.Config.XmlConfigurator.ConfigureAndWatch(fi);

            // 初始化日志 Log输出的内容会输出到本地
            Log.Init("GameServer");
            Log.Info("Game Server Init");

            // 创建游戏服务器
            GameServer server = new GameServer();
            server.Init();
            server.Start();

            Console.WriteLine("Game Server Running......");

            // 启动服务器控制台指令器
            CommandHelper.Run();

            // 退出服务器
            Log.Info("Game Server Exiting...");
            server.Stop();
            Log.Info("Game Server Exited");
        }
    }
}
