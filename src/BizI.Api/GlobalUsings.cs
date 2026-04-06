// System
global using System;
global using System.Collections.Generic;
global using System.Linq;
global using System.Threading;
global using System.Threading.Tasks;
global using System.Text;

// Microsoft
global using Microsoft.Extensions.Logging;
global using Microsoft.Extensions.Configuration;
global using Microsoft.AspNetCore.Builder;
global using Microsoft.AspNetCore.Http;
global using Microsoft.AspNetCore.Routing;

// Third-party
global using MediatR;

// Application — Commands, Queries, DTOs (API depends ONLY on Application)
global using BizI.Application.Common;
global using BizI.Application.Interfaces;
global using BizI.Application.DTOs.Product;
global using BizI.Application.DTOs.Order;
global using BizI.Application.DTOs.Customer;
global using BizI.Application.DTOs.CustomerGroup;
global using BizI.Application.DTOs.Inventory;
global using BizI.Application.DTOs.ImportOrder;
global using BizI.Application.DTOs.Payment;
global using BizI.Application.DTOs.Supplier;
global using BizI.Application.DTOs.Warehouse;
global using BizI.Application.DTOs.Role;
global using BizI.Application.DTOs.User;
global using BizI.Application.DTOs.Category;
global using BizI.Application.Features.Products;
global using BizI.Application.Features.Orders;
global using BizI.Application.Features.Customers;
global using BizI.Application.Features.CustomerGroups;
global using BizI.Application.Features.Inventory;
global using BizI.Application.Features.InventoryTransactions;
global using BizI.Application.Features.ImportOrders;
global using BizI.Application.Features.PaymentAndReturns;
global using BizI.Application.Features.Categories;
global using BizI.Application.Features.Suppliers;
global using BizI.Application.Features.Warehouses;
global using BizI.Application.Features.Roles;
global using BizI.Application.Features.Users;
global using BizI.Application.Features.AuditLogs;
