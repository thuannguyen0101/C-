namespace helloworld.entity
{
    public class Transaction
    {
        public string cardNumber { get; set; }
        public string description { get; set; }
        public int status { get; set;}
        
        public string created_At { get; set;}
        public string transaction_type { get; set;}

        public Transaction()
        {
        }

        public Transaction(string cardNumber, string description, int status, string createdAt, string transactionType)
        {
            this.cardNumber = cardNumber;
            this.description = description;
            this.status = status;
            created_At = createdAt;
            transaction_type = transactionType;
        }
    }
}