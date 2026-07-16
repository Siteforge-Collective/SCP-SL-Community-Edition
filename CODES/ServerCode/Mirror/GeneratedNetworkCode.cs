namespace Mirror
{
	[global::System.Runtime.InteropServices.StructLayout(global::System.Runtime.InteropServices.LayoutKind.Auto, CharSet = global::System.Runtime.InteropServices.CharSet.Auto)]
	public static class GeneratedNetworkCode
	{
		public static global::Mirror.ReadyMessage _Read_Mirror_002EReadyMessage(global::Mirror.NetworkReader reader)
		{
			return default(global::Mirror.ReadyMessage);
		}

		public static void _Write_Mirror_002EReadyMessage(global::Mirror.NetworkWriter writer, global::Mirror.ReadyMessage value)
		{
		}

		public static global::Mirror.NotReadyMessage _Read_Mirror_002ENotReadyMessage(global::Mirror.NetworkReader reader)
		{
			return default(global::Mirror.NotReadyMessage);
		}

		public static void _Write_Mirror_002ENotReadyMessage(global::Mirror.NetworkWriter writer, global::Mirror.NotReadyMessage value)
		{
		}

		public static global::Mirror.AddPlayerMessage _Read_Mirror_002EAddPlayerMessage(global::Mirror.NetworkReader reader)
		{
			return default(global::Mirror.AddPlayerMessage);
		}

		public static void _Write_Mirror_002EAddPlayerMessage(global::Mirror.NetworkWriter writer, global::Mirror.AddPlayerMessage value)
		{
		}

		public static global::Mirror.SceneMessage _Read_Mirror_002ESceneMessage(global::Mirror.NetworkReader reader)
		{
			return new global::Mirror.SceneMessage
			{
				sceneName = reader.ReadString(),
				sceneOperation = _Read_Mirror_002ESceneOperation(reader),
				customHandling = reader.ReadBoolean()
			};
		}

		public static global::Mirror.SceneOperation _Read_Mirror_002ESceneOperation(global::Mirror.NetworkReader reader)
		{
			return (global::Mirror.SceneOperation)global::Mirror.NetworkReaderExtensions.ReadByte(reader);
		}

		public static void _Write_Mirror_002ESceneMessage(global::Mirror.NetworkWriter writer, global::Mirror.SceneMessage value)
		{
			writer.WriteString(value.sceneName);
			_Write_Mirror_002ESceneOperation(writer, value.sceneOperation);
			writer.WriteBoolean(value.customHandling);
		}

		public static void _Write_Mirror_002ESceneOperation(global::Mirror.NetworkWriter writer, global::Mirror.SceneOperation value)
		{
			global::Mirror.NetworkWriterExtensions.WriteByte(writer, (byte)value);
		}

		public static global::Mirror.CommandMessage _Read_Mirror_002ECommandMessage(global::Mirror.NetworkReader reader)
		{
			return new global::Mirror.CommandMessage
			{
				netId = reader.ReadUInt32(),
				componentIndex = reader.ReadInt32(),
				functionHash = reader.ReadInt32(),
				payload = reader.ReadBytesAndSizeSegment()
			};
		}

		public static void _Write_Mirror_002ECommandMessage(global::Mirror.NetworkWriter writer, global::Mirror.CommandMessage value)
		{
			writer.WriteUInt32(value.netId);
			writer.WriteInt32(value.componentIndex);
			writer.WriteInt32(value.functionHash);
			writer.WriteBytesAndSizeSegment(value.payload);
		}

		public static global::Mirror.RpcMessage _Read_Mirror_002ERpcMessage(global::Mirror.NetworkReader reader)
		{
			return new global::Mirror.RpcMessage
			{
				netId = reader.ReadUInt32(),
				componentIndex = reader.ReadInt32(),
				functionHash = reader.ReadInt32(),
				payload = reader.ReadBytesAndSizeSegment()
			};
		}

		public static void _Write_Mirror_002ERpcMessage(global::Mirror.NetworkWriter writer, global::Mirror.RpcMessage value)
		{
			writer.WriteUInt32(value.netId);
			writer.WriteInt32(value.componentIndex);
			writer.WriteInt32(value.functionHash);
			writer.WriteBytesAndSizeSegment(value.payload);
		}

		public static global::Mirror.SpawnMessage _Read_Mirror_002ESpawnMessage(global::Mirror.NetworkReader reader)
		{
			return new global::Mirror.SpawnMessage
			{
				netId = reader.ReadUInt32(),
				isLocalPlayer = reader.ReadBoolean(),
				isOwner = reader.ReadBoolean(),
				sceneId = reader.ReadUInt64(),
				assetId = reader.ReadGuid(),
				position = reader.ReadVector3(),
				rotation = reader.ReadQuaternion(),
				scale = reader.ReadVector3(),
				payload = reader.ReadBytesAndSizeSegment()
			};
		}

		public static void _Write_Mirror_002ESpawnMessage(global::Mirror.NetworkWriter writer, global::Mirror.SpawnMessage value)
		{
			writer.WriteUInt32(value.netId);
			writer.WriteBoolean(value.isLocalPlayer);
			writer.WriteBoolean(value.isOwner);
			writer.WriteUInt64(value.sceneId);
			writer.WriteGuid(value.assetId);
			writer.WriteVector3(value.position);
			writer.WriteQuaternion(value.rotation);
			writer.WriteVector3(value.scale);
			writer.WriteBytesAndSizeSegment(value.payload);
		}

		public static global::Mirror.ObjectSpawnStartedMessage _Read_Mirror_002EObjectSpawnStartedMessage(global::Mirror.NetworkReader reader)
		{
			return default(global::Mirror.ObjectSpawnStartedMessage);
		}

		public static void _Write_Mirror_002EObjectSpawnStartedMessage(global::Mirror.NetworkWriter writer, global::Mirror.ObjectSpawnStartedMessage value)
		{
		}

		public static global::Mirror.ObjectSpawnFinishedMessage _Read_Mirror_002EObjectSpawnFinishedMessage(global::Mirror.NetworkReader reader)
		{
			return default(global::Mirror.ObjectSpawnFinishedMessage);
		}

		public static void _Write_Mirror_002EObjectSpawnFinishedMessage(global::Mirror.NetworkWriter writer, global::Mirror.ObjectSpawnFinishedMessage value)
		{
		}

		public static global::Mirror.ObjectDestroyMessage _Read_Mirror_002EObjectDestroyMessage(global::Mirror.NetworkReader reader)
		{
			return new global::Mirror.ObjectDestroyMessage
			{
				netId = reader.ReadUInt32()
			};
		}

		public static void _Write_Mirror_002EObjectDestroyMessage(global::Mirror.NetworkWriter writer, global::Mirror.ObjectDestroyMessage value)
		{
			writer.WriteUInt32(value.netId);
		}

		public static global::Mirror.ObjectHideMessage _Read_Mirror_002EObjectHideMessage(global::Mirror.NetworkReader reader)
		{
			return new global::Mirror.ObjectHideMessage
			{
				netId = reader.ReadUInt32()
			};
		}

		public static void _Write_Mirror_002EObjectHideMessage(global::Mirror.NetworkWriter writer, global::Mirror.ObjectHideMessage value)
		{
			writer.WriteUInt32(value.netId);
		}

		public static global::Mirror.UpdateVarsMessage _Read_Mirror_002EUpdateVarsMessage(global::Mirror.NetworkReader reader)
		{
			return new global::Mirror.UpdateVarsMessage
			{
				netId = reader.ReadUInt32(),
				payload = reader.ReadBytesAndSizeSegment()
			};
		}

		public static void _Write_Mirror_002EUpdateVarsMessage(global::Mirror.NetworkWriter writer, global::Mirror.UpdateVarsMessage value)
		{
			writer.WriteUInt32(value.netId);
			writer.WriteBytesAndSizeSegment(value.payload);
		}

		public static global::Mirror.NetworkPingMessage _Read_Mirror_002ENetworkPingMessage(global::Mirror.NetworkReader reader)
		{
			return new global::Mirror.NetworkPingMessage
			{
				clientTime = reader.ReadDouble()
			};
		}

		public static void _Write_Mirror_002ENetworkPingMessage(global::Mirror.NetworkWriter writer, global::Mirror.NetworkPingMessage value)
		{
			writer.WriteDouble(value.clientTime);
		}

		public static global::Mirror.NetworkPongMessage _Read_Mirror_002ENetworkPongMessage(global::Mirror.NetworkReader reader)
		{
			return new global::Mirror.NetworkPongMessage
			{
				clientTime = reader.ReadDouble(),
				serverTime = reader.ReadDouble()
			};
		}

		public static void _Write_Mirror_002ENetworkPongMessage(global::Mirror.NetworkWriter writer, global::Mirror.NetworkPongMessage value)
		{
			writer.WriteDouble(value.clientTime);
			writer.WriteDouble(value.serverTime);
		}

		public static Hitmarker.HitmarkerMessage _Read_Hitmarker_002FHitmarkerMessage(global::Mirror.NetworkReader reader)
		{
			return new Hitmarker.HitmarkerMessage
			{
				Size = global::Mirror.NetworkReaderExtensions.ReadByte(reader)
			};
		}

		public static void _Write_Hitmarker_002FHitmarkerMessage(global::Mirror.NetworkWriter writer, Hitmarker.HitmarkerMessage value)
		{
			global::Mirror.NetworkWriterExtensions.WriteByte(writer, value.Size);
		}

		public static Escape.EscapeMessage _Read_Escape_002FEscapeMessage(global::Mirror.NetworkReader reader)
		{
			return new Escape.EscapeMessage
			{
				ScenarioId = global::Mirror.NetworkReaderExtensions.ReadByte(reader),
				EscapeTime = reader.ReadUInt16()
			};
		}

		public static void _Write_Escape_002FEscapeMessage(global::Mirror.NetworkWriter writer, Escape.EscapeMessage value)
		{
			global::Mirror.NetworkWriterExtensions.WriteByte(writer, value.ScenarioId);
			writer.WriteUInt16(value.EscapeTime);
		}

		public static ServerShutdown.ServerShutdownMessage _Read_ServerShutdown_002FServerShutdownMessage(global::Mirror.NetworkReader reader)
		{
			return default(ServerShutdown.ServerShutdownMessage);
		}

		public static void _Write_ServerShutdown_002FServerShutdownMessage(global::Mirror.NetworkWriter writer, ServerShutdown.ServerShutdownMessage value)
		{
		}

		public static global::VoiceChat.VoiceChatMuteIndicator.SyncMuteMessage _Read_VoiceChat_002EVoiceChatMuteIndicator_002FSyncMuteMessage(global::Mirror.NetworkReader reader)
		{
			return new global::VoiceChat.VoiceChatMuteIndicator.SyncMuteMessage
			{
				Flags = global::Mirror.NetworkReaderExtensions.ReadByte(reader)
			};
		}

		public static void _Write_VoiceChat_002EVoiceChatMuteIndicator_002FSyncMuteMessage(global::Mirror.NetworkWriter writer, global::VoiceChat.VoiceChatMuteIndicator.SyncMuteMessage value)
		{
			global::Mirror.NetworkWriterExtensions.WriteByte(writer, value.Flags);
		}

		public static global::VoiceChat.VoiceChatPrivacySettings.VcPrivacyMessage _Read_VoiceChat_002EVoiceChatPrivacySettings_002FVcPrivacyMessage(global::Mirror.NetworkReader reader)
		{
			return new global::VoiceChat.VoiceChatPrivacySettings.VcPrivacyMessage
			{
				Flags = global::Mirror.NetworkReaderExtensions.ReadByte(reader)
			};
		}

		public static void _Write_VoiceChat_002EVoiceChatPrivacySettings_002FVcPrivacyMessage(global::Mirror.NetworkWriter writer, global::VoiceChat.VoiceChatPrivacySettings.VcPrivacyMessage value)
		{
			global::Mirror.NetworkWriterExtensions.WriteByte(writer, value.Flags);
		}

		public static global::VoiceChat.Playbacks.PersonalRadioPlayback.TransmitterPositionMessage _Read_VoiceChat_002EPlaybacks_002EPersonalRadioPlayback_002FTransmitterPositionMessage(global::Mirror.NetworkReader reader)
		{
			return new global::VoiceChat.Playbacks.PersonalRadioPlayback.TransmitterPositionMessage
			{
				Transmitter = reader.ReadRecyclablePlayerId(),
				WaypointId = global::Mirror.NetworkReaderExtensions.ReadByte(reader)
			};
		}

		public static void _Write_VoiceChat_002EPlaybacks_002EPersonalRadioPlayback_002FTransmitterPositionMessage(global::Mirror.NetworkWriter writer, global::VoiceChat.Playbacks.PersonalRadioPlayback.TransmitterPositionMessage value)
		{
			writer.WriteRecyclablePlayerId(value.Transmitter);
			global::Mirror.NetworkWriterExtensions.WriteByte(writer, value.WaypointId);
		}

		public static global::PlayerRoles.Voice.VoiceChatReceivePrefs.GroupMuteFlagsMessage _Read_PlayerRoles_002EVoice_002EVoiceChatReceivePrefs_002FGroupMuteFlagsMessage(global::Mirror.NetworkReader reader)
		{
			return new global::PlayerRoles.Voice.VoiceChatReceivePrefs.GroupMuteFlagsMessage
			{
				Flags = global::Mirror.NetworkReaderExtensions.ReadByte(reader)
			};
		}

		public static void _Write_PlayerRoles_002EVoice_002EVoiceChatReceivePrefs_002FGroupMuteFlagsMessage(global::Mirror.NetworkWriter writer, global::PlayerRoles.Voice.VoiceChatReceivePrefs.GroupMuteFlagsMessage value)
		{
			global::Mirror.NetworkWriterExtensions.WriteByte(writer, value.Flags);
		}

		public static global::PlayerRoles.Spectating.SpectatorNetworking.SpectatedNetIdSyncMessage _Read_PlayerRoles_002ESpectating_002ESpectatorNetworking_002FSpectatedNetIdSyncMessage(global::Mirror.NetworkReader reader)
		{
			return new global::PlayerRoles.Spectating.SpectatorNetworking.SpectatedNetIdSyncMessage
			{
				NetId = reader.ReadUInt32()
			};
		}

		public static void _Write_PlayerRoles_002ESpectating_002ESpectatorNetworking_002FSpectatedNetIdSyncMessage(global::Mirror.NetworkWriter writer, global::PlayerRoles.Spectating.SpectatorNetworking.SpectatedNetIdSyncMessage value)
		{
			writer.WriteUInt32(value.NetId);
		}

		public static global::PlayerRoles.PlayableScps.Scp106.Scp106PocketItemManager.WarningMessage _Read_PlayerRoles_002EPlayableScps_002EScp106_002EScp106PocketItemManager_002FWarningMessage(global::Mirror.NetworkReader reader)
		{
			return new global::PlayerRoles.PlayableScps.Scp106.Scp106PocketItemManager.WarningMessage
			{
				Position = global::RelativePositioning.RelativePositionSerialization.ReadRelativePosition(reader)
			};
		}

		public static void _Write_PlayerRoles_002EPlayableScps_002EScp106_002EScp106PocketItemManager_002FWarningMessage(global::Mirror.NetworkWriter writer, global::PlayerRoles.PlayableScps.Scp106.Scp106PocketItemManager.WarningMessage value)
		{
			global::RelativePositioning.RelativePositionSerialization.WriteRelativePosition(writer, value.Position);
		}

		public static global::PlayerRoles.PlayableScps.Scp049.Zombies.ZombieConfirmationBox.ScpReviveBlockMessage _Read_PlayerRoles_002EPlayableScps_002EScp049_002EZombies_002EZombieConfirmationBox_002FScpReviveBlockMessage(global::Mirror.NetworkReader reader)
		{
			return default(global::PlayerRoles.PlayableScps.Scp049.Zombies.ZombieConfirmationBox.ScpReviveBlockMessage);
		}

		public static void _Write_PlayerRoles_002EPlayableScps_002EScp049_002EZombies_002EZombieConfirmationBox_002FScpReviveBlockMessage(global::Mirror.NetworkWriter writer, global::PlayerRoles.PlayableScps.Scp049.Zombies.ZombieConfirmationBox.ScpReviveBlockMessage value)
		{
		}

		public static global::PlayerRoles.PlayableScps.HumeShield.DynamicHumeShieldController.ShieldBreakMessage _Read_PlayerRoles_002EPlayableScps_002EHumeShield_002EDynamicHumeShieldController_002FShieldBreakMessage(global::Mirror.NetworkReader reader)
		{
			return new global::PlayerRoles.PlayableScps.HumeShield.DynamicHumeShieldController.ShieldBreakMessage
			{
				Target = global::Utils.Networking.ReferenceHubReaderWriter.ReadReferenceHub(reader)
			};
		}

		public static void _Write_PlayerRoles_002EPlayableScps_002EHumeShield_002EDynamicHumeShieldController_002FShieldBreakMessage(global::Mirror.NetworkWriter writer, global::PlayerRoles.PlayableScps.HumeShield.DynamicHumeShieldController.ShieldBreakMessage value)
		{
			global::Utils.Networking.ReferenceHubReaderWriter.WriteReferenceHub(writer, value.Target);
		}

		public static global::PlayerRoles.FirstPersonControl.NetworkMessages.FpcNoclipToggleMessage _Read_PlayerRoles_002EFirstPersonControl_002ENetworkMessages_002EFpcNoclipToggleMessage(global::Mirror.NetworkReader reader)
		{
			return default(global::PlayerRoles.FirstPersonControl.NetworkMessages.FpcNoclipToggleMessage);
		}

		public static void _Write_PlayerRoles_002EFirstPersonControl_002ENetworkMessages_002EFpcNoclipToggleMessage(global::Mirror.NetworkWriter writer, global::PlayerRoles.FirstPersonControl.NetworkMessages.FpcNoclipToggleMessage value)
		{
		}

		public static global::InventorySystem.Items.Usables.Scp330.CandyPink.CandyExplosionMessage _Read_InventorySystem_002EItems_002EUsables_002EScp330_002ECandyPink_002FCandyExplosionMessage(global::Mirror.NetworkReader reader)
		{
			return new global::InventorySystem.Items.Usables.Scp330.CandyPink.CandyExplosionMessage
			{
				Origin = reader.ReadVector3()
			};
		}

		public static void _Write_InventorySystem_002EItems_002EUsables_002EScp330_002ECandyPink_002FCandyExplosionMessage(global::Mirror.NetworkWriter writer, global::InventorySystem.Items.Usables.Scp330.CandyPink.CandyExplosionMessage value)
		{
			writer.WriteVector3(value.Origin);
		}

		public static global::InventorySystem.Items.Usables.Scp244.Hypothermia.HumeShieldSubEffect.HumeBlockMsg _Read_InventorySystem_002EItems_002EUsables_002EScp244_002EHypothermia_002EHumeShieldSubEffect_002FHumeBlockMsg(global::Mirror.NetworkReader reader)
		{
			return default(global::InventorySystem.Items.Usables.Scp244.Hypothermia.HumeShieldSubEffect.HumeBlockMsg);
		}

		public static void _Write_InventorySystem_002EItems_002EUsables_002EScp244_002EHypothermia_002EHumeShieldSubEffect_002FHumeBlockMsg(global::Mirror.NetworkWriter writer, global::InventorySystem.Items.Usables.Scp244.Hypothermia.HumeShieldSubEffect.HumeBlockMsg value)
		{
		}

		public static global::InventorySystem.Items.Usables.Scp1576.Scp1576SpectatorWarningHandler.SpectatorWarningMessage _Read_InventorySystem_002EItems_002EUsables_002EScp1576_002EScp1576SpectatorWarningHandler_002FSpectatorWarningMessage(global::Mirror.NetworkReader reader)
		{
			return new global::InventorySystem.Items.Usables.Scp1576.Scp1576SpectatorWarningHandler.SpectatorWarningMessage
			{
				IsStop = reader.ReadBoolean()
			};
		}

		public static void _Write_InventorySystem_002EItems_002EUsables_002EScp1576_002EScp1576SpectatorWarningHandler_002FSpectatorWarningMessage(global::Mirror.NetworkWriter writer, global::InventorySystem.Items.Usables.Scp1576.Scp1576SpectatorWarningHandler.SpectatorWarningMessage value)
		{
			writer.WriteBoolean(value.IsStop);
		}

		public static global::InventorySystem.Items.Keycards.KeycardItem.UseMessage _Read_InventorySystem_002EItems_002EKeycards_002EKeycardItem_002FUseMessage(global::Mirror.NetworkReader reader)
		{
			return default(global::InventorySystem.Items.Keycards.KeycardItem.UseMessage);
		}

		public static void _Write_InventorySystem_002EItems_002EKeycards_002EKeycardItem_002FUseMessage(global::Mirror.NetworkWriter writer, global::InventorySystem.Items.Keycards.KeycardItem.UseMessage value)
		{
		}

		public static global::InventorySystem.Items.Firearms.Modules.AutomaticAction.RefusedShotMessage _Read_InventorySystem_002EItems_002EFirearms_002EModules_002EAutomaticAction_002FRefusedShotMessage(global::Mirror.NetworkReader reader)
		{
			return default(global::InventorySystem.Items.Firearms.Modules.AutomaticAction.RefusedShotMessage);
		}

		public static void _Write_InventorySystem_002EItems_002EFirearms_002EModules_002EAutomaticAction_002FRefusedShotMessage(global::Mirror.NetworkWriter writer, global::InventorySystem.Items.Firearms.Modules.AutomaticAction.RefusedShotMessage value)
		{
		}

		public static global::InventorySystem.Items.Firearms.Modules.DisruptorHitreg.DisruptorHitMessage _Read_InventorySystem_002EItems_002EFirearms_002EModules_002EDisruptorHitreg_002FDisruptorHitMessage(global::Mirror.NetworkReader reader)
		{
			return new global::InventorySystem.Items.Firearms.Modules.DisruptorHitreg.DisruptorHitMessage
			{
				Position = reader.ReadVector3(),
				Rotation = reader.ReadLowPrecisionQuaternion()
			};
		}

		public static void _Write_InventorySystem_002EItems_002EFirearms_002EModules_002EDisruptorHitreg_002FDisruptorHitMessage(global::Mirror.NetworkWriter writer, global::InventorySystem.Items.Firearms.Modules.DisruptorHitreg.DisruptorHitMessage value)
		{
			writer.WriteVector3(value.Position);
			writer.WriteLowPrecisionQuaternion(value.Rotation);
		}

		public static global::InventorySystem.Items.Firearms.Modules.CockMessage _Read_InventorySystem_002EItems_002EFirearms_002EModules_002ECockMessage(global::Mirror.NetworkReader reader)
		{
			return default(global::InventorySystem.Items.Firearms.Modules.CockMessage);
		}

		public static void _Write_InventorySystem_002EItems_002EFirearms_002EModules_002ECockMessage(global::Mirror.NetworkWriter writer, global::InventorySystem.Items.Firearms.Modules.CockMessage value)
		{
		}

		public static global::Achievements.AchievementManager.AchievementMessage _Read_Achievements_002EAchievementManager_002FAchievementMessage(global::Mirror.NetworkReader reader)
		{
			return new global::Achievements.AchievementManager.AchievementMessage
			{
				AchievementId = global::Mirror.NetworkReaderExtensions.ReadByte(reader)
			};
		}

		public static void _Write_Achievements_002EAchievementManager_002FAchievementMessage(global::Mirror.NetworkWriter writer, global::Achievements.AchievementManager.AchievementMessage value)
		{
			global::Mirror.NetworkWriterExtensions.WriteByte(writer, value.AchievementId);
		}

		public static global::Interactables.Interobjects.ElevatorManager.ElevatorSyncMsg _Read_Interactables_002EInterobjects_002EElevatorManager_002FElevatorSyncMsg(global::Mirror.NetworkReader reader)
		{
			return new global::Interactables.Interobjects.ElevatorManager.ElevatorSyncMsg
			{
				Data = global::Mirror.NetworkReaderExtensions.ReadByte(reader)
			};
		}

		public static void _Write_Interactables_002EInterobjects_002EElevatorManager_002FElevatorSyncMsg(global::Mirror.NetworkWriter writer, global::Interactables.Interobjects.ElevatorManager.ElevatorSyncMsg value)
		{
			global::Mirror.NetworkWriterExtensions.WriteByte(writer, value.Data);
		}

		public static ServerConfigSynchronizer.PredefinedBanTemplate _Read_ServerConfigSynchronizer_002FPredefinedBanTemplate(global::Mirror.NetworkReader reader)
		{
			return new ServerConfigSynchronizer.PredefinedBanTemplate
			{
				Duration = reader.ReadInt32(),
				FormattedDuration = reader.ReadString(),
				Reason = reader.ReadString()
			};
		}

		public static void _Write_ServerConfigSynchronizer_002FPredefinedBanTemplate(global::Mirror.NetworkWriter writer, ServerConfigSynchronizer.PredefinedBanTemplate value)
		{
			writer.WriteInt32(value.Duration);
			writer.WriteString(value.FormattedDuration);
			writer.WriteString(value.Reason);
		}

		public static void _Write_Broadcast_002FBroadcastFlags(global::Mirror.NetworkWriter writer, Broadcast.BroadcastFlags value)
		{
			global::Mirror.NetworkWriterExtensions.WriteByte(writer, (byte)value);
		}

		public static Broadcast.BroadcastFlags _Read_Broadcast_002FBroadcastFlags(global::Mirror.NetworkReader reader)
		{
			return (Broadcast.BroadcastFlags)global::Mirror.NetworkReaderExtensions.ReadByte(reader);
		}

		public static void _Write_UnityEngine_002EKeyCode(global::Mirror.NetworkWriter writer, global::UnityEngine.KeyCode value)
		{
			writer.WriteInt32((int)value);
		}

		public static global::UnityEngine.KeyCode _Read_UnityEngine_002EKeyCode(global::Mirror.NetworkReader reader)
		{
			return (global::UnityEngine.KeyCode)reader.ReadInt32();
		}

		public static void _Write_PlayerInfoArea(global::Mirror.NetworkWriter writer, PlayerInfoArea value)
		{
			writer.WriteInt32((int)value);
		}

		public static PlayerInfoArea _Read_PlayerInfoArea(global::Mirror.NetworkReader reader)
		{
			return (PlayerInfoArea)reader.ReadInt32();
		}

		public static void _Write_PlayerInteract_002FAlphaPanelOperations(global::Mirror.NetworkWriter writer, PlayerInteract.AlphaPanelOperations value)
		{
			global::Mirror.NetworkWriterExtensions.WriteByte(writer, (byte)value);
		}

		public static PlayerInteract.AlphaPanelOperations _Read_PlayerInteract_002FAlphaPanelOperations(global::Mirror.NetworkReader reader)
		{
			return (PlayerInteract.AlphaPanelOperations)global::Mirror.NetworkReaderExtensions.ReadByte(reader);
		}

		public static void _Write_RoundSummary_002FSumInfo_ClassList(global::Mirror.NetworkWriter writer, RoundSummary.SumInfo_ClassList value)
		{
			writer.WriteInt32(value.class_ds);
			writer.WriteInt32(value.scientists);
			writer.WriteInt32(value.chaos_insurgents);
			writer.WriteInt32(value.mtf_and_guards);
			writer.WriteInt32(value.scps_except_zombies);
			writer.WriteInt32(value.zombies);
			writer.WriteInt32(value.warhead_kills);
		}

		public static void _Write_RoundSummary_002FLeadingTeam(global::Mirror.NetworkWriter writer, RoundSummary.LeadingTeam value)
		{
			global::Mirror.NetworkWriterExtensions.WriteByte(writer, (byte)value);
		}

		public static RoundSummary.SumInfo_ClassList _Read_RoundSummary_002FSumInfo_ClassList(global::Mirror.NetworkReader reader)
		{
			return new RoundSummary.SumInfo_ClassList
			{
				class_ds = reader.ReadInt32(),
				scientists = reader.ReadInt32(),
				chaos_insurgents = reader.ReadInt32(),
				mtf_and_guards = reader.ReadInt32(),
				scps_except_zombies = reader.ReadInt32(),
				zombies = reader.ReadInt32(),
				warhead_kills = reader.ReadInt32()
			};
		}

		public static RoundSummary.LeadingTeam _Read_RoundSummary_002FLeadingTeam(global::Mirror.NetworkReader reader)
		{
			return (RoundSummary.LeadingTeam)global::Mirror.NetworkReaderExtensions.ReadByte(reader);
		}

		public static void _Write_RemoteAdmin_002EQueryProcessor_002FCommandData_005B_005D(global::Mirror.NetworkWriter writer, global::RemoteAdmin.QueryProcessor.CommandData[] value)
		{
			writer.WriteArray(value);
		}

		public static void _Write_RemoteAdmin_002EQueryProcessor_002FCommandData(global::Mirror.NetworkWriter writer, global::RemoteAdmin.QueryProcessor.CommandData value)
		{
			writer.WriteString(value.Command);
			_Write_System_002EString_005B_005D(writer, value.Usage);
			writer.WriteString(value.Description);
			writer.WriteString(value.AliasOf);
			writer.WriteBoolean(value.Hidden);
		}

		public static void _Write_System_002EString_005B_005D(global::Mirror.NetworkWriter writer, string[] value)
		{
			writer.WriteArray(value);
		}

		public static global::RemoteAdmin.QueryProcessor.CommandData[] _Read_RemoteAdmin_002EQueryProcessor_002FCommandData_005B_005D(global::Mirror.NetworkReader reader)
		{
			return reader.ReadArray<global::RemoteAdmin.QueryProcessor.CommandData>();
		}

		public static global::RemoteAdmin.QueryProcessor.CommandData _Read_RemoteAdmin_002EQueryProcessor_002FCommandData(global::Mirror.NetworkReader reader)
		{
			return new global::RemoteAdmin.QueryProcessor.CommandData
			{
				Command = reader.ReadString(),
				Usage = _Read_System_002EString_005B_005D(reader),
				Description = reader.ReadString(),
				AliasOf = reader.ReadString(),
				Hidden = reader.ReadBoolean()
			};
		}

		public static string[] _Read_System_002EString_005B_005D(global::Mirror.NetworkReader reader)
		{
			return reader.ReadArray<string>();
		}

		public static void _Write_LightContainmentZoneDecontamination_002EDecontaminationController_002FDecontaminationStatus(global::Mirror.NetworkWriter writer, global::LightContainmentZoneDecontamination.DecontaminationController.DecontaminationStatus value)
		{
			global::Mirror.NetworkWriterExtensions.WriteByte(writer, (byte)value);
		}

		public static global::LightContainmentZoneDecontamination.DecontaminationController.DecontaminationStatus _Read_LightContainmentZoneDecontamination_002EDecontaminationController_002FDecontaminationStatus(global::Mirror.NetworkReader reader)
		{
			return (global::LightContainmentZoneDecontamination.DecontaminationController.DecontaminationStatus)global::Mirror.NetworkReaderExtensions.ReadByte(reader);
		}

		public static void _Write_InventorySystem_002EItems_002EItemIdentifier_005B_005D(global::Mirror.NetworkWriter writer, global::InventorySystem.Items.ItemIdentifier[] value)
		{
			writer.WriteArray(value);
		}

		public static void _Write_InventorySystem_002EItems_002EItemIdentifier(global::Mirror.NetworkWriter writer, global::InventorySystem.Items.ItemIdentifier value)
		{
			_Write_ItemType(writer, value.TypeId);
			writer.WriteUInt16(value.SerialNumber);
		}

		public static void _Write_ItemType(global::Mirror.NetworkWriter writer, ItemType value)
		{
			writer.WriteInt32((int)value);
		}

		public static global::InventorySystem.Items.ItemIdentifier[] _Read_InventorySystem_002EItems_002EItemIdentifier_005B_005D(global::Mirror.NetworkReader reader)
		{
			return reader.ReadArray<global::InventorySystem.Items.ItemIdentifier>();
		}

		public static global::InventorySystem.Items.ItemIdentifier _Read_InventorySystem_002EItems_002EItemIdentifier(global::Mirror.NetworkReader reader)
		{
			return new global::InventorySystem.Items.ItemIdentifier
			{
				TypeId = _Read_ItemType(reader),
				SerialNumber = reader.ReadUInt16()
			};
		}

		public static ItemType _Read_ItemType(global::Mirror.NetworkReader reader)
		{
			return (ItemType)reader.ReadInt32();
		}

		public static void _Write_System_002EUInt16_005B_005D(global::Mirror.NetworkWriter writer, ushort[] value)
		{
			writer.WriteArray(value);
		}

		public static ushort[] _Read_System_002EUInt16_005B_005D(global::Mirror.NetworkReader reader)
		{
			return reader.ReadArray<ushort>();
		}

		public static void _Write_ActionName(global::Mirror.NetworkWriter writer, ActionName value)
		{
			writer.WriteInt32((int)value);
		}

		public static ActionName _Read_ActionName(global::Mirror.NetworkReader reader)
		{
			return (ActionName)reader.ReadInt32();
		}

		public static void _Write_InventorySystem_002EItems_002EUsables_002EScp330_002ECandyKindID(global::Mirror.NetworkWriter writer, global::InventorySystem.Items.Usables.Scp330.CandyKindID value)
		{
			global::Mirror.NetworkWriterExtensions.WriteByte(writer, (byte)value);
		}

		public static global::InventorySystem.Items.Usables.Scp330.CandyKindID _Read_InventorySystem_002EItems_002EUsables_002EScp330_002ECandyKindID(global::Mirror.NetworkReader reader)
		{
			return (global::InventorySystem.Items.Usables.Scp330.CandyKindID)global::Mirror.NetworkReaderExtensions.ReadByte(reader);
		}

		public static void _Write_InventorySystem_002EItems_002EFirearms_002EFirearmStatus(global::Mirror.NetworkWriter writer, global::InventorySystem.Items.Firearms.FirearmStatus value)
		{
			global::Mirror.NetworkWriterExtensions.WriteByte(writer, value.Ammo);
			_Write_InventorySystem_002EItems_002EFirearms_002EFirearmStatusFlags(writer, value.Flags);
			writer.WriteUInt32(value.Attachments);
		}

		public static void _Write_InventorySystem_002EItems_002EFirearms_002EFirearmStatusFlags(global::Mirror.NetworkWriter writer, global::InventorySystem.Items.Firearms.FirearmStatusFlags value)
		{
			global::Mirror.NetworkWriterExtensions.WriteByte(writer, (byte)value);
		}

		public static global::InventorySystem.Items.Firearms.FirearmStatus _Read_InventorySystem_002EItems_002EFirearms_002EFirearmStatus(global::Mirror.NetworkReader reader)
		{
			return new global::InventorySystem.Items.Firearms.FirearmStatus
			{
				Ammo = global::Mirror.NetworkReaderExtensions.ReadByte(reader),
				Flags = _Read_InventorySystem_002EItems_002EFirearms_002EFirearmStatusFlags(reader),
				Attachments = reader.ReadUInt32()
			};
		}

		public static global::InventorySystem.Items.Firearms.FirearmStatusFlags _Read_InventorySystem_002EItems_002EFirearms_002EFirearmStatusFlags(global::Mirror.NetworkReader reader)
		{
			return (global::InventorySystem.Items.Firearms.FirearmStatusFlags)global::Mirror.NetworkReaderExtensions.ReadByte(reader);
		}

		public static void _Write_UnityEngine_002EPrimitiveType(global::Mirror.NetworkWriter writer, global::UnityEngine.PrimitiveType value)
		{
			writer.WriteInt32((int)value);
		}

		public static global::UnityEngine.PrimitiveType _Read_UnityEngine_002EPrimitiveType(global::Mirror.NetworkReader reader)
		{
			return (global::UnityEngine.PrimitiveType)reader.ReadInt32();
		}

		public static void _Write_Scp914_002EScp914KnobSetting(global::Mirror.NetworkWriter writer, global::Scp914.Scp914KnobSetting value)
		{
			global::Mirror.NetworkWriterExtensions.WriteByte(writer, (byte)value);
		}

		public static global::Scp914.Scp914KnobSetting _Read_Scp914_002EScp914KnobSetting(global::Mirror.NetworkReader reader)
		{
			return (global::Scp914.Scp914KnobSetting)global::Mirror.NetworkReaderExtensions.ReadByte(reader);
		}

		[global::UnityEngine.RuntimeInitializeOnLoadMethod(global::UnityEngine.RuntimeInitializeLoadType.BeforeSceneLoad)]
		public static void InitReadWriters()
		{
			global::Mirror.Writer<byte>.write = global::Mirror.NetworkWriterExtensions.WriteByte;
			global::Mirror.Writer<sbyte>.write = global::Mirror.NetworkWriterExtensions.WriteSByte;
			global::Mirror.Writer<char>.write = global::Mirror.NetworkWriterExtensions.WriteChar;
			global::Mirror.Writer<bool>.write = global::Mirror.NetworkWriterExtensions.WriteBoolean;
			global::Mirror.Writer<ushort>.write = global::Mirror.NetworkWriterExtensions.WriteUInt16;
			global::Mirror.Writer<short>.write = global::Mirror.NetworkWriterExtensions.WriteInt16;
			global::Mirror.Writer<uint>.write = global::Mirror.NetworkWriterExtensions.WriteUInt32;
			global::Mirror.Writer<int>.write = global::Mirror.NetworkWriterExtensions.WriteInt32;
			global::Mirror.Writer<ulong>.write = global::Mirror.NetworkWriterExtensions.WriteUInt64;
			global::Mirror.Writer<long>.write = global::Mirror.NetworkWriterExtensions.WriteInt64;
			global::Mirror.Writer<float>.write = global::Mirror.NetworkWriterExtensions.WriteSingle;
			global::Mirror.Writer<double>.write = global::Mirror.NetworkWriterExtensions.WriteDouble;
			global::Mirror.Writer<decimal>.write = global::Mirror.NetworkWriterExtensions.WriteDecimal;
			global::Mirror.Writer<string>.write = global::Mirror.NetworkWriterExtensions.WriteString;
			global::Mirror.Writer<byte[]>.write = global::Mirror.NetworkWriterExtensions.WriteBytesAndSize;
			global::Mirror.Writer<global::System.ArraySegment<byte>>.write = global::Mirror.NetworkWriterExtensions.WriteBytesAndSizeSegment;
			global::Mirror.Writer<global::UnityEngine.Vector2>.write = global::Mirror.NetworkWriterExtensions.WriteVector2;
			global::Mirror.Writer<global::UnityEngine.Vector3>.write = global::Mirror.NetworkWriterExtensions.WriteVector3;
			global::Mirror.Writer<global::UnityEngine.Vector4>.write = global::Mirror.NetworkWriterExtensions.WriteVector4;
			global::Mirror.Writer<global::UnityEngine.Vector2Int>.write = global::Mirror.NetworkWriterExtensions.WriteVector2Int;
			global::Mirror.Writer<global::UnityEngine.Vector3Int>.write = global::Mirror.NetworkWriterExtensions.WriteVector3Int;
			global::Mirror.Writer<global::UnityEngine.Color>.write = global::Mirror.NetworkWriterExtensions.WriteColor;
			global::Mirror.Writer<global::UnityEngine.Color32>.write = global::Mirror.NetworkWriterExtensions.WriteColor32;
			global::Mirror.Writer<global::UnityEngine.Quaternion>.write = global::Mirror.NetworkWriterExtensions.WriteQuaternion;
			global::Mirror.Writer<global::UnityEngine.Rect>.write = global::Mirror.NetworkWriterExtensions.WriteRect;
			global::Mirror.Writer<global::UnityEngine.Plane>.write = global::Mirror.NetworkWriterExtensions.WritePlane;
			global::Mirror.Writer<global::UnityEngine.Ray>.write = global::Mirror.NetworkWriterExtensions.WriteRay;
			global::Mirror.Writer<global::UnityEngine.Matrix4x4>.write = global::Mirror.NetworkWriterExtensions.WriteMatrix4x4;
			global::Mirror.Writer<global::System.Guid>.write = global::Mirror.NetworkWriterExtensions.WriteGuid;
			global::Mirror.Writer<global::Mirror.NetworkIdentity>.write = global::Mirror.NetworkWriterExtensions.WriteNetworkIdentity;
			global::Mirror.Writer<global::Mirror.NetworkBehaviour>.write = global::Mirror.NetworkWriterExtensions.WriteNetworkBehaviour;
			global::Mirror.Writer<global::UnityEngine.Transform>.write = global::Mirror.NetworkWriterExtensions.WriteTransform;
			global::Mirror.Writer<global::UnityEngine.GameObject>.write = global::Mirror.NetworkWriterExtensions.WriteGameObject;
			global::Mirror.Writer<global::System.Uri>.write = global::Mirror.NetworkWriterExtensions.WriteUri;
			global::Mirror.Writer<global::Mirror.ReadyMessage>.write = _Write_Mirror_002EReadyMessage;
			global::Mirror.Writer<global::Mirror.NotReadyMessage>.write = _Write_Mirror_002ENotReadyMessage;
			global::Mirror.Writer<global::Mirror.AddPlayerMessage>.write = _Write_Mirror_002EAddPlayerMessage;
			global::Mirror.Writer<global::Mirror.SceneMessage>.write = _Write_Mirror_002ESceneMessage;
			global::Mirror.Writer<global::Mirror.SceneOperation>.write = _Write_Mirror_002ESceneOperation;
			global::Mirror.Writer<global::Mirror.CommandMessage>.write = _Write_Mirror_002ECommandMessage;
			global::Mirror.Writer<global::Mirror.RpcMessage>.write = _Write_Mirror_002ERpcMessage;
			global::Mirror.Writer<global::Mirror.SpawnMessage>.write = _Write_Mirror_002ESpawnMessage;
			global::Mirror.Writer<global::Mirror.ObjectSpawnStartedMessage>.write = _Write_Mirror_002EObjectSpawnStartedMessage;
			global::Mirror.Writer<global::Mirror.ObjectSpawnFinishedMessage>.write = _Write_Mirror_002EObjectSpawnFinishedMessage;
			global::Mirror.Writer<global::Mirror.ObjectDestroyMessage>.write = _Write_Mirror_002EObjectDestroyMessage;
			global::Mirror.Writer<global::Mirror.ObjectHideMessage>.write = _Write_Mirror_002EObjectHideMessage;
			global::Mirror.Writer<global::Mirror.UpdateVarsMessage>.write = _Write_Mirror_002EUpdateVarsMessage;
			global::Mirror.Writer<global::Mirror.NetworkPingMessage>.write = _Write_Mirror_002ENetworkPingMessage;
			global::Mirror.Writer<global::Mirror.NetworkPongMessage>.write = _Write_Mirror_002ENetworkPongMessage;
			global::Mirror.Writer<BreakableWindow.BreakableWindowStatus>.write = BreakableWindowStatusSerializer.WriteBreakableWindowStatus;
			global::Mirror.Writer<AlphaWarheadSyncInfo>.write = AlphaWarheadSyncInfoSerializer.WritePickupSyncInfo;
			global::Mirror.Writer<RecyclablePlayerId>.write = RecyclablePlayerIdReaderWriter.WriteRecyclablePlayerId;
			global::Mirror.Writer<ServerConfigSynchronizer.AmmoLimit>.write = AmmoLimitSerializer.WriteAmmoLimit;
			global::Mirror.Writer<RagdollData>.write = RagdollDataReaderWriter.WriteRagdollData;
			global::Mirror.Writer<TeslaHitMsg>.write = TeslaHitMsgSerializers.Serialize;
			global::Mirror.Writer<Offset>.write = OffsetSerializer.WriteOffset;
			global::Mirror.Writer<LowPrecisionQuaternion>.write = LowPrecisionQuaternionSerializer.WriteLowPrecisionQuaternion;
			global::Mirror.Writer<global::UnityEngine.AnimationCurve>.write = global::Utils.Networking.AnimationCurveReaderWriter.WriteAnimationCurve;
			global::Mirror.Writer<global::System.Collections.Generic.IReadOnlyCollection<global::Hints.HintEffect>>.write = global::Utils.Networking.HintEffectArrayReaderWriter.WriteHintEffectArray;
			global::Mirror.Writer<global::Hints.HintEffect>.write = global::Utils.Networking.HintEffectReaderWriter.WriteHintEffect;
			global::Mirror.Writer<global::System.Collections.Generic.IReadOnlyCollection<global::Hints.HintParameter>>.write = global::Utils.Networking.HintParameterArrayReaderWriter.WriteHintParameterArray;
			global::Mirror.Writer<global::Hints.HintParameter>.write = global::Utils.Networking.HintParameterReaderWriter.WriteHintParameter;
			global::Mirror.Writer<global::Hints.Hint>.write = global::Utils.Networking.HintReaderWriter.WriteHint;
			global::Mirror.Writer<bool?>.write = global::Utils.Networking.NullableBoolReaderWriter.WriteNullableBool;
			global::Mirror.Writer<ReferenceHub>.write = global::Utils.Networking.ReferenceHubReaderWriter.WriteReferenceHub;
			global::Mirror.Writer<global::VoiceChat.Networking.VoiceMessage>.write = global::VoiceChat.Networking.VoiceMessageReadersWriters.SerializeVoiceMessage;
			global::Mirror.Writer<global::Subtitles.SubtitleMessage>.write = global::Subtitles.SubtitleMessageExtensions.Serialize;
			global::Mirror.Writer<global::RoundRestarting.RoundRestartMessage>.write = global::RoundRestarting.RoundRestartMessageReaderWriter.WriteRoundRestartMessage;
			global::Mirror.Writer<global::RelativePositioning.RelativePosition>.write = global::RelativePositioning.RelativePositionSerialization.WriteRelativePosition;
			global::Mirror.Writer<global::PlayerStatsSystem.DamageHandlerBase>.write = global::PlayerStatsSystem.DamageHandlerReaderWriter.WriteDamageHandler;
			global::Mirror.Writer<global::PlayerStatsSystem.SyncedStatMessages.StatMessage>.write = global::PlayerStatsSystem.SyncedStatMessages.Serialize;
			global::Mirror.Writer<global::PlayerRoles.RoleTypeId>.write = global::PlayerRoles.PlayerRoleEnumsReadersWriters.WriteRoleType;
			global::Mirror.Writer<global::PlayerRoles.RoleSyncInfo>.write = global::PlayerRoles.PlayerRolesNetUtils.WriteRoleSyncInfo;
			global::Mirror.Writer<global::PlayerRoles.RoleSyncInfoPack>.write = global::PlayerRoles.PlayerRolesNetUtils.WriteRoleSyncInfoPack;
			global::Mirror.Writer<global::PlayerRoles.Spectating.SpectatorSpawnReason>.write = global::PlayerRoles.Spectating.SpectatorSpawnReasonReaderWriter.WriteSpawnReason;
			global::Mirror.Writer<global::PlayerRoles.RoleAssign.ScpSpawnPreferences.SpawnPreferences>.write = global::PlayerRoles.RoleAssign.ScpSpawnPreferences.WriteSpawnPreferences;
			global::Mirror.Writer<global::PlayerRoles.PlayableScps.Subroutines.SubroutineMessage>.write = global::PlayerRoles.PlayableScps.Subroutines.SubroutineMessageReaderWriter.WriteSubroutineMessage;
			global::Mirror.Writer<global::PlayerRoles.FirstPersonControl.NetworkMessages.FpcFromClientMessage>.write = global::PlayerRoles.FirstPersonControl.NetworkMessages.FpcMessagesReadersWriters.WriteFpcFromClientMessage;
			global::Mirror.Writer<global::PlayerRoles.FirstPersonControl.NetworkMessages.FpcPositionMessage>.write = global::PlayerRoles.FirstPersonControl.NetworkMessages.FpcMessagesReadersWriters.WriteFpcPositionMessage;
			global::Mirror.Writer<global::PlayerRoles.FirstPersonControl.NetworkMessages.FpcOverrideMessage>.write = global::PlayerRoles.FirstPersonControl.NetworkMessages.FpcMessagesReadersWriters.WriteFpcOverrideMessage;
			global::Mirror.Writer<global::PlayerRoles.FirstPersonControl.NetworkMessages.FpcFallDamageMessage>.write = global::PlayerRoles.FirstPersonControl.NetworkMessages.FpcMessagesReadersWriters.WriteFpcFallDamageMessage;
			global::Mirror.Writer<global::Hints.AlphaCurveHintEffect>.write = global::Hints.AlphaCurveHintEffectFunctions.Serialize;
			global::Mirror.Writer<global::Hints.AlphaEffect>.write = global::Hints.AlphaEffectFunctions.Serialize;
			global::Mirror.Writer<global::Hints.OutlineEffect>.write = global::Hints.OutlineEffectFunctions.Serialize;
			global::Mirror.Writer<global::Hints.TextHint>.write = global::Hints.TextHintFunctions.Serialize;
			global::Mirror.Writer<global::Hints.TranslationHint>.write = global::Hints.TranslationHintFunctions.Serialize;
			global::Mirror.Writer<global::Hints.AmmoHintParameter>.write = global::Hints.AmmoHintParameterFunctions.Serialize;
			global::Mirror.Writer<global::Hints.Scp330HintParameter>.write = global::Hints.Scp330HintParameterFunctions.Serialize;
			global::Mirror.Writer<global::Hints.ItemCategoryHintParameter>.write = global::Hints.ItemCategoryHintParameterFunctions.Serialize;
			global::Mirror.Writer<global::Hints.ItemHintParameter>.write = global::Hints.ItemHintParameterFunctions.Serialize;
			global::Mirror.Writer<global::Hints.ByteHintParameter>.write = global::Hints.ByteHintParameterFunctions.Serialize;
			global::Mirror.Writer<global::Hints.DoubleHintParameter>.write = global::Hints.DoubleHintParameterFunctions.Serialize;
			global::Mirror.Writer<global::Hints.FloatHintParameter>.write = global::Hints.FloatHintParameterFunctions.Serialize;
			global::Mirror.Writer<global::Hints.IntHintParameter>.write = global::Hints.IntHintParameterFunctions.Serialize;
			global::Mirror.Writer<global::Hints.LongHintParameter>.write = global::Hints.LongHintParameterFunctions.Serialize;
			global::Mirror.Writer<global::Hints.PackedLongHintParameter>.write = global::Hints.PackedLongHintParameterFunctions.Serialize;
			global::Mirror.Writer<global::Hints.PackedULongHintParameter>.write = global::Hints.PackedULongHintParameterFunctions.Serialize;
			global::Mirror.Writer<global::Hints.SByteHintParameter>.write = global::Hints.SByteHintParameterFunctions.Serialize;
			global::Mirror.Writer<global::Hints.ShortHintParameter>.write = global::Hints.ShortHintParameterFunctions.Serialize;
			global::Mirror.Writer<global::Hints.StringHintParameter>.write = global::Hints.StringHintParameterFunctions.Serialize;
			global::Mirror.Writer<global::Hints.TimespanHintParameter>.write = global::Hints.TimespanHintParameterFunctions.Serialize;
			global::Mirror.Writer<global::Hints.UIntHintParameter>.write = global::Hints.UIntHintParameterFunctions.Serialize;
			global::Mirror.Writer<global::Hints.ULongHintParameter>.write = global::Hints.ULongHintParameterFunctions.Serialize;
			global::Mirror.Writer<global::Hints.UShortHintParameter>.write = global::Hints.UShortHintParameterFunctions.Serialize;
			global::Mirror.Writer<global::Hints.HintMessage>.write = global::Hints.HintMessageParameterFunctions.Serialize;
			global::Mirror.Writer<global::InventorySystem.Searching.SearchInvalidation>.write = global::InventorySystem.Searching.SearchInvalidationFunctions.Serialize;
			global::Mirror.Writer<global::InventorySystem.Searching.SearchRequest>.write = global::InventorySystem.Searching.SearchRequestFunctions.Serialize;
			global::Mirror.Writer<global::InventorySystem.Searching.SearchSession>.write = global::InventorySystem.Searching.SearchSessionFunctions.Serialize;
			global::Mirror.Writer<global::InventorySystem.Disarming.DisarmMessage>.write = global::InventorySystem.Disarming.DisarmMessageSerializers.Serialize;
			global::Mirror.Writer<global::InventorySystem.Disarming.DisarmedPlayersListMessage>.write = global::InventorySystem.Disarming.DisarmedPlayersListMessageSerializers.Serialize;
			global::Mirror.Writer<global::InventorySystem.Items.Pickups.PickupSyncInfo>.write = global::InventorySystem.Items.Pickups.PickupSyncInfoSerializer.WritePickupSyncInfo;
			global::Mirror.Writer<global::InventorySystem.Items.Usables.StatusMessage>.write = global::InventorySystem.Items.Usables.StatusMessageFunctions.Serialize;
			global::Mirror.Writer<global::InventorySystem.Items.Usables.ItemCooldownMessage>.write = global::InventorySystem.Items.Usables.ItemCooldownMessageFunctions.Serialize;
			global::Mirror.Writer<global::InventorySystem.Items.Usables.Scp330.SyncScp330Message>.write = global::InventorySystem.Items.Usables.Scp330.Scp330NetworkHandler.SerializeSyncMessage;
			global::Mirror.Writer<global::InventorySystem.Items.Usables.Scp330.SelectScp330Message>.write = global::InventorySystem.Items.Usables.Scp330.Scp330NetworkHandler.SerializeSelectMessage;
			global::Mirror.Writer<global::InventorySystem.Items.ThrowableProjectiles.ThrowableNetworkHandler.ThrowableItemRequestMessage>.write = global::InventorySystem.Items.ThrowableProjectiles.ThrowableNetworkHandler.SerializeRequestMsg;
			global::Mirror.Writer<global::InventorySystem.Items.ThrowableProjectiles.ThrowableNetworkHandler.ThrowableItemAudioMessage>.write = global::InventorySystem.Items.ThrowableProjectiles.ThrowableNetworkHandler.SerializeAudioMsg;
			global::Mirror.Writer<global::InventorySystem.Items.Radio.RadioStatusMessage>.write = global::InventorySystem.Items.Radio.RadioMessages.WriteRadioStatusMessage;
			global::Mirror.Writer<global::InventorySystem.Items.Radio.ClientRadioCommandMessage>.write = global::InventorySystem.Items.Radio.RadioMessages.WriteClientRadioCommandMessage;
			global::Mirror.Writer<global::InventorySystem.Items.Flashlight.FlashlightNetworkHandler.FlashlightMessage>.write = global::InventorySystem.Items.Flashlight.FlashlightNetworkHandler.Serialize;
			global::Mirror.Writer<global::InventorySystem.Items.Firearms.GunAudioMessage>.write = global::InventorySystem.Items.Firearms.FirearmAudioManager.Serialize;
			global::Mirror.Writer<global::InventorySystem.Items.Firearms.Modules.ShotgunResyncMessage>.write = global::InventorySystem.Items.Firearms.Modules.PumpMessageHandler.Serialize;
			global::Mirror.Writer<global::InventorySystem.Items.Firearms.BasicMessages.GunHitMessage>.write = global::InventorySystem.Items.Firearms.BasicMessages.GunHitMessageExtensions.Serialize;
			global::Mirror.Writer<global::InventorySystem.Items.Firearms.BasicMessages.RequestMessage>.write = global::InventorySystem.Items.Firearms.BasicMessages.RequestMessageFunctions.Serialize;
			global::Mirror.Writer<global::InventorySystem.Items.Firearms.BasicMessages.ShotMessage>.write = global::InventorySystem.Items.Firearms.BasicMessages.ShotMessageFunctions.Serialize;
			global::Mirror.Writer<global::InventorySystem.Items.Firearms.BasicMessages.StatusMessage>.write = global::InventorySystem.Items.Firearms.BasicMessages.StatusMessageFunctions.Serialize;
			global::Mirror.Writer<global::InventorySystem.Items.Firearms.Attachments.AttachmentsSetupPreference>.write = global::InventorySystem.Items.Firearms.Attachments.AttachmentsSetupPreferenceFunctions.Serialize;
			global::Mirror.Writer<global::InventorySystem.Items.Firearms.Attachments.AttachmentsChangeRequest>.write = global::InventorySystem.Items.Firearms.Attachments.AttachmentsChangeRequestFunctions.Serialize;
			global::Mirror.Writer<global::InventorySystem.Items.Firearms.Attachments.Components.ReflexSightSyncMessage>.write = global::InventorySystem.Items.Firearms.Attachments.Components.ReflexSightSyncMessageReaderWriter.WriteReflexSightSyncMessage;
			global::Mirror.Writer<global::InventorySystem.Items.Autosync.AutosyncMessage>.write = global::InventorySystem.Items.Autosync.AutosyncMessageHandler.WriteAutosyncMessage;
			global::Mirror.Writer<global::InventorySystem.Items.MicroHID.HidStatusMessage>.write = global::InventorySystem.Items.MicroHID.HidStatusMessageFunctions.Serialize;
			global::Mirror.Writer<global::Respawning.NamingRules.UnitNameMessage>.write = global::Respawning.NamingRules.UnitNameMessageHandler.WriteUnitName;
			global::Mirror.Writer<Hitmarker.HitmarkerMessage>.write = _Write_Hitmarker_002FHitmarkerMessage;
			global::Mirror.Writer<Escape.EscapeMessage>.write = _Write_Escape_002FEscapeMessage;
			global::Mirror.Writer<ServerShutdown.ServerShutdownMessage>.write = _Write_ServerShutdown_002FServerShutdownMessage;
			global::Mirror.Writer<global::VoiceChat.VoiceChatMuteIndicator.SyncMuteMessage>.write = _Write_VoiceChat_002EVoiceChatMuteIndicator_002FSyncMuteMessage;
			global::Mirror.Writer<global::VoiceChat.VoiceChatPrivacySettings.VcPrivacyMessage>.write = _Write_VoiceChat_002EVoiceChatPrivacySettings_002FVcPrivacyMessage;
			global::Mirror.Writer<global::VoiceChat.Playbacks.PersonalRadioPlayback.TransmitterPositionMessage>.write = _Write_VoiceChat_002EPlaybacks_002EPersonalRadioPlayback_002FTransmitterPositionMessage;
			global::Mirror.Writer<global::PlayerRoles.Voice.VoiceChatReceivePrefs.GroupMuteFlagsMessage>.write = _Write_PlayerRoles_002EVoice_002EVoiceChatReceivePrefs_002FGroupMuteFlagsMessage;
			global::Mirror.Writer<global::PlayerRoles.Spectating.SpectatorNetworking.SpectatedNetIdSyncMessage>.write = _Write_PlayerRoles_002ESpectating_002ESpectatorNetworking_002FSpectatedNetIdSyncMessage;
			global::Mirror.Writer<global::PlayerRoles.PlayableScps.Scp106.Scp106PocketItemManager.WarningMessage>.write = _Write_PlayerRoles_002EPlayableScps_002EScp106_002EScp106PocketItemManager_002FWarningMessage;
			global::Mirror.Writer<global::PlayerRoles.PlayableScps.Scp049.Zombies.ZombieConfirmationBox.ScpReviveBlockMessage>.write = _Write_PlayerRoles_002EPlayableScps_002EScp049_002EZombies_002EZombieConfirmationBox_002FScpReviveBlockMessage;
			global::Mirror.Writer<global::PlayerRoles.PlayableScps.HumeShield.DynamicHumeShieldController.ShieldBreakMessage>.write = _Write_PlayerRoles_002EPlayableScps_002EHumeShield_002EDynamicHumeShieldController_002FShieldBreakMessage;
			global::Mirror.Writer<global::PlayerRoles.FirstPersonControl.NetworkMessages.FpcNoclipToggleMessage>.write = _Write_PlayerRoles_002EFirstPersonControl_002ENetworkMessages_002EFpcNoclipToggleMessage;
			global::Mirror.Writer<global::InventorySystem.Items.Usables.Scp330.CandyPink.CandyExplosionMessage>.write = _Write_InventorySystem_002EItems_002EUsables_002EScp330_002ECandyPink_002FCandyExplosionMessage;
			global::Mirror.Writer<global::InventorySystem.Items.Usables.Scp244.Hypothermia.HumeShieldSubEffect.HumeBlockMsg>.write = _Write_InventorySystem_002EItems_002EUsables_002EScp244_002EHypothermia_002EHumeShieldSubEffect_002FHumeBlockMsg;
			global::Mirror.Writer<global::InventorySystem.Items.Usables.Scp1576.Scp1576SpectatorWarningHandler.SpectatorWarningMessage>.write = _Write_InventorySystem_002EItems_002EUsables_002EScp1576_002EScp1576SpectatorWarningHandler_002FSpectatorWarningMessage;
			global::Mirror.Writer<global::InventorySystem.Items.Keycards.KeycardItem.UseMessage>.write = _Write_InventorySystem_002EItems_002EKeycards_002EKeycardItem_002FUseMessage;
			global::Mirror.Writer<global::InventorySystem.Items.Firearms.Modules.AutomaticAction.RefusedShotMessage>.write = _Write_InventorySystem_002EItems_002EFirearms_002EModules_002EAutomaticAction_002FRefusedShotMessage;
			global::Mirror.Writer<global::InventorySystem.Items.Firearms.Modules.DisruptorHitreg.DisruptorHitMessage>.write = _Write_InventorySystem_002EItems_002EFirearms_002EModules_002EDisruptorHitreg_002FDisruptorHitMessage;
			global::Mirror.Writer<global::InventorySystem.Items.Firearms.Modules.CockMessage>.write = _Write_InventorySystem_002EItems_002EFirearms_002EModules_002ECockMessage;
			global::Mirror.Writer<global::Achievements.AchievementManager.AchievementMessage>.write = _Write_Achievements_002EAchievementManager_002FAchievementMessage;
			global::Mirror.Writer<global::Interactables.Interobjects.ElevatorManager.ElevatorSyncMsg>.write = _Write_Interactables_002EInterobjects_002EElevatorManager_002FElevatorSyncMsg;
			global::Mirror.Writer<ServerConfigSynchronizer.PredefinedBanTemplate>.write = _Write_ServerConfigSynchronizer_002FPredefinedBanTemplate;
			global::Mirror.Writer<Broadcast.BroadcastFlags>.write = _Write_Broadcast_002FBroadcastFlags;
			global::Mirror.Writer<global::UnityEngine.KeyCode>.write = _Write_UnityEngine_002EKeyCode;
			global::Mirror.Writer<PlayerInfoArea>.write = _Write_PlayerInfoArea;
			global::Mirror.Writer<PlayerInteract.AlphaPanelOperations>.write = _Write_PlayerInteract_002FAlphaPanelOperations;
			global::Mirror.Writer<RoundSummary.SumInfo_ClassList>.write = _Write_RoundSummary_002FSumInfo_ClassList;
			global::Mirror.Writer<RoundSummary.LeadingTeam>.write = _Write_RoundSummary_002FLeadingTeam;
			global::Mirror.Writer<global::RemoteAdmin.QueryProcessor.CommandData[]>.write = _Write_RemoteAdmin_002EQueryProcessor_002FCommandData_005B_005D;
			global::Mirror.Writer<global::RemoteAdmin.QueryProcessor.CommandData>.write = _Write_RemoteAdmin_002EQueryProcessor_002FCommandData;
			global::Mirror.Writer<string[]>.write = _Write_System_002EString_005B_005D;
			global::Mirror.Writer<global::LightContainmentZoneDecontamination.DecontaminationController.DecontaminationStatus>.write = _Write_LightContainmentZoneDecontamination_002EDecontaminationController_002FDecontaminationStatus;
			global::Mirror.Writer<global::InventorySystem.Items.ItemIdentifier[]>.write = _Write_InventorySystem_002EItems_002EItemIdentifier_005B_005D;
			global::Mirror.Writer<global::InventorySystem.Items.ItemIdentifier>.write = _Write_InventorySystem_002EItems_002EItemIdentifier;
			global::Mirror.Writer<ItemType>.write = _Write_ItemType;
			global::Mirror.Writer<ushort[]>.write = _Write_System_002EUInt16_005B_005D;
			global::Mirror.Writer<ActionName>.write = _Write_ActionName;
			global::Mirror.Writer<global::InventorySystem.Items.Usables.Scp330.CandyKindID>.write = _Write_InventorySystem_002EItems_002EUsables_002EScp330_002ECandyKindID;
			global::Mirror.Writer<global::InventorySystem.Items.Firearms.FirearmStatus>.write = _Write_InventorySystem_002EItems_002EFirearms_002EFirearmStatus;
			global::Mirror.Writer<global::InventorySystem.Items.Firearms.FirearmStatusFlags>.write = _Write_InventorySystem_002EItems_002EFirearms_002EFirearmStatusFlags;
			global::Mirror.Writer<global::UnityEngine.PrimitiveType>.write = _Write_UnityEngine_002EPrimitiveType;
			global::Mirror.Writer<global::Scp914.Scp914KnobSetting>.write = _Write_Scp914_002EScp914KnobSetting;
			global::Mirror.Reader<byte>.read = global::Mirror.NetworkReaderExtensions.ReadByte;
			global::Mirror.Reader<sbyte>.read = global::Mirror.NetworkReaderExtensions.ReadSByte;
			global::Mirror.Reader<char>.read = global::Mirror.NetworkReaderExtensions.ReadChar;
			global::Mirror.Reader<bool>.read = global::Mirror.NetworkReaderExtensions.ReadBoolean;
			global::Mirror.Reader<short>.read = global::Mirror.NetworkReaderExtensions.ReadInt16;
			global::Mirror.Reader<ushort>.read = global::Mirror.NetworkReaderExtensions.ReadUInt16;
			global::Mirror.Reader<int>.read = global::Mirror.NetworkReaderExtensions.ReadInt32;
			global::Mirror.Reader<uint>.read = global::Mirror.NetworkReaderExtensions.ReadUInt32;
			global::Mirror.Reader<long>.read = global::Mirror.NetworkReaderExtensions.ReadInt64;
			global::Mirror.Reader<ulong>.read = global::Mirror.NetworkReaderExtensions.ReadUInt64;
			global::Mirror.Reader<float>.read = global::Mirror.NetworkReaderExtensions.ReadSingle;
			global::Mirror.Reader<double>.read = global::Mirror.NetworkReaderExtensions.ReadDouble;
			global::Mirror.Reader<decimal>.read = global::Mirror.NetworkReaderExtensions.ReadDecimal;
			global::Mirror.Reader<string>.read = global::Mirror.NetworkReaderExtensions.ReadString;
			global::Mirror.Reader<byte[]>.read = global::Mirror.NetworkReaderExtensions.ReadBytesAndSize;
			global::Mirror.Reader<global::System.ArraySegment<byte>>.read = global::Mirror.NetworkReaderExtensions.ReadBytesAndSizeSegment;
			global::Mirror.Reader<global::UnityEngine.Vector2>.read = global::Mirror.NetworkReaderExtensions.ReadVector2;
			global::Mirror.Reader<global::UnityEngine.Vector3>.read = global::Mirror.NetworkReaderExtensions.ReadVector3;
			global::Mirror.Reader<global::UnityEngine.Vector4>.read = global::Mirror.NetworkReaderExtensions.ReadVector4;
			global::Mirror.Reader<global::UnityEngine.Vector2Int>.read = global::Mirror.NetworkReaderExtensions.ReadVector2Int;
			global::Mirror.Reader<global::UnityEngine.Vector3Int>.read = global::Mirror.NetworkReaderExtensions.ReadVector3Int;
			global::Mirror.Reader<global::UnityEngine.Color>.read = global::Mirror.NetworkReaderExtensions.ReadColor;
			global::Mirror.Reader<global::UnityEngine.Color32>.read = global::Mirror.NetworkReaderExtensions.ReadColor32;
			global::Mirror.Reader<global::UnityEngine.Quaternion>.read = global::Mirror.NetworkReaderExtensions.ReadQuaternion;
			global::Mirror.Reader<global::UnityEngine.Rect>.read = global::Mirror.NetworkReaderExtensions.ReadRect;
			global::Mirror.Reader<global::UnityEngine.Plane>.read = global::Mirror.NetworkReaderExtensions.ReadPlane;
			global::Mirror.Reader<global::UnityEngine.Ray>.read = global::Mirror.NetworkReaderExtensions.ReadRay;
			global::Mirror.Reader<global::UnityEngine.Matrix4x4>.read = global::Mirror.NetworkReaderExtensions.ReadMatrix4x4;
			global::Mirror.Reader<global::System.Guid>.read = global::Mirror.NetworkReaderExtensions.ReadGuid;
			global::Mirror.Reader<global::UnityEngine.Transform>.read = global::Mirror.NetworkReaderExtensions.ReadTransform;
			global::Mirror.Reader<global::UnityEngine.GameObject>.read = global::Mirror.NetworkReaderExtensions.ReadGameObject;
			global::Mirror.Reader<global::Mirror.NetworkIdentity>.read = global::Mirror.NetworkReaderExtensions.ReadNetworkIdentity;
			global::Mirror.Reader<global::Mirror.NetworkBehaviour>.read = global::Mirror.NetworkReaderExtensions.ReadNetworkBehaviour;
			global::Mirror.Reader<global::Mirror.NetworkBehaviour.NetworkBehaviourSyncVar>.read = global::Mirror.NetworkReaderExtensions.ReadNetworkBehaviourSyncVar;
			global::Mirror.Reader<global::System.Uri>.read = global::Mirror.NetworkReaderExtensions.ReadUri;
			global::Mirror.Reader<global::Mirror.ReadyMessage>.read = _Read_Mirror_002EReadyMessage;
			global::Mirror.Reader<global::Mirror.NotReadyMessage>.read = _Read_Mirror_002ENotReadyMessage;
			global::Mirror.Reader<global::Mirror.AddPlayerMessage>.read = _Read_Mirror_002EAddPlayerMessage;
			global::Mirror.Reader<global::Mirror.SceneMessage>.read = _Read_Mirror_002ESceneMessage;
			global::Mirror.Reader<global::Mirror.SceneOperation>.read = _Read_Mirror_002ESceneOperation;
			global::Mirror.Reader<global::Mirror.CommandMessage>.read = _Read_Mirror_002ECommandMessage;
			global::Mirror.Reader<global::Mirror.RpcMessage>.read = _Read_Mirror_002ERpcMessage;
			global::Mirror.Reader<global::Mirror.SpawnMessage>.read = _Read_Mirror_002ESpawnMessage;
			global::Mirror.Reader<global::Mirror.ObjectSpawnStartedMessage>.read = _Read_Mirror_002EObjectSpawnStartedMessage;
			global::Mirror.Reader<global::Mirror.ObjectSpawnFinishedMessage>.read = _Read_Mirror_002EObjectSpawnFinishedMessage;
			global::Mirror.Reader<global::Mirror.ObjectDestroyMessage>.read = _Read_Mirror_002EObjectDestroyMessage;
			global::Mirror.Reader<global::Mirror.ObjectHideMessage>.read = _Read_Mirror_002EObjectHideMessage;
			global::Mirror.Reader<global::Mirror.UpdateVarsMessage>.read = _Read_Mirror_002EUpdateVarsMessage;
			global::Mirror.Reader<global::Mirror.NetworkPingMessage>.read = _Read_Mirror_002ENetworkPingMessage;
			global::Mirror.Reader<global::Mirror.NetworkPongMessage>.read = _Read_Mirror_002ENetworkPongMessage;
			global::Mirror.Reader<BreakableWindow.BreakableWindowStatus>.read = BreakableWindowStatusSerializer.ReadBreakableWindowStatus;
			global::Mirror.Reader<AlphaWarheadSyncInfo>.read = AlphaWarheadSyncInfoSerializer.ReadPickupSyncInfo;
			global::Mirror.Reader<RecyclablePlayerId>.read = RecyclablePlayerIdReaderWriter.ReadRecyclablePlayerId;
			global::Mirror.Reader<ServerConfigSynchronizer.AmmoLimit>.read = AmmoLimitSerializer.ReadAmmoLimit;
			global::Mirror.Reader<RagdollData>.read = RagdollDataReaderWriter.ReadRagdollData;
			global::Mirror.Reader<TeslaHitMsg>.read = TeslaHitMsgSerializers.Deserialize;
			global::Mirror.Reader<Offset>.read = OffsetSerializer.ReadOffset;
			global::Mirror.Reader<LowPrecisionQuaternion>.read = LowPrecisionQuaternionSerializer.ReadLowPrecisionQuaternion;
			global::Mirror.Reader<global::UnityEngine.AnimationCurve>.read = global::Utils.Networking.AnimationCurveReaderWriter.ReadAnimationCurve;
			global::Mirror.Reader<global::Hints.HintEffect[]>.read = global::Utils.Networking.HintEffectArrayReaderWriter.ReadHintEffectArray;
			global::Mirror.Reader<global::Hints.HintEffect>.read = global::Utils.Networking.HintEffectReaderWriter.ReadHintEffect;
			global::Mirror.Reader<global::Hints.HintParameter[]>.read = global::Utils.Networking.HintParameterArrayReaderWriter.ReadHintParameterArray;
			global::Mirror.Reader<global::Hints.HintParameter>.read = global::Utils.Networking.HintParameterReaderWriter.ReadHintParameter;
			global::Mirror.Reader<global::Hints.Hint>.read = global::Utils.Networking.HintReaderWriter.ReadHint;
			global::Mirror.Reader<bool?>.read = global::Utils.Networking.NullableBoolReaderWriter.ReadNullableBool;
			global::Mirror.Reader<ReferenceHub>.read = global::Utils.Networking.ReferenceHubReaderWriter.ReadReferenceHub;
			global::Mirror.Reader<global::VoiceChat.Networking.VoiceMessage>.read = global::VoiceChat.Networking.VoiceMessageReadersWriters.DeserializeVoiceMessage;
			global::Mirror.Reader<global::Subtitles.SubtitleMessage>.read = global::Subtitles.SubtitleMessageExtensions.Deserialize;
			global::Mirror.Reader<global::RoundRestarting.RoundRestartMessage>.read = global::RoundRestarting.RoundRestartMessageReaderWriter.ReadRoundRestartMessage;
			global::Mirror.Reader<global::RelativePositioning.RelativePosition>.read = global::RelativePositioning.RelativePositionSerialization.ReadRelativePosition;
			global::Mirror.Reader<global::PlayerStatsSystem.DamageHandlerBase>.read = global::PlayerStatsSystem.DamageHandlerReaderWriter.ReadDamageHandler;
			global::Mirror.Reader<global::PlayerStatsSystem.SyncedStatMessages.StatMessage>.read = global::PlayerStatsSystem.SyncedStatMessages.Deserialize;
			global::Mirror.Reader<global::PlayerRoles.RoleTypeId>.read = global::PlayerRoles.PlayerRoleEnumsReadersWriters.ReadRoleType;
			global::Mirror.Reader<global::PlayerRoles.RoleSyncInfo>.read = global::PlayerRoles.PlayerRolesNetUtils.ReadRoleSyncInfo;
			global::Mirror.Reader<global::PlayerRoles.RoleSyncInfoPack>.read = global::PlayerRoles.PlayerRolesNetUtils.ReadRoleSyncInfoPack;
			global::Mirror.Reader<global::PlayerRoles.Spectating.SpectatorSpawnReason>.read = global::PlayerRoles.Spectating.SpectatorSpawnReasonReaderWriter.ReadSpawnReason;
			global::Mirror.Reader<global::PlayerRoles.RoleAssign.ScpSpawnPreferences.SpawnPreferences>.read = global::PlayerRoles.RoleAssign.ScpSpawnPreferences.ReadSpawnPreferences;
			global::Mirror.Reader<global::PlayerRoles.PlayableScps.Subroutines.SubroutineMessage>.read = global::PlayerRoles.PlayableScps.Subroutines.SubroutineMessageReaderWriter.ReadSubroutineMessage;
			global::Mirror.Reader<global::PlayerRoles.FirstPersonControl.NetworkMessages.FpcFromClientMessage>.read = global::PlayerRoles.FirstPersonControl.NetworkMessages.FpcMessagesReadersWriters.ReadFpcFromClientMessage;
			global::Mirror.Reader<global::PlayerRoles.FirstPersonControl.NetworkMessages.FpcPositionMessage>.read = global::PlayerRoles.FirstPersonControl.NetworkMessages.FpcMessagesReadersWriters.ReadFpcPositionMessage;
			global::Mirror.Reader<global::PlayerRoles.FirstPersonControl.NetworkMessages.FpcOverrideMessage>.read = global::PlayerRoles.FirstPersonControl.NetworkMessages.FpcMessagesReadersWriters.ReadFpcOverrideMessage;
			global::Mirror.Reader<global::PlayerRoles.FirstPersonControl.NetworkMessages.FpcFallDamageMessage>.read = global::PlayerRoles.FirstPersonControl.NetworkMessages.FpcMessagesReadersWriters.ReadFpcFallDamageMessage;
			global::Mirror.Reader<global::Hints.AlphaCurveHintEffect>.read = global::Hints.AlphaCurveHintEffectFunctions.Deserialize;
			global::Mirror.Reader<global::Hints.AlphaEffect>.read = global::Hints.AlphaEffectFunctions.Deserialize;
			global::Mirror.Reader<global::Hints.OutlineEffect>.read = global::Hints.OutlineEffectFunctions.Deserialize;
			global::Mirror.Reader<global::Hints.TextHint>.read = global::Hints.TextHintFunctions.Deserialize;
			global::Mirror.Reader<global::Hints.TranslationHint>.read = global::Hints.TranslationHintFunctions.Deserialize;
			global::Mirror.Reader<global::Hints.AmmoHintParameter>.read = global::Hints.AmmoHintParameterFunctions.Deserialize;
			global::Mirror.Reader<global::Hints.Scp330HintParameter>.read = global::Hints.Scp330HintParameterFunctions.Deserialize;
			global::Mirror.Reader<global::Hints.ItemCategoryHintParameter>.read = global::Hints.ItemCategoryHintParameterFunctions.Deserialize;
			global::Mirror.Reader<global::Hints.ItemHintParameter>.read = global::Hints.ItemHintParameterFunctions.Deserialize;
			global::Mirror.Reader<global::Hints.ByteHintParameter>.read = global::Hints.ByteHintParameterFunctions.Deserialize;
			global::Mirror.Reader<global::Hints.DoubleHintParameter>.read = global::Hints.DoubleHintParameterFunctions.Deserialize;
			global::Mirror.Reader<global::Hints.FloatHintParameter>.read = global::Hints.FloatHintParameterFunctions.Deserialize;
			global::Mirror.Reader<global::Hints.IntHintParameter>.read = global::Hints.IntHintParameterFunctions.Deserialize;
			global::Mirror.Reader<global::Hints.LongHintParameter>.read = global::Hints.LongHintParameterFunctions.Deserialize;
			global::Mirror.Reader<global::Hints.PackedLongHintParameter>.read = global::Hints.PackedLongHintParameterFunctions.Deserialize;
			global::Mirror.Reader<global::Hints.PackedULongHintParameter>.read = global::Hints.PackedULongHintParameterFunctions.Deserialize;
			global::Mirror.Reader<global::Hints.SByteHintParameter>.read = global::Hints.SByteHintParameterFunctions.Deserialize;
			global::Mirror.Reader<global::Hints.ShortHintParameter>.read = global::Hints.ShortHintParameterFunctions.Deserialize;
			global::Mirror.Reader<global::Hints.StringHintParameter>.read = global::Hints.StringHintParameterFunctions.Deserialize;
			global::Mirror.Reader<global::Hints.TimespanHintParameter>.read = global::Hints.TimespanHintParameterFunctions.Deserialize;
			global::Mirror.Reader<global::Hints.UIntHintParameter>.read = global::Hints.UIntHintParameterFunctions.Deserialize;
			global::Mirror.Reader<global::Hints.ULongHintParameter>.read = global::Hints.ULongHintParameterFunctions.Deserialize;
			global::Mirror.Reader<global::Hints.UShortHintParameter>.read = global::Hints.UShortHintParameterFunctions.Deserialize;
			global::Mirror.Reader<global::Hints.HintMessage>.read = global::Hints.HintMessageParameterFunctions.Deserialize;
			global::Mirror.Reader<global::InventorySystem.Searching.SearchInvalidation>.read = global::InventorySystem.Searching.SearchInvalidationFunctions.Deserialize;
			global::Mirror.Reader<global::InventorySystem.Searching.SearchRequest>.read = global::InventorySystem.Searching.SearchRequestFunctions.Deserialize;
			global::Mirror.Reader<global::InventorySystem.Searching.SearchSession>.read = global::InventorySystem.Searching.SearchSessionFunctions.Deserialize;
			global::Mirror.Reader<global::InventorySystem.Disarming.DisarmMessage>.read = global::InventorySystem.Disarming.DisarmMessageSerializers.Deserialize;
			global::Mirror.Reader<global::InventorySystem.Disarming.DisarmedPlayersListMessage>.read = global::InventorySystem.Disarming.DisarmedPlayersListMessageSerializers.Deserialize;
			global::Mirror.Reader<global::InventorySystem.Items.Pickups.PickupSyncInfo>.read = global::InventorySystem.Items.Pickups.PickupSyncInfoSerializer.ReadPickupSyncInfo;
			global::Mirror.Reader<global::InventorySystem.Items.Usables.StatusMessage>.read = global::InventorySystem.Items.Usables.StatusMessageFunctions.Deserialize;
			global::Mirror.Reader<global::InventorySystem.Items.Usables.ItemCooldownMessage>.read = global::InventorySystem.Items.Usables.ItemCooldownMessageFunctions.Deserialize;
			global::Mirror.Reader<global::InventorySystem.Items.Usables.Scp330.SyncScp330Message>.read = global::InventorySystem.Items.Usables.Scp330.Scp330NetworkHandler.DeserializeSyncMessage;
			global::Mirror.Reader<global::InventorySystem.Items.Usables.Scp330.SelectScp330Message>.read = global::InventorySystem.Items.Usables.Scp330.Scp330NetworkHandler.DeserializeSelectMessage;
			global::Mirror.Reader<global::InventorySystem.Items.ThrowableProjectiles.ThrowableNetworkHandler.ThrowableItemRequestMessage>.read = global::InventorySystem.Items.ThrowableProjectiles.ThrowableNetworkHandler.DeserializeRequestMsg;
			global::Mirror.Reader<global::InventorySystem.Items.ThrowableProjectiles.ThrowableNetworkHandler.ThrowableItemAudioMessage>.read = global::InventorySystem.Items.ThrowableProjectiles.ThrowableNetworkHandler.DeserializeAudioMsg;
			global::Mirror.Reader<global::InventorySystem.Items.Radio.RadioStatusMessage>.read = global::InventorySystem.Items.Radio.RadioMessages.ReadRadioStatusMessage;
			global::Mirror.Reader<global::InventorySystem.Items.Radio.ClientRadioCommandMessage>.read = global::InventorySystem.Items.Radio.RadioMessages.ReadClientRadioCommandMessage;
			global::Mirror.Reader<global::InventorySystem.Items.Flashlight.FlashlightNetworkHandler.FlashlightMessage>.read = global::InventorySystem.Items.Flashlight.FlashlightNetworkHandler.Deserialize;
			global::Mirror.Reader<global::InventorySystem.Items.Firearms.GunAudioMessage>.read = global::InventorySystem.Items.Firearms.FirearmAudioManager.Deserialize;
			global::Mirror.Reader<global::InventorySystem.Items.Firearms.Modules.ShotgunResyncMessage>.read = global::InventorySystem.Items.Firearms.Modules.PumpMessageHandler.Deserialize;
			global::Mirror.Reader<global::InventorySystem.Items.Firearms.BasicMessages.GunHitMessage>.read = global::InventorySystem.Items.Firearms.BasicMessages.GunHitMessageExtensions.Deserialize;
			global::Mirror.Reader<global::InventorySystem.Items.Firearms.BasicMessages.RequestMessage>.read = global::InventorySystem.Items.Firearms.BasicMessages.RequestMessageFunctions.Deserialize;
			global::Mirror.Reader<global::InventorySystem.Items.Firearms.BasicMessages.ShotMessage>.read = global::InventorySystem.Items.Firearms.BasicMessages.ShotMessageFunctions.Deserialize;
			global::Mirror.Reader<global::InventorySystem.Items.Firearms.BasicMessages.StatusMessage>.read = global::InventorySystem.Items.Firearms.BasicMessages.StatusMessageFunctions.Deserialize;
			global::Mirror.Reader<global::InventorySystem.Items.Firearms.Attachments.AttachmentsSetupPreference>.read = global::InventorySystem.Items.Firearms.Attachments.AttachmentsSetupPreferenceFunctions.Deserialize;
			global::Mirror.Reader<global::InventorySystem.Items.Firearms.Attachments.AttachmentsChangeRequest>.read = global::InventorySystem.Items.Firearms.Attachments.AttachmentsChangeRequestFunctions.Deserialize;
			global::Mirror.Reader<global::InventorySystem.Items.Firearms.Attachments.Components.ReflexSightSyncMessage>.read = global::InventorySystem.Items.Firearms.Attachments.Components.ReflexSightSyncMessageReaderWriter.ReadReflexSightSyncMessage;
			global::Mirror.Reader<global::InventorySystem.Items.Autosync.AutosyncMessage>.read = global::InventorySystem.Items.Autosync.AutosyncMessageHandler.ReadAutosyncMessage;
			global::Mirror.Reader<global::InventorySystem.Items.MicroHID.HidStatusMessage>.read = global::InventorySystem.Items.MicroHID.HidStatusMessageFunctions.Deserialize;
			global::Mirror.Reader<global::Respawning.NamingRules.UnitNameMessage>.read = global::Respawning.NamingRules.UnitNameMessageHandler.ReadUnitName;
			global::Mirror.Reader<Hitmarker.HitmarkerMessage>.read = _Read_Hitmarker_002FHitmarkerMessage;
			global::Mirror.Reader<Escape.EscapeMessage>.read = _Read_Escape_002FEscapeMessage;
			global::Mirror.Reader<ServerShutdown.ServerShutdownMessage>.read = _Read_ServerShutdown_002FServerShutdownMessage;
			global::Mirror.Reader<global::VoiceChat.VoiceChatMuteIndicator.SyncMuteMessage>.read = _Read_VoiceChat_002EVoiceChatMuteIndicator_002FSyncMuteMessage;
			global::Mirror.Reader<global::VoiceChat.VoiceChatPrivacySettings.VcPrivacyMessage>.read = _Read_VoiceChat_002EVoiceChatPrivacySettings_002FVcPrivacyMessage;
			global::Mirror.Reader<global::VoiceChat.Playbacks.PersonalRadioPlayback.TransmitterPositionMessage>.read = _Read_VoiceChat_002EPlaybacks_002EPersonalRadioPlayback_002FTransmitterPositionMessage;
			global::Mirror.Reader<global::PlayerRoles.Voice.VoiceChatReceivePrefs.GroupMuteFlagsMessage>.read = _Read_PlayerRoles_002EVoice_002EVoiceChatReceivePrefs_002FGroupMuteFlagsMessage;
			global::Mirror.Reader<global::PlayerRoles.Spectating.SpectatorNetworking.SpectatedNetIdSyncMessage>.read = _Read_PlayerRoles_002ESpectating_002ESpectatorNetworking_002FSpectatedNetIdSyncMessage;
			global::Mirror.Reader<global::PlayerRoles.PlayableScps.Scp106.Scp106PocketItemManager.WarningMessage>.read = _Read_PlayerRoles_002EPlayableScps_002EScp106_002EScp106PocketItemManager_002FWarningMessage;
			global::Mirror.Reader<global::PlayerRoles.PlayableScps.Scp049.Zombies.ZombieConfirmationBox.ScpReviveBlockMessage>.read = _Read_PlayerRoles_002EPlayableScps_002EScp049_002EZombies_002EZombieConfirmationBox_002FScpReviveBlockMessage;
			global::Mirror.Reader<global::PlayerRoles.PlayableScps.HumeShield.DynamicHumeShieldController.ShieldBreakMessage>.read = _Read_PlayerRoles_002EPlayableScps_002EHumeShield_002EDynamicHumeShieldController_002FShieldBreakMessage;
			global::Mirror.Reader<global::PlayerRoles.FirstPersonControl.NetworkMessages.FpcNoclipToggleMessage>.read = _Read_PlayerRoles_002EFirstPersonControl_002ENetworkMessages_002EFpcNoclipToggleMessage;
			global::Mirror.Reader<global::InventorySystem.Items.Usables.Scp330.CandyPink.CandyExplosionMessage>.read = _Read_InventorySystem_002EItems_002EUsables_002EScp330_002ECandyPink_002FCandyExplosionMessage;
			global::Mirror.Reader<global::InventorySystem.Items.Usables.Scp244.Hypothermia.HumeShieldSubEffect.HumeBlockMsg>.read = _Read_InventorySystem_002EItems_002EUsables_002EScp244_002EHypothermia_002EHumeShieldSubEffect_002FHumeBlockMsg;
			global::Mirror.Reader<global::InventorySystem.Items.Usables.Scp1576.Scp1576SpectatorWarningHandler.SpectatorWarningMessage>.read = _Read_InventorySystem_002EItems_002EUsables_002EScp1576_002EScp1576SpectatorWarningHandler_002FSpectatorWarningMessage;
			global::Mirror.Reader<global::InventorySystem.Items.Keycards.KeycardItem.UseMessage>.read = _Read_InventorySystem_002EItems_002EKeycards_002EKeycardItem_002FUseMessage;
			global::Mirror.Reader<global::InventorySystem.Items.Firearms.Modules.AutomaticAction.RefusedShotMessage>.read = _Read_InventorySystem_002EItems_002EFirearms_002EModules_002EAutomaticAction_002FRefusedShotMessage;
			global::Mirror.Reader<global::InventorySystem.Items.Firearms.Modules.DisruptorHitreg.DisruptorHitMessage>.read = _Read_InventorySystem_002EItems_002EFirearms_002EModules_002EDisruptorHitreg_002FDisruptorHitMessage;
			global::Mirror.Reader<global::InventorySystem.Items.Firearms.Modules.CockMessage>.read = _Read_InventorySystem_002EItems_002EFirearms_002EModules_002ECockMessage;
			global::Mirror.Reader<global::Achievements.AchievementManager.AchievementMessage>.read = _Read_Achievements_002EAchievementManager_002FAchievementMessage;
			global::Mirror.Reader<global::Interactables.Interobjects.ElevatorManager.ElevatorSyncMsg>.read = _Read_Interactables_002EInterobjects_002EElevatorManager_002FElevatorSyncMsg;
			global::Mirror.Reader<ServerConfigSynchronizer.PredefinedBanTemplate>.read = _Read_ServerConfigSynchronizer_002FPredefinedBanTemplate;
			global::Mirror.Reader<Broadcast.BroadcastFlags>.read = _Read_Broadcast_002FBroadcastFlags;
			global::Mirror.Reader<global::UnityEngine.KeyCode>.read = _Read_UnityEngine_002EKeyCode;
			global::Mirror.Reader<PlayerInfoArea>.read = _Read_PlayerInfoArea;
			global::Mirror.Reader<PlayerInteract.AlphaPanelOperations>.read = _Read_PlayerInteract_002FAlphaPanelOperations;
			global::Mirror.Reader<RoundSummary.SumInfo_ClassList>.read = _Read_RoundSummary_002FSumInfo_ClassList;
			global::Mirror.Reader<RoundSummary.LeadingTeam>.read = _Read_RoundSummary_002FLeadingTeam;
			global::Mirror.Reader<global::RemoteAdmin.QueryProcessor.CommandData[]>.read = _Read_RemoteAdmin_002EQueryProcessor_002FCommandData_005B_005D;
			global::Mirror.Reader<global::RemoteAdmin.QueryProcessor.CommandData>.read = _Read_RemoteAdmin_002EQueryProcessor_002FCommandData;
			global::Mirror.Reader<string[]>.read = _Read_System_002EString_005B_005D;
			global::Mirror.Reader<global::LightContainmentZoneDecontamination.DecontaminationController.DecontaminationStatus>.read = _Read_LightContainmentZoneDecontamination_002EDecontaminationController_002FDecontaminationStatus;
			global::Mirror.Reader<global::InventorySystem.Items.ItemIdentifier[]>.read = _Read_InventorySystem_002EItems_002EItemIdentifier_005B_005D;
			global::Mirror.Reader<global::InventorySystem.Items.ItemIdentifier>.read = _Read_InventorySystem_002EItems_002EItemIdentifier;
			global::Mirror.Reader<ItemType>.read = _Read_ItemType;
			global::Mirror.Reader<ushort[]>.read = _Read_System_002EUInt16_005B_005D;
			global::Mirror.Reader<ActionName>.read = _Read_ActionName;
			global::Mirror.Reader<global::InventorySystem.Items.Usables.Scp330.CandyKindID>.read = _Read_InventorySystem_002EItems_002EUsables_002EScp330_002ECandyKindID;
			global::Mirror.Reader<global::InventorySystem.Items.Firearms.FirearmStatus>.read = _Read_InventorySystem_002EItems_002EFirearms_002EFirearmStatus;
			global::Mirror.Reader<global::InventorySystem.Items.Firearms.FirearmStatusFlags>.read = _Read_InventorySystem_002EItems_002EFirearms_002EFirearmStatusFlags;
			global::Mirror.Reader<global::UnityEngine.PrimitiveType>.read = _Read_UnityEngine_002EPrimitiveType;
			global::Mirror.Reader<global::Scp914.Scp914KnobSetting>.read = _Read_Scp914_002EScp914KnobSetting;
		}
	}
}
