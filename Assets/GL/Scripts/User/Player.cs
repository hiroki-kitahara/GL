﻿using System;
using UnityEngine;

namespace GL.Scripts.User
{
    /// <summary>
    /// ユーザーデータのプレイヤークラス
    /// </summary>
    [Serializable]
    public sealed class Player
    {
        [SerializeField]
        public Guid Id;
        
        [SerializeField][Range(1.0f, 100.0f)]
        public int Level;
        
        [SerializeField]
        public Battle.CharacterControllers.Blueprints.Player Blueprint;

        private Player()
        {
        }

        public static Player Create(int level, Battle.CharacterControllers.Blueprints.Player blueprint)
        {
            return new Player(){Id = Guid.NewGuid(), Level = level, Blueprint = blueprint};
        }
    }
}