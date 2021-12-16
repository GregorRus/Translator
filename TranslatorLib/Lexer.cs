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
using System.Text;

namespace TranslatorLib
{
    public enum TokenType
    {
        ConstantToken,
        IdentifierToken,
        SpecialToken,
        EndOfFile
    }

    public struct TokenLocation
    {
        public LiterLocation Begin { get; init; }
        public LiterLocation End { get; init; }

        public TokenLocation(LiterLocation begin, LiterLocation end)
        {
            Begin = begin;
            End = end;
        }

        public int Length => End.Column - Begin.Column + 1;

        public int Line => Begin.Line;

        public override string ToString()
        {
            return $"{Begin}-{End}";
        }
    }

    public class Token : IStageElement
    {
        public Token(string content, TokenType type, TokenLocation location)
        {
            Content = content;
            Type = type;
            Location = location;
        }

        public string Content { get; init; }
        public TokenType Type { get; init; }
        public TokenLocation Location { get; init; }

        public bool IsLast()
        {
            return Type == TokenType.EndOfFile;
        }

        public override string ToString()
        {
            return $"[\"{SanitizedContent}\", {Type}, {Location}]";
        }

        public string SanitizedContent => Type switch
        {
            TokenType.EndOfFile => "end_of_file",
            _ => Content
        };
    }

    public class Lexer : IStage<Token>
    {
        private readonly Transliterator Transliterator;

        private Token? CurrentToken;

        public Token CurrentElement => CurrentToken ?? throw new InvalidOperationException("Invalid operation performed: no available current element before TakeElement call.");

        public Lexer(Transliterator transliterator)
        {
            Transliterator = transliterator;
            transliterator.TakeElement();
        }

        public Token TakeElement()
        {
            Liter liter = Transliterator.CurrentElement;

            bool notPrepared = true;
            while (notPrepared)
            {
                // Skip ignorable characters
                while (liter.Type == LiterType.Whitespace ||
                    liter.Type == LiterType.Ignorable)
                {
                    liter = Transliterator.TakeElement();
                }

                switch (liter.Type)
                {
                    case LiterType.Delimeter:
                        if (liter.Character == '\0')
                        {
                            TokenLocation location = new(liter.Location, liter.Location);
                            CurrentToken = new(liter.Character.ToString(), TokenType.EndOfFile, location);
                            return CurrentElement;
                        }
                        break;

                    case LiterType.Special:
                        if (liter.Character == '/')
                        {
                            liter = Transliterator.TakeElement();
                            if (liter.Type == LiterType.Special)
                            {
                                if (liter.Character == '/')
                                {
                                    PassOnelineComment();
                                }
                                else if (liter.Character == '*')
                                {
                                    PassMultilineComment();
                                }
                            }
                        }
                        else
                        {
                            notPrepared = false;
                        }
                        break;

                    case LiterType.Prohibited:
                    case LiterType.Other:
                        throw new LexerException(liter);

                    default:
                        notPrepared = false;
                        break;
                }
                if (notPrepared)
                {
                    liter = Transliterator.TakeElement();
                }
            }

            switch (liter.Type)
            {
                case LiterType.Letter:
                    CurrentToken = ProcessIdentifierToken();
                    return CurrentElement;

                case LiterType.Digit:
                    CurrentToken = ProcessConstantToken();
                    return CurrentElement;

                case LiterType.Special:
                    CurrentToken = ProcessSpecialToken();
                    return CurrentElement;

            }
            throw new LexerException(liter);
        }

