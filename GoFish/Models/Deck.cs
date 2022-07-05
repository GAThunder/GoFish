using GoFish.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoFish.Models
{
    public class Deck :IDeck
    {
        private static Random rng = new Random();

        public List<Card> Stack { get; set; } = new List<Card>();

        public void RemoveCard(Card cardToRemove)
        {
            this.Stack.Remove(cardToRemove);
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
        public void Load(string filePath)
        {
            
            if (!File.Exists(filePath))
            {
                Console.WriteLine("File does not exist.");
            }
            else
            {
                List<string> list = File.ReadAllLines(filePath).ToList();

                foreach (string line in list)
                {
                    string[] cols = line.Split(',');
                    Card c = new Card();
                    c.Suit = cols[0];
                    c.Rank = cols[1];
                    c.Value = Int32.Parse(cols[2]);

                    this.Stack.Add(c);
                }
            }

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
    }
}
