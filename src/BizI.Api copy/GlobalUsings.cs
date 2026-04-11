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

// Application — Common
global using BizI.Application.Common;
global using BizI.Application.Interfaces;

// Application — AuditLogs feature slices
global using BizI.Application.Features.AuditLogs.Create;
global using BizI.Application.Features.AuditLogs.GetAll;
global using BizI.Application.Features.AuditLogs.GetById;
global using BizI.Application.Features.AuditLogs.Dtos;

// Application — Categories feature slices
global using BizI.Application.Features.Categories.Create;
global using BizI.Application.Features.Categories.Update;
global using BizI.Application.Features.Categories.Delete;
global using BizI.Application.Features.Categories.GetAll;
global using BizI.Application.Features.Categories.GetById;
global using BizI.Application.Features.Categories.Dtos;

// Application — CustomerGroups feature slices
global using BizI.Application.Features.CustomerGroups.Create;
global using BizI.Application.Features.CustomerGroups.Update;
global using BizI.Application.Features.CustomerGroups.Delete;
global using BizI.Application.Features.CustomerGroups.GetAll;
global using BizI.Application.Features.CustomerGroups.GetById;
global using BizI.Application.Features.CustomerGroups.Dtos;

// Application — Customers feature slices
global using BizI.Application.Features.Customers.Create;
global using BizI.Application.Features.Customers.Update;
global using BizI.Application.Features.Customers.Delete;
global using BizI.Application.Features.Customers.GetAll;
global using BizI.Application.Features.Customers.GetById;
global using BizI.Application.Features.Customers.Dtos;

// Application — Orders feature slices
global using BizI.Application.Features.Orders.Create;
global using BizI.Application.Features.Orders.Delete;
global using BizI.Application.Features.Orders.Return;
global using BizI.Application.Features.Orders.GetAll;
global using BizI.Application.Features.Orders.GetById;
global using BizI.Application.Features.Orders.Dtos;

// Application — PaymentAndReturns feature slices
global using BizI.Application.Features.PaymentAndReturns.CreatePayment;
global using BizI.Application.Features.PaymentAndReturns.DeletePayment;
global using BizI.Application.Features.PaymentAndReturns.GetAllPayments;
global using BizI.Application.Features.PaymentAndReturns.GetPaymentById;
global using BizI.Application.Features.PaymentAndReturns.CreateDebt;
global using BizI.Application.Features.PaymentAndReturns.UpdateDebt;
global using BizI.Application.Features.PaymentAndReturns.DeleteDebt;
global using BizI.Application.Features.PaymentAndReturns.GetAllDebts;
global using BizI.Application.Features.PaymentAndReturns.GetDebtById;
global using BizI.Application.Features.PaymentAndReturns.CreateReturnOrder;
global using BizI.Application.Features.PaymentAndReturns.DeleteReturnOrder;
global using BizI.Application.Features.PaymentAndReturns.GetAllReturnOrders;
global using BizI.Application.Features.PaymentAndReturns.GetReturnOrderById;
global using BizI.Application.Features.PaymentAndReturns.Dtos;

// Application — Products feature slices
global using BizI.Application.Features.Products.Create;
global using BizI.Application.Features.Products.Update;
global using BizI.Application.Features.Products.Delete;
global using BizI.Application.Features.Products.GetAll;
global using BizI.Application.Features.Products.GetById;
global using BizI.Application.Features.Products.Dtos;

// Application — Suppliers feature slices
global using BizI.Application.Features.Suppliers.Create;
global using BizI.Application.Features.Suppliers.Update;
global using BizI.Application.Features.Suppliers.Delete;
global using BizI.Application.Features.Suppliers.GetAll;
global using BizI.Application.Features.Suppliers.GetById;
global using BizI.Application.Features.Suppliers.Dtos;

// Application — Warehouses feature slices
global using BizI.Application.Features.Warehouses.Create;
global using BizI.Application.Features.Warehouses.Update;
global using BizI.Application.Features.Warehouses.Delete;
global using BizI.Application.Features.Warehouses.GetAll;
global using BizI.Application.Features.Warehouses.GetById;
global using BizI.Application.Features.Warehouses.Dtos;

// Application — Roles feature slices
global using BizI.Application.Features.Roles.Create;
global using BizI.Application.Features.Roles.Update;
global using BizI.Application.Features.Roles.Delete;
global using BizI.Application.Features.Roles.GetAll;
global using BizI.Application.Features.Roles.GetById;
global using BizI.Application.Features.Roles.Dtos;

// Application — Users feature slices
global using BizI.Application.Features.Users.Create;
global using BizI.Application.Features.Users.Update;
global using BizI.Application.Features.Users.Delete;
global using BizI.Application.Features.Users.GetAll;
global using BizI.Application.Features.Users.GetById;
global using BizI.Application.Features.Users.Dtos;

// Application — Inventory feature slices
global using BizI.Application.Features.Inventory.GetAll;
global using BizI.Application.Features.Inventory.GetByProduct;
global using BizI.Application.Features.Inventory.GetByWarehouse;
global using BizI.Application.Features.Inventory.ImportStock;
global using BizI.Application.Features.Inventory.ExportStock;
global using BizI.Application.Features.Inventory.ReturnStock;
global using BizI.Application.Features.Inventory.AdjustStock;
global using BizI.Application.Features.Inventory.Dtos;

// Application — InventoryTransactions feature slices
global using BizI.Application.Features.InventoryTransactions.GetAll;
global using BizI.Application.Features.InventoryTransactions.GetById;
global using BizI.Application.Features.InventoryTransactions.Dtos;

// Application — ImportOrders feature slices
global using BizI.Application.Features.ImportOrders.GetAll;
global using BizI.Application.Features.ImportOrders.GetById;
global using BizI.Application.Features.ImportOrders.Create;
global using BizI.Application.Features.ImportOrders.Confirm;
global using BizI.Application.Features.ImportOrders.Receive;
global using BizI.Application.Features.ImportOrders.Delete;
global using BizI.Application.Features.ImportOrders.Dtos;

// Application — StockItems feature slices
global using BizI.Application.Features.StockItems.Create;
global using BizI.Application.Features.StockItems.Update;
global using BizI.Application.Features.StockItems.Delete;
global using BizI.Application.Features.StockItems.GetAll;
global using BizI.Application.Features.StockItems.GetById;
global using BizI.Application.Features.StockItems.Dtos;

// Application — StockOperations feature slices
global using BizI.Application.Features.StockOperations.Create;
global using BizI.Application.Features.StockOperations.GetAll;
global using BizI.Application.Features.StockOperations.GetById;
global using BizI.Application.Features.StockOperations.Dtos;

// Application — StockTransactions feature slices
global using BizI.Application.Features.StockTransactions.Create;
global using BizI.Application.Features.StockTransactions.GetAll;
global using BizI.Application.Features.StockTransactions.GetById;
global using BizI.Application.Features.StockTransactions.Dtos;
