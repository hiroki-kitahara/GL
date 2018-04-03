﻿using UnityEngine;
using HK.Framework.EventSystems;
using UniRx;

namespace HK.GL.Test
{
	/// <summary>
	/// .
	/// </summary>
	public class Hoge : MonoBehaviour
	{
		void Awake()
		{
			UniRxEvent.GlobalBroker.Receive<GLTestEvent>()
                .Subscribe(g => Debug.Log("Hoge = " + g.Value));
        }
	}
}
