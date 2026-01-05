using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kutuphane_Yonetimi.Models.Siniflarim
{
    public class PanelViewModel
    {
        public int ToplamKitap { get; set; }
        public int BugunOduncSayisi { get; set; }
        public List<BugunOduncDto> BugunOdunc { get; set; }
        public List<BugunOduncDto> BugunSonIade { get; set; }
        public int ToplamUye { get; set; }
        public int KategoriSayisi { get; set; }
        public List<KategoriDto> OnemliKategoriler { get; set; }
        public string EnPopulerKitap { get; set; }
        public string SonEklenenKitap { get; set; }
        public string SonUye { get; set; }
        public string EnAktifUye { get; set; }
        public int BugunIadeSayisi { get; set; }
        public int BugunSonIadeSayisi { get; set; }

        public List<UyeOranDto> TopAktifUyeler { get; set; }
        public List<UyeOranDto> TopCezaUyeler { get; set; }
    }

    public class BugunOduncDto
    {
        public string KitapAdi { get; set; }
        public string UyeAdi { get; set; }
    }

    public class KategoriDto
    {
        public string Kategori { get; set; }
        public int KitapSayisi { get; set; }
    }

    public class UyeOranDto
    {
        public string AdSoyad { get; set; }
        public int Deger { get; set; }   // AktiviteSayisi veya CezaSayisi
        public int Oran { get; set; }    // Yüzdelik oran
    }

}