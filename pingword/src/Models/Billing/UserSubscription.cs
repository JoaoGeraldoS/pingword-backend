namespace pingword.src.Models.Billing
{
    public class UserSubscription
    {
        public string UserId { get; set; }

        public string ProductId { get; set; }

        public string PurchaseToken { get; set; }

        public DateTime ExpiryDate { get; set; }

        public bool IsActive { get; set; }

        public DateTime LastUpdate { get; set; }
    }
}
