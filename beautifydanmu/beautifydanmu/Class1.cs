using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using BilibiliDM_PluginFramework;

namespace beautifydanmu
{
    public class Class1 : BilibiliDM_PluginFramework.DMPlugin
    {
        public Class1()
        {
            this.Connected += Class1_Connected;
            this.Disconnected += Class1_Disconnected;
            this.ReceivedDanmaku += Class1_ReceivedDanmaku;
            this.ReceivedRoomCount += Class1_ReceivedRoomCount;
            this.PluginAuth = "冬花ice";
            this.PluginName = "弹幕界面和美化";
            this.PluginCont = "flowerimsnow@hotmail.com";
            this.PluginVer = "1-Rel-1";
            this.PluginDesc = "通过HTML显示美化的弹幕";
        }
        private void Class1_ReceivedRoomCount(object sender, BilibiliDM_PluginFramework.ReceivedRoomCountArgs e)
        {
            
        }

        private void Class1_ReceivedDanmaku(object sender, BilibiliDM_PluginFramework.ReceivedDanmakuArgs e)
        {
            var ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            long times = Convert.ToInt64(ts.TotalMilliseconds);
            if (e.Danmaku.MsgType == MsgTypeEnum.Comment)
            {
                SendToLocalServer("1," + e.Danmaku.UserName + "," + e.Danmaku.CommentText);
            } else if (e.Danmaku.MsgType == MsgTypeEnum.GiftSend)
            {
                SendToLocalServer("2," + e.Danmaku.UserName + "," + e.Danmaku.GiftName + " * " + e.Danmaku.GiftCount);
            } else if (e.Danmaku.MsgType == MsgTypeEnum.Welcome || e.Danmaku.MsgType == MsgTypeEnum.WelcomeGuard)
            {
                SendToLocalServer("3," + e.Danmaku.UserName);
            } else if (e.Danmaku.MsgType == MsgTypeEnum.Interact)
            {
                if (e.Danmaku.InteractType == InteractTypeEnum.Enter)
                {
                    SendToLocalServer("3," + e.Danmaku.UserName);                    
                } else if (e.Danmaku.InteractType == InteractTypeEnum.Follow ||
                           e.Danmaku.InteractType == InteractTypeEnum.MutualFollow ||
                           e.Danmaku.InteractType == InteractTypeEnum.SpecialFollow)
                {
                    SendToLocalServer("4," + e.Danmaku.UserName);
                } else if (e.Danmaku.InteractType == InteractTypeEnum.Share)
                {
                    SendToLocalServer("5," + e.Danmaku.UserName);
                }
            }
        }

        private void Class1_Disconnected(object sender, BilibiliDM_PluginFramework.DisconnectEvtArgs e)
        {
            
        }

        private void Class1_Connected(object sender, BilibiliDM_PluginFramework.ConnectedEvtArgs e)
        {
            
        }

        public override void Admin()
        {
            base.Admin();
        }

        public override void Stop()
        {
            base.Stop();
        }

        public override void Start()
        {
            base.Start(); 
        }

        private void SendToLocalServer(string content)
        {
            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPAddress ip = IPAddress.Parse("127.0.0.1");
            IPEndPoint endPoint = new IPEndPoint(ip, 80);
            socket.Connect(endPoint);
            socket.Send(Encoding.UTF8.GetBytes("POSTDANMU\n"));
            socket.Send(Encoding.UTF8.GetBytes(content));
            socket.Close();
        }
    }
}