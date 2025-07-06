using KayosTech.Components;
using KayosTech.ReelDeal.Prototype.LogSystem;
using KayosTech.ReelDeal.Prototype.SessionConfig.Action;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static KayosTech.Components.TMPLinkOpenerWithHover;

namespace KayosTech.ReelDeal.Prototype.SessionConfig
{
    public class PlexStatusPanelHandler : MonoBehaviour, IDisplayHandler
    {
        public DisplayTarget Target => DisplayTarget.PlexConnectionWindow;

        [SerializeField] GameObject connectButton;
        [SerializeField] TextMeshProUGUI linkCodeMessage;
        [SerializeField] TextMeshProUGUI AuthPollStatus;

        private void Awake()
        {
            connectButton.SetActive(true);
            linkCodeMessage.gameObject.SetActive(false);

            EventManager.OnDisplayActionDispatched += HandleIncomingDisplayAction;
        }

        private void HandleIncomingDisplayAction(IDisplayAction action)
        {
            if (action.Target != Target) return;

            DevLog.Highlight($"11 - DisplayAction received: {action}", "Data Flow");

            switch (action)
            {
                case Action.LinkCodeMessage lcMsg:
                    connectButton.SetActive(false);
                    linkCodeMessage.gameObject.SetActive(true);
                    linkCodeMessage.text = lcMsg.Message;

                    var pollIntent = new PollAuthIntent();
                    EventManager.DispatchServiceIntent(pollIntent);
                    break;
                case Action.AuthPollProcessing apStat:
                    if (!AuthPollStatus.IsActive())  AuthPollStatus.gameObject.SetActive(true);
                    AuthPollStatus.text = apStat.Message;
                    break;
                case Action.AuthPollSuccess apSucc:
                    linkCodeMessage.gameObject.SetActive(false);
                    if (!AuthPollStatus.IsActive()) AuthPollStatus.gameObject.SetActive(true);
                    AuthPollStatus.text = apSucc.Message;
                    break;
                default:
                    DevLog.Warning($"Unsupported Action Type: {action.GetType().Name}", "Display Routing");
                    break;


            }
            
        }


    }
}
