
namespace Pug.Graph;

#if NET5_0_OR_GREATER

public record NodeProcessingResult<TResult>( TResult? Result, bool TraverseNode );

#else

	public record NodeProcessingResult<TResult>
	{
		public TResult? Result
		{
			get;
			set;
		}

		public bool TraverseNode
		{
			get;
			set;
		}
	}

#endif