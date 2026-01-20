using PaymentGateway.Application.DTO;
using PaymentGateway.Domain.Enums;
using PaymentGateway.Domain.Interfaces;
using PaymentGateway.Domain.Models;

namespace PaymentGateway.Application.Services
{
    internal class FakePaymentGatewayService : IPaymentGatewayService
    {
        private readonly ITransactionRepository _repository;
        private readonly Random _random = new();

        public FakePaymentGatewayService(ITransactionRepository repository)
        {
            _repository = repository;
        }

        public async Task<ProcessPaymentResponseDTO> ProcessPaymentAsync(ProcessPaymentRequestDTO request)
        {
            // Simula latência de rede (100-500ms)
            await Task.Delay(_random.Next(100, 500));

            var transaction = new Transaction
            {
                ExternalReference = request.ExternalReference,
                Amount = request.Amount,
                PaymentMethod = request.PaymentMethod,
                Status = TransactionStatus.Processing,
                CreatedAt = DateTime.UtcNow
            };

            // Simula validação e processamento
            var (status, message) = SimulatePaymentProcessing(request);
            
            transaction.Status = status;
            transaction.ProcessedAt = DateTime.UtcNow;
            transaction.ErrorMessage = status != TransactionStatus.Approved ? message : null;

            // Gera dados de PIX se for pagamento PIX
            if (request.PaymentMethod == GatewayPaymentMethod.Pix && status == TransactionStatus.Approved)
            {
                transaction.PixQrCode = GenerateFakePixQrCode(transaction.Id, request.Amount);
                transaction.PixCopyPaste = GenerateFakePixCopyPaste(transaction.Id, request.Amount);
            }

            await _repository.AddAsync(transaction);

            return new ProcessPaymentResponseDTO
            {
                TransactionId = transaction.Id,
                ExternalReference = transaction.ExternalReference,
                Status = transaction.Status,
                Amount = transaction.Amount,
                Message = message,
                ProcessedAt = transaction.ProcessedAt ?? DateTime.UtcNow,
                PixQrCode = transaction.PixQrCode,
                PixCopyPaste = transaction.PixCopyPaste
            };
        }

        public async Task<TransactionStatusDTO?> GetTransactionStatusAsync(string transactionId)
        {
            var transaction = await _repository.GetByIdAsync(transactionId);
            
            if (transaction == null)
                return null;

            return new TransactionStatusDTO
            {
                TransactionId = transaction.Id,
                ExternalReference = transaction.ExternalReference,
                Status = transaction.Status,
                Amount = transaction.Amount,
                CreatedAt = transaction.CreatedAt,
                ProcessedAt = transaction.ProcessedAt,
                ErrorMessage = transaction.ErrorMessage
            };
        }

        public async Task<RefundResponseDTO> RefundAsync(RefundRequestDTO request)
        {
            await Task.Delay(_random.Next(100, 300));

            var transaction = await _repository.GetByIdAsync(request.TransactionId);
            
            if (transaction == null)
            {
                return new RefundResponseDTO
                {
                    RefundId = Guid.NewGuid().ToString("N"),
                    TransactionId = request.TransactionId,
                    RefundedAmount = 0,
                    Success = false,
                    Message = "Transaction not found",
                    ProcessedAt = DateTime.UtcNow
                };
            }

            if (transaction.Status != TransactionStatus.Approved)
            {
                return new RefundResponseDTO
                {
                    RefundId = Guid.NewGuid().ToString("N"),
                    TransactionId = request.TransactionId,
                    RefundedAmount = 0,
                    Success = false,
                    Message = "Only approved transactions can be refunded",
                    ProcessedAt = DateTime.UtcNow
                };
            }

            var refundAmount = request.Amount ?? transaction.Amount;
            var availableForRefund = transaction.Amount - transaction.RefundedAmount;

            if (refundAmount > availableForRefund)
            {
                return new RefundResponseDTO
                {
                    RefundId = Guid.NewGuid().ToString("N"),
                    TransactionId = request.TransactionId,
                    RefundedAmount = 0,
                    Success = false,
                    Message = $"Refund amount exceeds available amount. Available: {availableForRefund:C}",
                    ProcessedAt = DateTime.UtcNow
                };
            }

            transaction.RefundedAmount += refundAmount;
            if (transaction.RefundedAmount >= transaction.Amount)
            {
                transaction.Status = TransactionStatus.Refunded;
            }

            await _repository.UpdateAsync(transaction);

            return new RefundResponseDTO
            {
                RefundId = Guid.NewGuid().ToString("N"),
                TransactionId = transaction.Id,
                RefundedAmount = refundAmount,
                Success = true,
                Message = "Refund processed successfully",
                ProcessedAt = DateTime.UtcNow
            };
        }

