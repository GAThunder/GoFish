using GoFish.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoFish.Models
{
    internal class Hand : IDeck
    {

        private static Random rng = new Random();
        public List<Card> Stack { get; set; } = new List<Card>();
        public void RemoveCard(Card cardToRemove)
        {
            this.Stack.Remove(cardToRemove);
        }
        public bool ContainsRank(string rank)
        {
            return Stack.Any(card => card.Rank == rank);
        }
        public List<Card> FindAllRank (string rank)
        {
            return Stack.FindAll(card => card.Rank == rank);
        }
        public void TakeAllRank (List<Card> correctGuesses)
        {
            foreach (Card card in correctGuesses)
            {
                this.Stack.Add(card);
            }
        }
        public void RemoveAllRank (List<Card> correctGuesses)
        {
            foreach (Card card in correctGuesses)
            {
                this.Stack.Remove(card);
            }
        }
        public List<Card> FindPotentialBooks ()
        {
            List<Card> potentialBooks = new List<Card>();
            List<Card> books = new List<Card>();
            foreach (Card card in this.Stack)
            {
                potentialBooks = this.FindAllRank(card.Rank);
                if (potentialBooks.Count == 4)
                {
                    foreach (Card book in potentialBooks)
                    {
                        books.Add(card);
                    }
                }
            }
            return books;
        }
        public void MakeBooks (Hand Matches, List<Card> matches)
        {
            Matches.TakeAllRank(matches);
            this.RemoveAllRank(matches);
        }
        public void Print()
        {
            if (this.Stack != null)
            {
                foreach (Card c in this.Stack)
                {
                    Console.WriteLine(c.ToString());
                }
            }
        }
        public void Shuffle()
        {
            int n = this.Stack.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                Card tempCard = this.Stack[k];
                this.Stack[k] = this.Stack[n];
                this.Stack[n] = tempCard;
            }
        }
        public void Draw(IDeck drawingDeck)
        {
            if (drawingDeck.Stack.Any())
            {
                this.Stack.Add(drawingDeck.Stack.First());
                drawingDeck.Stack.Remove(drawingDeck.Stack.First());
            }

            else
            {
                Console.WriteLine("No cards to draw");
            }

        }
        
    }
}
