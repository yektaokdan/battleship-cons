using System;

namespace amiralBatti.yekta.okdan
{
    class Program
    {
        //Gemilerin rastgele yerleştirilmesi için random komutunu kullanacağım.
        static Random random = new Random();// belirli metodlarda çağıracağım için static yapıda olması gerekiyor.

        class Pozisyon
        {

            public int x { get; set; }
            public int y { get; set; }//Pozisyonların x y değerleri.

            public bool vurulduMu { get; set; }//Pozisyonun vurulup vurulmadığını ifade eden değişken.
            public Pozisyon() { x = 0; y = 0; }
            public Pozisyon(int x, int y)
            {
                //Constructor yapılandırıcı metod.
                this.x = x;
                this.y = y;
                vurulduMu = false;
            }
        }
        class Gemi
        {
            //Gemi sınıfını burada oluşturdum.
            //govdeyı belirten govde dizisi.
            public Pozisyon[] govde { get; set; }//Pozisyon üzerinden oluşturduğum gove dizisi ve metodları.
            public int gemiBoyut { get; set; }//Gemi boyutu ve set  get metodu.

            public Gemi(int gemiBoyut)
            {
                //boyuta gore yeni gemi yaratan constructor.
                govde = new Pozisyon[gemiBoyut];
                for (int i = 0; i < gemiBoyut; i++)
                {
                    govde[i] = new Pozisyon();
                }
            }
            public Pozisyon vuruyorMu(Pozisyon atis)
            {
                foreach (Pozisyon pozisyon in govde)
                {
                    if (atis.x == pozisyon.x && atis.y == pozisyon.y)//Parametre olarak gelen atış pozisyonu, govdelerden biriyle eşit haldeyse pozisyon nesnesi dönüyor.
                    {
                        return pozisyon;
                    }
                }
                return null; // Bu değer döner ise isabet başarısız olmuştur.
            }
        }
        class Oyuncu
        {
            //Oyuncudan türeyecek olan varlığın özellikleri
            public string isim { get; set; }//Oyuncunun ismi.
            public int skor { get; set; }//Oyuncunun puan değeri.
            public int oyuncuNum { get; set; }//Oyuncu sınıfından üremiş varlığın numarası.
            public Gemi[] gemiler { get; set; }//Oyuncu sınıfından üremiş varlığın gemileri.

            public Oyuncu(string isim, int oyuncuNum)//Oyuncunun özellikleri için constructor metod.
            {
                this.isim = isim;//eşitlendi
                this.oyuncuNum = oyuncuNum;//eşitlendi
                this.skor = 0;//skor sıfırlandı
                this.gemiler = new Gemi[3]; //her oyuncunun üçer adet gemisi olacak.
            }

            public bool gemiMevcutMu()
            {
                //Eğer oyuncu sınıfıdan üremiş varlığın vurulmamış gemisi halen mevcutsa oyun devam edecek.
                foreach (Gemi gemi in gemiler)
                {
                    //her geminin gövdesini kontrol ediyorum.
                    foreach (Pozisyon pozisyon in gemi.govde)
                    {
                        if (!pozisyon.vurulduMu)
                        {
                            //eğer pozisyonlardaki gemilerden en az bir tanesi vurulmadıysa oyun devam edecek.
                            return true;
                        }
                    }
                }
                return false;//aksi durum için ise false değeri dönüyor.
            }

            private bool ustUsteMi(int x)
            {

                //aynı satır da birden fazla gemi olmamasını sağlayacak metod.
                foreach (Gemi gemi in gemiler)
                {
                    //Her geminin govdesindeki x degerini kontrol ettim
                    if (gemi != null)//Gemi boş değil ise işleme devam ediyorum.
                    {
                        if (gemi.govde[0].x == x)//Aynı satırdalar ise true döner.
                        {
                            return true;
                        }
                    }
                }
                return false; //Aksi durum için ise false değeri dönüyor.
            }

