namespace Utf8Json.Internal.Emit
{
	internal class InnerExceptionMetaMember : global::Utf8Json.Internal.Emit.MetaMember
	{
		private static readonly global::System.Reflection.MethodInfo getInnerException = global::Utf8Json.Internal.Emit.ExpressionUtility.GetPropertyInfo((global::System.Exception ex) => ex.InnerException).GetGetMethod();

		private static readonly global::System.Reflection.MethodInfo nongenericSerialize = global::Utf8Json.Internal.Emit.ExpressionUtility.GetMethodInfo((global::Utf8Json.JsonWriter writer) => global::Utf8Json.JsonSerializer.NonGeneric.Serialize(ref writer, null, null));

		internal global::Utf8Json.Internal.Emit.ArgumentField argWriter;

		internal global::Utf8Json.Internal.Emit.ArgumentField argValue;

		internal global::Utf8Json.Internal.Emit.ArgumentField argResolver;

		public InnerExceptionMetaMember(string name)
			: base(typeof(global::System.Exception), name, name, isWritable: false, isReadable: true)
		{
		}

		public override void EmitLoadValue(global::System.Reflection.Emit.ILGenerator il)
		{
			il.Emit(global::System.Reflection.Emit.OpCodes.Callvirt, getInnerException);
		}

		public override void EmitStoreValue(global::System.Reflection.Emit.ILGenerator il)
		{
			throw new global::System.NotSupportedException();
		}

		public void EmitSerializeDirectly(global::System.Reflection.Emit.ILGenerator il)
		{
			argWriter.EmitLoad();
			argValue.EmitLoad();
			il.Emit(global::System.Reflection.Emit.OpCodes.Callvirt, getInnerException);
			argResolver.EmitLoad();
			il.EmitCall(nongenericSerialize);
		}
	}
}
