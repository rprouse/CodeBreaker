using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MasterMind
{
    public class Game
    {
        const char INCORRECT = '_';
        const char INCORRECT_POS = 'X';
        const char CORRECT_POS = 'C';

        const string WIN = "You guessed correctly! Enter R to play again.";
        const string GUESS = "Enter four digits from 1 to 6 to guess the secret. Enter R to reset and play again.";
        const string INVALID = "Invalid entry. " + GUESS;

        Random _random = new Random();
        char[] _answer = new char[4];
        IList<char[]> _guesses = new List<char[]>();
        string _message;

        public Game()
        {
            Reset();
        }

        public void GameLoop()
        {
            do
            {
                OutputHeader();
                OutputLegend();
                OutputAnswer();
                OutputGuesses();
                OutputMessage();
            } while (Input());
        }

        static void OutputHeader()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("MasterMind");
            Console.WriteLine();
        }

        static void OutputLegend()
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Legend");
            Console.WriteLine();
            Console.WriteLine($"{INCORRECT} - Incorrect guess");
            Console.WriteLine($"{INCORRECT_POS} - Correct guess, but wrong position");
            Console.WriteLine($"{CORRECT_POS} - Correct guess");
        }

        void OutputAnswer()
        {
#if true
            Console.Write("Answer is ");
            foreach (char c in _answer)
                Console.Write(c);
            Console.WriteLine();
            Console.WriteLine();
#endif
        }

        void OutputGuesses()
        {
            Console.ResetColor();
            int move = 1;
            foreach (var guess in _guesses)
            {
                Console.WriteLine($"{move++}. {guess[0]}{guess[1]}{guess[2]}{guess[3]} {Result(guess)}");
            }
        }

        void OutputMessage()
        {
            Console.WriteLine();
            Console.WriteLine(_message);
            Console.WriteLine();
        }

        bool Input()
        {
            Console.Write("> ");
            var input = Console.ReadLine().ToLowerInvariant();
            if (input == "r")
            {
                Reset();
                return true;
            }
            else if (input == "q")
            {
                return false;
            }
            ParseInput(input);
            return true;
        }

        bool ParseInput(string input)
        {
            if (input.Length != 4 || input.Any(c => !"123456".Contains(c)))
            {
                _message = INVALID;
                return false;
            }
            var guess = new char[4];
            for (int i = 0; i < 4; i++)
                guess[i] = input[i];

            _guesses.Add(guess);
            _message = Correct(guess) ? WIN : GUESS;
            return true;
        }

        string Result(char[] guess)
        {
            char[] result = { INCORRECT, INCORRECT, INCORRECT, INCORRECT };
            var answer = (char[])_answer.Clone();
            for(int i = 0; i < 4; i++)
            {
                if(guess[i] == answer[i])
                {
                    result[i] = CORRECT_POS;
                    answer[i] = ' ';
                }
            }
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    if (i != j && guess[i] == answer[j])
                    {
                        result[i] = INCORRECT_POS;
                        answer[j] = ' ';
                    }
                }
            }
            return new string(result);
        }

        bool Correct(char[] guess)
        {
            for (int i = 0; i < 4; i++)
            {
                if (guess[i] != _answer[i]) return false;
            }
            return true;
        }

        void Reset()
        {
            for (int i = 0; i < 4; i++)
                _answer[i] = (char)(_random.Next(1, 7) + '0');
            _guesses.Clear();
            _message = GUESS;
        }
    }
}
