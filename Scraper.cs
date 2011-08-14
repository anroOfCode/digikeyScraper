using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

using System.Net;
using HtmlAgilityPack;

namespace DigikeyApi
{
    public class Scraper
    {
        public static Product scrapePart(string partId)
        {
            try
            {
                WebClient wc = new WebClient();
                string pageContents = wc.DownloadString("http://search.digikey.com/scripts/DkSearch/dksus.dll?Detail&name=" + System.Web.HttpUtility.UrlEncode(partId));
                pageContents = pageContents.Replace("<CS=0><RF=141>", "");
                HtmlDocument doc = new HtmlDocument();
                doc.LoadHtml(pageContents);
                HtmlNode partNumberNode = doc.DocumentNode.SelectSingleNode("//td[@id='reportpartnumber']");
                HtmlNode quantityNode = doc.DocumentNode.SelectSingleNode("//td[@id='quantityavailable']");
                HtmlNodeCollection pricingNodes = doc.DocumentNode.SelectNodes("//table[@id='pricing']/tr");

                Product p = new Product();
                p.partNumber = partNumberNode.InnerText;
                try
                {
                    p.quantityAvailiable = Convert.ToInt32(Regex.Replace(quantityNode.InnerText, "[^0-9]", ""));
                }
                catch
                {
                    p.quantityAvailiable = 0;
                    p.warning += "Quantity might not be availiable\n";
                }
                

                p.pricing = new SortedDictionary<int, decimal>();

                for (int i = 1; i < pricingNodes.Count(); i++)
                {
                    int quantity = Convert.ToInt32(Regex.Replace(pricingNodes[i].ChildNodes[0].InnerText, "[^0-9]", ""));
                    decimal unitPrice = Convert.ToDecimal(pricingNodes[i].ChildNodes[1].InnerText);
                    p.pricing.Add(quantity, unitPrice);
                }

                return p;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception on part {0}: {1}", partId, ex.Message);
                return null;
            }
        }
    }
}
