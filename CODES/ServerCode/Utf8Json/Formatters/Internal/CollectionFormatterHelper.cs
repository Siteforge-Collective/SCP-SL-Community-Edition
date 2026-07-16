namespace Utf8Json.Formatters.Internal
{
	internal static class CollectionFormatterHelper
	{
		internal static readonly byte[][] groupingName;

		internal static readonly global::Utf8Json.Internal.AutomataDictionary groupingAutomata;

		static CollectionFormatterHelper()
		{
			groupingName = new byte[2][]
			{
				global::Utf8Json.JsonWriter.GetEncodedPropertyNameWithBeginObject("Key"),
				global::Utf8Json.JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("Elements")
			};
			groupingAutomata = new global::Utf8Json.Internal.AutomataDictionary
			{
				{
					global::Utf8Json.JsonWriter.GetEncodedPropertyNameWithoutQuotation("Key"),
					0
				},
				{
					global::Utf8Json.JsonWriter.GetEncodedPropertyNameWithoutQuotation("Elements"),
					1
				}
			};
		}
	}
}
