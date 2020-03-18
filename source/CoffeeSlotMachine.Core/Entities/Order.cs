using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace CoffeeSlotMachine.Core.Entities
{
    /// <summary>
    /// Bestellung verwaltet das bestellte Produkt, die eingeworfenen Münzen und
    /// die Münzen die zurückgegeben werden.
    /// </summary>
    public class Order : EntityObject
    {
        /// <summary>
        /// Datum und Uhrzeit der Bestellung
        /// </summary>
        public DateTime Time { get; set; }

        /// <summary>
        /// Werte der eingeworfenen Münzen als Text. Die einzelnen 
        /// Münzwerte sind durch ; getrennt (z.B. "10;20;10;50")
        /// </summary>
        public String ThrownInCoinValues { get; set; }

        /// <summary>
        /// Zurückgegebene Münzwerte mit ; getrennt
        /// </summary>
        public String ReturnCoinValues { get; set; }

        /// <summary>
        /// Summe der eingeworfenen Cents.
        /// </summary>
        public int ThrownInCents { get; set; }

        /// <summary>
        /// Summe der Cents die zurückgegeben werden
        /// </summary>
        public int ReturnCents { get; set; }


        public int ProductId { get; set; }

        [ForeignKey(nameof(ProductId))]
        public Product Product { get; set; }

        /// <summary>
        /// Kann der Automat mangels Kleingeld nicht
        /// mehr herausgeben, wird der Rest als Spende verbucht
        /// </summary>
        public int DonationCents { get; set; }

        public Order()
        {
            Time = DateTime.Now;
            ThrownInCoinValues = "";
            ReturnCoinValues = "";
        }

        /// <summary>
        /// Münze wird eingenommen.
        /// </summary>
        /// <param name="coinValue"></param>
        /// <returns>isFinished ist true, wenn der Produktpreis zumindest erreicht wurde</returns>
        public bool InsertCoin(int coinValue)
        {
            bool result = false;

            ThrownInCents += coinValue;
            if(ThrownInCoinValues == "")
            {
                ThrownInCoinValues = $"{coinValue}";
            }
            else
            {
                ThrownInCoinValues = $"{ThrownInCoinValues};{coinValue}";
            }
            if (ThrownInCents >= Product.PriceInCents)
            {
                result = true;
            }

            return result;
        }

        /// <summary>
        /// Übernahme des Einwurfs in das Münzdepot.
        /// Rückgabe des Retourgeldes aus der Kasse. Staffelung des Retourgeldes
        /// hängt vom Inhalt der Kasse ab.
        /// </summary>
        /// <param name="coins">Aktueller Zustand des Münzdepots</param>
        public IEnumerable<Coin> FinishPayment(IEnumerable<Coin> coins)
        {
            Coin[] coinArray = coins
                .OrderByDescending(c => c.CoinValue)
                .ToArray();
                
            if(ReturnCents > 0)
            {
                for (int i = 0; i < coinArray.Length; i++)
                {
                    while (coinArray[i].Amount > 0 && ReturnCents - coinArray[i].CoinValue >= 0)
                    {
                        ReturnCoinValues = $"{ReturnCoinValues}{coinArray[i].CoinValue};";
                        ReturnCents -= coinArray[i].CoinValue;
                        coinArray[i].Amount--;
                    }
                }
            }

            if (ReturnCoinValues.Length > 0)
                ReturnCoinValues = ReturnCoinValues.Remove(ReturnCoinValues.Length - 1);

            DonationCents = ReturnCents;

            return coinArray.OrderBy(c => c.CoinValue);
        }
    }
}
