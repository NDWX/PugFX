namespace Pug.Graph;

public interface INodeTraversalStrategy<in TNode>
{
	void AddTraversed( TNode node );

	bool ShouldSkip( TNode node );
}