        private Token ProcessConstantToken()
        {
            //(010)*110(111)*

            // A -> 0B
            // B -> 1C
            // C -> 0A

            // A -> 1D
            // D -> 1E
            // E -> 0
            // E -> 0F

            // F -> 1G
            // G -> 1H
            // H -> 1F
            // H -> 1

            //|-------|-------|-------|
            //|       |   0   |   1   |
            //|-------|-------|-------|
            //|  {A}  |  {B}  |  {D}  |
            //|-------|-------|-------|
            //|  {B}  |       |  {C}  |
            //|-------|-------|-------|
            //|  {C}  |  {A}  |       |
            //|-------|-------|-------|
            //|  {D}  |       |  {E}  |
            //|-------|-------|-------|
            //|  {E}  |{F,Fin}|       |
            //|-------|-------|-------|
            //|  {F}  |       |  {G}  |
            //|-------|-------|-------|
            //|  {G}  |       |  {H}  |
            //|-------|-------|-------|
            //|  {H}  |       | {Fin} |
            //|-------|-------|-------|
            //| {Fin} |       |       |
            //|-------|-------|-------|

            //|-------|-------|-------|
            //|       |   0   |   1   |
            //|-------|-------|-------|
            //|  {A}  |  {B}  |  {D}  |
            //|-------|-------|-------|
            //|  {B}  |       |  {C}  |
            //|-------|-------|-------|
            //|  {C}  |  {A}  |       |
            //|-------|-------|-------|
            //|  {D}  |       |  {E}  |
            //|-------|-------|-------|
            //|  {E}  |{F,Fin}|       |
            //|-------|-------|-------|
            //|{F,Fin}|       |  {G}  |
            //|-------|-------|-------|
            //|  {G}  |       |  {H}  |
            //|-------|-------|-------|
            //|  {H}  |       |{F,Fin}|
            //|-------|-------|-------|

            StringBuilder contentBuilder = new();

            Liter currentLiter = Transliterator.CurrentElement;
            LiterLocation begin = currentLiter.Location;
            LiterLocation end;

        A:
            switch (currentLiter.Character)
            {
                case '0':
                    contentBuilder.Append(currentLiter.Character);
                    end = currentLiter.Location;
                    currentLiter = Transliterator.TakeElement();
                    goto B;
                case '1':
                    contentBuilder.Append(currentLiter.Character);
                    end = currentLiter.Location;
                    currentLiter = Transliterator.TakeElement();
                    goto D;
                default:
                    throw new LexerException(currentLiter, "'0' or '1'");
            }

        B:
            if (currentLiter.Character == '1')
            {

                contentBuilder.Append(currentLiter.Character);
                end = currentLiter.Location;
                currentLiter = Transliterator.TakeElement();
                goto C;
            }
            else
            {
                throw new LexerException(currentLiter, "'1'");
            }

        C:
            if (currentLiter.Character == '0')
            {

                contentBuilder.Append(currentLiter.Character);
                end = currentLiter.Location;
                currentLiter = Transliterator.TakeElement();
                goto A;
            }
            else
            {
                throw new LexerException(currentLiter, "'0'");
            }

        D:
            if (currentLiter.Character == '1')
            {

                contentBuilder.Append(currentLiter.Character);
                end = currentLiter.Location;
                currentLiter = Transliterator.TakeElement();
                goto E;
            }
            else
            {
                throw new LexerException(currentLiter, "'1'");
            }

        E:
            if (currentLiter.Character == '0')
            {

                contentBuilder.Append(currentLiter.Character);
                end = currentLiter.Location;
                currentLiter = Transliterator.TakeElement();
                goto F_Fin;
            }
            else
            {
                throw new LexerException(currentLiter, "'0'");
            }

        F_Fin:
            if (currentLiter.Character == '1')
            {

                contentBuilder.Append(currentLiter.Character);
                end = currentLiter.Location;
                currentLiter = Transliterator.TakeElement();
                goto G;
            }
            else if (currentLiter.Type == LiterType.Digit)
            {
                throw new LexerException(currentLiter, "'1'");
            }
            else
            {
                TokenLocation location = new(begin, end);
                return new Token(contentBuilder.ToString(), TokenType.ConstantToken, location);
            }

        G:
            if (currentLiter.Character == '1')
            {

                contentBuilder.Append(currentLiter.Character);
                end = currentLiter.Location;
                currentLiter = Transliterator.TakeElement();
                goto H;
            }
            else
            {
                throw new LexerException(currentLiter, "'1'");
            }

        H:
            if (currentLiter.Character == '1')
            {

                contentBuilder.Append(currentLiter.Character);
                end = currentLiter.Location;
                currentLiter = Transliterator.TakeElement();
                goto F_Fin;
            }
            else
            {
                throw new LexerException(currentLiter, "'1'");
            }

        }

