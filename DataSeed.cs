using System;
using System.Collections.Generic;
using System.Linq;
using Advantage.API.Models;

namespace Advantage.API
{
    public class DataSeed
    {
        private readonly ApiContext _context;
        public DataSeed(ApiContext ctx)
        {
            _context = ctx;
        }

        public void SeedData(int nCustomers, int nOrders)
        {
            if (!_context.Customers.Any())
            {
                SeedCustomers(nCustomers);
                _context.SaveChanges();
            }

            if (!_context.Orders.Any())
            {
                SeedOrders(nOrders);
                _context.SaveChanges();
            }

            if (!_context.Servers.Any())
            {
                SeedServers();
                _context.SaveChanges();
            }

        }

        private void SeedServers()
        {
            List<Server> servers = BuildServerList();

            foreach (var server in servers)
            {
                _context.Servers.Add(server);
            }

        }

        private List<Server> BuildServerList()
        {
            var servers = new List<Server>();
            return new List<Server>()
            {
                new Server
                {
                    Id = 1,
                    Name = "Dev-Web",
                    IsOnline = true
                },
                new Server
                {
                    Id = 2,
                    Name = "Dev-Mail",
                    IsOnline = false
                },
                new Server
                {
                    Id = 3,
                    Name = "Prod-Web",
                    IsOnline = true
                },
                new Server
                {
                    Id = 4,
                    Name = "Prod-Mail",
                    IsOnline = true
                },
                new Server
                {
                    Id = 5,
                    Name = "Prod-Services",
                    IsOnline = true
                },
                  new Server
                {
                    Id = 6,
                    Name = "QA-Web",
                    IsOnline = true
                },
                new Server
                {
                    Id = 7,
                    Name = "QA-Mail",
                    IsOnline = true
                },
                new Server
                {
                    Id = 8,
                    Name = "QA-Services",
                    IsOnline = true
                },
            };
        }

        private List<Customer> BuildCustomerList(int nCustomers)
        {
            var customers = new List<Customer>();
            var names = new List<string>();
            for (var i = 1; i <= nCustomers; i++)
            {
                var name = Helpers.MakeUniqueCustomerName(names);
                customers.Add(new Customer
                {
                    Id = i,
                    Name = name,
                    Email = Helpers.MakeCustomerEmail(name),
                    State = Helpers.MakeCustomerState()
                });
            }

            return customers;
        }

        private void SeedOrders(int nCustomers)
        {
            List<Order> orders = BuildOrderList(nCustomers);
            foreach (var order in orders)
                _context.Add(order);
        }

        private List<Order> BuildOrderList(int nOrders)
        {
            var orders = new List<Order>();
            var rand = new Random();

            for (var i = 1; i < nOrders; i++)
            {
                var randCustomerId = rand.Next(1, _context.Customers.Count());
                var placed = Helpers.GetRandomOrderPlaced();
                var completed = Helpers.GetRandomOrderCompleted(placed);
                var customers = _context.Customers.ToList();

                orders.Add(new Order
                {
                    Id = i,
                    Customer = customers.First(c => c.Id == randCustomerId),
                    OrderTotal = Helpers.GetRandomOrderTotal(),
                    OrderCompleted = completed,
                    OrderPlaced = placed
                });
            }
            return orders;
        }

        private void SeedCustomers(int nCustomers)
        {
            List<Customer> customers = BuildCustomerList(nCustomers);

            foreach (var customer in customers)
            {
                _context.Add(customer);
            }
        }
    }
}