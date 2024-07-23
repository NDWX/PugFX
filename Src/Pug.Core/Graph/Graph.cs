using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pug.Graph;

// ReSharper disable once UnusedType.Global
public static class Graph
{
   private static TResult? DefaultResultAggregator<TResult, TContext, TNode>(
      NodeProcessingResult<TResult> r, TResult? result1,
      NodeProcessingContext<TContext, TNode> nodeProcessingContext ) => r.Result;

   private static bool DefaultStopCondition<TResult, TContext>( TResult? result, TContext context ) => false;

   public static async Task<TResult> TraverseAsync<TNode, TContext, TResult>(
      this TNode node,
      Func<TNode, TContext, Task<IEnumerable<TNode>>> childNodesFinder,
      INodeTraversalStrategy<TNode> traversalStrategy,
      TContext context,
      Func<TNode, NodeProcessingContext<TContext, TNode>, Task<NodeProcessingResult<TResult>>> process,
      Func<NodeProcessingResult<TResult>, TResult?, NodeProcessingContext<TContext, TNode>, TResult?>?
         resultAggregator = null,
      Func<TResult?, TContext, bool>? stopCondition = null )
   {
      resultAggregator ??= DefaultResultAggregator;

      stopCondition ??= DefaultStopCondition;

      TResult? result = default(TResult);

      IEnumerable<TNode> childNodes = new[] { node };

      while( childNodes.Any() )
      {
         List<TNode> accumulatedChildNodes = [];

         foreach( TNode childNode in childNodes )
         {
            if( traversalStrategy.ShouldSkip( childNode ) )
               continue;

            IEnumerable<TNode> foundChildNodes = await childNodesFinder( childNode, context );

            int childNodeIndex = -1;

            foreach( TNode foundChildNode in foundChildNodes )
            {
               childNodeIndex++;

               NodeProcessingContext<TContext, TNode> processingContext =
#if NET5_0_OR_GREATER
                  new ( context, childNode, foundChildNodes, childNodeIndex );
#else
                  new ()
                  {
                     DigContext = context,
                     Parent = childNode,
                     ParentChildNodes = foundChildNodes,
                     NodeIndex = childNodeIndex
                  };
#endif
               NodeProcessingResult<TResult> processingResult =
                  await process( foundChildNode, processingContext );

               if( processingResult.TraverseNode )
                  accumulatedChildNodes.Add( foundChildNode );

               result = resultAggregator( processingResult, result, processingContext );

               if( stopCondition( result, context ) )
                  return result;
            }

            traversalStrategy.AddTraversed( childNode );
         }

         childNodes = accumulatedChildNodes;
      }

      return result;
   }
}