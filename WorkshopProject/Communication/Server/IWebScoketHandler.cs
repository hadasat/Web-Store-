﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;

namespace WorkshopProject.Communication.Server
{
    public interface IWebScoketHandler
    {
        void onMessage(List<byte[]> bufferCollector, WebSocketReceiveResult receiveResult, uint myId);
        void onClose(List<byte[]> bufferCollector, WebSocketReceiveResult receiveResult, uint myId);
        void onCloseError(uint myId);

    }
}
