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
    public class PlexStatusPanelHandler : MonoBehaviour
    {
        public const DisplayTarget Target = DisplayTarget.PlexConnectionWindow;

        [SerializeField] GameObject connectButton;
        [SerializeField] TextMeshProUGUI linkCodeMessage;

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

            if(action is LinkCodeDisplayAction linkCodeAction)
            {
                connectButton.SetActive(false);
                linkCodeMessage.gameObject.SetActive(true);

                //Assign the formatted TMP message
                linkCodeMessage.text = linkCodeAction.Message;
            }



        }


    }
}
