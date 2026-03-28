using System;

namespace WebApi.Controllers
{
    public class OrdersController 
    {
        public const string DefaultMessage = "This controller does nothing. Endpoints are in Program.cs";
        public static string DoNothing() => DefaultMessage;
    }
}