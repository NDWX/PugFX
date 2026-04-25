using System.Collections.Generic;

#if NET5_0_OR_GREATER

public record NodeProcessingContext<TDigContext, TNode>(
    TDigContext DigContext,
    TNode Parent,
    IEnumerable<TNode> ParentChildNodes,
    int NodeIndex );

#else

public record NodeProcessingContext<TDigContext, TNode>
{
	public TDigContext DigContext
	{
		get;
		set;
	}

	public TNode Parent
	{
		get;
		set;
	}

	public IEnumerable<TNode> ParentChildNodes
	{
		get;
		set;
	}

	public int NodeIndex
	{
		get;
		set;
	}
}

#endif