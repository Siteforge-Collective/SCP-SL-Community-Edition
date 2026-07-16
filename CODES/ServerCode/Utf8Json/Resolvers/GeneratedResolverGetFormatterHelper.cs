namespace Utf8Json.Resolvers
{
	internal static class GeneratedResolverGetFormatterHelper
	{
		private static readonly global::System.Collections.Generic.Dictionary<global::System.Type, int> lookup;

		static GeneratedResolverGetFormatterHelper()
		{
			lookup = new global::System.Collections.Generic.Dictionary<global::System.Type, int>(32)
			{
				{
					typeof(ServerListItem[]),
					0
				},
				{
					typeof(global::System.Collections.Generic.List<string>),
					1
				},
				{
					typeof(NewsListItem[]),
					2
				},
				{
					typeof(DiscordEmbedField[]),
					3
				},
				{
					typeof(DiscordEmbed[]),
					4
				},
				{
					typeof(global::System.Collections.Generic.List<global::Authenticator.AuthenticatorPlayerObject>),
					5
				},
				{
					typeof(CreditsListMember[]),
					6
				},
				{
					typeof(CreditsListCategory[]),
					7
				},
				{
					typeof(AuthenticatiorAuthReject[]),
					8
				},
				{
					typeof(ServerListItem),
					9
				},
				{
					typeof(ServerList),
					10
				},
				{
					typeof(PlayerListSerialized),
					11
				},
				{
					typeof(NewsListItem),
					12
				},
				{
					typeof(NewsList),
					13
				},
				{
					typeof(DiscordEmbedField),
					14
				},
				{
					typeof(DiscordEmbed),
					15
				},
				{
					typeof(DiscordWebhook),
					16
				},
				{
					typeof(global::Authenticator.AuthenticatorPlayerObject),
					17
				},
				{
					typeof(global::Authenticator.AuthenticatorPlayerObjects),
					18
				},
				{
					typeof(CreditsListMember),
					19
				},
				{
					typeof(CreditsListCategory),
					20
				},
				{
					typeof(CreditsList),
					21
				},
				{
					typeof(AuthenticateResponse),
					22
				},
				{
					typeof(AuthenticatiorAuthReject),
					23
				},
				{
					typeof(AuthenticatorResponse),
					24
				},
				{
					typeof(NewsRaw),
					25
				},
				{
					typeof(PublicKeyResponse),
					26
				},
				{
					typeof(QueryRaReply),
					27
				},
				{
					typeof(RenewResponse),
					28
				},
				{
					typeof(ServerListSigned),
					29
				},
				{
					typeof(TranslationManifest),
					30
				},
				{
					typeof(RequestSignatureResponse),
					31
				}
			};
		}

		internal static object GetFormatter(global::System.Type t)
		{
			if (!lookup.TryGetValue(t, out var value))
			{
				return null;
			}
			switch (value)
			{
			case 0:
				return new global::Utf8Json.Formatters.ArrayFormatter<ServerListItem>();
			case 1:
				return new global::Utf8Json.Formatters.ListFormatter<string>();
			case 2:
				return new global::Utf8Json.Formatters.ArrayFormatter<NewsListItem>();
			case 3:
				return new global::Utf8Json.Formatters.ArrayFormatter<DiscordEmbedField>();
			case 4:
				return new global::Utf8Json.Formatters.ArrayFormatter<DiscordEmbed>();
			case 5:
				return new global::Utf8Json.Formatters.ListFormatter<global::Authenticator.AuthenticatorPlayerObject>();
			case 6:
				return new global::Utf8Json.Formatters.ArrayFormatter<CreditsListMember>();
			case 7:
				return new global::Utf8Json.Formatters.ArrayFormatter<CreditsListCategory>();
			case 8:
				return new global::Utf8Json.Formatters.ArrayFormatter<AuthenticatiorAuthReject>();
			case 9:
				return new global::Utf8Json.Formatters.ServerListItemFormatter();
			case 10:
				return new global::Utf8Json.Formatters.ServerListFormatter();
			case 11:
				return new global::Utf8Json.Formatters.PlayerListSerializedFormatter();
			case 12:
				return new global::Utf8Json.Formatters.NewsListItemFormatter();
			case 13:
				return new global::Utf8Json.Formatters.NewsListFormatter();
			case 14:
				return new global::Utf8Json.Formatters.DiscordEmbedFieldFormatter();
			case 15:
				return new global::Utf8Json.Formatters.DiscordEmbedFormatter();
			case 16:
				return new global::Utf8Json.Formatters.DiscordWebhookFormatter();
			case 17:
				return new global::Utf8Json.Formatters.Authenticator.AuthenticatorPlayerObjectFormatter();
			case 18:
				return new global::Utf8Json.Formatters.Authenticator.AuthenticatorPlayerObjectsFormatter();
			case 19:
				return new global::Utf8Json.Formatters.CreditsListMemberFormatter();
			case 20:
				return new global::Utf8Json.Formatters.CreditsListCategoryFormatter();
			case 21:
				return new global::Utf8Json.Formatters.CreditsListFormatter();
			case 22:
				return new global::Utf8Json.Formatters.AuthenticateResponseFormatter();
			case 23:
				return new global::Utf8Json.Formatters.AuthenticatiorAuthRejectFormatter();
			case 24:
				return new global::Utf8Json.Formatters.AuthenticatorResponseFormatter();
			case 25:
				return new global::Utf8Json.Formatters.NewsRawFormatter();
			case 26:
				return new global::Utf8Json.Formatters.PublicKeyResponseFormatter();
			case 27:
				return new global::Utf8Json.Formatters.QueryRaReplyFormatter();
			case 28:
				return new global::Utf8Json.Formatters.RenewResponseFormatter();
			case 29:
				return new global::Utf8Json.Formatters.ServerListSignedFormatter();
			case 30:
				return new global::Utf8Json.Formatters.TranslationManifestFormatter();
			case 31:
				return new global::Utf8Json.Formatters.RequestSignatureResponseFormatter();
			default:
				return null;
			}
		}
	}
}
