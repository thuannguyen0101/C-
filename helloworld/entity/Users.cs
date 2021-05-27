namespace helloworld.entity
{
    public class Users
    {
        
        public string fullName { set; get; }
        public string email{ set; get; }
        public string password{ set; get; }
        public string phone{ set; get; }
        public string cardNumber{ set; get; }
        
        public string balance{ set; get; }
        
        public string Salt{ set; get; }
        
        public Users()
        {
            
        }

        public Users(string fullName, string email, string password, string phone, string cardNumber, string balance, string salt)
        {
            this.fullName = fullName;
            this.email = email;
            this.password = password;
            this.phone = phone;
            this.cardNumber = cardNumber;
            this.balance = balance;
            Salt = salt;
        }
    }
}