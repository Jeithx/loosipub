using Azure.Core;
using Core.Utilities.RandomHelper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Dynamic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace CoreXNugetPackage.Utilities.VirtualPos
{
    public enum VposType
    {
        Garanti = 1,
        VakifBank = 2,
        VakifKatilim = 3,
        NestPay = 4
    }

    public class VPos
    {
        public static string SelectVPos(VposType type, PaymentRequest request)
        {
            if (VposType.Garanti == type)
                return GarantiProvider(request);
            else if (VposType.VakifBank == type)
                return VakifBankProvider(request);
            else if (VposType.VakifKatilim == type)
                return VakifKatilimProvider(request);
            else if (VposType.NestPay == type)
                return NestPayProvider(request);


            return "";
        }

        #region GarantiPos
        private static string GarantiProvider(PaymentRequest request)
        {
            string ipAddress = request.CustomerIpAddress;

            string terminaluserid = request.TerminalUserId;
            string terminalid = request.TerminalId;
            string terminalmerchantid = request.MerchantId;
            string terminalprovuserid = request.ProvUserId;
            string terminalprovpassword = request.ProvPassword;
            string storekey = request.TerminalStoreKey;//garanti sanal pos ekranından üreteceğimiz güvenlik anahtarı
            string mode = request.Mode;//PROD | TEST
            string successUrl = request.SuccessUrl;//Başarılı Url
            string errorurl = request.FailUrl;//Hata Url
            string type = request.Type;

            var parameterResult = new PaymentParameterResult();
            try
            {
                var parameters = new Dictionary<string, object>();

                //kart numarasından çizgi ve boşlukları kaldırıyoruz
                //string cardNumber = request.CardNumber.Replace("-", string.Empty);
                //cardNumber = cardNumber.Replace(" ", string.Empty).Trim();
                parameters.Add("cardnumber", request.CardNumber);

                parameters.Add("cardcvv2", request.CvvCode);//kart güvenlik kodu
                parameters.Add("cardexpiredatemonth", request.ExpireMonth);//kart bitiş ay'ı
                parameters.Add("cardexpiredateyear", request.ExpireYear);//kart bitiş yıl'ı
                parameters.Add("secure3dsecuritylevel", request.SecurityLevel);//SMS onaylı ödeme modeli 3DPay olarak adlandırılıyor.
                parameters.Add("mode", mode);
                parameters.Add("apiversion", "v0.01");
                parameters.Add("terminalprovuserid", terminalprovuserid);
                parameters.Add("terminaluserid", terminaluserid);
                parameters.Add("terminalmerchantid", terminalmerchantid);
                parameters.Add("terminalid", terminalid);
                parameters.Add("txntype", type);//direk satış
                parameters.Add("txncurrencycode", request.CurrencyIsoCode);//TL ISO code | EURO 978 | Dolar 840
                parameters.Add("motoind", "N");

                if (ipAddress.Equals("::1"))
                    parameters.Add("customeripaddress", "127.0.0.1");
                else
                    parameters.Add("customeripaddress", ipAddress);

                parameters.Add("orderaddressname1", request.CardHolderName);
                string ordId = Guid.NewGuid().ToString();
                parameters.Add("orderid", ordId);//sipariş numarası

                //işlem başarılı da olsa başarısız da olsa callback sayfasına yönlendirerek kendi tarafımızda işlem sonucunu kontrol ediyoruz
                parameters.Add("successurl", successUrl);//başarılı dönüş adresi
                parameters.Add("errorurl", errorurl);//hatalı dönüş adresi

                //garanti bankasında tutar bilgisinde nokta, virgül gibi değerler istenmiyor. 1.10 TL'lik işlem 110 olarak gönderilmeli. Yani tutarı 100 ile çarpabiliriz.
                string amount = (request.TotalAmount * 100m).ToString("0.##", new CultureInfo("en-US"));//virgülden sonraki sıfırlara gerek yok
                parameters.Add("txnamount", amount);

                string installment = request.Installment.ToString();
                if (request.Installment <= 1)
                    installment = string.Empty; //0 veya 1 olması durumunda taksit bilgisini boş gönderiyoruz

                parameters.Add("txninstallmentcount", installment);//taksit sayısı | boş tek çekim olur

                //garanti tarafından terminal numarasını 9 haneye tamamlamak için başına sıfır eklenmesi isteniyor. 9 haneli bir terminal numarasında buna ihtiyaç olmuyor.
                string _terminalid = string.Format("{0:000000000}", int.Parse(terminalid));
                string securityData = GetSHA1(terminalprovpassword + _terminalid).ToUpper();//provizyon şifresi ve 9 haneli terminal numarasının birleşimi ile bir hash oluşturuluyor
                string hashstr = terminalid + ordId + amount + successUrl + errorurl + type + installment + storekey + securityData;//ilgili veriler birleştirilip hash oluşturuluyor
                parameters.Add("secure3dhash", GetSHA1(hashstr).ToUpper());//ToUpper ile tüm karakterlerin büyük harf olması gerekiyor

                parameterResult.Parameters = parameters;
                parameterResult.Success = true;

                //Garanti Canlı https://sanalposprov.garanti.com.tr/servlet/gt3dengine
                //TEST https://sanalposprovtest.garanti.com.tr/servlet/gt3dengine
                parameterResult.PaymentUrl = new Uri("https://sanalposprov.garanti.com.tr/servlet/gt3dengine");
            }
            catch (Exception ex)
            {
                parameterResult.Success = false;
                parameterResult.ErrorMessage = ex.ToString();
            }

            return CreatePaymentForm(parameterResult.Parameters, parameterResult.PaymentUrl);
        }
        private static string CreatePaymentForm(IDictionary<string, object> parameters, Uri paymentUrl, bool appendSubmitScript = true)
        {
            if (parameters == null || !parameters.Any())
                throw new ArgumentNullException(nameof(parameters));

            if (paymentUrl == null)
                throw new ArgumentNullException(nameof(paymentUrl));

            var formId = "PaymentForm";
            var formBuilder = new StringBuilder();
            formBuilder.Append($"<form id=\"{formId}\" name=\"{formId}\" action=\"{paymentUrl}\" role=\"form\" method=\"POST\">");
            foreach (var parameter in parameters)
            {
                formBuilder.Append($"<input type=\"hidden\" name=\"{parameter.Key}\" value=\"{parameter.Value}\">");
            }
            formBuilder.Append("</form>");

            if (appendSubmitScript)
            {
                var scriptBuilder = new StringBuilder();
                scriptBuilder.Append("<script>");
                scriptBuilder.Append($"document.getElementById(\"{formId}\").submit();");
                scriptBuilder.Append("</script>");
                formBuilder.Append(scriptBuilder.ToString());
            }

            return formBuilder.ToString();
        }
        private static string GetSHA1(string SHA1Data)
        {
            SHA1 sha = new SHA1CryptoServiceProvider();
            string HashedPassword = SHA1Data;
            byte[] hashbytes = Encoding.GetEncoding("ISO-8859-9").GetBytes(HashedPassword);
            byte[] inputbytes = sha.ComputeHash(hashbytes);
            return GetHexaDecimal(inputbytes);
        }
        private static string GetHexaDecimal(byte[] bytes)
        {
            StringBuilder s = new StringBuilder();
            int length = bytes.Length;
            for (int n = 0; n <= length - 1; n++)
            {
                s.Append(String.Format("{0,2:x}", bytes[n]).Replace(" ", "0"));
            }
            return s.ToString();
        }
        #endregion
        #region VakıfBankPos
        private static string VakifBankProvider(PaymentRequest model)
        {
            string transactionId = Guid.NewGuid().ToString();
            string expiryDate = model.ExpireYear + string.Format("{0:00}", model.ExpireMonth);
            try
            {
                string successUrl = model.SuccessUrl;//Başarılı Url
                string failUrl = model.FailUrl;//Hata Url

                string installmentCount = ""; //Taksit Sayısı
                if (model.Installment > 1)
                {
                    installmentCount = model.Installment.ToString();
                }

                var sData = JsonConvert.SerializeObject(new
                {
                    expiryDate = CultureInfo.CurrentCulture.Calendar.ToFourDigitYear(model.ExpireYear) + string.Format("{0:00}", model.ExpireMonth),
                    cvvCode = model.CvvCode,
                    cardHolderName = model.CardHolderName,
                    coursePrice = model.TotalAmount.ToString().Replace(",", "."),
                    installment = installmentCount,
                    orderNumber = model.OrderNumber
                });

                string data = $"Pan=" + model.CardNumber +
                    "&ExpiryDate=" + expiryDate +
                    "&PurchaseAmount=" + model.TotalAmount.ToString().Replace(",", ".") +
                    "&Currency=" + model.CurrencyIsoCode +
                    "&BrandName=" + (model.CardNumber.Substring(0, 1) == "5" ? "200" : "100") +
                    "&VerifyEnrollmentRequestId=" + transactionId +
                    "&SessionInfo=" + RandomHelper.Encrypt(sData) +
                    "&MerchantId=" + model.MerchantId +
                    "&MerchantPassword=" + model.ProvPassword +
                    "&SuccessUrl=" + successUrl + "" +
                    "&FailureUrl=" + failUrl + "" +
                    "&InstallmentCount=" + installmentCount;

                byte[] dataStream = Encoding.UTF8.GetBytes(data);
                HttpWebRequest webRequest = (HttpWebRequest)HttpWebRequest.Create("https://3dsecure.vakifbank.com.tr:4443/MPIAPI/MPI_Enrollment.aspx"); //Mpi Enrollment Adresi
                webRequest.Method = "POST";
                webRequest.ContentType = "application/x-www-form-urlencoded";
                webRequest.ContentLength = dataStream.Length;
                webRequest.KeepAlive = false;
                string responseFromServer = "";

                using (Stream newStream = webRequest.GetRequestStream())
                {
                    newStream.Write(dataStream, 0, dataStream.Length);
                    newStream.Close();
                }

                using (WebResponse webResponse = webRequest.GetResponse())
                {
                    using (StreamReader reader = new StreamReader(webResponse.GetResponseStream()))
                    {
                        responseFromServer = reader.ReadToEnd();
                        reader.Close();
                    }

                    webResponse.Close();
                }

                if (string.IsNullOrEmpty(responseFromServer))
                {
                    var payModel = new PaymentResult
                    {
                        Success = false,
                        ErrorCode = "099",
                        ErrorMessage = "Hata",
                        TransactionId = transactionId,
                        BankName = "Vakıf Bank"
                    };

                    return "";
                }

                var xmlDocument = new XmlDocument();
                xmlDocument.LoadXml(responseFromServer);


                var statusNode = xmlDocument.SelectSingleNode("IPaySecure/Message/VERes/Status");
                var pareqNode = xmlDocument.SelectSingleNode("IPaySecure/Message/VERes/PaReq");
                var acsUrlNode = xmlDocument.SelectSingleNode("IPaySecure/Message/VERes/ACSUrl");
                var termUrlNode = xmlDocument.SelectSingleNode("IPaySecure/Message/VERes/TermUrl");
                var mdNode = xmlDocument.SelectSingleNode("IPaySecure/Message/VERes/MD");
                var messageErrorCodeNode = xmlDocument.SelectSingleNode("IPaySecure/MessageErrorCode");

                string statusText = "";

                if (statusNode != null)
                {
                    statusText = statusNode.InnerText;
                }

                //3d secure programına dahil
                if (statusText == "Y")
                {
                    string postBackForm =
                       @"<html>
                          <head>
                            <meta name=""viewport"" content=""width=device-width"" />
                            <title>MpiForm</title>
                            <script>
                              function postPage() {
                              document.forms[""frmMpiForm""].submit();
                              }
                            </script>
                          </head>
                          <body onload=""javascript:postPage();"">
                            <form action=""@ACSUrl"" method=""post"" id=""frmMpiForm"" name=""frmMpiForm"">
                              <input type=""hidden"" name=""PaReq"" value=""@PAReq"" />
                              <input type=""hidden"" name=""TermUrl"" value=""@TermUrl"" />
                              <input type=""hidden"" name=""MD"" value=""@MD "" />
                              <noscript>
                                <input type=""submit"" id=""btnSubmit"" value=""Gönder"" />
                              </noscript>
                            </form>
                          </body>
                        </html>";

                    postBackForm = postBackForm.Replace("@ACSUrl", acsUrlNode.InnerText);
                    postBackForm = postBackForm.Replace("@PAReq", pareqNode.InnerText);
                    postBackForm = postBackForm.Replace("@TermUrl", termUrlNode.InnerText);
                    postBackForm = postBackForm.Replace("@MD", mdNode.InnerText);
                    return postBackForm;
                }
                else if (statusText == "E")
                {
                    var payModel = new PaymentResult
                    {
                        Success = false,
                        ErrorCode = messageErrorCodeNode.InnerText,
                        ErrorMessage = statusText + " - " + messageErrorCodeNode.InnerText,
                        TransactionId = transactionId,
                        BankName = "Vakıf Bank"
                    };

                    return "";
                }
            }
            catch (Exception)
            {
                try
                {
                    var payModel = new PaymentResult
                    {
                        Success = false,
                        ErrorCode = "099",
                        ErrorMessage = "Ödeme tamamlanamadı",
                        TransactionId = transactionId,
                        BankName = "Vakıf Bank"
                    };

                    return "";
                }
                catch (Exception)
                {
                    var payModel = new PaymentResult
                    {
                        Success = false,
                        ErrorCode = "099",
                        ErrorMessage = "Ödeme tamamlanamadı",
                        TransactionId = transactionId,
                        BankName = "Vakıf Bank"
                    };

                    return "";
                }
            }
            return "";
        }
        #endregion
        #region VakifKatilimPos
        private static string VakifKatilimProvider(PaymentRequest model)
        {
            int MerchantId = int.Parse(model.MerchantId);
            int CustomerId = int.Parse(model.TerminalId);
            string UserName = model.ProvUserId;
            string pass = model.ProvPassword;
            string merchantOrderId = Guid.NewGuid().ToString();

            string successUrl = model.SuccessUrl;//Başarılı Url
            string errorurl = model.FailUrl;//Hata Url

            VPosMessageContract request = new VPosMessageContract();
            request.CardNumber = model.CardNumber;
            request.CardExpireDateMonth = string.Format("{0:00}", model.ExpireMonth);
            request.CardExpireDateYear = string.Format("{0:00}", model.ExpireYear);
            request.CardCVV2 = model.CvvCode;
            request.CardHolderName = model.CardHolderName;
            request.FECCurrencyCode = model.CurrencyIsoCode;
            request.CurrencyCode = model.CurrencyIsoCode;
            request.APIVersion = "1.0.0";
            request.MerchantId = MerchantId;
            request.CustomerId = CustomerId;
            request.UserName = UserName;
            request.MerchantOrderId = merchantOrderId;
            request.InstallmentCount = 0;
            request.Amount = (model.TotalAmount * 100m).ToString("0.##", new CultureInfo("en-US"));
            request.DisplayAmount = request.Amount.ToString();
            request.TransactionSecurity = 3;
            request.OkUrl = successUrl;
            request.FailUrl = errorurl;
            //HashData Hesaplanmasi
            string hashPassword = ComputeHash(pass);
            string hashString = CreateHashString(merchantId: request.MerchantId, merchantOrderId: merchantOrderId, amount: request.Amount, okUrl: successUrl, failUrl: errorurl, username: request.UserName, hashPassword: hashPassword);
            request.HashData = ComputeHash(hashString);

            VPosTransactionResponseContract response = new VPosTransactionResponseContract();
            XmlSerializer x = new XmlSerializer(request.GetType());
            StringWriter sw = new StringWriter();
            x.Serialize(sw, request);
            XsdDataContractExporter exporter = new XsdDataContractExporter();
            string requestMessage = sw.ToString();
            string result = DataPost("https://boa.vakifkatilim.com.tr/VirtualPOS.Gateway/Home/ThreeDModelPayGate", requestMessage);
            //try
            //{
            //    response =
            //   DeserializeObject<VPosTransactionResponseContract>(result.Replace("&#x0;", "").Replace("encoding =\"utf-16\"", ""));
            //}
            //catch (Exception ex)
            //{
            //    string errorMessage = "Mesaj1: " + ex.Message + ": Mesaj2: " + ex.StackTrace + ": Mesaj3: " + ex.InnerException;
            //}
            return result;
            //return View();
        }
        private static string DataPost(string uriToPost, string dataToPost)
        {
            byte[] buffer = Encoding.UTF8.GetBytes(dataToPost);
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 |
           SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls |
           SecurityProtocolType.Tls11;
            HttpWebRequest WebReq = (HttpWebRequest)WebRequest.Create(uriToPost);
            WebReq.Timeout = 50 * 60 * 1000;
            WebReq.Method = "POST";
            WebReq.ContentType = "application/xml";
            WebReq.ContentLength = buffer.Length;
            WebReq.CookieContainer = new CookieContainer();
            Stream ReqStream = WebReq.GetRequestStream();
            ReqStream.Write(buffer, 0, buffer.Length);
            ReqStream.Close();
            WebResponse WebRes = WebReq.GetResponse();
            Stream ResStream = WebRes.GetResponseStream();
            StreamReader ResReader = new StreamReader(ResStream);
            string responseString = ResReader.ReadToEnd();
            return responseString;
        }
        private static T DeserializeObject<T>(string deserializedObject) where T : class
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
            MemoryStream memoryStream = new MemoryStream(new
           UTF8Encoding().GetBytes(deserializedObject));
            System.Xml.XmlTextWriter xmlTextWriter = new System.Xml.XmlTextWriter((Stream)memoryStream, Encoding.UTF8);
            return (T)xmlSerializer.Deserialize((Stream)memoryStream);
        }

        private static string RemoveDiacritics(string text)
        {
            text = text.Replace("Ä\u009f", "ğ").Replace("Ä±", "ı");
            return text;
        }
        private static String ComputeHash(String hashstr)
        {
            System.Security.Cryptography.SHA1 sha = new
           System.Security.Cryptography.SHA1CryptoServiceProvider();
            byte[] hashbytes = System.Text.Encoding.GetEncoding("ISO-8859-9").GetBytes(hashstr);
            byte[] inputbytes = sha.ComputeHash(hashbytes);
            String hash = Convert.ToBase64String(inputbytes);
            return hash;
        }
        private static String CreateHashString(int merchantId, string amount, string merchantOrderId = "", string okUrl = "", string failUrl = "", string username = "", string hashPassword = "")
        {
            string newHash = string.Concat(merchantId
            , merchantOrderId
            , amount
            , okUrl
            , failUrl
            , username
            , hashPassword);
            return newHash;
        }
        #endregion
        #region NestPay-ZiraatPos
        //Kart Numarası(Visa) : 4546711234567894
        //Kart Numarası(Master Card) : 5401341234567891
        //Son Kullanma Tarihi :12/26
        //Güvenlik Numarası : 000
        //Kart 3D Secure Şifresi : a
        private static string NestPayProvider(PaymentRequest request)
        {
            string clientId = request.MerchantId;
            string processType = request.Mode;
            string storeKey = request.TerminalStoreKey;
            string storeType = request.Type;
            string random = DateTime.Now.ToString();
            string orderNumber = Guid.NewGuid().ToString("N");

            string actionUrl = "https://entegrasyon.asseco-see.com.tr/fim/est3Dgate";
            string callbackUrl = request.SuccessUrl;

            var parameters = new Dictionary<string, object>();
            parameters.Add("clientid", clientId);
            parameters.Add("oid", orderNumber);//sipariş numarası

            parameters.Add("pan", request.CardNumber);
            parameters.Add("cardHolderName", request.CardHolderName);
            parameters.Add("Ecom_Payment_Card_ExpDate_Month", request.ExpireMonth);//kart bitiş ay'ı
            parameters.Add("Ecom_Payment_Card_ExpDate_Year", request.ExpireYear);//kart bitiş yıl'ı
            parameters.Add("cv2", request.CvvCode);//kart güvenlik kodu
            parameters.Add("cardType", request.CardNumber.Substring(0, 1) == "5" ? "2" : "1");//kart tipi visa 1 | master 2 | amex 3

            //işlem başarılı da olsa başarısız da olsa callback sayfasına yönlendirerek kendi tarafımızda işlem sonucunu kontrol ediyoruz
            parameters.Add("okUrl", callbackUrl);//başarılı dönüş adresi
            parameters.Add("failUrl", callbackUrl);//hatalı dönüş adresi
            parameters.Add("islemtipi", processType);//direk satış
            parameters.Add("rnd", random);//rastgele bir sayı üretilmesi isteniyor
            parameters.Add("currency", request.CurrencyIsoCode);//ISO code TL 949 | EURO 978 | Dolar 840
            parameters.Add("storetype", storeType);
            parameters.Add("lang", request.LanguageIsoCode);//iki haneli dil iso kodu

            //kuruş ayrımı nokta olmalı!!!
            string totalAmount = request.TotalAmount.ToString(new CultureInfo("en-US"));
            parameters.Add("amount", totalAmount);

            string installment = request.Installment.ToString();
            if (request.Installment < 2)//imece kart durumunda taksit boş olacak
                installment = string.Empty;//0 veya 1 olması durumunda taksit bilgisini boş gönderiyoruz

            //normal taksit
            parameters.Add("taksit", installment);//taksit sayısı | 1 veya boş tek çekim olur

            var hashBuilder = new StringBuilder();
            hashBuilder.Append(clientId);
            hashBuilder.Append(orderNumber);
            hashBuilder.Append(totalAmount);
            hashBuilder.Append(callbackUrl);
            hashBuilder.Append(callbackUrl);
            hashBuilder.Append(processType);
            hashBuilder.Append(installment);
            hashBuilder.Append(random);
            hashBuilder.Append(storeKey);

            var hashData = GetSha1(hashBuilder.ToString());
            parameters.Add("hash", hashData);//hash data

            return CreateFormHtml(parameters, actionUrl);
        }
        private static string GetSha1(string text)
        {
            var cryptoServiceProvider = new SHA1CryptoServiceProvider();
            var inputBytes = cryptoServiceProvider.ComputeHash(Encoding.UTF8.GetBytes(text));
            var hashData = Convert.ToBase64String(inputBytes);

            return hashData;
        }
        private static string CreateFormHtml(IDictionary<string, object> parameters, string actionUrl)
        {
            string formId = "PaymentForm";
            StringBuilder formBuilder = new StringBuilder();
            formBuilder.Append($"<form id=\"{formId}\" name=\"{formId}\" action=\"{actionUrl}\" role=\"form\" method=\"POST\">");

            foreach (KeyValuePair<string, object> parameter in parameters)
            {
                formBuilder.Append($"<input type=\"hidden\" name=\"{parameter.Key}\" value=\"{parameter.Value}\">");
            }

            formBuilder.Append("</form>");

            StringBuilder scriptBuilder = new StringBuilder();
            scriptBuilder.Append("<script>");
            scriptBuilder.Append($"document.{formId}.submit();");
            scriptBuilder.Append("</script>");
            formBuilder.Append(scriptBuilder.ToString());

            return formBuilder.ToString();
        }
        #endregion
    }

    public class PaymentRequest
    {
        public string CardHolderName { get; set; }
        public string CardNumber { get; set; }
        public int ExpireMonth { get; set; }
        public int ExpireYear { get; set; }
        public string CvvCode { get; set; }
        public int Installment { get; set; }
        public decimal TotalAmount { get; set; }
        public string OrderNumber { get; set; }
        public string CurrencyIsoCode { get; set; }
        public string LanguageIsoCode { get; set; }
        public string CustomerIpAddress { get; set; }

        public string SuccessUrl { get; set; }
        public string FailUrl { get; set; }

        public int Prefix { get; set; }

        //Bank Info
        public string? TerminalId { get; set; }
        public string? TerminalUserId { get; set; }
        public string? MerchantId { get; set; }
        public string? ProvUserId { get; set; }
        public string? ProvPassword { get; set; }
        public string? TerminalStoreKey { get; set; }
        public string? Mode { get; set; }
        public string? Type { get; set; }
        public string? SecurityLevel { get; set; }
    }
    public class PaymentParameterResult
    {
        public PaymentParameterResult()
        {
            Parameters = new Dictionary<string, object>();
        }

        public IDictionary<string, object> Parameters { get; set; }
        public Uri PaymentUrl { get; set; }
        public bool Success { get; set; }
        public string Message { get; set; }
        public string ErrorMessage { get; set; }
    }

    public class PaymentResult
    {
        public bool Success { get; set; }
        public string ResponseCode { get; set; }
        public string TransactionId { get; set; }
        public string ErrorMessage { get; set; }
        public string ErrorCode { get; set; }
        public double Amount { get; set; }
        public string BankName { get; set; }
    }
    //Vakıf Katılım
    public class VPosMessageContract
    {
        public int TransactionSecurity { get; set; }
        public string APIVersion { get; set; }
        public string OkUrl { get; set; }
        public string FailUrl { get; set; }
        public string HashData { get; set; }
        public int MerchantId { get; set; }
        public int SubMerchantId { get; set; }
        public int CustomerId { get; set; }
        public string UserName { get; set; }
        public string MerchantOrderId { get; set; }
        public int InstallmentCount { get; set; }
        public string Amount { get; set; }
        public int FECAmount { get; set; }
        public string DisplayAmount { get; set; }
        public string FECCurrencyCode { get; set; }
        public string CurrencyCode { get; set; }
        public string CardNumber { get; set; }
        public string CardExpireDateYear { get; set; }
        public string CardExpireDateMonth { get; set; }
        public string CardCVV2 { get; set; }
        public string CardHolderName { get; set; }
        public int PaymentType { get; set; }
        /// Order Servisleri Alanlari 
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal? UpperLimit { get; set; }
        public decimal? LowerLimit { get; set; }
        public string ProvNumber { get; set; }
    }
    public class VPosTransactionResponseContract
    {
        public VPosMessageContract VPosMessage { get; set; }
        public VPosCardDataContract VPosCardData { get; set; }
        public List<OrderContract> VPosOrderData { get; set; }
        // eğer 3d secure ise geriye bu response dönülür. 
        public bool IsEnrolled { get; set; }
        public bool IsVirtual { get; set; }
        public string PareqHtmlFormString { get; set; }
        // eğer işlem 3dsecure değilse. 
        public string ProvisionNumber { get; set; }
        public string RRN { get; set; }
        public string Stan { get; set; }
        public string ResponseCode { get; set; }
        public bool IsSuccess { get; set; }
        public string ResponseMessage { get; set; }
        public int OrderId { get; set; }
        public DateTime TransactionTime { get; set; }
        public string MerchantOrderId { get; set; }
        public string HashData { get; set; }
        public string MD { get; set; }
        public string ReferenceId { get; set; }
    }
    public partial class VposCardInfoContract
    {
        public string CardGuid { get; set; }
        public string CardNumber { get; set; }
        public string CardHolderName { get; set; }
        public DateTime? SystemDate { get; set; }
    }
    public partial class OrderContract
    {
        public int OrderId { get; set; }
        public string MerchantOrderId { get; set; }
        public int MerchantId { get; set; }
        public string PosTerminalId { get; set; }
        public int OrderStatus { get; set; }
        public string OrderStatusDescription { get; set; }
        public int OrderType { get; set; }
        public string OrderTypeDescription { get; set; }
        public int TransactionStatus { get; set; }
        public string TransactionStatusDescription { get; set; }
        public int? LastOrderStatus { get; set; }
        public string LastOrderStatusDescription { get; set; }
        public int? EndOfDayStatus { get; set; }
        public string EndOfDayStatusDescription { get; set; }
        public string FEC { get; set; }
        public string FecDescription { get; set; }
        public int? TransactionSecurity { get; set; }
        public string TransactionSecurityDescription { get; set; }
        public string CardHolderName { get; set; }
        public string CardType { get; set; }
        public string CardNumber { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal FirstAmount { get; set; }
        public decimal FECAmount { get; set; }
        public decimal? CancelAmount { get; set; }
        public decimal? DrawbackAmount { get; set; }
        public decimal? ClosedAmount { get; set; }
        public int InstallmentCount { get; set; }
        public string ResponseCode { get; set; }
        public string ResponseExplain { get; set; }
        public string ProvNumber { get; set; }
        public string RRN { get; set; }
        public string Stan { get; set; }
        public string MerchantUserName { get; set; }
        public Int64 BatchId { get; set; }
    }
    public class VPosBaseContract
    {
        public string HashData { get; set; }
        public int MerchantId { get; set; }
        public int SubMerchantId { get; set; }
        public int CustomerId { get; set; }
        public string UserName { get; set; }
        public string CardNumber { get; set; }
        public string CardExpireDateYear { get; set; }
        public string CardExpireDateMonth { get; set; }
        public string CardCVV2 { get; set; }
        public string CardHolderName { get; set; }
        [StringLength(11)]
        public string IdentityTaxNumber { get; set; }
    }
    public class VPosCardDataContract : VPosBaseContract
    {
        public List<VposCardInfoContract> VposCardInfoContract { get; set; }
        public DateTime? BeginDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string CardGuid { get; set; }
    }
}
