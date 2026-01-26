using PaymentGateway.Domain.Enums;

namespace PaymentGateway.Application.DTO
{
    public class ProcessPaymentRequestDTO
    {
        public long ExternalReference { get; set; }        
        public decimal Amount { get; set; }
        public GatewayPaymentMethod PaymentMethod { get; set; }
        public CardInfoDTO? CardInfo { get; set; }
        public PixInfoDTO? PixInfo { get; set; }
    }

    public class CardInfoDTO
    {
        public string CardNumber { get; set; } = string.Empty;
        public string CardHolder { get; set; } = string.Empty;
        public string ExpirationDate { get; set; } = string.Empty;
        public string Cvv { get; set; } = string.Empty;
    }

    public class PixInfoDTO
    {
        public string PixKey { get; set; } = string.Empty;
    }
}
