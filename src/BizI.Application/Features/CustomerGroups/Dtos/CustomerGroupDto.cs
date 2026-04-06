namespace BizI.Application.Features.CustomerGroups.Dtos;

public record CustomerGroupDto(
    Guid Id,
    string Name,
    decimal DiscountPercent);

public record CreateCustomerGroupDto(string Name, decimal DiscountPercent = 0m);

public record UpdateCustomerGroupDto(Guid Id, string Name, decimal DiscountPercent = 0m);
