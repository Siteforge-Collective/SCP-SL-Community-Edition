namespace Utf8Json.Formatters.Internal
{
	internal static class StandardClassLibraryFormatterHelper
	{
		internal static readonly byte[][] keyValuePairName;

		internal static readonly global::Utf8Json.Internal.AutomataDictionary keyValuePairAutomata;

		static StandardClassLibraryFormatterHelper()
		{
			keyValuePairName = new byte[2][]
			{
				global::Utf8Json.JsonWriter.GetEncodedPropertyNameWithBeginObject("Key"),
				global::Utf8Json.JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("Value")
			};
			keyValuePairAutomata = new global::Utf8Json.Internal.AutomataDictionary
			{
				{
					global::Utf8Json.JsonWriter.GetEncodedPropertyNameWithoutQuotation("Key"),
					0
				},
				{
					global::Utf8Json.JsonWriter.GetEncodedPropertyNameWithoutQuotation("Value"),
					1
				}
			};
		}
	}
}
