using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace OgrenciDersPanosu.Models
{
    public class Ders
    {
        //[ForeignKey("Ogretmen")]

        [Key]
        public string DersId { get; set; }

        public string DersAdi { get; set; }


        public OgretmenModel Ogretmen { get; set; }
        public string OgretmenId { get; set; }
        public virtual ICollection<Not> Notlar {get;set;}

        public virtual ICollection<Derslik_Gonderi> Gonderiler { get; set; }

    }
}