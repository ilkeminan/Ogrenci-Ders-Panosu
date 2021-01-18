using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace OgrenciDersPanosu.Models
{
    [Table("Begeni")]
    public class Begeni
    {
        [Key]
        public string BegeniId { get; set; }
        public string GonderiId { get; set; }
        public string begenenID { get; set; }
        public DateTime zaman { get; set; }
    }
}