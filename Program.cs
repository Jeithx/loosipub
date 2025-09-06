using Generator;

internal class Program
{
    private static void Main(string[] args)
    {
        Console.WriteLine("Program başlatılıyor...");

        Console.WriteLine("İşlem başlatılıyor...");
        string anaDizin = Directory.GetCurrentDirectory();
        Console.WriteLine("Dosya yolu bulunuyor...");
        // Ana dizinine kadar olan kısmı sondan sil
        for (int i = 0; i < 4; i++)
        {
            anaDizin = Directory.GetParent(anaDizin).FullName;
        }

        #region Dizin Tanımlamaları
        var sharedKlasoru = anaDizin + "\\Core";
        var domainKlasoru = anaDizin + "\\Entities";
        var applicationKlasoru = anaDizin + "\\Business";
        var apiKlasoru = anaDizin + "\\API";
        var dataAccessAbstractKlasoru = anaDizin + "\\DataAccess";
        var dataConcreteAbstractKlasoru = anaDizin + "\\DataAccess";
        #endregion

        #region Veri karşılaştırma için çekilen dosyalar
        string[] modeller = Directory.GetFiles(domainKlasoru + "\\Models");
        modeller = modeller.Where(m => !Path.GetFileName(m).StartsWith("_"))
     .ToArray();


        string[] dataAccess = Directory.GetFiles(dataAccessAbstractKlasoru + "\\Abstract");
        dataAccess = dataAccess.Where(m => !Path.GetFileName(m).StartsWith("_"))
     .ToArray();
        #endregion


        Console.WriteLine("Dosyalar taranıyor...");
        List<string> eklenecekModeller = new List<string>();
        foreach (string dosyaYolu in modeller)
        {
            string dosyaIsmi = Path.GetFileName(dosyaYolu).Split(".cs")[0];
            Console.WriteLine("Dosya: " + dosyaIsmi);

            //Context klasörü dışındakilere işlem yapılacak
            if (!dosyaIsmi.Contains("Context"))
            {
                var yeniDosyaIsmi = "I" + dosyaIsmi + "DAL.cs";
                var dosyaVarmi = dataAccess.Any(x => x.Contains(yeniDosyaIsmi));
                if (!dosyaVarmi)
                {
                    eklenecekModeller.Add(dosyaIsmi);
                    Console.WriteLine("Eklemesi yapılacak model: " + dosyaIsmi);
                }
            }
        }

        if (eklenecekModeller.Count() == 0)
        {
            Console.WriteLine("Mevcut durum güncel.");
        }
        else
        {
            Console.WriteLine("Dosya tarama bitti. Ekleme işlemi başlatılıyor...");
            foreach (var item in eklenecekModeller)
            {
                if (item.Contains("UserSubscriptionPlan")) // model adını değiştir
                {
                    Console.WriteLine("Ekleniyor: " + item);
                    string yeniDosyaYolu, metin, itemTolower;

                    Console.WriteLine("RD ve WD oluşturuluyor..");
                    DtoGenerator.DtoEkle(sharedKlasoru, item, out yeniDosyaYolu, out metin);
                    Console.WriteLine("RD ve WD oluşturuldu.");

                    Console.WriteLine("DataAccessAbstractEkle oluşturuluyor...");
                    DataAccessAbstractHelper.DataAccessAbstractEkle(dataAccessAbstractKlasoru, item, out yeniDosyaYolu, out metin);
                    Console.WriteLine("DataAccessAbstractEkle oluşturuldu.");

                    Console.WriteLine("DataAccessConcreteEkle oluşturuluyor...");
                    DataAccessConcreteHelper.DataAccessConcreteEkle(dataConcreteAbstractKlasoru, item, out yeniDosyaYolu, out metin);
                    Console.WriteLine("DataAccessConcreteEkle oluşturuldu.");

                    Console.WriteLine("Interfaceler oluşturuluyor...");
                    InterfaceGenerator.InterfaceCreate(applicationKlasoru, item, out yeniDosyaYolu, out metin);
                    Console.WriteLine("Interfaceler oluşturuldu.");

                    Console.WriteLine("Services oluşturuluyor...");
                    ServiceGenerator.ServiceCreate(applicationKlasoru, item, out yeniDosyaYolu, out metin);
                    Console.WriteLine("Services oluşturuldu.");

                    Console.WriteLine("Api ekleniyor...");
                    ApiGenerator.ApiEkle(apiKlasoru, item, out yeniDosyaYolu, out metin);
                    Console.WriteLine("Api eklendi.");
                }
            }
        }
    }
}

