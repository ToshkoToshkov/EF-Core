namespace ProductShop.DTOs.Input
{
    public class ProductsInputDto
    {
        public string Name { get; set; }

        public decimal Price { get; set; }

        public int SellerId { get; set; }
        //public User Seller { get; set; }

        public int? BuyerId { get; set; }
        //public User Buyer { get; set; }
    }
}
