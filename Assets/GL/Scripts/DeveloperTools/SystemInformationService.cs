﻿using GL.User;
using SRDebugger;
using SRDebugger.Services;
using SRF.Service;
using UnityEngine;
using UnityEngine.Assertions;

namespace GL.DeveloperTools
{
    /// <summary>
    /// SRDebuggerのSystemにGLのデータを表示するクラス
    /// </summary>
    public sealed class SystemInformationService
    {
        public void Setup()
        {
            var systemInformation = SRServiceManager.GetService<ISystemInformationService>();
            systemInformation.Add(InfoEntry.Create("UserName", () => UserData.Instance.UserName), "GL.UserData");
            systemInformation.Add(InfoEntry.Create("Wallet.Gold", () => UserData.Instance.Wallet.Gold.Value), "GL.UserData");
            systemInformation.Add(InfoEntry.Create("Wallet.Experience", () => UserData.Instance.Wallet.Experience.Value), "GL.UserData");
        }
    }
}
