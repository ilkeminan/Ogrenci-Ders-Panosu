using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace OgrenciDersPanosu.Models
{
    public class Not
    {

        //[ForeignKey("Ders")]

        public string DersId { get; set; }
        public Ders Ders { get; set; }
        

        //[ForeignKey("Ogrenci")]

        public string OgrenciId { get; set; }
        public OgrenciModel Ogrenci { get; set; }
        

        public int Sinav1 { get; set; }

        public int Sinav2 { get; set; }

        public int Sinav3 { get; set; }

        public int Sozlu1 { get; set; }

        public int Sozlu2 { get; set; }

        public int Sozlu3 { get; set; }

        [Key]
        public string NotId { get; set; }

    }
}