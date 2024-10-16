namespace Banktive.Web.Models
{
    public class Constants
    {
        //Currencies
        public const int Currency_XRP = 1;

        //Status
        public const int PaymentCreated = 1;
        public const int PaymentCancelled = 2;
        public const int PaymentConfirmed = 3;
        public const int PaymentRejected = 4;

        //Types
        public const int RegularPayment = 1;
        public const int CheckPayment = 2;
        public const int ServicePayment = 3;
        public const int TimeDepositPayment = 4;

        // Check status
        public const int CheckCreated = 1;
        public const int CheckCancelled = 2;
        public const int CheckConfirmed = 3;
        public const int CheckCashed = 4;

        // Escrow status
        public const int EscrowCreated = 1;
        public const int EscrowCancelled = 2;
        public const int EscrowConfirmed = 3;
        public const int EscrowCashed = 4;
        public const int EscrowRejected = 5;
        public const int EscrowExpired = 6;

        // Escrow types
        public const int TimeEscrow = 1;
        public const int ConditionalEscrow = 2;
    }
}
