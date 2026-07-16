namespace Serialization
{
	public class CommentsObjectGraphVisitor : global::YamlDotNet.Serialization.ObjectGraphVisitors.ChainedObjectGraphVisitor
	{
		public CommentsObjectGraphVisitor(global::YamlDotNet.Serialization.IObjectGraphVisitor<global::YamlDotNet.Core.IEmitter> nextVisitor)
			: base(nextVisitor)
		{
		}

		public override bool EnterMapping(global::YamlDotNet.Serialization.IPropertyDescriptor key, global::YamlDotNet.Serialization.IObjectDescriptor value, global::YamlDotNet.Core.IEmitter context)
		{
			if (value is global::Serialization.CommentsObjectDescriptor commentsObjectDescriptor && commentsObjectDescriptor.Comment != null)
			{
				context.Emit(new global::YamlDotNet.Core.Events.Comment(commentsObjectDescriptor.Comment, isInline: false));
			}
			return base.EnterMapping(key, value, context);
		}
	}
}
