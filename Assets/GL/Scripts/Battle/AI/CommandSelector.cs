﻿using System;
using System.Collections.Generic;
using GL.Battle.CharacterControllers;
using GL.Database;
using GL.Events.Battle;
using GL.Extensions;
using HK.Framework.EventSystems;
using UnityEngine;
using UnityEngine.Assertions;

namespace GL.Battle.AIControllers
{
    /// <summary>
    /// コマンドを選択するクラス
    /// </summary>
    [CreateAssetMenu(menuName = "GL/AI/CommandSelector")]
    public sealed class CommandSelector : ScriptableObject
    {
        [SerializeField]
        private Condition[] conditions;

        [SerializeField]
        private InvokeCommand[] invokeCommands;

        public bool Suitable(Character invoker)
        {
            foreach (var c in this.conditions)
            {
                if (c.Suitable(invoker))
                {
                    return true;
                }
            }

            return false;
        }

        public void Invoke(Character invoker)
        {
            var invokeCommand = this.invokeCommands[UnityEngine.Random.Range(0, this.invokeCommands.Length)];
            invokeCommand.Invoke(invoker);
        }

#if UNITY_EDITOR
        public CommandSelector Set(Condition[] conditions, InvokeCommand[] invokeCommands)
        {
            this.conditions = conditions;
            this.invokeCommands = invokeCommands;

            return this;
        }
#endif
    }
}
