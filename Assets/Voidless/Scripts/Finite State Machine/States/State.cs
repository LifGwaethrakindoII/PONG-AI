﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Voidless
{
public abstract class State<T> : IState<T> where T : IFiniteStateAgent<T>
{
	private List<IStateTransition<T>> _transitions; 	/// <summary>State's Transitions.</summary>

	/// <summary>Gets and Sets transitions property.</summary>
	public List<IStateTransition<T>> transitions
	{
		get { return _transitions; }
		set { _transitions = value; }
	}

	/// <summary>Message invoked when entering state.</summary>
	/// <param name="_delta">Optional Delta Time information.</param>
	public abstract void OnEnterState(T _instance);

	/// <summary>Message invoked when executing state.</summary>
	/// <param name="_delta">Optional Delta Time information.</param>
	public abstract IEnumerator OnExecuteState(T _instance);

	/// <summary>Message invoked when leaving state.</summary>
	/// <param name="_delta">Optional Delta Time information.</param>
	public abstract void OnExitState(T _instance);
}
}