            public void gemiOlustur(Tahta oyunTahtasi)
            {
                for (int i = 2; i < 5; i++)//2 ile 5 arası değer döndürerek toplamda 3 gemi oluşturmasını sağlyorum.
                {
                    //yeni gemi nesnesi türetiyorum
                    gemiler[i - 2] = new Gemi(i);
                    int x;
                    do
                    {
                        if (oyuncuNum == 1)//oyuncu1 ise tahtanın üstünde
                        {
                            x = random.Next(0, oyunTahtasi.gemiBoyut / 2);
                        }
                        else //oyuncu2 ise tahtanın alt tarafına gemiler yerleştirilir.
                        {
                            x = random.Next(oyunTahtasi.gemiBoyut / 2, oyunTahtasi.gemiBoyut);
                        }

                    } while (ustUsteMi(x));
                    //gemilerin gövdelerini oluşturuyorum.
                    gemiler[i - 2].govde[0] = new Pozisyon(x, random.Next(0, oyunTahtasi.gemiBoyut - i));
                    for (int j = 0; j < i; j++)
                    {
                        //gemilerin gövdelerini sağa dogru yerleştirdim
                        gemiler[i - 2].govde[j] = new Pozisyon(x, gemiler[i - 2].govde[0].y + j);
                    }
                }
            }
            public void atisYap(Tahta oyunTahtasi, Pozisyon atis)
            {
                bool vurduMu = false;
                if (oyuncuNum == 1)//oyuncu1 ise
                {
                    foreach (Gemi gemi in oyunTahtasi.oyuncu2.gemiler)
                    {
                        //atış hamleleri(pozisyonlar) oyuncunun her gemisinin her gövdesi için kontrol ediliyor.
                        Pozisyon vuruyormu = gemi.vuruyorMu(atis);
                        if (vuruyormu != null)
                        {
                            foreach (Pozisyon govde in gemi.govde)
                            {
                                if (govde.x == vuruyormu.x && govde.y == vuruyormu.y && !govde.vurulduMu)
                                {
                                    //eger vurulduysa skor arttılıyor ve govdenın vuruldumu degerı true olarak doner.
                                    govde.vurulduMu = true;
                                    this.skor++;
                                    vurduMu = true;
                                    break;
                                }
                            }
                        }
                    }
                }
                else
                {
                    //oyuncu2 ise aynı islemler diger rakip oyuncu icin yapılıyor.
                    foreach (Gemi gemi in oyunTahtasi.oyuncu1.gemiler)
                    {
                        //atış hamleleri(pozisyonlar) oyuncunun her gemisinin her gövdesi için kontrol ediliyor.
                        Pozisyon vuruyormu = gemi.vuruyorMu(atis);
                        if (vuruyormu != null)
                        {
                            foreach (Pozisyon govde in gemi.govde)
                            {
                                if (govde.x == vuruyormu.x && govde.y == vuruyormu.y && !govde.vurulduMu)
                                {
                                    //eger vurulduysa skor arttılıyor ve govdenın vuruldumu degerı true olarak doner.
                                    govde.vurulduMu = true;
                                    this.skor++;
                                    vurduMu = true;
                                    break;
                                }
                            }
                        }
                    }
                }

                if (vurduMu)
                {
                    Console.WriteLine("Rakip gemiyi doğru tahmin ettin! İsabetli atış, skor:" + skor);
                }
                else
                    Console.WriteLine("Iskaladın!! Bir sonraki sefere");
            }
        }
        class Tahta
        {
            public int[,] oyunTahta { get; set; }//parantez içine yazdığım virgül sayesinde 10 a 10luk veya farklı iki değer için dizi oluşturabiliyorum.
            public Oyuncu oyuncu1 { get; set; }//Oyuncu sınıfından oyuncu değerleri atıyorum.
            public Oyuncu oyuncu2 { get; set; }//Oyuncu sınıfından oyuncu değerleri atıyorum.
            public int gemiBoyut { get; set; }//int türünde gemiboyut değeri belirtiyorum.
            public Tahta(Oyuncu oyuncu1, Oyuncu oyuncu2, int gemiBoyut)
            {
                //Tahta sınıfına verdiğim değerler için constructor metodu.
                this.oyuncu1 = oyuncu1;//Oyuncu sınıfından türeyen bu nesneyi yapılandırıyorum.
                this.oyuncu2 = oyuncu2;//Oyuncu sınıfından türeyen bu nesneyi yapılandırıyorum.
                this.gemiBoyut = gemiBoyut;//boyutu kendi boyut nesneme eşitliyorum.
            }
            public void oyuncuTahtaGuncelle()
            {
                oyunTahta = new int[gemiBoyut, gemiBoyut];
                for (int i = 0; i < gemiBoyut; i++)
                {
                    for (int j = 0; j < gemiBoyut; j++)
                    {
                        //oyuncu tahtasının tüm değerlerini ilk olarak sıfıra eşitliyorum.
                        oyunTahta[i, j] = 0;
                    }
                }
                foreach (Gemi gemi in oyuncu1.gemiler)
                {
                    foreach (Pozisyon pozisyon in gemi.govde)
                    {
                        //Vurulan oyuncu1 gemilerini -1, vurulmayanları ise 1 olarak işaretledim.
                        if (pozisyon.vurulduMu)
                        {
                            oyunTahta[pozisyon.x, pozisyon.y] = -1;
                        }
                        else
                        {
                            oyunTahta[pozisyon.x, pozisyon.y] = 1;
                        }
                    }
                }
                foreach (Gemi gemi in oyuncu2.gemiler)
                {
                    foreach (Pozisyon pozisyon in gemi.govde)
                    {
                        //Vurulan oyuncu2 gemilerini -2, vurulmayanları ise 2 olarak işaretledim.
                        if (pozisyon.vurulduMu)
                        {
                            oyunTahta[pozisyon.x, pozisyon.y] = -2;
                        }
                        else
                        {
                            oyunTahta[pozisyon.x, pozisyon.y] = 2;
                        }
                    }
                }
            }

