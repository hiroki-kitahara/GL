﻿using System;
using UnityEngine;

namespace GL.Battle.Commands.Element.Blueprints
{
    /// <summary>
    /// 攻撃コマンドの設定データ.
    /// </summary>
    [CreateAssetMenu(menuName = "GL/Commands/Blueprints/Attack")]
    public sealed class Attack : Blueprint
    {
        [SerializeField]
        private Implements.Attack.Parameter parameter = new Implements.Attack.Parameter();
        
        public override IImplement Create()
        {
            return new Implements.Attack(this.parameter);
        }

#if UNITY_EDITOR
        public override string FileName => $"Attack_{this.parameter.Rate}_{this.parameter.AttributeType}_{this.parameter.LogicType}_{this.parameter.Accuracy}_{this.parameter.AddCritical}";

        public override Blueprint SetupFromEditor(string data)
        {
            var s = data.Split('_');
            this.parameter.Rate = float.Parse(s[1]);
            this.parameter.AttributeType = (Constants.AttributeType)Enum.Parse(typeof(Constants.AttributeType), s[2]);
            this.parameter.LogicType = (Constants.AttackLogicType)Enum.Parse(typeof(Constants.AttackLogicType), s[3]);
            this.parameter.Accuracy = float.Parse(s[4]);
            this.parameter.AddCritical = float.Parse(s[5]);

            return this;
        }
#endif
    }
}
