using FUtility;
using HarmonyLib;
using System;
using UnityEngine;

namespace PrintingPodRecharge.Integration.TwitchIntegration
{
    public class ToastHelper
    {
        public delegate GameObject InstantiateToastWithGoTargetDelegate(string title, string body, GameObject target);
        public delegate GameObject InstantiateToastDelegate(string title, string body);

        public static InstantiateToastWithGoTargetDelegate InstantiateToastWithGoTarget;
        public static InstantiateToastDelegate InstantiateToast;

        public static void Init()
        {
            var t_ToastUiManager = Type.GetType("ONITwitchCore.ToastUiManager, ONITwitchCore");

            if(t_ToastUiManager != null)
            {
                Log.Info(" Initializing integration with ONI Twitch toasts");

                var m_InstantiateToastWithGoTarget = AccessTools.Method(t_ToastUiManager, "InstantiateToastWithGoTarget");
                var m_InstantiateToast = AccessTools.Method(t_ToastUiManager, "InstantiateToast");

                InstantiateToastWithGoTarget = AccessTools.MethodDelegate<InstantiateToastWithGoTargetDelegate>(m_InstantiateToastWithGoTarget);
                InstantiateToast = AccessTools.MethodDelegate<InstantiateToastDelegate>(m_InstantiateToast);
            }
        }

        public static void ToastToTarget(string msg, string body, GameObject target)
        {
            InstantiateToastWithGoTarget?.Invoke(msg, body, target);
        }

        public static void Toast(string msg, string body)
        {
            InstantiateToast?.Invoke(msg, body);
        }
    }
}
