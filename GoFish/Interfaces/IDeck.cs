using GoFish.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoFish.Interfaces
{
    internal interface IDeck
    {
        List<Card> Stack { get; set; }
        void Shuffle();
        void Print();
    }
}
