namespace BizI.Application.DTOs.CustomerGroup;

public record CustomerGroupDto(
    string Id,
    string Name,
    decimal DiscountPercent);

public record CreateCustomerGroupDto(string Name, decimal DiscountPercent = 0m);

public record UpdateCustomerGroupDto(string Id, string Name, decimal DiscountPercent = 0m);
