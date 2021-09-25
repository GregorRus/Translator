using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TranslatorLib
{
    public enum TokenType
    {
        ConstantToken,
        IdentifierToken,
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
        }

        public Token TakeElement()
        {
            Liter liter = Transliterator.TakeElement();

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
                            return new(liter.Character.ToString(), TokenType.EndOfFile, location);
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
                            throw new NotImplementedException($"Unsupported character '{liter.SanitizedCharacter}' at {liter.Location}.");
                        }
                        break;

                    case LiterType.Prohibited:
                    case LiterType.Other:
                        throw new NotImplementedException($"Unsupported character '{liter.SanitizedCharacter}' at {liter.Location}.");

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

            }
            throw new NotImplementedException($"Unsupported character '{liter.SanitizedCharacter}' at {liter.Location}.");
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
            // H -> 1

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
                    throw new Exception($"Invalid liter {currentLiter}, expected '0' or '1'.");
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
                throw new Exception($"Invalid liter {currentLiter}, expected '1'.");
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
                throw new Exception($"Invalid liter {currentLiter}, expected '0'.");
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
                throw new Exception($"Invalid liter {currentLiter}, expected '1'.");
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
                throw new Exception($"Invalid liter {currentLiter}, expected '0'.");
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
                throw new Exception($"Invalid liter {currentLiter}, expected '1'.");
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
                throw new Exception($"Invalid liter {currentLiter}, expected '1'.");
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
                throw new Exception($"Invalid liter {currentLiter}, expected '1'.");
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

            StringBuilder contentBuilder = new();

            Liter currentLiter = Transliterator.CurrentElement;
            LiterLocation begin = currentLiter.Location;
            LiterLocation end;

        A:
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
                    throw new Exception($"Invalid liter {currentLiter}, expected 'a', 'b', 'c' or 'd'.");
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
                        throw new Exception($"Invalid liter {currentLiter}, expected 'a', 'b', 'c' or 'd'.");
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
                        throw new Exception($"Invalid liter {currentLiter}, expected 'a', 'b', or 'c'.");
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
                        throw new Exception($"Invalid liter {currentLiter}, expected 'a', or 'b'.");
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
                    throw new Exception($"Invalid liter {currentLiter}, expected 'a'.");
                }
            }
            else
            {
                TokenLocation location = new(begin, end);
                return new Token(contentBuilder.ToString(), TokenType.IdentifierToken, location);
            }
        }

        private void PassOnelineComment()
        {
            Liter liter = Transliterator.TakeElement();
            while (liter.Type != LiterType.Delimeter && liter.Character != '\n')
            {
                liter = Transliterator.TakeElement();
            }
        }

        private void PassMultilineComment()
        {
            Liter liter = Transliterator.TakeElement();

            while (true)
            {
                while (liter.Type != LiterType.Special || liter.Character != '*')
                {
                    liter = Transliterator.TakeElement();
                }
                liter = Transliterator.TakeElement();
                if (liter.Type == LiterType.Special && liter.Character == '/')
                {
                    return;
                }
            }
        }
    }
}
