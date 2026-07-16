namespace Utf8Json.Internal.Emit
{
	internal struct ArgumentField
	{
		private readonly int i;

		private readonly bool @ref;

		private readonly global::System.Reflection.Emit.ILGenerator il;

		public ArgumentField(global::System.Reflection.Emit.ILGenerator il, int i, bool @ref = false)
		{
			this.il = il;
			this.i = i;
			this.@ref = @ref;
		}

		public ArgumentField(global::System.Reflection.Emit.ILGenerator il, int i, global::System.Type type)
		{
			this.il = il;
			this.i = i;
			@ref = ((!type.IsClass && !type.IsInterface && !type.IsAbstract) ? true : false);
		}

		public void EmitLoad()
		{
			if (@ref)
			{
				il.EmitLdarga(i);
			}
			else
			{
				il.EmitLdarg(i);
			}
		}

		public void EmitStore()
		{
			il.EmitStarg(i);
		}
	}
}
