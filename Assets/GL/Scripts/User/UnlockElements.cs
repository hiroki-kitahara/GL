﻿using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;

namespace GL.User
{
    /// <summary>
    /// アンロック出来る要素を制御するクラス
    /// </summary>
    [Serializable]
    public sealed class UnlockElements
    {
        [SerializeField]
        private List<string> enemyParties = new List<string>();
        public List<string> EnemyParties => this.enemyParties;

        /// <summary>
        /// 重複していない要素を返す
        /// </summary>
        public UnlockElements GetNotDuplicate(UnlockElements other)
        {
            var result = new UnlockElements();
            result.enemyParties = other.enemyParties.Where(x => this.enemyParties.FindIndex(e => e == x) < 0).ToList();

            return result;
        }
    }
}