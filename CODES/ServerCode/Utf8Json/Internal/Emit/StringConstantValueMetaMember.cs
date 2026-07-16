namespace Utf8Json.Internal.Emit
{
	internal class StringConstantValueMetaMember : global::Utf8Json.Internal.Emit.MetaMember
	{
		private readonly string constant;

		public StringConstantValueMetaMember(string name, string constant)
			: base(typeof(string), name, name, isWritable: false, isReadable: true)
		{
			this.constant = constant;
		}

		public override void EmitLoadValue(global::System.Reflection.Emit.ILGenerator il)
		{
			il.Emit(global::System.Reflection.Emit.OpCodes.Pop);
			il.Emit(global::System.Reflection.Emit.OpCodes.Ldstr, constant);
		}

		public override void EmitStoreValue(global::System.Reflection.Emit.ILGenerator il)
		{
			throw new global::System.NotSupportedException();
		}
	}
}
