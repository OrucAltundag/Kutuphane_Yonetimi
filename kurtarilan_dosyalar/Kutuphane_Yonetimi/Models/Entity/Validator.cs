using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc; // AllowHtml için gerekebilir

namespace Kutuphane_Yonetimi.Models.Entity
{
    // Bu kısım Entity Framework'ün oluşturduğu sınıfla (partial) birleşir.
    [MetadataType(typeof(TBL_MESAJLAR_METADATA))]
    public partial class TBL_MESAJLAR
    {
        // Burası boş kalmalı.
    }

    // Kuralları buraya yazıyoruz
    public class TBL_MESAJLAR_METADATA
    {
        [Required(ErrorMessage = "Lütfen alıcı mail adresini yazınız.")]
        [EmailAddress(ErrorMessage = "Geçersiz mail formatı! (Örn: isim@site.com)")]
        public string ALICI { get; set; }

        [Required(ErrorMessage = "Konu başlığı boş bırakılamaz.")]
        [StringLength(50, ErrorMessage = "Konu en fazla 50 karakter olabilir.")]
        public string KONU { get; set; }

        [Required(ErrorMessage = "Mesaj içeriği boş bırakılamaz.")]
        public string ICERIK { get; set; }
    }

    // Orijinal sınıfı Metadata ile bağlıyoruz
    [MetadataType(typeof(TBL_PERSONEL_METADATA))]
    public partial class TBL_PERSONEL
    {
        // Boş kalacak
    }

    // Kuralları buraya yazıyoruz
    public class TBL_PERSONEL_METADATA
    {
        [Required(ErrorMessage = "TC Kimlik numarası boş bırakılamaz.")]
       
        [RegularExpression(@"^[0-9]{11}$", ErrorMessage = "TC Kimlik No tam 11 haneli olmalı ve sadece rakamlardan oluşmalıdır.")]
        public string TC { get; set; }

        [Required(ErrorMessage = "Şifre boş bırakılamaz.")]
        public string SIFRE { get; set; }

        // NOT: Eğer 'PERSONEL' (Ad Soyad) alanına [Required] eklersen, 
        // Giriş Yap ekranında Ad Soyad girilmediği için Model her zaman geçersiz (Invalid) olur.
        // O yüzden sadece TC ve Şifre'ye kural yazıyoruz şimdilik.
    }

    [MetadataType(typeof(TBL_UYELER_METADATA))]
    public partial class TBL_UYELER
    {
        // Boş
    }

    public class TBL_UYELER_METADATA
    {
        [Required(ErrorMessage = "Üye adı boş bırakılamaz.")]
        [StringLength(20, ErrorMessage = "Ad en fazla 20 karakter olabilir.")]
        public string AD { get; set; }

        [Required(ErrorMessage = "Üye soyadı boş bırakılamaz.")]
        [StringLength(20, ErrorMessage = "Soyad en fazla 20 karakter olabilir.")]
        public string SOYAD { get; set; }

        [Required(ErrorMessage = "Mail adresi boş bırakılamaz.")]
        [EmailAddress(ErrorMessage = "Geçersiz mail formatı!")]
        public string MAIL { get; set; }

        [Required(ErrorMessage = "Kullanıcı adı boş bırakılamaz.")]
        public string KULLANICIADI { get; set; }

        [Required(ErrorMessage = "Telefon numarası zorunludur.")]
        // Regex Açıklaması: ^[1-9] (0 ile başlayamaz), [0-9]{9} (devamında 9 rakam), $ (bitir)
        [RegularExpression(@"^[1-9][0-9]{9}$", ErrorMessage = "Telefon 10 hane olmalı ve başında 0 olmamalıdır. (Örn: 5321234567)")]
        public string TELEFON { get; set; }


        public string FOTOGRAF { get; set; }

        // Okul ve Fotoğraf zorunlu değilse [Required] koyma.
        public string OKUL { get; set; }
    }
}