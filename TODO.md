# BizI Project TODO List

This document tracks the progress of the BizI (PilotSale) management system development.

## 🟢 Phase 1: MVP - Core Sales & Inventory (Current)

### 🏗️ Foundation & Auth

- [x] Implement Project Architecture (Clean Architecture)
- [x] Configure Multi-tenant dynamic database connection (LiteDB per TenantID)
- [x] Tenant extraction from JWT middleware
- [x] User Registration with BCrypt hashing
- [x] User Login with JWT token generation
- [x] Request Validation using FluentValidation + MediatR pipeline behaviors

### 📦 Product Management

- [x] Product CRUD (Create, Read, Update, Delete)
- [x] Product Stock tracking (initial stock)
- [ ] Product Categories (Basic)
- [ ] SKU/Barcode field support

### 🛒 Sales & Orders

- [x] Create Order command with stock deduct check
- [x] Order calculation (Total, Tax, Discount)
- [x] Handle Order Returns (stock restoration)
- [ ] Order Status management (Pending, Paid, Cancelled)

### 📉 Inventory Flow

- [x] **Import Stock**: Manually increase stock level via API
- [x] **Export Stock**: Manually decrease stock level (Manual adjustment)
- [x] **Symmetry**: Automatic stock deduction on Sale
- [x] **Returns**: Automatic stock increase on Return Order
- [ ] Inventory Movement Logs (for audit trails)

---

## 🟡 Phase 2: Management & Analytics

### 👥 Customer Management

- [ ] Customer profile creation (Name, Phone, History)
- [ ] Customer groups (VIP, Regular, New)
- [ ] Loyalty point system (Simple % based)

### 📊 Dashboard & Reporting

- [ ] Total Sales (Daily, Weekly, Monthly)
- [ ] Top Selling Products (by quantity and revenue)
- [ ] Low Stock Alerts (Configurable threshold)
- [ ] Profit Margin Calculation per product

### ⌨️ UI/UX Improvements (Integrations)

- [ ] Simple UI for inventory management (HTML/JS)
- [ ] Image upload capability for Products (to local storage/Blob)
- [ ] Search & Filter enhancements (fuzzy search)

---

## 🔵 Phase 3: Advanced Business Features

### 🏢 Enterprise Scaling

- [ ] Multi-branch support within one tenant
- [ ] Role-Based Access Control (RBAC): Admin, Manager, Cashier
- [ ] External Integrations (Payment Gateways, Shipping Providers)

### 🛠️ Advanced Inventory

- [ ] Batch & Expiry date tracking
- [ ] Supplier management & Purchase Orders (PO)
- [ ] Stock-take / Stock auditing module

### 📈 AI & Automation

- [ ] Sales forecasting based on historical data
- [ ] AI-driven reorder suggestions
- [ ] Automated daily sales reports via email/Telegram

---

_Last Updated: April 5, 2026_
