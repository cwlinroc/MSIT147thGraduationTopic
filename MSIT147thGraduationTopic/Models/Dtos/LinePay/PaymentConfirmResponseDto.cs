namespace MSIT147thGraduationTopic.Models.Dtos.LinePay
{
    public class PaymentConfirmResponseDto
    {
        public string ReturnCode { get; set; }
        public string ReturnMessage { get; set; }
        public ConfirmResponseInfoDto Info { get; set; }
    }

    public class ConfirmResponseInfoDto
    {
        public string OrderId { get; set; }
        public long TransactionId { get; set; }
        public string AuthorizationExpireDate { get; set; }
        public string RegKey { get; set; }
        public ConfirmResponsePayInfoDto[] PayInfo { get; set; }
    }

    public class ConfirmResponsePayInfoDto
    {
        public string Method { get; set; }
        public int Amount { get; set; }  //PaymentAmount
        public string CreditCardNickname { get; set; }
        public string CreditCardBrand { get; set; }
        public string MaskedCreditCardNumber { get; set; }
        public ConfirmResponsePackageDto[] Packages { get; set; }
        public ConfirmResponseShippingOptionsDto Shipping { get; set; }
    }
    public class ConfirmResponsePackageDto
    {
        public string Id { get; set; }
        public int Amount { get; set; }
        public int UserFeeAmount { get; set; }
    }
    public class ConfirmResponseShippingOptionsDto
    {
        public string MethodId { get; set; }
        public int FeeAmount { get; set; }
        public ShippingAddressDto Address { get; set; }
    }

}
