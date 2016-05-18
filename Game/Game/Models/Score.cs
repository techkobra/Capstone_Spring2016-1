using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ReviewsApp.Models
{
    public class Score
    {
        [Key, Column(Order = 0), DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int Points { get; set; }
        public DateTime? Date { get; set; }
        public int UserId { get; set; }
        public virtual ApplicationUser User { get; set; }
    }
}