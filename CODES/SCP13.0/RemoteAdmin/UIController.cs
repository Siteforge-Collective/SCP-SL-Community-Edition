using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using RemoteAdmin.Communication;
using TMPro;
using ToggleableMenus;
using UnityEngine;
using UnityEngine.UI;

namespace RemoteAdmin
{
	public class UIController : SimpleToggleableMenu
	{
		[CompilerGenerated]
		private sealed class _003CInvalidPasswordAnimation_003Ed__44 : IEnumerator<float>, IEnumerator, IDisposable
		{
			private int _003C_003E1__state;

			private float _003C_003E2__current;

			public UIController _003C_003E4__this;

			private RawImage _003Craw_003E5__2;

			private Texture _003Ct_003E5__3;

			float IEnumerator<float>.Current
			{
				[DebuggerHidden]
				get
				{
					return 0f;
				}
			}

			object IEnumerator.Current
			{
				[DebuggerHidden]
				get
				{
					return null;
				}
			}

			[DebuggerHidden]
			public _003CInvalidPasswordAnimation_003Ed__44(int _003C_003E1__state)
			{
			}

			[DebuggerHidden]
			void IDisposable.Dispose()
			{
			}

			private bool MoveNext()
			{
				return false;
			}

			bool IEnumerator.MoveNext()
			{
				//ILSpy generated this explicit interface implementation from .override directive in MoveNext
				return this.MoveNext();
			}

			[DebuggerHidden]
			void IEnumerator.Reset()
			{
			}
		}

		public static UIController Singleton;

		public GameObject RootLogin;

		public GameObject RootPanel;

		public GameObject RootTbra;

		public Texture WrongPasswordTexture;

		public Button ConfirmButton;

		public InputField PasswordField;

		public Canvas RACanvas;

		public Text PasswordPlaceholder;

		internal bool LoggedIn;

		private bool _invalidPasswordAnimationInProgress;

		private bool _textBasedVersion;

		public GameObject ConsoleToolTip;

		public RectTransform ConsoleToolTipBackground;

		public TMP_Text ConsoleToolTipText;

		private TMP_InputField[] _allTmpFields;

		private InputField[] _allRegularFields;

		private float _clientToolTipWaitTime;

		private float _clientLastTipTime;

		private float _toolTipDelayTimer;

		private readonly Vector3 _toolTipOffset;

		private const float ToolTipPadding = 4f;

		private const string PasswordPlaceholderText = "PASSWORD...";

		private const string PasswordAuthDisabledPlaceholderText = "Password-based auth disabled";

		[field: SerializeField]
		public TMP_Dropdown PlayerSortingDropdown { get; set; }

		public override bool CanToggle => false;

		public override bool LockMovement => false;

		protected override void Awake()
		{
		}

		private void Update()
		{
		}

		public static RaPlayerList.PlayerSorting GetSorting()
		{
			return default(RaPlayerList.PlayerSorting);
		}

		public void SetSorting(int value)
		{
		}

		private bool IsAnyInputFieldFocused()
		{
			return false;
		}

		public void ChangeConsoleStage()
		{
		}

		public void CallSendPassword()
		{
		}

		public void ChangeTextMode(bool b)
		{
		}

		private void RefreshStatus()
		{
		}

		internal void SetRemoteAdminState(bool state)
		{
		}

		protected override void OnToggled()
		{
		}

		internal void AnimateInvalidPassword()
		{
		}

		[IteratorStateMachine(typeof(_003CInvalidPasswordAnimation_003Ed__44))]
		private IEnumerator<float> InvalidPasswordAnimation()
		{
			return null;
		}

		public void SetToolTip(string tip, float blockseconds = 0.1f, bool delayedDisplay = false)
		{
		}

		private void HideToolTip()
		{
		}
	}
}
