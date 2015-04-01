﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

namespace MiniWeChat
{
    public class ChatListPanel : BasePanel
    {
        private const int CHAT_FRAME_HEIGHT = 200;

        public VerticalLayoutGroup _chatGrid;

        private List<ChatFrame> _chatFrameList = new List<ChatFrame>();

        public void Start()
        {
            //int chatNum = 10;
            //_chatGrid.GetComponent<RectTransform>().sizeDelta = new Vector2(GlobalVars.DEFAULT_SCREEN_WIDTH, CHAT_FRAME_HEIGHT * chatNum);
            //for (int i = 0; i < chatNum; i++)
            //{
            //    GameObject go = UIManager.GetInstance().AddChild(_chatGrid.gameObject, EUIType.ChatFrame);
            //}
        }

        public override void Show(object param = null)
        {
            base.Show(param);
            MessageDispatcher.GetInstance().RegisterMessageHandler((uint)EUIMessage.UPDATE_RECEIVE_CHAT, OnUpdateReceiveChat);
            RefreshChatFrames();
        }

        public override void Hide()
        {
            base.Hide();
            MessageDispatcher.GetInstance().UnRegisterMessageHandler((uint)EUIMessage.UPDATE_RECEIVE_CHAT, OnUpdateReceiveChat);
        }

        public void OnUpdateReceiveChat(uint iMessageType, object kParam)
        {
            RefreshChatFrames();
        }

        public void RefreshChatFrames()
        {
            _chatGrid.GetComponent<RectTransform>().sizeDelta = new Vector2(GlobalVars.DEFAULT_SCREEN_WIDTH, CHAT_FRAME_HEIGHT * GlobalChat.GetInstance().Count);

            int i = 0;
            foreach (ChatLog chatLog in GlobalChat.GetInstance())
            {
                if (i >= GlobalChat.GetInstance().Count)
                {
                    GameObject go = UIManager.GetInstance().AddChild(_chatGrid.gameObject, EUIType.ChatFrame);
                    _chatFrameList.Add(go.GetComponent<ChatFrame>());
                }
                _chatFrameList[i].Show(chatLog);
                i++;

                if (_chatFrameList.Count > GlobalChat.GetInstance().Count)
                {
                    for (i = GlobalChat.GetInstance().Count; i < _chatFrameList.Count; i++)
                    {
                        GameObject.Destroy(_chatFrameList[i].gameObject);
                    }
                    _chatFrameList = _chatFrameList.GetRange(0, GlobalChat.GetInstance().Count);
                }
            }
        }
    }
}
