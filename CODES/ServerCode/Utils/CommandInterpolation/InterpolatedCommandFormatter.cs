namespace Utils.CommandInterpolation
{
	public class InterpolatedCommandFormatter
	{
		private class InterpolatedCommandFormatterContext
		{
			public global::System.Collections.Generic.List<string> Arguments { get; }

			public global::System.Text.StringBuilder Builder { get; }

			public InterpolatedCommandFormatterContext()
			{
				Arguments = new global::System.Collections.Generic.List<string>();
				Builder = new global::System.Text.StringBuilder();
			}

			public void Clear()
			{
				Arguments.Clear();
				Builder.Clear();
			}
		}

		private readonly global::System.Collections.Generic.Stack<global::Utils.CommandInterpolation.InterpolatedCommandFormatter.InterpolatedCommandFormatterContext> availableContexts;

		private global::System.Collections.Generic.Dictionary<string, global::System.Func<global::System.Collections.Generic.List<string>, string>> commands;

		public char StartClosure { get; set; }

		public char EndClosure { get; set; }

		public char Escape { get; set; }

		public char ArgumentSplitter { get; set; }

		public global::System.Collections.Generic.Dictionary<string, global::System.Func<global::System.Collections.Generic.List<string>, string>> Commands
		{
			get
			{
				return commands;
			}
			set
			{
				commands = value ?? throw new global::System.ArgumentNullException("value");
			}
		}

		public InterpolatedCommandFormatter(int initialContexts = 4)
		{
			availableContexts = new global::System.Collections.Generic.Stack<global::Utils.CommandInterpolation.InterpolatedCommandFormatter.InterpolatedCommandFormatterContext>();
			for (int i = 0; i < initialContexts; i++)
			{
				availableContexts.Push(new global::Utils.CommandInterpolation.InterpolatedCommandFormatter.InterpolatedCommandFormatterContext());
			}
			Commands = new global::System.Collections.Generic.Dictionary<string, global::System.Func<global::System.Collections.Generic.List<string>, string>>();
		}

		private void ProcessInterpolation(string raw, global::Utils.CommandInterpolation.InterpolatedCommandFormatter.InterpolatedCommandFormatterContext context)
		{
			bool flag = false;
			int num = 0;
			int num2 = 0;
			for (int i = 0; i < raw.Length; i++)
			{
				char c = raw[i];
				if (c == Escape && !flag)
				{
					flag = true;
				}
				else if (flag)
				{
					flag = false;
					switch (c)
					{
					case 'n':
						context.Builder.Append('\n');
						continue;
					case '\\':
						context.Builder.Append('\\');
						continue;
					}
					if (c != StartClosure && c != EndClosure && c != ArgumentSplitter && c != Escape)
					{
						throw new global::System.InvalidOperationException($"Unrecognized escape character at column {i}.");
					}
				}
				else if (c == StartClosure)
				{
					if (num++ == 0)
					{
						num2 = i + 1;
					}
				}
				else if (c == EndClosure)
				{
					if (num-- == 0)
					{
						throw new global::System.InvalidOperationException($"Unmatched closing character at column {i}.");
					}
					if (num != 0)
					{
						continue;
					}
					string processed = raw.Substring(num2, i - num2);
					if (!ProcessInterpolatedCommand(processed, context.Arguments, out processed))
					{
						throw new global::System.InvalidOperationException(string.Format("Invalid command at column {0}: {1}", num2, string.Join(", ", global::System.Linq.Enumerable.Select(context.Arguments, (string x) => "\"" + x + "\""))));
					}
					context.Arguments.Clear();
					context.Builder.Append(processed);
				}
				else if (num == 0)
				{
					context.Builder.Append(c);
				}
			}
			if (flag)
			{
				throw new global::System.InvalidOperationException("Unable to end string with an escape character.");
			}
			if (num > 0)
			{
				throw new global::System.InvalidOperationException("Unmatched opening character(s).");
			}
		}

		private bool ProcessInterpolatedCommand(string raw, global::System.Collections.Generic.List<string> argumentBuffer, out string processed)
		{
			ProcessArguments(raw, argumentBuffer);
			string key = argumentBuffer[0];
			if (Commands.TryGetValue(key, out var value))
			{
				processed = value(argumentBuffer);
				return true;
			}
			processed = null;
			return false;
		}

		private void ProcessArguments(string raw, global::System.Collections.Generic.ICollection<string> arguments)
		{
			int num = 0;
			int num2 = 0;
			bool flag = false;
			for (int i = 0; i < raw.Length; i++)
			{
				char c = raw[i];
				if (flag)
				{
					flag = false;
				}
				else if (c == Escape)
				{
					flag = true;
				}
				else if (c == StartClosure)
				{
					num2++;
				}
				else if (c == EndClosure)
				{
					num2--;
				}
				else if (c == ArgumentSplitter && num2 == 0)
				{
					arguments.Add(raw.Substring(num, i - num));
					num = i + 1;
				}
			}
			arguments.Add(raw.Substring(num, raw.Length - num));
		}

		private global::Utils.CommandInterpolation.InterpolatedCommandFormatter.InterpolatedCommandFormatterContext SafePopContext()
		{
			if (availableContexts.Count != 0)
			{
				return availableContexts.Pop();
			}
			return new global::Utils.CommandInterpolation.InterpolatedCommandFormatter.InterpolatedCommandFormatterContext();
		}

		public bool TryProcessExpression(string raw, string source, out string result)
		{
			bool result2 = false;
			try
			{
				result = ProcessExpression(raw);
				result2 = true;
			}
			catch (global::System.InvalidOperationException ex)
			{
				result = "Command interpolation (" + source + ") threw an error: " + ex.Message;
			}
			catch (global::Utils.CommandInterpolation.CommandInputException ex2)
			{
				string text = ((ex2.ArgumentValue is global::System.Collections.Generic.IEnumerable<object> source2) ? string.Join(", ", global::System.Linq.Enumerable.Select(source2, (object x) => $"\"{x}\"")) : ex2.ArgumentValue.ToString());
				result = "A command errored in " + source + " command interpolation: " + ex2.Message + "\nArgument name: " + ex2.ArgumentName + "\nArgument value: " + text;
			}
			return result2;
		}

		public string ProcessExpression(string raw)
		{
			global::Utils.CommandInterpolation.InterpolatedCommandFormatter.InterpolatedCommandFormatterContext interpolatedCommandFormatterContext = SafePopContext();
			ProcessInterpolation(raw, interpolatedCommandFormatterContext);
			string result = interpolatedCommandFormatterContext.Builder.ToString();
			interpolatedCommandFormatterContext.Clear();
			availableContexts.Push(interpolatedCommandFormatterContext);
			return result;
		}
	}
}
