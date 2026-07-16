namespace Utf8Json.Internal.Emit
{
	internal static class ILGeneratorExtensions
	{
		public static void EmitLdloc(this global::System.Reflection.Emit.ILGenerator il, int index)
		{
			switch (index)
			{
			case 0:
				il.Emit(global::System.Reflection.Emit.OpCodes.Ldloc_0);
				return;
			case 1:
				il.Emit(global::System.Reflection.Emit.OpCodes.Ldloc_1);
				return;
			case 2:
				il.Emit(global::System.Reflection.Emit.OpCodes.Ldloc_2);
				return;
			case 3:
				il.Emit(global::System.Reflection.Emit.OpCodes.Ldloc_3);
				return;
			}
			if (index <= 255)
			{
				il.Emit(global::System.Reflection.Emit.OpCodes.Ldloc_S, (byte)index);
			}
			else
			{
				il.Emit(global::System.Reflection.Emit.OpCodes.Ldloc, (short)index);
			}
		}

		public static void EmitLdloc(this global::System.Reflection.Emit.ILGenerator il, global::System.Reflection.Emit.LocalBuilder local)
		{
			il.EmitLdloc(local.LocalIndex);
		}

		public static void EmitStloc(this global::System.Reflection.Emit.ILGenerator il, int index)
		{
			switch (index)
			{
			case 0:
				il.Emit(global::System.Reflection.Emit.OpCodes.Stloc_0);
				return;
			case 1:
				il.Emit(global::System.Reflection.Emit.OpCodes.Stloc_1);
				return;
			case 2:
				il.Emit(global::System.Reflection.Emit.OpCodes.Stloc_2);
				return;
			case 3:
				il.Emit(global::System.Reflection.Emit.OpCodes.Stloc_3);
				return;
			}
			if (index <= 255)
			{
				il.Emit(global::System.Reflection.Emit.OpCodes.Stloc_S, (byte)index);
			}
			else
			{
				il.Emit(global::System.Reflection.Emit.OpCodes.Stloc, (short)index);
			}
		}

		public static void EmitStloc(this global::System.Reflection.Emit.ILGenerator il, global::System.Reflection.Emit.LocalBuilder local)
		{
			il.EmitStloc(local.LocalIndex);
		}

		public static void EmitLdloca(this global::System.Reflection.Emit.ILGenerator il, int index)
		{
			if (index <= 255)
			{
				il.Emit(global::System.Reflection.Emit.OpCodes.Ldloca_S, (byte)index);
			}
			else
			{
				il.Emit(global::System.Reflection.Emit.OpCodes.Ldloca, (short)index);
			}
		}

		public static void EmitLdloca(this global::System.Reflection.Emit.ILGenerator il, global::System.Reflection.Emit.LocalBuilder local)
		{
			il.EmitLdloca(local.LocalIndex);
		}

		public static void EmitTrue(this global::System.Reflection.Emit.ILGenerator il)
		{
			il.EmitBoolean(value: true);
		}

		public static void EmitFalse(this global::System.Reflection.Emit.ILGenerator il)
		{
			il.EmitBoolean(value: false);
		}

		public static void EmitBoolean(this global::System.Reflection.Emit.ILGenerator il, bool value)
		{
			il.EmitLdc_I4(value ? 1 : 0);
		}

		public static void EmitLdc_I4(this global::System.Reflection.Emit.ILGenerator il, int value)
		{
			switch (value)
			{
			case -1:
				il.Emit(global::System.Reflection.Emit.OpCodes.Ldc_I4_M1);
				return;
			case 0:
				il.Emit(global::System.Reflection.Emit.OpCodes.Ldc_I4_0);
				return;
			case 1:
				il.Emit(global::System.Reflection.Emit.OpCodes.Ldc_I4_1);
				return;
			case 2:
				il.Emit(global::System.Reflection.Emit.OpCodes.Ldc_I4_2);
				return;
			case 3:
				il.Emit(global::System.Reflection.Emit.OpCodes.Ldc_I4_3);
				return;
			case 4:
				il.Emit(global::System.Reflection.Emit.OpCodes.Ldc_I4_4);
				return;
			case 5:
				il.Emit(global::System.Reflection.Emit.OpCodes.Ldc_I4_5);
				return;
			case 6:
				il.Emit(global::System.Reflection.Emit.OpCodes.Ldc_I4_6);
				return;
			case 7:
				il.Emit(global::System.Reflection.Emit.OpCodes.Ldc_I4_7);
				return;
			case 8:
				il.Emit(global::System.Reflection.Emit.OpCodes.Ldc_I4_8);
				return;
			}
			if (value >= -128 && value <= 127)
			{
				il.Emit(global::System.Reflection.Emit.OpCodes.Ldc_I4_S, (sbyte)value);
			}
			else
			{
				il.Emit(global::System.Reflection.Emit.OpCodes.Ldc_I4, value);
			}
		}

		public static void EmitUnboxOrCast(this global::System.Reflection.Emit.ILGenerator il, global::System.Type type)
		{
			if (type.IsValueType)
			{
				il.Emit(global::System.Reflection.Emit.OpCodes.Unbox_Any, type);
			}
			else
			{
				il.Emit(global::System.Reflection.Emit.OpCodes.Castclass, type);
			}
		}

		public static void EmitBoxOrDoNothing(this global::System.Reflection.Emit.ILGenerator il, global::System.Type type)
		{
			if (type.IsValueType)
			{
				il.Emit(global::System.Reflection.Emit.OpCodes.Box, type);
			}
		}

		public static void EmitLdarg(this global::System.Reflection.Emit.ILGenerator il, int index)
		{
			switch (index)
			{
			case 0:
				il.Emit(global::System.Reflection.Emit.OpCodes.Ldarg_0);
				return;
			case 1:
				il.Emit(global::System.Reflection.Emit.OpCodes.Ldarg_1);
				return;
			case 2:
				il.Emit(global::System.Reflection.Emit.OpCodes.Ldarg_2);
				return;
			case 3:
				il.Emit(global::System.Reflection.Emit.OpCodes.Ldarg_3);
				return;
			}
			if (index <= 255)
			{
				il.Emit(global::System.Reflection.Emit.OpCodes.Ldarg_S, (byte)index);
			}
			else
			{
				il.Emit(global::System.Reflection.Emit.OpCodes.Ldarg, index);
			}
		}

		public static void EmitLoadThis(this global::System.Reflection.Emit.ILGenerator il)
		{
			il.EmitLdarg(0);
		}

		public static void EmitLdarga(this global::System.Reflection.Emit.ILGenerator il, int index)
		{
			if (index <= 255)
			{
				il.Emit(global::System.Reflection.Emit.OpCodes.Ldarga_S, (byte)index);
			}
			else
			{
				il.Emit(global::System.Reflection.Emit.OpCodes.Ldarga, index);
			}
		}

		public static void EmitStarg(this global::System.Reflection.Emit.ILGenerator il, int index)
		{
			if (index <= 255)
			{
				il.Emit(global::System.Reflection.Emit.OpCodes.Starg_S, (byte)index);
			}
			else
			{
				il.Emit(global::System.Reflection.Emit.OpCodes.Starg, index);
			}
		}

		public static void EmitPop(this global::System.Reflection.Emit.ILGenerator il, int count)
		{
			for (int i = 0; i < count; i++)
			{
				il.Emit(global::System.Reflection.Emit.OpCodes.Pop);
			}
		}

		public static void EmitCall(this global::System.Reflection.Emit.ILGenerator il, global::System.Reflection.MethodInfo methodInfo)
		{
			if (methodInfo.IsFinal || !methodInfo.IsVirtual)
			{
				il.Emit(global::System.Reflection.Emit.OpCodes.Call, methodInfo);
			}
			else
			{
				il.Emit(global::System.Reflection.Emit.OpCodes.Callvirt, methodInfo);
			}
		}

		public static void EmitLdfld(this global::System.Reflection.Emit.ILGenerator il, global::System.Reflection.FieldInfo fieldInfo)
		{
			il.Emit(global::System.Reflection.Emit.OpCodes.Ldfld, fieldInfo);
		}

		public static void EmitLdsfld(this global::System.Reflection.Emit.ILGenerator il, global::System.Reflection.FieldInfo fieldInfo)
		{
			il.Emit(global::System.Reflection.Emit.OpCodes.Ldsfld, fieldInfo);
		}

		public static void EmitRet(this global::System.Reflection.Emit.ILGenerator il)
		{
			il.Emit(global::System.Reflection.Emit.OpCodes.Ret);
		}

		public static void EmitIntZeroReturn(this global::System.Reflection.Emit.ILGenerator il)
		{
			il.EmitLdc_I4(0);
			il.Emit(global::System.Reflection.Emit.OpCodes.Ret);
		}

		public static void EmitNullReturn(this global::System.Reflection.Emit.ILGenerator il)
		{
			il.Emit(global::System.Reflection.Emit.OpCodes.Ldnull);
			il.Emit(global::System.Reflection.Emit.OpCodes.Ret);
		}

		public static void EmitULong(this global::System.Reflection.Emit.ILGenerator il, ulong value)
		{
			il.Emit(global::System.Reflection.Emit.OpCodes.Ldc_I8, (long)value);
		}

		public static void EmitThrowNotimplemented(this global::System.Reflection.Emit.ILGenerator il)
		{
			il.Emit(global::System.Reflection.Emit.OpCodes.Newobj, global::System.Linq.Enumerable.First(global::System.Reflection.IntrospectionExtensions.GetTypeInfo(typeof(global::System.NotImplementedException)).DeclaredConstructors, (global::System.Reflection.ConstructorInfo x) => x.GetParameters().Length == 0));
			il.Emit(global::System.Reflection.Emit.OpCodes.Throw);
		}

		public static void EmitIncrementFor(this global::System.Reflection.Emit.ILGenerator il, global::System.Reflection.Emit.LocalBuilder conditionGreater, global::System.Action<global::System.Reflection.Emit.LocalBuilder> emitBody)
		{
			global::System.Reflection.Emit.Label label = il.DefineLabel();
			global::System.Reflection.Emit.Label label2 = il.DefineLabel();
			global::System.Reflection.Emit.LocalBuilder localBuilder = il.DeclareLocal(typeof(int));
			il.EmitLdc_I4(0);
			il.EmitStloc(localBuilder);
			il.Emit(global::System.Reflection.Emit.OpCodes.Br, label2);
			il.MarkLabel(label);
			emitBody(localBuilder);
			il.EmitLdloc(localBuilder);
			il.EmitLdc_I4(1);
			il.Emit(global::System.Reflection.Emit.OpCodes.Add);
			il.EmitStloc(localBuilder);
			il.MarkLabel(label2);
			il.EmitLdloc(localBuilder);
			il.EmitLdloc(conditionGreater);
			il.Emit(global::System.Reflection.Emit.OpCodes.Blt, label);
		}
	}
}
