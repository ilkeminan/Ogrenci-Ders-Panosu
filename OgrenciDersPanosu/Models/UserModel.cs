using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OgrenciDersPanosu.Models
{
    public class LoginOgrenci
    {
        [DisplayName("Öğrenci No"), Required(ErrorMessage ="{0} alanı zorunludur.")]
        public string OgrenciNo { get; set; }
        [DisplayName("Şifre"), Required(ErrorMessage = "{0} alanı zorunludur.")]
        public string Sifre { get; set; }
    }
    public class RegisterOgrenci
    {
        [DisplayName("Öğrenci No"), Required(ErrorMessage = "{0} alanı zorunludur.")]
        public string OgrenciNo { get; set; }
        [DisplayName("Öğrenci İsim"), Required(ErrorMessage = "{0} alanı zorunludur.")]
        public string OgrenciIsim { get; set; }
        [DisplayName("Öğrenci Soyisim"), Required(ErrorMessage = "{0} alanı zorunludur.")]
        public string OgrenciSoyisim { get; set; }
        [DisplayName("Şifre"), Required(ErrorMessage = "{0} alanı zorunludur.")]
        public string Sifre { get; set; }
    }
    public class LoginOgretmen
    {
        [DisplayName("Öğretmen Id"), Required(ErrorMessage = "{0} alanı zorunludur.")]
        public string OgretmenId { get; set; }
        [DisplayName("Şifre"), Required(ErrorMessage = "{0} alanı zorunludur.")]
        public string Sifre { get; set; }
    }
    public class RegisterOgretmen
    {
        [DisplayName("Öğretmen Id"), Required(ErrorMessage = "{0} alanı zorunludur.")]
        public string OgretmenId { get; set; }
        [DisplayName("Öğretmen İsim"), Required(ErrorMessage = "{0} alanı zorunludur.")]
        public string OgretmenIsim { get; set; }
        [DisplayName("Öğretmen Soyisim"), Required(ErrorMessage = "{0} alanı zorunludur.")]
        public string OgretmenSoyisim { get; set; }
        [DisplayName("Şifre"), Required(ErrorMessage = "{0} alanı zorunludur.")]
        public string Sifre { get; set; }
    }
    public class LoginAdmin
    {
        [DisplayName("Admin Id"), Required(ErrorMessage = "{0} alanı zorunludur.")]
        public string AdminId { get; set; }
        [DisplayName("Şifre"), Required(ErrorMessage = "{0} alanı zorunludur.")]
        public string Sifre { get; set; }
    }
    public class RegisterAdmin
    {
        [DisplayName("Admin Id"), Required(ErrorMessage = "{0} alanı zorunludur.")]
        public string AdminId { get; set; }
        [DisplayName("Admin İsim"), Required(ErrorMessage = "{0} alanı zorunludur.")]
        public string AdminIsim { get; set; }
        [DisplayName("Admin Soyisim"), Required(ErrorMessage = "{0} alanı zorunludur.")]
        public string AdminSoyisim { get; set; }
        [DisplayName("Şifre"), Required(ErrorMessage = "{0} alanı zorunludur.")]
        public string Sifre { get; set; }
    }

}