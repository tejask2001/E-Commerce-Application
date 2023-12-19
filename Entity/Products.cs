﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce_App.Entity
{
    public class Products
    {
        int productId;
        string name;
        double price;
        string description;
        int stockQuantity;

        public Products() { }
        public Products(int productId, string name, double price, string description, int stockQuantity)
        {
            this.productId = productId;
            this.name = name;
            this.price = price;
            this.description = description;
            this.stockQuantity = stockQuantity;
        }

        public int ProductId
        {
            get { return productId; }
            set { productId = value; }
        }
        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        public double Price
        {
            get { return price; }
            set { price = value; }
        }
        public string Description
        {
            get { return description; }
            set { description = value; }
        }

        public int StockQuantity
        {
            get { return stockQuantity; }
            set { stockQuantity = value; }
        }

        public override string ToString()
        {
            return $"Name \t\t: {name}\n" +
                $"Price \t\t: {price}\n" +
                $"Description\t: {description}\n"+
                $"StockQuantity\t: {stockQuantity}\n" +
                $"\n";
        }
    }
}
