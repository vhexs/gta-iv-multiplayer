// Copyright 2014 Adrian Chlubek. This file is part of GTA Multiplayer IV project.
// Use of this source code is governed by a MIT license that can be
// found in the LICENSE file.
using System;
using System.Collections.Generic;
using System.IO;

namespace MIVClient
{
    public class ChatController
    {
        public Queue<string> chatconsole;
        public string currentTypedText;
        public Queue<string> debugconsole;
        private Client client;

        public ChatController(Client client)
        {
            this.client = client;
            currentTypedText = "";
            chatconsole = new Queue<string>();
            debugconsole = new Queue<string>();
            writeChat("Warning: This is alpha software.");
            writeChat("MIV 0.1 - Press L to connect.");
        }

        public void writeChat(string text)
        {
            chatconsole.Enqueue(text);
            try
            {
                File.AppendAllText("miv_chatHistory.txt", DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString() + ": " + text + "\r\n");
            }
            catch { }
            while (chatconsole.Count > 12)
            {
                chatconsole.Dequeue();
            }
        }

        public void writeDebug(string text)
        {
            debugconsole.Enqueue(text);
            while (debugconsole.Count > 40)
            {
                debugconsole.Dequeue();
            }
        }
    }
}