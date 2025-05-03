using System;

namespace WinUiApp.Data.Data
{
    public class Review
    {
        public int ReviewID { get; set; }
        public int RatingID { get; set; }
        public int? UserID { get; set; }
        public string Content { get; set; }
        public DateTime? CreationDate { get; set; }
        public bool? IsActive { get; set; }

        public Rating Rating { get; set; }
        public User User { get; set; }
    }
}
