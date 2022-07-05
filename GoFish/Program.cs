using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GoFish.Models;

namespace GoFish
{
    internal class Program
    {

        static bool IsGameover(Deck deck, Hand playerHand, Hand computerHand)
        {
            if (!deck.Stack.Any() && (!playerHand.Stack.Any() || !computerHand.Stack.Any()))
            {
                return true;
            }
            return false;
        }
        static void MakeMatchesandCheckEnd(Deck deck, Hand playerHand, Hand playerMatches, Hand computerHand, ref bool gameActive, bool playerTurn)
        {
            List<Card> potentialMatches = playerHand.FindPotentialBooks(); // Check to see if gaining cards gave the player a match
            string currentMatcher;
            if (playerTurn == true)
            {
                currentMatcher = "You have";
            }
            else
            {
                currentMatcher = "The computer has";
            }
            if (potentialMatches.Any()) // if so remove the match from the player hand, and add to the player's matches, notify player in console
            {
                playerHand.MakeBooks(playerMatches, potentialMatches);
                Console.WriteLine($"{currentMatcher} made a book of {potentialMatches[0].Rank}'s! {currentMatcher} made a point!");
            }
            if (IsGameover(deck, playerHand, computerHand)) // see if the game is over and the state has to end
            {
                gameActive = false;
            }
            if (!playerHand.Stack.Any() && deck.Stack.Any()) // see if the player's hand is empty and they need to draw a card, won't trigger if game is over
            {
                playerHand.Draw(deck);
            }
        }
        static bool PlayerTurnLogic(Deck deck, Hand playerHand, Hand playerMatches, Hand computerHand, Hand computerMatches, Hand humanGuesses, ref bool gameActive, bool playerTurn)
        {
            playerHand.Print();
            Console.WriteLine();
            Console.WriteLine("Please select a card! You only need to enter a valid value. Don't include the suit.");
            string guessValue = Console.ReadLine();

            if (playerHand.ContainsRank(guessValue)) //See if the player even has the guess
            {
                

                Console.WriteLine($"Do you have any {guessValue}'s?");
                Console.ReadLine();
                AddToHumanGuesses(guessValue, humanGuesses, playerHand); // The computer takes note of which cards have been guessed

                List<Card> guessedCards = computerHand.FindAllRank(guessValue); // see if the computer has any matching cards
                if (guessedCards.Any()) // if they do, add them to the player hand, remove them from computer hand
                {
                    playerHand.TakeAllRank(guessedCards);
                    computerHand.RemoveAllRank(guessedCards);
                    Console.WriteLine($"They gave you {guessedCards.Count()} {guessValue}'s! You get to go again!");

                    MakeMatchesandCheckEnd(deck, playerHand, playerMatches, computerHand, ref gameActive, playerTurn);

                    Console.ReadLine();
                    return true;
                }
                else
                {
                    Console.WriteLine($"They didn't have any {guessValue}'s! Go Fish!");

                    if (!deck.Stack.Any()) // see if they can draw, if not, it is the comptuter's turn
                    {
                        Console.WriteLine("The deck is all out of cards! Its your opponents turn!");
                        Console.ReadLine();
                        return false;
                    }

                    else if (deck.Stack.First().Rank == guessValue) // if the card they draw is the one they asked for, they get to go again!
                    {
                        playerHand.Draw(deck);

                        MakeMatchesandCheckEnd(deck, playerHand, playerMatches, computerHand, ref gameActive, playerTurn);

                        Console.WriteLine($"You fished your wish! Go again!");
                        Console.ReadLine();
                        return true;
                    }

                    else // otherwise, they draw a card, and it is the computer's turn to guess
                    {
                        playerHand.Draw(deck);
                        Console.WriteLine("You drew a card!");
                        
                        MakeMatchesandCheckEnd(deck, playerHand, playerMatches, computerHand, ref gameActive, playerTurn);
                        playerHand.Print();
                        Console.WriteLine("It is now the Computer's turn!");
                        Console.ReadLine();
                        
                        return false;
                    }

                }
            }
            else
            {
                Console.WriteLine($"You don't have any {guessValue}'s in your hand. Try again! Make sure you guess a card that is in your hand.");
                return true;
            }
        }
        static void AddToHumanGuesses(string guessValue, Hand humanGuesses, Hand playerHand)
        {
            if (!humanGuesses.ContainsRank(guessValue))
            {
                List<Card> cardsToGuess = playerHand.FindAllRank(guessValue);
                humanGuesses.Stack.Add(cardsToGuess.First());
            }
        }
        static Card ReturnRandomCPUCard (Hand computerHand)
        {
            Random rng = new Random();
            int handCount = computerHand.Stack.Count();
            int randomCardIndex = rng.Next(handCount);
            return computerHand.Stack[randomCardIndex];
        }
        static Card ComputerGuess(Hand computerHand, Hand humanGuesses)
        {
            Random rng = new Random();

            if (humanGuesses.Stack.Any()) //See if there are any guesses from the player left, if so, see if any match cards the CPU has in their hand and guess a random one
            {
                List<Card> informedComputerGuess = new List<Card>();
                foreach (Card guessCard in humanGuesses.Stack)
                {
                    /*if (computerHand.Stack.FindAll(cardinHand => cardinHand.Rank == card.Rank) != null)
                    {
                        informedComputerGuess.Add(card);
                    }*/
                    foreach (Card cardInHand in computerHand.Stack)
                    {
                        if (cardInHand.Rank == guessCard.Rank)
                        {
                            informedComputerGuess.Add(guessCard);
                        }
                    }

                }
                if (informedComputerGuess.Any())
                {
                    int guessCount = informedComputerGuess.Count();
                    int randomCardIndex = rng.Next(guessCount);
                    humanGuesses.Stack.Remove(informedComputerGuess[randomCardIndex]);
                    return informedComputerGuess[randomCardIndex];
                }
                
            }
                 // if not, just guess a random card from their hand
            
            return ReturnRandomCPUCard(computerHand);
        }
        static bool ComputerTurnLogic(Deck deck, Hand playerHand, Hand playerMatches, Hand computerHand, Hand computerMatches, Hand humanGuesses, ref bool gameActive, bool playerTurn)
        {
            Console.WriteLine("Its the computer's turn to make a guess!");
            Card cardToGuess = ComputerGuess(computerHand, humanGuesses);
            Console.WriteLine($"The computer asks if you have any {cardToGuess.Rank}'s!");
            Console.ReadLine();

            List<Card> guessedCards = playerHand.FindAllRank(cardToGuess.Rank); // see if the player has any matching cards
            if (guessedCards.Any()) // if they do, add them to the computer hand, remove them from player hand
            {
                computerHand.TakeAllRank(guessedCards);
                playerHand.RemoveAllRank(guessedCards);
                Console.WriteLine($"You gave the computer {guessedCards.Count()} {cardToGuess.Rank}'s!");
                Console.ReadLine();

                MakeMatchesandCheckEnd(deck, computerHand, computerMatches, playerHand, ref gameActive, playerTurn);

                return false;
            }
            else
            {
                Console.WriteLine($"You don't have any {cardToGuess.Rank}'s! The computer has to Go Fish!");
                if (!deck.Stack.Any()) // see if they can draw, if not, it is the comptuter's turn
                {
                    Console.WriteLine("The deck is all out of cards! The computer can not draw. It is your turn!");
                    Console.ReadLine();
                    return true;
                }
                else if (deck.Stack.First().Rank == cardToGuess.Rank)
                {
                    computerHand.Draw(deck);
                    Console.WriteLine("The computer fished its wish and gets to go again!");
                    MakeMatchesandCheckEnd(deck, computerHand, computerMatches, playerHand, ref gameActive, playerTurn);
                    Console.ReadLine();

                    return false;
                }
                else
                {
                    computerHand.Draw(deck);
                    Console.WriteLine("The computer drew a card!");

                    MakeMatchesandCheckEnd(deck, computerHand, computerMatches, playerHand, ref gameActive, playerTurn);

                    Console.WriteLine("It is now your turn!");
                    Console.ReadLine();
                    return true;
                }
            }

        }
        static void runGame()
        {
            string path = @"C:\data\GoFish\StandardDeck.txt";

            bool gameActive = true;
            bool playerTurn = true;

            Deck deck = new Deck();
            deck.Load(path);
            deck.Shuffle();
            Hand playerHand = new Hand();
            Hand playerMatches = new Hand();
            Hand computerHand = new Hand();
            Hand computerMatches = new Hand();
            Hand humanGuesses = new Hand();

            for (int i = 0; i < 7; i++) //Deal starting hands
            {
                playerHand.Draw(deck);
                computerHand.Draw(deck);
            }

            while (gameActive)
            {
                if (playerTurn)
                {
                   playerTurn = PlayerTurnLogic(deck, playerHand, playerMatches, computerHand, computerMatches, humanGuesses, ref gameActive, playerTurn);
                }
                else
                {
                    playerTurn = ComputerTurnLogic(deck, playerHand, playerMatches, computerHand, computerMatches, humanGuesses, ref gameActive, playerTurn);
                }
            }

            int playerScore = playerMatches.Stack.Count() / 4;
            int computerScore = computerMatches.Stack.Count() / 4;

            if (playerScore > computerScore)
            {
                Console.WriteLine($"Congratulations! You won! The final score was {playerScore} to {computerScore}");
            }
            else
            {
                Console.WriteLine($"Looks like the computer lucked out! Better luck next time! The final score was {computerScore} to {playerScore}");
            }
        }
      
        static void Main(string[] args)
        {

            Console.WriteLine("Welcome to Go Fish!");

            runGame();

            Console.ReadLine();
        }
    }
}
