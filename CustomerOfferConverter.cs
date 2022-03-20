using System.Xml;

namespace CustomerOfferSearchResultToProductsXml
{
    public class CustomerOfferConverter
    {
        public static void ConvertToFile(string customerOfferXmlFilePath, string productsResultFilePath)
        {
            var productsXmlDocument = new XmlDocument();
            var xmlDeclaration = productsXmlDocument.CreateXmlDeclaration("1.0", "UTF-8", null);
            productsXmlDocument.AppendChild(xmlDeclaration);
            var rootElement = productsXmlDocument.CreateElement("Products");
            productsXmlDocument.AppendChild(rootElement);

            var customerOfferXmlDocument = new XmlDocument();
            customerOfferXmlDocument.Load(customerOfferXmlFilePath);
            var productsOffersCollection = customerOfferXmlDocument.SelectSingleNode("Ticket").SelectSingleNode("ProductsOffers").SelectNodes("ProductOffer");

            foreach (var productOfferObject in productsOffersCollection)
            {
                var productOfferXmlElement = productOfferObject as XmlElement;
                string status = productOfferXmlElement?.SelectSingleNode("Status")?.InnerText.ToLower() ?? "";
                if (status != "success")
                {
                    continue;
                }

                var priceProductNameXmlNode = productOfferXmlElement.SelectSingleNode("PriceListProduct").SelectSingleNode("Name");
                var vendorSkuXmlNode = productOfferXmlElement.SelectSingleNode("PriceListProduct").SelectSingleNode("VendorSku");
                var quantityXmlNode = productOfferXmlElement.SelectSingleNode("Product").SelectSingleNode("Quantity");
                
                var productXmlElement = productsXmlDocument.CreateElement("Product");
                productXmlElement.AppendChild(productsXmlDocument.ImportNode(priceProductNameXmlNode, true));
                productXmlElement.AppendChild(productsXmlDocument.ImportNode(vendorSkuXmlNode, true));
                productXmlElement.AppendChild(productsXmlDocument.ImportNode(quantityXmlNode, true));
                rootElement.AppendChild(productXmlElement);
            }
            productsXmlDocument.Save(productsResultFilePath);
        }
    }
}

