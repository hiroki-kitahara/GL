﻿using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace GL.UI.PopupControllers
{
    /// <summary>
    /// シンプルなポップアップのボタンを制御するクラス
    /// </summary>
    public sealed class SimplePopupButtonController : MonoBehaviour
    {
        [SerializeField]
        private Button button;
        public Button Button => this.button;

        [SerializeField]
        private Text text;
        public Text Text => this.text;
    }
}