        private Token ProcessIdentifierToken()
        {
            //(a|b|c|d)+ В порядке обратном алфавитному

            // A -> dB
            // A -> cC
            // A -> bD
            // A -> aE

            // B -> d
            // B -> dB
            // B -> cC
            // B -> bD
            // B -> aE

            // C -> c
            // C -> cC
            // C -> bD
            // C -> aE

            // D -> b
            // D -> bD
            // D -> aE

            // E -> a
            // E -> aE

            //|-------|-------|-------|-------|-------|
            //|       |   a   |   b   |   c   |   d   |
            //|-------|-------|-------|-------|-------|
            //|  {A}  |  {E}  |  {D}  |  {C}  |  {B}  |
            //|-------|-------|-------|-------|-------|
            //|  {B}  |  {E}  |  {D}  |  {C}  |{B,Fin}|
            //|-------|-------|-------|-------|-------|
            //|  {C}  |  {E}  |  {D}  |{C,Fin}|       |
            //|-------|-------|-------|-------|-------|
            //|  {D}  |  {E}  |{D,Fin}|       |       |
            //|-------|-------|-------|-------|-------|
            //|  {E}  |{E,Fin}|       |       |       |
            //|-------|-------|-------|-------|-------|
            //| {Fin} |       |       |       |       |
            //|-------|-------|-------|-------|-------|

            //|-------|-------|-------|-------|-------|
            //|       |   a   |   b   |   c   |   d   |
            //|-------|-------|-------|-------|-------|
            //|  {A}  |{E,Fin}|{D,Fin}|{C,Fin}|{B,Fin}|
            //|-------|-------|-------|-------|-------|
            //|{B,Fin}|{E,Fin}|{D,Fin}|{C,Fin}|{B,Fin}|
            //|-------|-------|-------|-------|-------|
            //|{C,Fin}|{E,Fin}|{D,Fin}|{C,Fin}|       |
            //|-------|-------|-------|-------|-------|
            //|{D,Fin}|{E,Fin}|{D,Fin}|       |       |
            //|-------|-------|-------|-------|-------|
            //|{E,Fin}|{E,Fin}|       |       |       |
            //|-------|-------|-------|-------|-------|

            StringBuilder contentBuilder = new();

            Liter currentLiter = Transliterator.CurrentElement;
            LiterLocation begin = currentLiter.Location;
            LiterLocation end;

#pragma warning disable CS0164 // Отсутствует ссылка на эту метку.
        A:
#pragma warning restore CS0164 // Отсутствует ссылка на эту метку.
            switch (currentLiter.Character)
            {
                case 'a':
                    contentBuilder.Append(currentLiter.Character);
                    end = currentLiter.Location;
                    currentLiter = Transliterator.TakeElement();
                    goto E_Fin;
                case 'b':
                    contentBuilder.Append(currentLiter.Character);
                    end = currentLiter.Location;
                    currentLiter = Transliterator.TakeElement();
                    goto D_Fin;
                case 'c':
                    contentBuilder.Append(currentLiter.Character);
                    end = currentLiter.Location;
                    currentLiter = Transliterator.TakeElement();
                    goto C_Fin;
                case 'd':
                    contentBuilder.Append(currentLiter.Character);
                    end = currentLiter.Location;
                    currentLiter = Transliterator.TakeElement();
                    goto B_Fin;
                default:
                    throw new LexerException(currentLiter, "'a', 'b', 'c' or 'd'");
            }

        B_Fin:
            if (currentLiter.Type == LiterType.Letter)
            {
                switch (currentLiter.Character)
                {
                    case 'a':
                        contentBuilder.Append(currentLiter.Character);
                        end = currentLiter.Location;
                        currentLiter = Transliterator.TakeElement();
                        goto E_Fin;
                    case 'b':
                        contentBuilder.Append(currentLiter.Character);
                        end = currentLiter.Location;
                        currentLiter = Transliterator.TakeElement();
                        goto D_Fin;
                    case 'c':
                        contentBuilder.Append(currentLiter.Character);
                        end = currentLiter.Location;
                        currentLiter = Transliterator.TakeElement();
                        goto C_Fin;
                    case 'd':
                        contentBuilder.Append(currentLiter.Character);
                        end = currentLiter.Location;
                        currentLiter = Transliterator.TakeElement();
                        goto B_Fin;
                    default:
                        throw new LexerException(currentLiter, "'a', 'b', 'c' or 'd'");
                }
            }
            else
            {
                TokenLocation location = new(begin, end);
                return new Token(contentBuilder.ToString(), TokenType.IdentifierToken, location);
            }

        C_Fin:
            if (currentLiter.Type == LiterType.Letter)
            {
                switch (currentLiter.Character)
                {
                    case 'a':
                        contentBuilder.Append(currentLiter.Character);
                        end = currentLiter.Location;
                        currentLiter = Transliterator.TakeElement();
                        goto E_Fin;
                    case 'b':
                        contentBuilder.Append(currentLiter.Character);
                        end = currentLiter.Location;
                        currentLiter = Transliterator.TakeElement();
                        goto D_Fin;
                    case 'c':
                        contentBuilder.Append(currentLiter.Character);
                        end = currentLiter.Location;
                        currentLiter = Transliterator.TakeElement();
                        goto C_Fin;
                    default:
                        throw new LexerException(currentLiter, "'a', 'b', or 'c'");
                }
            }
            else
            {
                TokenLocation location = new(begin, end);
                return new Token(contentBuilder.ToString(), TokenType.IdentifierToken, location);
            }

        D_Fin:
            if (currentLiter.Type == LiterType.Letter)
            {
                switch (currentLiter.Character)
                {
                    case 'a':
                        contentBuilder.Append(currentLiter.Character);
                        end = currentLiter.Location;
                        currentLiter = Transliterator.TakeElement();
                        goto E_Fin;
                    case 'b':
                        contentBuilder.Append(currentLiter.Character);
                        end = currentLiter.Location;
                        currentLiter = Transliterator.TakeElement();
                        goto D_Fin;
                    default:
                        throw new LexerException(currentLiter, "'a', or 'b'");
                }
            }
            else
            {
                TokenLocation location = new(begin, end);
                return new Token(contentBuilder.ToString(), TokenType.IdentifierToken, location);
            }

        E_Fin:
            if (currentLiter.Type == LiterType.Letter)
            {
                if (currentLiter.Character == 'a')
                {
                    contentBuilder.Append(currentLiter.Character);
                    end = currentLiter.Location;
                    currentLiter = Transliterator.TakeElement();
                    if (currentLiter.Type == LiterType.Letter)
                    {
                        goto E_Fin;
                    }
                    else
                    {
                        TokenLocation location = new(begin, end);
                        return new Token(contentBuilder.ToString(), TokenType.IdentifierToken, location);
                    }
                }
                else
                {
                    throw new LexerException(currentLiter, "'a'");
                }
            }
            else
            {
                TokenLocation location = new(begin, end);
                return new Token(contentBuilder.ToString(), TokenType.IdentifierToken, location);
            }
        }

        private Token ProcessSpecialToken()
        {
            Liter liter = Transliterator.CurrentElement;
            _ = Transliterator.TakeElement();

            return new(liter.Character.ToString(), TokenType.SpecialToken, new(liter.Location, liter.Location));
        }

        private void PassOnelineComment()
        {
            Liter liter = Transliterator.TakeElement();
            while (liter.Type != LiterType.Delimeter
                && (liter.Character != '\n' && liter.Character != '\0'))
            {
                liter = Transliterator.TakeElement();
            }
        }

        private void PassMultilineComment()
        {
            Liter liter = Transliterator.TakeElement();

            while (true)
            {
                while ((liter.Type != LiterType.Special || liter.Character != '*')
                     && (liter.Type != LiterType.Delimeter || liter.Character != '\0'))
                {
                    liter = Transliterator.TakeElement();
                }
                liter = Transliterator.TakeElement();
                if (liter.Type == LiterType.Special && liter.Character == '/')
                {
                    return;
                }
                else if (liter.Type == LiterType.Delimeter && liter.Character == '\0')
                {
                    throw new LexerException(liter);
                }
            }
        }
    }
}
