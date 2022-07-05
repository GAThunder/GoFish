using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoFish.Models
{
    public class Card
    {
        public int Value { get; set; }

        public string Rank { get; set; }

        public string Suit { get; set; }

        public override string ToString()
        {
            return $"{Suit}, {Rank}";
        }
    }
}
