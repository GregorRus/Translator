using System.Linq;
using System.Text;

namespace TranslatorLib
{
    public class Generator
    {
        private ContextAnalyzer ContextAnalyzer { get; init; }
        private ContextNode ContextTreeRoot => ContextAnalyzer.RootElement;

        public Generator(ContextAnalyzer contextAnalyzer)
        {
            ContextAnalyzer = contextAnalyzer;
        }

        public string GetResult()
        {
            StringBuilder stringBuilder = new();
            ContextNode root = ContextAnalyzer.TakeTreeRootElement();

            ProcessNode(stringBuilder, root);

            return stringBuilder.ToString();
        }

        private void ProcessNode(StringBuilder stringBuilder, ContextNode node, int level = 0)
        {
            if (node.HasSingleToken || node.Name == "O")
            {
                for (int i = 1; i < level; ++i)
                {
                    stringBuilder.Append("  ");
                }
                if (!node.HasSingleToken)
                {
                    stringBuilder.Append("  ");
                }
                stringBuilder.AppendLine(FormatNodeToString(node));
            }
            else
            {
                if (node.Name == "A")
                {
                    if (node.Childs.FirstOrDefault()?.Name == "Negation")
                    {
                        ++level;
                    }
                    if (node.Childs.FirstOrDefault()?.Name == "OpeningBrackets")
                    {
                        --level;
                    }
                }
                if (node.Childs.Any(child => child.HasSingleToken))
                {
                    ++level;
                }
                foreach (var child in node.Childs)
                {
                    ProcessNode(stringBuilder, child, level);
                }
            }
        }

        private static string FormatNodeToString(ContextNode node)
        {
            ContextNode[] childs = node.Childs;
            SyntaxTreeNode syntaxNode = node.UnderlyingSyntaxNode;
            Token? token = syntaxNode.UnderlyingToken;
            Token? childToken = syntaxNode.Childs.FirstOrDefault()?.UnderlyingToken;

            if (token?.Type == TokenType.SpecialToken)
            {
                return token.Content switch
                {
                    "¬" => "Не",
                    "∧" => "И",
                    "∨" => "Или",
                    _ => token.Content
                };
            }

            switch (syntaxNode.Name)
            {
                case "O":
                    return $"{childs[0].Value} = {childs[2].Value}";
            }
            return $"Узел {syntaxNode.Name}";
        }
    }
}
