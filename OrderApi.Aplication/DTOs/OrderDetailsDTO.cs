using System.ComponentModel.DataAnnotations;

namespace OrderApi.Aplication.DTOs
{
    public record OrderDetailsDTO(
      [Required] int OrderId,
      [Required] int ProductId,
      [Required] int ClientId,
      [Required, EmailAddress] string Email,
      [Required] string Address,
      [Required] string TelephoneNumber,
      [Required] string Name,
      [Required] string ProductName,
      [Required] int PurchaseQuantity,
      [Required, DataType(DataType.Currency)] decimal UnitPrice,
      [Required, DataType(DataType.Currency)] decimal TotalPrice,
      [Required] DateTime OrderDate
    );
}
