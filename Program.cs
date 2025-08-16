using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laboratory_Activity_6
{
    namespace RestaurantSplitter
    {
        class MenuItem
        {
            public string Name { get; set; }
            public double Price { get; set; }
            public List<string> AssignedDiners { get; set; }

            public MenuItem(string name, double price, List<string> assigned)
            {
                Name = name;
                Price = price;
                AssignedDiners = assigned;
            }
        }

        class Diner
        {
            public string Name { get; set; }
            public double TipPercent { get; set; }
            public List<MenuItem> Items { get; set; } = new List<MenuItem>();

            public Diner(string name, double tipPercent)
            {
                Name = name;
                TipPercent = tipPercent;
            }

            public double Subtotal()
            {
                return Items.Sum(i => i.Price);
            }

            public double TotalWithServiceAndTip(double serviceChargePercent)
            {
                double subtotal = Subtotal();
                double serviceCharge = subtotal * (serviceChargePercent / 100);
                double tip = subtotal * (TipPercent / 100);
                return subtotal + serviceCharge + tip;
            }
        }

        internal class Program
        {
            static double GetValidDouble(string prompt)
            {
                double val;
                while (true)
                {
                    Console.Write(prompt);
                    if (double.TryParse(Console.ReadLine(), out val) && val >= 0)
                        return val;
                    Console.WriteLine("Invalid input. Please enter a positive number.");
                }
            }

            static void Main(string[] args)
            {
                List<MenuItem> menuItems = new List<MenuItem>();
                List<Diner> diners = new List<Diner>();

                Console.Write("Enter number of diners: ");
                int dinerCount = int.Parse(Console.ReadLine());
                for (int i = 0; i < dinerCount; i++)
                {
                    Console.Write($"Enter name of diner {i + 1}: ");
                    string name = Console.ReadLine();
                    double tipPercent = GetValidDouble($"Enter tip % for {name}: ");
                    diners.Add(new Diner(name, tipPercent));
                }

                Console.Write("Enter number of menu items: ");
                int itemCount = int.Parse(Console.ReadLine());
                for (int i = 0; i < itemCount; i++)
                {
                    Console.WriteLine($"Menu Item {i + 1}");
                    Console.Write("Enter item name: ");
                    string itemName = Console.ReadLine();
                    double price = GetValidDouble("Enter price: ");

                    Console.Write("Enter assigned diners (comma separated names, leave blank for shared by all): ");
                    string assignedInput = Console.ReadLine();
                    List<string> assignedDiners;

                    if (string.IsNullOrWhiteSpace(assignedInput))
                    {
                        assignedDiners = diners.Select(d => d.Name).ToList(); 
                    }
                    else
                    {
                        assignedDiners = assignedInput.Split(',').Select(d => d.Trim()).ToList();
                    }

                    menuItems.Add(new MenuItem(itemName, price, assignedDiners));
                }

                double serviceChargePercent = GetValidDouble("\nEnter service charge %: ");

                foreach (var item in menuItems)
                {
                    double splitPrice = item.Price / item.AssignedDiners.Count;
                    foreach (var dinerName in item.AssignedDiners)
                    {
                        Diner diner = diners.FirstOrDefault(d => d.Name.Equals(dinerName, StringComparison.OrdinalIgnoreCase));
                        if (diner != null)
                        {
                            diner.Items.Add(new MenuItem(item.Name, splitPrice, null));
                        }
                    }
                }

                Console.WriteLine("BILL SPLIT");
                foreach (var diner in diners)
                {
                    Console.WriteLine($"\nDiner: {diner.Name}");
                    Console.WriteLine("Items:");
                    foreach (var item in diner.Items)
                    {
                        Console.WriteLine($" - {item.Name}: {item.Price:F2}");
                    }
                    double subtotal = diner.Subtotal();
                    double total = diner.TotalWithServiceAndTip(serviceChargePercent);
                    Console.WriteLine($"Subtotal: {subtotal:F2}");
                    Console.WriteLine($"Total (with service & tip): {total:F2}");
                }
            }
        }
    }
}
















