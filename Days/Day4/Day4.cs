using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2023.Days.Day4
{

    public class Card
    {
        public List<int> playerNumbers = new List<int>();
        public List<int> winningNumbers = new List<int>();

        public int MatchingCardValues
        {
            get
            {
                return playerNumbers.Where(x => winningNumbers.Contains(x)).Count();
            }
        }
        public int score
        {
            get
            {
                
                int score = 0;

                for (int i = 0; i < MatchingCardValues; i++)
                {
                    if(score == 0)
                    {
                        score = 1;
                    }
                    else
                    {
                        score *= 2;
                    }
                }


                return score;
            }
        }
    }


    public class Day4
    {
        List<Card> cards = new List<Card>();
        public Day4(string[] inputs) 
        {
            for (int i = 0; i < inputs.Length; i++)
            {
                Card newCard = new Card();

                var valueStrings = inputs[i].Split(':')[1].Trim().Split('|');

                var winningNumbers = valueStrings[0].Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries).ToList();
                var playerNumbers = valueStrings[1].Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries).Where(x => !string.IsNullOrEmpty(x)).ToList();

                foreach (var item in winningNumbers)
                {
                    newCard.winningNumbers.Add(int.Parse(item));
                }

                foreach (var item in playerNumbers)
                {
                    newCard.playerNumbers.Add(int.Parse(item));
                }
                Console.WriteLine($"Creating new Card {i}");
                cards.Add(newCard);
            }

            int sum = 0;
            foreach (var item in cards)
            {
                Console.WriteLine($"{item.score}");
                sum += item.score;
            }

            Console.WriteLine($"Score: {sum}");


            Dictionary<int, int> cardCopies = new Dictionary<int, int>();
            for (int i = 0; i < cards.Count; i++)
            {
                Console.WriteLine($"Adding Card {i}");
                cardCopies.Add(i, 1);
            }

            Console.WriteLine($"Card Count: {cards.Count}");
            Console.WriteLine($"cardCopies Count: {cardCopies.Count}");

            for (int i = 0; i < cardCopies.Count; i++)
            {
                Console.WriteLine($"Accessing Card {i}");

                var numberOfCards = cardCopies[i];
                var nextCount = cards[i].MatchingCardValues;
                Console.WriteLine($"Matching Count is is {nextCount} adding {numberOfCards} copies of next {nextCount} cards");
                //how many cards after this do we add
                for (int j = i+1; j < i+1+nextCount; j++)
                {
                    //how many times we add new copies of the next cards
                    if (j < cards.Count)
                    {
                        Console.WriteLine($"Adding a copyt of card {j} new total being {cardCopies[j] + 1}");
                        cardCopies[j] = cardCopies[j] + numberOfCards;
                    }
                }
                for (int x = 0; x < cardCopies.Count; x++)
                {
                    Console.WriteLine(cardCopies[x]);
                }
                Console.WriteLine("%%%%%%%%%%%%%%%%%%%%%%");
            }

            int cardSum = 0;
            for (int i = 0; i < cardCopies.Count; i++)
            {
                Console.WriteLine(cardCopies[i]);
                cardSum += cardCopies[i];
            }
            Console.WriteLine($"cardSum{cardSum}");

        }


        public void Run()
        {

        }
    }
}