            public void tahtaCiz(int oyuncuNum)
            {
                Console.Write(" ");
                for (int i = 0; i < gemiBoyut; i++)
                {
                    Console.Write(i + " ");
                }
                Console.WriteLine();
                for (int i = 0; i < gemiBoyut; i++)
                {
                    Console.Write(i + " ");
                    for (int j = 0; j < gemiBoyut; j++)
                    {
                        if (oyuncuNum == 0)
                        {
                            //parametre sıfır ise tahta gizlenmeden ekrana basılıyor.
                            Console.Write(oyunTahta[i, j] + " ");
                        }
                        else
                        {
                            if (oyunTahta[i, j] == oyuncuNum)
                            {
                                //tahta rakip oyuncu numarası ise 0 ile gizleniyor.
                                Console.Write("0 ");
                            }
                            //değilse değer aynen yazılıyor.
                            else Console.Write(oyunTahta[i, j] + " ");
                        }
                    }
                    Console.WriteLine();
                }
            }

            public bool oyunBittiMi()
            {
                //oyunculardan birinin gemisi kalmadıysa oyun bitiyor.
                if (oyuncu1.gemiMevcutMu() && oyuncu2.gemiMevcutMu())//burada her iki oyuncunundan gemilerinin kalıp kalmadıgını kontrol ediyorum
                {
                    return true;
                }
                else return false;
            }
        }

        static Pozisyon Oyna(Oyuncu oyuncu)
        {
            //oyuncu sınıfından türeyen nesnelerin satır ve sütun değerlerini tahmin ederek oynamanı sağlayan metod.
            //input olarak alınan x ve y değerleri pozisyon nesnesi olarak döndürülüyor.
            int x, y;
            Console.WriteLine("Atış Nereye?");
            Console.Write("Satır giriniz: ");
            x = int.Parse(Console.ReadLine());
            Console.Write("Sütun giriniz: ");
            y = int.Parse(Console.ReadLine());
            Console.WriteLine("");
            return new Pozisyon(x, y);
        }
        static void Main(string[] args)
        {
        baslangic:
            Oyuncu oyuncu1 = new Oyuncu("Oyuncu 1", 1);
            Oyuncu oyuncu2 = new Oyuncu("Oyuncu 2", 2);
            Tahta oyunTahtasi = new Tahta(oyuncu1, oyuncu2, 10);
            oyunTahtasi.oyuncu1.gemiOlustur(oyunTahtasi);
            oyunTahtasi.oyuncu2.gemiOlustur(oyunTahtasi);
            oyunTahtasi.oyuncuTahtaGuncelle();
            /* 
             Oyuncuların tahtalarının görünürlüğü için ufak bir ayar ekledim açıklamaları şu şekilde
            oyunTahtasi.tahtaCiz(0); << komut but
            tahtaCiz 0 ise tamamı görünür, 1 ise oyuncu2 nin 2 ise oyuncu1 gemileri görünür
            */
            while (oyunTahtasi.oyunBittiMi())
            {
                //Oyun bitene kadar sırayla her iki oyuncuda hamle yapar.
                oyunTahtasi.tahtaCiz(2);
                Console.WriteLine("-----" + oyuncu1.isim + "in sırası-----");
                oyunTahtasi.oyuncu1.atisYap(oyunTahtasi, Oyna(oyuncu1));
                oyunTahtasi.oyuncuTahtaGuncelle();
                oyunTahtasi.tahtaCiz(1);
                Console.WriteLine("-----" + oyuncu2.isim + "in sırası-----");
                oyunTahtasi.oyuncu2.atisYap(oyunTahtasi, Oyna(oyuncu2));
                oyunTahtasi.oyuncuTahtaGuncelle();
            }
            Console.WriteLine("Oyun sona erdi..");
            if (oyunTahtasi.oyuncu1.skor > oyunTahtasi.oyuncu2.skor)//Hangi oyuncunun skoru daha yüksekse oyunu onun kazanmasını saglıyorum.
            {
                Console.WriteLine("Oyuncu 1 Kazandı!!!");
            }
            else
            {
                Console.WriteLine("Oyuncu 2 Kazandı!!!");
            }
            string cikti;
        hataYerTutucu://Kişi input sırasında hatalı bir girdi yaparsa tekrar bu konuma getiriyor ve soruyu bastan sorup if else döngüsüne tekrar sokuyor.
            Console.WriteLine("Tekrar oynamak ister misin ? Evet |E|, Hayır|H|");
            cikti = Console.ReadLine();
            if (cikti == "E")
            {
                goto baslangic;//perdenin ön tarafı olan kod kısmının en başına geri yolluyor ve oyun baştan başlıyor.
            }
            else if (cikti == "H")
            {
                Console.WriteLine("Görüşmek üzere !! Bir sonraki oyununda bol şans..");//Kişi hayırı seçerse sadece bir yazı çıktısı ile veda edip oyun bitiyor.
            }
            else
            {
                Console.WriteLine("Hatalı bir girdi yaptınız lütfen tekrar deneyin..");//Kişi hatalı bir girdi yaparsa yukarıda belirttiğim yer tutucunun satırına geri gidiyor ve döngüye baştan giriyor.
                goto hataYerTutucu;
            }
            Console.ReadKey();
        }
    }
}
