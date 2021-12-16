using System;
using System.Linq;

namespace TranslatorLib
{
    public class ContextNode : ITreeElement<ContextNode>
    {
        public SyntaxTreeNode UnderlyingSyntaxNode { get; init; }

        public ContextNode[] Childs { get; init; }

        public ContextNode? Parent { get; private set; }

        public ContextNode(SyntaxTreeNode underlyingSyntaxNode, ContextNode[] childs)
        {
            UnderlyingSyntaxNode = underlyingSyntaxNode;
            Childs = childs;
            foreach (var child in Childs)
            {
                child.Parent = this;
            }
        }

        public override string ToString()
        {
            return $"{UnderlyingSyntaxNode.Name}: {UnderlyingSyntaxNode.UnderlyingToken?.ToString()} ({Childs.Length})";
        }
    }

    public class ContextAnalyzer : ITreeStage<ContextNode>
    {
        private SyntaxAnalyzer SyntaxAnalyzer { get; init; }
        private SyntaxTreeNode SyntaxTreeRoot => SyntaxAnalyzer.RootElement;

        private ContextNode? TreeNodeRoot;

        private HashTableList HashTableList { get; init; }

        public ContextNode RootElement => TreeNodeRoot ?? throw new InvalidOperationException("Invalid operation performed: no available current element before TakeElement call.");

        public ContextAnalyzer(SyntaxAnalyzer syntaxAnalyzer, HashTableList hashTableList)
        {
            SyntaxAnalyzer = syntaxAnalyzer;
            HashTableList = hashTableList;
        }

        public ContextNode TakeTreeRootElement()
        {
            return TreeNodeRoot = ProcessNode(SyntaxAnalyzer.TakeTreeRootElement());
        }

        private ContextNode ProcessNode(SyntaxTreeNode syntaxNode)
        {
            Token? token = syntaxNode.UnderlyingToken;
            if (token != null)
            {
                switch (token.Type)
                {
                    case TokenType.IdentifierToken:
                        bool preexisted = !HashTableList.SecondaryHashTable.TryAddItem(token.Content, token);
                        if (preexisted)
                        {
                            throw new ContextAnalyzerException(syntaxNode, $"this identifier (\"{token.Content}\") already exist");
                        }
                        break;
                    case TokenType.ConstantToken:
                        HashTableList.PrimaryHashTable.TryAddItem(token.Content, token);
                        break;
                    case TokenType.SpecialToken:
                        HashTableList.SignHashTable.TryAddItem(token.Content, token);
                        break;
                }
            }

            var newChilds = from child in syntaxNode.Childs select ProcessNode(child);
            return new ContextNode(syntaxNode, newChilds.ToArray());
        }
    }
}
