using System.ComponentModel.DataAnnotations;
namespace CurrencyConverter.Core.Models
{
    public class CurrencyConversionRequestViewModel : IValidatableObject
    {
        [Required(ErrorMessage = "Client Name is required.")]
        [RegularExpression(@"^[a-zA-Z]+\s[a-zA-Z]+$", ErrorMessage = "Client Name must contain exactly two words.")]
        public string ClientName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Personal Number is required.")]
        [RegularExpression(@"^\d{11}$", ErrorMessage = "Personal Number must be exactly 11 numeric digits.")]
        public string PersonalNumber { get; set; } = string.Empty;

        [Required(ErrorMessage = "Origin Currency is required.")]
        public string FromCurrency { get; set; } = string.Empty;

        [Required(ErrorMessage = "Destination Currency is required.")]
        public string ToCurrency { get; set; } = string.Empty;

        [Required(ErrorMessage = "Date is required.")]
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

        [Required(ErrorMessage = "Amount is required.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Amount must be greater than zero.")]
        public decimal Amount { get; set; }
        public decimal Rate { get; set; }
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (FromCurrency.ToUpper() != "GEL" && ToCurrency.ToUpper() != "GEL")
            {
                yield return new ValidationResult("One of the currencies must be GEL.", new[] { nameof(FromCurrency), nameof(ToCurrency) });
            }

            if (FromCurrency.ToUpper() == "GEL" && ToCurrency.ToUpper() == "GEL")
            {
                yield return new ValidationResult("Both currencies cannot be GEL.", new[] { nameof(FromCurrency), nameof(ToCurrency) });
            }
        }
    }

}
