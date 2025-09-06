using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Core.Utilities.NVIVerify
{
    public class NviVerify
    {
        public class NviVerification
        {
            public async static Task<bool> VerifyAsync(string identityNumber, string firstName, string lastName, int birthYear)
            {
                try
                {
                    HttpClient client = new HttpClient();
                    String str1 = @"
            <soap12:Envelope xmlns:xsi='http://www.w3.org/2001/XMLSchema-instance' xmlns:xsd='http://www.w3.org/2001/XMLSchema' xmlns:soap12='http://www.w3.org/2003/05/soap-envelope'>
                <soap12:Body>
                    <TCKimlikNoDogrula xmlns='http://tckimlik.nvi.gov.tr/WS'>
                        <TCKimlikNo>" + identityNumber + "</TCKimlikNo>" +
                                "<Ad>" + firstName + "</Ad>" +
                                "<Soyad>" + lastName + "</Soyad>" +
                                "<DogumYili>" + birthYear + "</DogumYili>" +
                            "</TCKimlikNoDogrula>" +
                        "</soap12:Body>" +
                    "</soap12:Envelope>";

                    HttpContent content = new StringContent(str1, Encoding.UTF8, "application/soap+xml");
                    client.DefaultRequestHeaders.Add("SOAPAction", "http://tckimlik.nvi.gov.tr/WS/TCKimlikNoDogrula");
                    var response = await client.PostAsync("https://tckimlik.nvi.gov.tr/Service/KPSPublic.asmx", content);
                    var result = response.Content.ReadAsStringAsync().Result;

                    XmlDocument xml = new XmlDocument();
                    xml.LoadXml(result);
                    return Convert.ToBoolean(xml.ChildNodes[1].FirstChild.FirstChild.FirstChild.FirstChild.InnerText);
                }
                catch (Exception)
                {
                    return false;
                }
            }
        }
    }
}
