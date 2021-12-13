/*
 *  MIT License
 *
 *  Copyright (c) 2021 Chibirev Grigoriy
 *
 *  Permission is hereby granted, free of charge, to any person obtaining a copy
 *  of this software and associated documentation files (the "Software"), to deal
 *  in the Software without restriction, including without limitation the rights
 *  to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 *  copies of the Software, and to permit persons to whom the Software is
 *  furnished to do so, subject to the following conditions:
 *
 *  The above copyright notice and this permission notice shall be included in all
 *  copies or substantial portions of the Software.
 *
 *  THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 *  IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 *  FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 *  AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 *  LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 *  OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
 *  SOFTWARE.
 */

using System;

namespace TranslatorLib
{
    public class SyntaxTreeNode : ITreeElement<SyntaxTreeNode>
    {
        public SyntaxTreeNode(string name, Token? token = null, params SyntaxTreeNode[] childs)
        {
            Name = name;
            Childs = childs;
            UnderlyingToken = token;

            foreach (var child in Childs)
            {
                if (child.Parent != null)
                {
                    throw new ArgumentException("Node already has parent");
                }
                child.Parent = this;
            }
        }

        public string Name { get; init; }

        public SyntaxTreeNode[] Childs { get; init; }

        public SyntaxTreeNode? Parent { get; private set; }

        public Token? UnderlyingToken { get; init; }

        public override string ToString()
        {
            return $"{Name}: {UnderlyingToken?.ToString()} ({Childs.Length})";
        }
    }

    public class SyntaxAnalyzer : ITreeStage<SyntaxTreeNode>
    {
        private readonly Lexer Lexer;

        private SyntaxTreeNode? TreeRootNode;

        public SyntaxTreeNode RootElement => TreeRootNode ?? throw new InvalidOperationException("Invalid operation performed: no available current element before TakeElement call.");

        public SyntaxAnalyzer(Lexer lexer)
        {
            Lexer = lexer;
        }

        public SyntaxTreeNode TakeTreeRootElement()
        {
            Lexer.TakeElement();
            return TreeRootNode = Gram_D();
        }

        private SyntaxTreeNode Gram_D()
        {
            SyntaxTreeNode gram_k = Gram_K();
            if (Lexer.CurrentElement.Type == TokenType.SpecialToken && Lexer.CurrentElement.Content == "∨")
            {
                SyntaxTreeNode gram_b = Gram_B();
                return new("D", null, gram_k, gram_b);
            }
            return new("D", null, gram_k);
        }

        private SyntaxTreeNode Gram_K()
        {
            SyntaxTreeNode gram_a = Gram_A();
            if (Lexer.CurrentElement.Type == TokenType.SpecialToken && Lexer.CurrentElement.Content == "∧")
            {
                SyntaxTreeNode gram_c = Gram_C();
                return new("K", null, gram_a, gram_c);
            }
            return new("K", null, gram_a);
        }

        private SyntaxTreeNode Gram_B()
        {
            Token token = Lexer.CurrentElement;
            if (token.Type == TokenType.SpecialToken && token.Content == "∨")
            {
                SyntaxTreeNode disjunction = new("Disjunction", token);
                token = Lexer.TakeElement();
                SyntaxTreeNode gram_k = Gram_K();
                token = Lexer.CurrentElement;
                if (token.Type == TokenType.SpecialToken && token.Content == "∨")
                {
                    SyntaxTreeNode gram_b = Gram_B();
                    return new("B", null, disjunction, gram_k, gram_b);
                }
                return new("B", null, disjunction, gram_k);
            }
            throw new Exception($"Invalid syntax for B, expected ∨, actually {Lexer.CurrentElement}");
        }

        private SyntaxTreeNode Gram_C()
        {
            Token token = Lexer.CurrentElement;
            if (token.Type == TokenType.SpecialToken && token.Content == "∧")
            {
                SyntaxTreeNode conjunction = new("Conjunction", token);
                token = Lexer.TakeElement();
                SyntaxTreeNode gram_a = Gram_A();
                token = Lexer.CurrentElement;
                if (token.Type == TokenType.SpecialToken && token.Content == "∧")
                {
                    SyntaxTreeNode gram_c = Gram_C();
                    return new("C", null, conjunction, gram_a, gram_c);
                }
                return new("C", null, conjunction, gram_a);
            }
            throw new Exception($"Invalid syntax for C, expected ∧, actually {Lexer.CurrentElement}");
        }

        private SyntaxTreeNode Gram_A()
        {
            Token token = Lexer.CurrentElement;

            if (token.Type == TokenType.SpecialToken && token.Content == "¬")
            {
                SyntaxTreeNode negation = new("Negation", token);
                Lexer.TakeElement();
                SyntaxTreeNode gram_a = Gram_A();
                return new("A", null, negation, gram_a);
            }
            else if (token.Type == TokenType.SpecialToken && token.Content == "(")
            {
                SyntaxTreeNode opening_bracket = new("OpeningBracket", token);
                Lexer.TakeElement();
                SyntaxTreeNode gram_d = Gram_D();
                token = Lexer.CurrentElement;
                SyntaxTreeNode closing_bracket = new("ClosingBracket", token);
                Lexer.TakeElement();
                return new("A", null, opening_bracket, gram_d, closing_bracket);
            }
            SyntaxTreeNode gram_o = Gram_O();
            return new("A", null, gram_o);
        }

        private SyntaxTreeNode Gram_O()
        {
            Token token = Lexer.CurrentElement;

            if (token.Type == TokenType.ConstantToken || token.Type == TokenType.IdentifierToken)
            {
                SyntaxTreeNode operand_1 = new("Operand1", token);
                token = Lexer.TakeElement();
                if (token.Type == TokenType.SpecialToken && token.Content == "=")
                {
                    SyntaxTreeNode assignment = new("Assignment", token);
                    token = Lexer.TakeElement();
                    if (token.Type == TokenType.ConstantToken || token.Type == TokenType.IdentifierToken)
                    {
                        SyntaxTreeNode operand_2 = new("Operand2", token);
                        Lexer.TakeElement();
                        return new("O", null, operand_1, assignment, operand_2);
                    }
                }
            }
            throw new Exception("Invalid syntax for O");
        }
    }
}
