using System;

//Se mantiene el namespace
namespace WebApi.Controllers
{
    public class OrdersController 
    {
        
        public const string DefaultMessage = "This controller does nothing. Endpoints are in Program.cs";

        public string DoNothing() => DefaultMessage;
    }
}