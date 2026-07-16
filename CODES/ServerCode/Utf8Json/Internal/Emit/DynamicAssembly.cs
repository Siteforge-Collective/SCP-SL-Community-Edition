namespace Utf8Json.Internal.Emit
{
	internal class DynamicAssembly
	{
		private readonly global::System.Reflection.Emit.AssemblyBuilder assemblyBuilder;

		private readonly global::System.Reflection.Emit.ModuleBuilder moduleBuilder;

		private readonly object gate = new object();

		public global::System.Reflection.Emit.TypeBuilder DefineType(string name, global::System.Reflection.TypeAttributes attr)
		{
			lock (gate)
			{
				return moduleBuilder.DefineType(name, attr);
			}
		}

		public global::System.Reflection.Emit.TypeBuilder DefineType(string name, global::System.Reflection.TypeAttributes attr, global::System.Type parent)
		{
			lock (gate)
			{
				return moduleBuilder.DefineType(name, attr, parent);
			}
		}

		public global::System.Reflection.Emit.TypeBuilder DefineType(string name, global::System.Reflection.TypeAttributes attr, global::System.Type parent, global::System.Type[] interfaces)
		{
			lock (gate)
			{
				return moduleBuilder.DefineType(name, attr, parent, interfaces);
			}
		}

		public DynamicAssembly(string moduleName)
		{
			assemblyBuilder = global::System.AppDomain.CurrentDomain.DefineDynamicAssembly(new global::System.Reflection.AssemblyName(moduleName), global::System.Reflection.Emit.AssemblyBuilderAccess.Run);
			moduleBuilder = assemblyBuilder.DefineDynamicModule(moduleName);
		}
	}
}
