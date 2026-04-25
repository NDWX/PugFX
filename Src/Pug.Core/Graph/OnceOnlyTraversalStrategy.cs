using System.Collections.Generic;

namespace Pug.Graph;

public class OnceOnlyTraversalStrategy<TNode> : INodeTraversalStrategy<TNode>
{
	private readonly IList<TNode> traversedNodes = new List<TNode>();

	public virtual void AddTraversed( TNode node )
	{
		traversedNodes.Add( node );
	}

	public virtual bool ShouldSkip( TNode node )
	{
		return traversedNodes.Contains( node );
	}
}