using Microsoft.CodeAnalysis;
using System;

namespace DocumentationGenerator
{

    public interface INodeInspectionHandler
    {
        Func<Compilation, SyntaxTree, SyntaxNode, IterationDirection> InspectNode { get; }
        Type HandlerType { get; }
    }

    public class NodeInspectionHandler<T> : INodeInspectionHandler where T : SyntaxNode
    {
        public Func<Compilation, SyntaxTree, T, IterationDirection> InspectNode;
        public Type HandlerType => typeof(T);

        Func<Compilation, SyntaxTree, SyntaxNode, IterationDirection> INodeInspectionHandler.InspectNode => (c, t, n) => { return InspectNode(c, t, n as T); };

        public NodeInspectionHandler(Func<Compilation, SyntaxTree, T, IterationDirection> inspectNode)
        {
            InspectNode = inspectNode;
        }
    }

    public enum IterationDirection
    {
        SkipChildren,
        IterateChildren,
    }
}
