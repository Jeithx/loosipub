namespace Core.Constants
{
    public static class Messages
    {
        public static string UnAuthorize = "Geçerli token bulunamadı!";
        public static string KullaniciKaydiTamamlanmadi = "Hesabınız hala inceleme aşamasında.";

        public static string AddingProcessSuccessful = "Ekleme işlemi başarıyla tamamlandı.";
        public static string AddingProcessFailed = "Ekleme işlemi sırasında bir hata oluştu.";
        public static string UpdateProcessSuccessful = "Güncelleme işlemi başarıyla tamamlandı.";
        public static string UpdateProcessFailed = "Güncelleme işlemi sırasında bir hata oluştu.";
        public static string DeleteProcessFailed = "Silme işlemi sırasında bir hata oluştur.";
        public static string DeleteProcessSuccessful = "Silme işlemi başarıyla tamamlandı.";
        public static string GetProcessFailed = "Listeleme işlemi sırasında bir hata oluştu.";
        public static string GetProcessSuccessful = "Listeleme işlemi başarıyla tamamlandı.";

        public static string ProcessSuccess = "İşlem başarıyla tamamlandı.";
        public static string ProcessFailed = "İşlem başarısız!";

        public static string TokenCreated = "Token oluşturuldu";
        public static string TokenError = "Token oluşturulamadı!";

        public static string LoginError = "Kullanıcı adı veya şifre hatalı";
        public static string LoginSuccessful = "Giriş başarılı";
        public static string UserNotFound = "Kullanıcı bulunamadı";
        public static string UserAlreadyExists = "Bu mail adresi üzerine mevcut bir kayıt var.";
        public static string UserRegistered = "Kullanıcı başarıyla kaydedildi";

        public static string NotFound = "Istenilen veri bulunamadı.";
        public static string RecaptchaError = "Lütfen güvenliği doğrulayınız!";

        public static string RequiredWar = "Lütfen tüm bilgileri eksiksiz giriniz.";
        public static string Processing = "İşlem başarıyla başlatıldı. Tamamalandığında size mail ile bildirilecektir.";
        public static string NotificationSuccess = "Bildirim gönderme işlemi başarıyla tamamlandı.";
        public static string NotificationFailed = "Bildirim gönderme işlemi sırasında hata oluştu.";
        public static string PasswordWrong = "Şifre hatalı.";
        public static string PasswordStrong = "Şifre uygun değildir";
        public static string StoreAddressAlreadyExists = "Store adres bulunmaktadır";
        public static string StockDecreaseSuccessful = "Stok başarıyla düşürüldü.";
        public static string InsufficientStock = "Yetersiz stok.";
        public static string ProductNotFound = "Ürün bulunamadı.";
        public static string StockUpdateFailed = "Stok güncelleme işlemi başarısız oldu.";

        public static string TagAlreadyExists = "Bu isimde bir etiket zaten mevcut.";
        public static string SenderAndReceiverCannotBeSame = "Gönderen ve alıcı aynı olamaz.";

        public static string UserIsLocked = "Hesapta birden fazla hatalı deneme olmasından dolayı kilitlenmiştir.";
    }
}
