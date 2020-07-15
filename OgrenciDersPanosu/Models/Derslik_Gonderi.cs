using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace OgrenciDersPanosu.Models
{
    public class Derslik_Gonderi
    {
        [Key]
        public string GonderiId { get; set; }
        public string Gonderi { get; set; }
        public string UstGonderiID { get; set; }
        public string gonderenIsmi { get; set; }
        public DateTime zaman { get; set; }

        public string dersId { get; set; }
        public Ders Ders { get; set; }
    }
}