using System;
using System.Collections.Generic;
using System.Collections.ObjectModel; // Necesario para ReadOnlyCollection
using Domain.Entities;

namespace Domain.Services; // CORRECCIÓN 1: Namespace nombrado

public static class OrderService
{
    // CORRECCIÓN 3: Encapsulación de la colección
    private static readonly List<Order> _lastOrders = new List<Order>();

    // Exponemos una vista de solo lectura. Los externos pueden leer, pero no alterar la lista.
    public static ReadOnlyCollection<Order> LastOrders => _lastOrders.AsReadOnly();

    // CORRECCIÓN 2: Instancia única de Random para toda la clase
    private static readonly Random _random = new Random();

    public static Order CreateTerribleOrder(string customer, string product, int qty, decimal price)
    {
        var o = new Order 
        { 
            Id = _random.Next(1, 9999999), 
            CustomerName = customer, 
            ProductName = product, 
            Quantity = qty, 
            UnitPrice = price 
        };
        
        _lastOrders.Add(o); // El servicio sí puede modificar su propia lista privada
        
        Infrastructure.Logging.Logger.Log("Created order " + o.Id + " for " + customer);
        return o;
    }
}