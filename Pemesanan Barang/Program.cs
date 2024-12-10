using System;

public interface IInventoryService
{
    bool CheckStock(string productId, int quantity);
    void UpdateStock(string productId, int quantity);
    bool CheckProductHealth(string productId);
}

// Single Responsibility Principle (SRP)
public class InventoryService : IInventoryService
{
    private int stock = 10;

    public bool CheckStock(string productId, int quantity)
    {
        Console.WriteLine($"📋 Checking stock for product: {productId}\n");
        return stock >= quantity;
    }

    public void UpdateStock(string productId, int quantity)
    {
        Console.WriteLine($"📈 Updating stock for product: {productId} by quantity: {quantity}\n");
        stock -= quantity;
    }

    public bool CheckProductHealth(string productId)
    {
        Console.WriteLine($"🔍 Checking health for product: {productId}\n");
        return productId.StartsWith("BY");
    }
}

public interface IPaymentService
{
    bool ProcessPayment(string accountNumber, int amount);
}

// Single Responsibility Principle (SRP)
public class PaymentService : IPaymentService
{
    private int accountBalance = 450000;

    public bool ProcessPayment(string accountNumber, int amount)
    {
        if (accountBalance < amount)
        {
            Console.WriteLine("❌ Insufficient balance.\n");
            return false;
        }

        Console.WriteLine($"💲 Processing payment of ${amount} from account: {accountNumber}\n");
        accountBalance -= amount;
        return true;
    }
}

public interface IShippingService
{
    void ArrangeShipping(string productId, string shippingAddress);
}

// Single Responsibility Principle (SRP)
public class ShippingService : IShippingService
{
    public void ArrangeShipping(string productId, string shippingAddress)
    {
        Console.WriteLine($"🎁 Arranging shipping for product: {productId} to address: {shippingAddress}\n");
    }
}

// Facade for Order Management
public class OrderService
{
    private readonly IInventoryService _inventoryService;
    private readonly IPaymentService _paymentService;
    private readonly IShippingService _shippingService;

    public OrderService(IInventoryService inventoryService, IPaymentService paymentService, IShippingService shippingService)
    {
        _inventoryService = inventoryService;
        _paymentService = paymentService;
        _shippingService = shippingService;
    }

    public bool PlaceOrder(string productId, int quantity, string accountNumber, int amount, string shippingAddress)
    {
        if (!_inventoryService.CheckStock(productId, quantity))
        {
            Console.WriteLine("❌ Product out of stock. Order not placed.\n");
            return false;
        }

        if (!_inventoryService.CheckProductHealth(productId))
        {
            Console.WriteLine("❌ Product is not in good condition. Order not placed.\n");
            return false;
        }

        if (!_paymentService.ProcessPayment(accountNumber, amount))
        {
            Console.WriteLine("❌ Payment failed. Order not placed.\n");
            return false;
        }

        _inventoryService.UpdateStock(productId, quantity);
        _shippingService.ArrangeShipping(productId, shippingAddress);
        Console.WriteLine("✅ Order placed successfully!\n");
        return true;
    }
}

public class Program
{
    public static void Main(string[] args)
    {
        // Dependency Injection via constructor
        IInventoryService inventoryService = new InventoryService();
        IPaymentService paymentService = new PaymentService();
        IShippingService shippingService = new ShippingService();

        OrderService orderService = new OrderService(inventoryService, paymentService, shippingService);

        string productId = "BY0N3";
        int quantity = 4;
        string accountNumber = "ACC123";
        int amount = 300000;
        string shippingAddress = "Indonesia, Surabaya, Sukolilo, Jl. Bumi Marina Perunggu No. 99";

        orderService.PlaceOrder(productId, quantity, accountNumber, amount, shippingAddress);
    }
}
