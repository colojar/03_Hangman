//Hangman is a game where a word is secretly chosen and the player guesses letters to fill in the word.
//Each correct guess fills in that letter in the word.  Guess too many wrong letters and the player loses.

//Tips: It is all a matter of taking the letter the player guesses and looping through the word comparing it to each letter in the word character by character. If the letters match, you show that letter to the player. If you reach the end of the word and no letters have been matched, it is a wrong guess.
//Remember that strings are often treated as an array of characters. Most languages have a function to fetch a single letter from a string.
//Keep track of how many wrong guesses the player has done and use this number to determine if the game has been won or lost.

//Added Difficulty: Increase the length of the words and choose more complex unknown words to have the player guess.

namespace _03_Hangman
{
    internal class Program
    {
        const string POSITIVE_USER_RESPONSE = "y";
        const string NEGATIVE_USER_RESPONSE = "n";
        static void Main()
        {
            const string QUIT_USER_RESPONSE = "q";
            const string RESTART_USER_RESPONSE = "r";
            while (true)
            {

                // I'm lazy and don't want to type the word each time the program starts.
                List<string> words = [
                    "planet", "rocket", "castle", "forest", "hunter",
                    "dragon", "shadow", "pirate", "silver", "glider",
                    "puzzle", "goblin", "crater", "vortex", "beacon",
                    "temple", "wizard", "plasma", "sphinx", "serpent"
                ];
                // Display the logo
                HangmanLogo();

                Console.WriteLine();
                Console.WriteLine("Please enter a word to guess or press enter to use a random word.");
                Console.WriteLine($"(Press '{QUIT_USER_RESPONSE}' to quit or '{RESTART_USER_RESPONSE}' to restart)");
                string? wordToGuess = "";
                ConsoleKeyInfo keyInfo;
                do
                {
                    keyInfo = Console.ReadKey(intercept: true); // Read key without displaying it
                    if (keyInfo.Key != ConsoleKey.Enter)
                    {
                        wordToGuess += keyInfo.KeyChar;
                        Console.Write("*"); // Display a '*' for each character typed
                    }
                } while (keyInfo.Key != ConsoleKey.Enter);
                Console.WriteLine();
                if (wordToGuess == QUIT_USER_RESPONSE)
                {
                    break;
                }
                else if (wordToGuess == RESTART_USER_RESPONSE)
                {
                    Console.Clear();
                    continue;
                }
                if (string.IsNullOrWhiteSpace(wordToGuess))
                {
                    Random random = new();
                    int index = random.Next(words.Count); // Generate a random index
                    wordToGuess = words[index]; // Return the word at the random index
                }
                else
                {
                    wordToGuess = wordToGuess.ToLower();
                }

                int maxWrongGuesses;
                do
                {
                    Console.WriteLine("Please enter the number of wrong guesses allowed or press enter to use the length of the word.");
                    string? maxWrongGuessesInput = Console.ReadLine();
                    if (string.IsNullOrWhiteSpace(maxWrongGuessesInput))
                    {
                        maxWrongGuesses = wordToGuess.Length;
                        break;
                    }
                    if (int.TryParse(maxWrongGuessesInput, out maxWrongGuesses))
                    {
                        break;
                    }
                    else
                    {
                        Console.WriteLine("Invalid input. Please enter a number.");
                    }
                } while (true);


                // Guess loop
                bool lastGuess = false;
                bool endGame = false;
                string nextMessage = ""; // Variable to store the message for the next iteration of the loop
                int wrongGuesses = 0;
                string guessedLetters = "";
                string displayWord = new('_', wordToGuess.Length);
                while (displayWord.Contains('_'))
                {
                    HangmanLogo();
                    if (!string.IsNullOrEmpty(nextMessage))
                    {
                        Console.WriteLine();
                        Console.WriteLine(nextMessage); // Display the message from the previous iteration
                        Console.WriteLine();
                        nextMessage = ""; // Clear the message after displaying it
                    }

                    if (endGame)
                    {
                        if (!PlayAgain()) { return; }
                    }

                    if (lastGuess)
                    {
                        Console.WriteLine($"Do you want to try to guess the word? ({POSITIVE_USER_RESPONSE}/{NEGATIVE_USER_RESPONSE})");
                        string? guessWord = Console.ReadLine()?.ToLower();
                        if (guessWord == POSITIVE_USER_RESPONSE)
                        {
                            Console.Write("Please enter your guess: ");
                            string? wordGuess = Console.ReadLine()?.ToLower();
                            if (wordGuess == wordToGuess)
                            {
                                nextMessage = $"Congratulations! You've guessed the word: {wordToGuess}";
                            }
                            else
                            {
                                nextMessage = $"Sorry, that's not correct. The word was: {wordToGuess}";
                            }
                            endGame = true; // Set endGame to true to exit the loop
                            continue;
                        }
                        HangmanLogo();
                    }

                    Console.WriteLine($"Word to guess: {displayWord}");
                    Console.WriteLine($"Guessed letters: {guessedLetters}");
                    Console.WriteLine($"Wrong guesses left: {maxWrongGuesses - wrongGuesses}");
                    Console.Write("Please enter a letter to guess: ");
                    string? letter = Console.ReadLine();
                    if (string.IsNullOrWhiteSpace(letter))
                    {
                        continue;
                    }
                    letter = letter.ToLower();
                    if (letter.Length > 1)
                    {
                        nextMessage = "Please enter only one letter.";
                        continue;
                    }
                    if (guessedLetters.Contains(letter))
                    {
                        nextMessage = "You have already guessed that letter.";
                        continue;
                    }
                    guessedLetters += letter;
                    if (wordToGuess.Contains(letter))
                    {
                        for (int i = 0; i < wordToGuess.Length; i++)
                        {
                            if (wordToGuess[i] == letter[0])
                            {
                                displayWord = displayWord.Remove(i, 1).Insert(i, letter);
                            }
                        }
                        nextMessage = $"Good guess! The letter '{letter}' is in the word.";
                    }
                    else
                    {
                        Console.WriteLine($"Sorry, the letter '{letter}' is not in the word.");
                        wrongGuesses++;
                    }
                    if (1 == maxWrongGuesses - wrongGuesses)
                    {
                        nextMessage = "You are one wrong guess away from losing!";
                        lastGuess = true;
                    }

                    if (displayWord == wordToGuess)
                    {
                        HangmanLogo();
                        Console.WriteLine($"Congratulations! You've guessed the word: {wordToGuess}");
                        if (!PlayAgain()) { return; }

                    }
                    else if (wrongGuesses == maxWrongGuesses)
                    {
                        HangmanLogo();
                        Console.WriteLine($"Sorry, you've run out of guesses. The word was: {wordToGuess}");
                        if (!PlayAgain()) { return; }
                    }
                }
                continue;
            }
        }
        static void HangmanLogo()
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.Clear();
            Console.WriteLine("  _                                              ");
            Console.WriteLine(" | |                                             ");
            Console.WriteLine(" | |__   __ _ _ __   __ _ _ __ ___   __ _ _ __   ");
            Console.WriteLine(" | '_ \\ / _` | '_ \\ / _` | '_ ` _ \\ / _` | '_ \\  ");
            Console.WriteLine(" | | | | (_| | | | | (_| | | | | | | (_| | | | | ");
            Console.WriteLine(" |_| |_|\\__,_|_| |_|\\__, |_| |_| |_|\\__,_|_| |_| ");
            Console.WriteLine("                     __/ |                      ");
            Console.WriteLine("                    |___/                       ");
            Console.WriteLine();
            Console.WriteLine("   +---+ ");
            Console.WriteLine("   |   | ");
            Console.WriteLine("   O   | ");
            Console.WriteLine("  /|\\  | ");
            Console.WriteLine("  / \\  | ");
            Console.WriteLine("       | ");
            Console.WriteLine("=========");
        }
        static bool PlayAgain()
        {
            Console.WriteLine($"Do you want to play again? {POSITIVE_USER_RESPONSE}/{NEGATIVE_USER_RESPONSE}");
            string? playAgain = Console.ReadLine()?.ToLower();
            if (playAgain == POSITIVE_USER_RESPONSE)
            {
                return true;
            }
            return false;
        }
    }
}
