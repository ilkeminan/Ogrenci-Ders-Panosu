using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace OgrenciDersPanosu.Models
{
    [Table("Ogrenci")]
    public class OgrenciModel
    {
        [Key]
        public string OgrenciId { get; set; } 

        public string Ad { get; set; }

        public string Soyad { get; set; }

        public virtual ICollection<Not> Notlar { get; set; }
        
    }
}