using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OgrenciDersPanosu.Models
{
    public class LoginOgrenci
    {
        [Required]
        public string OgrenciNo { get; set; }
        [Required]
        public string Sifre { get; set; }
    }
    public class RegisterOgrenci
    {
        [Required]
        public string OgrenciNo { get; set; }
        [Required]
        public string OgrenciIsim { get; set; }
        [Required]
        public string OgrenciSoyisim { get; set; }
        [Required]
        public string Sifre { get; set; }
    }
    public class LoginOgretmen
    {
        [Required]
        public string OgretmenId { get; set; }
        [Required]
        public string Sifre { get; set; }
    }
    public class RegisterOgretmen
    {
        [Required]
        public string OgretmenId { get; set; }
        [Required]
        public string OgretmenIsim { get; set; }
        [Required]
        public string OgretmenSoyisim { get; set; }
        [Required]
        public string Sifre { get; set; }
    }
    public class LoginAdmin
    {
        [Required]
        public string AdminId { get; set; }
        [Required]
        public string Sifre { get; set; }
    }
    public class RegisterAdmin
    {
        [Required]
        public string AdminId { get; set; }
        [Required]
        public string AdminIsim { get; set; }
        [Required]
        public string AdminSoyisim { get; set; }
        [Required]
        public string Sifre { get; set; }
    }

}