using System;

namespace TranslatorLib
{
    public abstract class TranslatorException : Exception
    {
        public string? Expected { get; init; }

        protected TranslatorException(string? message) : base(message)
        {
        }

        protected TranslatorException(string? message, string expected) : base(message)
        {
            Expected = expected;
        }
    }

    public class LexerException : TranslatorException
    {
        public Liter Liter { get; init; }

        public LexerException(Liter liter)
            : base($"Invalid character '{liter.SanitizedCharacter}' at {liter.Location}.")
        {
            Liter = liter;
        }

        public LexerException(Liter liter, string expected)
            : base($"Invalid character '{liter.SanitizedCharacter}' at {liter.Location}, {expected}.", expected)
        {
            Liter = liter;
        }

        public bool EndOfFile => Liter.IsLast();
    }

    public class SyntaxAnalyzerException : TranslatorException
    {
        public Token Token { get; init; }

        public SyntaxAnalyzerException(Token token)
            : base($"Invalid syntax for C at token '{token.SanitizedContent}' at {token.Location}.")
        {
            Token = token;
        }

        public SyntaxAnalyzerException(Token token, string expected)
            : base($"Invalid syntax for C, expected {expected}, actually '{token.SanitizedContent}' at {token.Location}.", expected)
        {
            Token = token;
            Expected = expected;
        }

        public bool EndOfFile => Token.IsLast();
    }

    public class ContextAnalyzerException : TranslatorException
    {
        public SyntaxTreeNode SyntaxTreeNode { get; init; }

        public ContextAnalyzerException(SyntaxTreeNode syntaxTreeNode) : base($"Invalid expression at {syntaxTreeNode}.")
        {
            SyntaxTreeNode = syntaxTreeNode;
        }

        public ContextAnalyzerException(SyntaxTreeNode syntaxTreeNode, string reason) : base($"Invalid expression at {syntaxTreeNode}, by reason {reason}.")
        {
            SyntaxTreeNode = syntaxTreeNode;
        }
    }
}
