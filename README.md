# TradingPlatform Project

**Boosting Performance with Unsafe Code in .NET**

This project demonstrates high-performance memory manipulation techniques using `unsafe` code in .NET. The goal is to build a performant trading application that leverages low-level optimizations using pointer arithmetic, stack allocation, unmanaged memory, and fixed-size buffers.

---

## ✅ Project Setup

### Features Implemented

- Enabled `unsafe` code in the project file.
- Integrated pointer types and `fixed` statements for optimized order management.
- Utilized `stackalloc` for temporary memory allocation.
- Employed fixed-size buffers to efficiently manage trade reporting.
- Combined all concepts to build a Bulk Order Cancellation feature.

---

## 🛠️ Technical Breakdown

### 🔧 Enabling Unsafe Code

Modify the `.csproj` file to enable unsafe code:

```xml
<PropertyGroup>
  <AllowUnsafeBlocks>true</AllowUnsafeBlocks>
</PropertyGroup>

[C# Unsafe Code](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/unsafe-code)

[Pointer Types](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/unsafe-code#pointer-types)


## 📁 Project Structure

/TradingPlatform
├── Program.cs
├── TradingPlatform.csproj
├── README.md

├── Core/
│   ├── Trade.cs
│   ├── OrderBook.cs
│   ├── OrderCancellationRequest.cs
│   ├── PriceNotificationEventArgs.cs
│   └── User.cs

├── Services/
│   ├── TradeLogger.cs


---

## 📦 Requirements

- .NET 7.0 or later
- Enable `unsafe` context in the project settings
- Visual Studio / JetBrains Rider / VS Code with C# extension

---

## ▶️ Run the Project

```bash
dotnet build
dotnet run

# Branches

- main
    Setting Up the Project and Implementing Basic Order Book Initialization
    ● Enable Unsafe Code: Modify your project file to allow unsafe code, enabling the use of pointers for performance optimization.
    ● Pointer Types: Use pointer types to gain direct access to memory for faster data manipulation.
    ● Stack Allocation: Utilize stackalloc for efficient temporary storage, reducing heap memory
    usage.

- pointer-orderbook-and-events
    Implementing the Order Book with Pointer Types
    ● Pointer Arithmetic: Traverse and manipulate data structures efficiently using pointers.
    ● Native Memory Management: Allocate and free unmanaged memory with NativeMemory for
    precise control over memory usage.

- simulate-incoming-orders
    Using Fixed and Moveable Variables for Price Notifications
    ● Fixed Statement: Use the fixed statement to pin variables in memory and prevent garbage collection from relocating them.
    in variables during critical operations to ensure stable pointers and avoid data corruption
    ● Memory Safety: Pin variables during critical operations to ensure stable pointers and avoid data corruption.

- order-fulfillment
    Pointer Conversions and Expressions in Order Fulfillment
    ● Pointer Conversions: Convert between different pointer types as needed for flexible memory manipulation.
    ● Pointer Arithmetic: Perform arithmetic operations on pointers to efficiently traverse and manage data.
    ● Data Integrity: Maintain data integrity by using proper pointer conversions and arithmetic techniques.

- trade_report_generation
    Generate Trade Report And Log 

- cancel-order
    Implementing Fixed-Size Buffers and Stack Allocation for Order Cancellation
    ● Fixed-Size Buffers: Declare fixed-size buffers to ensure predictable memory usage and improve performance.
    ● Manual Memory Management: Optimize performance in high-frequency applications by mastering manual memory management techniques.
