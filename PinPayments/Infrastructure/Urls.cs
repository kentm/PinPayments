using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace PinPayments.Infrastructure
{
    internal static class Urls
    {
        public static string Card
        {
            get { return BaseUrl + "/1/cards"; }
        }

        public static string ChargesSearch
        {
            get { return BaseUrl + "/1/charges/search"; }
        }
        
        public static string Charge
        {
            get { return BaseUrl + "/1/charges"; }
        }

        public static string Charges
        {
            get { return BaseUrl + "/1/charges/"; }
        }

        public static string CustomerAdd
        {
            get { return BaseUrl + "/1/customers"; }
        }

        public static string Customers
        {
            get { return BaseUrl + "/1/customers"; }
        }

        public static string CustomerCharges
        {
            get { return BaseUrl + "/1/customers/{token}/charges"; }
        }

        public static string Refund
        {
            get { return BaseUrl + "/1/charges/{token}/refunds"; }
        }

        public static string Subscriptions
        {
            get { return BaseUrl + "/1/subscriptions"; }
        }

        public static string Subscription
        {
            get { return BaseUrl + "/1/subscriptions/{token}"; }
        }

        public static string SubscriptionReactivate
        {
            get { return BaseUrl + "/1/subscriptions/{token}/reactivate"; }
        }

        public static string SubscriptionLedger
        {
            get { return BaseUrl + "/1/subscriptions/{token}/ledger"; }
        }

        public static string Plans
        {
            get { return BaseUrl + "/1/plans"; }
        }

        public static string Plan
        {
            get { return BaseUrl + "/1/plans/{token}"; }
        }

        public static string PlanSubscriptions
        {
            get { return BaseUrl + "/1/plans/{token}/subscriptions"; }
        }

        private static string BaseUrl
        {
            get { return ConfigurationManager.AppSettings["URI"]; }
        }
    }
}
