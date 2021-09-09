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
        IdentifierToken
    }

    public class Token
    {
        public Token(string content, TokenType type)
        {
            Content = content;
            Type = type;
        }

        public string Content { get; init; }
        public TokenType Type { get; init; }

        public override string ToString()
        {
            return $"[\"{Content}\", {Type}]";
        }
    }

    public static class Lexer
    {
        public static List<Token> Process(List<Liter> liters)
        {
            List<Token> tokens = new();
            for (int i = 0; i < liters.Count;)
            {
                Liter liter = liters[i];
                Token token = null;
                switch (liter.Type)
                {
                    case LiterType.Letter:
                        (token, i) = ProcessIdentifierToken(liters, i);
                        break;
                    case LiterType.Digit:
                        (token, i) = ProcessConstantToken(liters, i);
                        break;
                    case LiterType.Whitespace:
                        ++i;
                        break;
                    case LiterType.Delimeter:
                        if (liter.Character == '0')
                        {
                            return tokens;
                        }
                        ++i;
                        break;
                    case LiterType.Special:
                        throw new NotImplementedException($"Unsupported character {liter.Character} at {liter.Location}.");
                    case LiterType.Ignorable:
                        ++i;
                        break;
                    case LiterType.Prohibited:
                    case LiterType.Other:
                    default:
                        throw new Exception($"Invalid character {liter.Character} at {liter.Location}.");
                }
                if (token != null)
                {
                    tokens.Add(token);
                }
            }
            return tokens;
        }

        private static (Token, int stopPosition) ProcessConstantToken(List<Liter> liters, int currentPosition)
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

            Liter currentLiter = liters[currentPosition++];

            A:
            switch (currentLiter.Character)
            {
                case '0':
                    contentBuilder.Append(currentLiter.Character);
                    currentLiter = liters[currentPosition++];
                    goto B;
                case '1':
                    contentBuilder.Append(currentLiter.Character);
                    currentLiter = liters[currentPosition++];
                    goto D;
                default:
                    throw new Exception($"Invalid liter {currentLiter}, expected '0' or '1'.");
            }

            B:
            if (currentLiter.Character == '1')
            {

                contentBuilder.Append(currentLiter.Character);
                currentLiter = liters[currentPosition++];
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
                currentLiter = liters[currentPosition++];
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
                currentLiter = liters[currentPosition++];
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
                currentLiter = liters[currentPosition++];
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
                currentLiter = liters[currentPosition++];
                goto G;
            }
            else if (currentLiter.Type == LiterType.Digit)
            {
                throw new Exception($"Invalid liter {currentLiter}, expected '1'.");
            }
            else
            {
                return (new Token(contentBuilder.ToString(), TokenType.ConstantToken), currentPosition);
            }

            G:
            if (currentLiter.Character == '1')
            {

                contentBuilder.Append(currentLiter.Character);
                currentLiter = liters[currentPosition++];
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
                currentLiter = liters[currentPosition++];
                goto F_Fin;
            }
            else
            {
                throw new Exception($"Invalid liter {currentLiter}, expected '1'.");
            }

        }

        private static (Token, int stopPosition) ProcessIdentifierToken(List<Liter> liters, int currentPosition)
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

            Liter currentLiter = liters[currentPosition++];

        A:
            switch (currentLiter.Character)
            {
                case 'a':
                    contentBuilder.Append(currentLiter.Character);
                    currentLiter = liters[currentPosition++];
                    goto E_Fin;
                case 'b':
                    contentBuilder.Append(currentLiter.Character);
                    currentLiter = liters[currentPosition++];
                    goto D_Fin;
                case 'c':
                    contentBuilder.Append(currentLiter.Character);
                    currentLiter = liters[currentPosition++];
                    goto C_Fin;
                case 'd':
                    contentBuilder.Append(currentLiter.Character);
                    currentLiter = liters[currentPosition++];
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
                        currentLiter = liters[currentPosition++];
                        goto E_Fin;
                    case 'b':
                        contentBuilder.Append(currentLiter.Character);
                        currentLiter = liters[currentPosition++];
                        goto D_Fin;
                    case 'c':
                        contentBuilder.Append(currentLiter.Character);
                        currentLiter = liters[currentPosition++];
                        goto C_Fin;
                    case 'd':
                        contentBuilder.Append(currentLiter.Character);
                        currentLiter = liters[currentPosition++];
                        goto B_Fin;
                    default:
                        throw new Exception($"Invalid liter {currentLiter}, expected 'a', 'b', 'c' or 'd'.");
                }
            }
            else
            {
                return (new Token(contentBuilder.ToString(), TokenType.IdentifierToken), currentPosition);
            }

        C_Fin:
            if (currentLiter.Type == LiterType.Letter)
            {
                switch (currentLiter.Character)
                {
                    case 'a':
                        contentBuilder.Append(currentLiter.Character);
                        currentLiter = liters[currentPosition++];
                        goto E_Fin;
                    case 'b':
                        contentBuilder.Append(currentLiter.Character);
                        currentLiter = liters[currentPosition++];
                        goto D_Fin;
                    case 'c':
                        contentBuilder.Append(currentLiter.Character);
                        currentLiter = liters[currentPosition++];
                        goto C_Fin;
                    default:
                        throw new Exception($"Invalid liter {currentLiter}, expected 'a', 'b', or 'c'.");
                }
            }
            else
            {
                return (new Token(contentBuilder.ToString(), TokenType.IdentifierToken), currentPosition);
            }

        D_Fin:
            if (currentLiter.Type == LiterType.Letter)
            {
                switch (currentLiter.Character)
                {
                    case 'a':
                        contentBuilder.Append(currentLiter.Character);
                        currentLiter = liters[currentPosition++];
                        goto E_Fin;
                    case 'b':
                        contentBuilder.Append(currentLiter.Character);
                        currentLiter = liters[currentPosition++];
                        goto D_Fin;
                    default:
                        throw new Exception($"Invalid liter {currentLiter}, expected 'a', or 'b'.");
                }
            }
            else
            {
                return (new Token(contentBuilder.ToString(), TokenType.IdentifierToken), currentPosition);
            }

        E_Fin:
            if (currentLiter.Type == LiterType.Letter)
            {
                if (currentLiter.Character == 'a')
                {
                    contentBuilder.Append(currentLiter.Character);
                    currentLiter = liters[currentPosition++];
                    if (currentLiter.Type == LiterType.Digit)
                    {
                        goto E_Fin;
                    }
                    else
                    {
                        return (new Token(contentBuilder.ToString(), TokenType.IdentifierToken), currentPosition);
                    }
                }
                else
                {
                    throw new Exception($"Invalid liter {currentLiter}, expected 'a'.");
                }
            }
            else
            {
                return (new Token(contentBuilder.ToString(), TokenType.IdentifierToken), currentPosition);
            }
        }
    }
}
