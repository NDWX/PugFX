namespace Pug.Graph;

public class InclusiveTraversalStrategy<TNode> : INodeTraversalStrategy<TNode>
{
	public void AddTraversed( TNode node )
	{
	}

	public bool ShouldSkip( TNode node )
	{
		return true;
	}
}