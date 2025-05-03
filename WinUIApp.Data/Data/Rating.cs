using System;

namespace WinUiApp.Data.Data
{
    public class Rating
    {
        public int RatingID { get; set; }
        public int DrinkID { get; set; }
        public int UserID { get; set; }
        public float? RatingValue { get; set; }
        public DateTime? RatingDate { get; set; }
        public bool? IsActive { get; set; }

        public User User { get; set; }
        public Drink Drink { get; set; }
    }
}