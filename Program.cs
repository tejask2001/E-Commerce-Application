// See https://aka.ms/new-console-template for more information

using E_Commerce_App.dao;
using E_Commerce_App.Entity;


IOrderProcessorRepository impl = new OrderProcessorRepositoryImpl();
IServiceRepository service= new ServiceRepository();

int choice = 0;
int i = 1;
do
{
    Console.WriteLine("Welcome to E-commerce Application\n" +
        "\nPress 1: Register Customer\n" +
    "Press 2: Create Product\n" +
    "Press 3: Delete Product\n" +
    "Press 4: Add to Cart\n" +
    "Press 5: View Cart\n" +
    "Press 6: Place Order\n" +
    "Press 7: View Customer Order\n" +
    "Press 0 to Exit");
    choice = Convert.ToInt32(Console.ReadLine());
    switch (choice)
    {
        case 0:
            i = 0;
            break;
        case 1:
            service.CreateCustomer();
            break;
        case 2:
            service.CreateProduct();
            break;
        case 3:
            service.DeleteProduct();
            break;
        case 4:
            service.AddToCart();
            break;
        case 5:
            service.ProductInCart();
            break;
        case 6:
            service.PlaceOrder();
            break;
        case 7:
            service.GetOrderByCustomer();
            break;
        default:
            Console.WriteLine("Invalid Inputs....");
            break;
    }
    Console.WriteLine("\nPress 0 to exit or any key to continue...");
    int exit=Convert.ToInt32(Console.ReadLine());
    if (exit == 0)
        break;
} while (i > 0);
