// System
global using System;
global using System.Collections.Generic;
global using System.Linq;
global using System.Threading;
global using System.Threading.Tasks;
global using System.Text;

// Third-party
global using MediatR;
global using FluentValidation;

// Project
global using BizI.Domain.Entities;
global using BizI.Domain.Enums;
global using BizI.Domain.Interfaces;
global using BizI.Domain.Exceptions;
global using BizI.Application.Common;
global using BizI.Application.Interfaces;
global using BizI.Application.Features.Inventory;
global using BizI.Application.Features.Orders;
global using BizI.Application.Features.Products;
global using BizI.Application.Features.Warehouses;