        public async Task<IEnumerable<TransactionStatusDTO>> GetAllTransactionsAsync()
        {
            var transactions = await _repository.GetAllAsync();
            
            return transactions.Select(t => new TransactionStatusDTO
            {
                TransactionId = t.Id,
                ExternalReference = t.ExternalReference,
                Status = t.Status,
                Amount = t.Amount,
                CreatedAt = t.CreatedAt,
                ProcessedAt = t.ProcessedAt,
                ErrorMessage = t.ErrorMessage
            });
        }

        private (TransactionStatus status, string message) SimulatePaymentProcessing(ProcessPaymentRequestDTO request)
        {
            // Validações básicas
            if (request.Amount <= 0)
                return (TransactionStatus.Declined, "Invalid amount");

            if (request.Amount > 10000)
                return (TransactionStatus.Declined, "Amount exceeds limit");

            // Simula validação de cartão
            if (request.PaymentMethod == GatewayPaymentMethod.CreditCard || request.PaymentMethod == GatewayPaymentMethod.DebitCard)
            {
                if (request.CardInfo == null)
                    return (TransactionStatus.Error, "Card information is required");

                var cardValidation = ValidateCard(request.CardInfo);
                if (!cardValidation.isValid)
                    return (TransactionStatus.Declined, cardValidation.message);
            }

            // Simula 5% de falha aleatória para parecer mais realista
            if (_random.Next(100) < 5)
                return (TransactionStatus.Error, "Gateway temporarily unavailable. Please try again.");

            // Cartões de teste especiais para simular cenários
            if (request.CardInfo?.CardNumber != null)
            {
                // Cartão que sempre falha
                if (request.CardInfo.CardNumber.EndsWith("0000"))
                    return (TransactionStatus.Declined, "Card declined by issuer");

                // Cartão com saldo insuficiente
                if (request.CardInfo.CardNumber.EndsWith("1111") && request.Amount > 100)
                    return (TransactionStatus.Declined, "Insufficient funds");
            }

            return (TransactionStatus.Approved, "Payment approved successfully");
        }

        private (bool isValid, string message) ValidateCard(CardInfoDTO card)
        {
            if (string.IsNullOrWhiteSpace(card.CardNumber))
                return (false, "Card number is required");

            var cardNumber = card.CardNumber.Replace(" ", "").Replace("-", "");
            
            if (cardNumber.Length < 13 || cardNumber.Length > 19)
                return (false, "Invalid card number length");

            if (!cardNumber.All(char.IsDigit))
                return (false, "Card number must contain only digits");

            if (string.IsNullOrWhiteSpace(card.CardHolder))
                return (false, "Card holder name is required");

            if (string.IsNullOrWhiteSpace(card.ExpirationDate))
                return (false, "Expiration date is required");

            if (string.IsNullOrWhiteSpace(card.Cvv) || card.Cvv.Length < 3 || card.Cvv.Length > 4)
                return (false, "Invalid CVV");

            // Validação simples de Luhn (checksum de cartão)
            if (!PassesLuhnCheck(cardNumber))
                return (false, "Invalid card number");

            return (true, string.Empty);
        }

        private bool PassesLuhnCheck(string cardNumber)
        {
            int sum = 0;
            bool alternate = false;
            
            for (int i = cardNumber.Length - 1; i >= 0; i--)
            {
                int digit = cardNumber[i] - '0';
                
                if (alternate)
                {
                    digit *= 2;
                    if (digit > 9)
                        digit -= 9;
                }
                
                sum += digit;
                alternate = !alternate;
            }
            
            return sum % 10 == 0;
        }

        private string GenerateFakePixQrCode(string transactionId, decimal amount)
        {
            // Simula um código QR base64 (em produção seria um QR code real)
            return Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes($"PIX:{transactionId}:{amount}"));
        }

        private string GenerateFakePixCopyPaste(string transactionId, decimal amount)
        {
            // Simula código PIX copia e cola
            return $"00020126580014br.gov.bcb.pix0136{transactionId}5204000053039865406{amount:F2}5802BR";
        }
    }
}
