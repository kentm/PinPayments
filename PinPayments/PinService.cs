using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PinPayments.Models;
using System.Net;
using System.Configuration;
using System.IO;
using Newtonsoft.Json;
using PinPayments.Infrastructure;
using PinPayments.Actions;

namespace PinPayments
{
    public class PinService
    {
        public PinService()
        {
        }

        public PinService(string pinKey)
        {
            PinPaymentsConfig.SetApiKey( pinKey);
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls;
        }

        #region Cards

        public CardCreateResponse CardCreate(Card c)
        {
            var url = Urls.Card;
            var postData = ParameterBuilder.ApplyAllParameters(c, "");

            var response = Requestor.PostString(url, postData);
            return JsonConvert.DeserializeObject<CardCreateResponse>(response);
        }

        #endregion

        #region Charges

        public Charges Charges()
        {
            var url = Urls.Charges;
            var response = Requestor.GetString(url);

            var result = JsonConvert.DeserializeObject<Charges>(response);
            return result;
        }

        public Charges CustomerCharges(string customerToken)
        {
            var url = Urls.CustomerCharges.Replace("{token}", customerToken);
            var response = Requestor.GetString(url);

            var result = JsonConvert.DeserializeObject<Charges>(response);
            return result;
        }

        public ChargeDetail Charge(string token)
        {
            var url = Urls.Charges + token;
            var response = Requestor.GetString(url);

            var result = JsonConvert.DeserializeObject<ChargeDetail>(response);
            return result;
        }

        public ChargeResponse Charge(PostCharge c)
        {
            var url = Urls.Charge;
            var postData = ParameterBuilder.ApplyAllParameters(c, "");

            if (c.Card != null)
            {
                postData += "&card[number]=" + c.Card.CardNumber + "&card[expiry_month]=" + c.Card.ExpiryMonth + "&card[expiry_year]=" + c.Card.ExpiryYear + "&card[cvc]=" + c.Card.CVC + "&card[name]=" + c.Card.Name;
                postData += "&card[address_line1]=" + c.Card.Address1 + "&card[address_line2]=" + c.Card.Address2 + "&card[address_city]=" + c.Card.City + "&card[address_postcode]=" + c.Card.Postcode;
                postData += "&card[address_state]=" + c.Card.State + "&card[address_country]=" + c.Card.Country;
            }
            else if (c.CustomerToken != null)
            {
                postData += "&customer_token=" + c.CustomerToken;
            }
            else if (c.CardToken != null)
            {
                postData += "&card_token=" + c.CardToken;
            }
            else
            {
                throw new PinException(HttpStatusCode.BadRequest, null, "You need to supply either the Card, the Customer Token or a Card Token for payment");
            }
            var response = Requestor.PostString(url, postData);
            return JsonConvert.DeserializeObject<ChargeResponse>(response);

        }

        public Charges ChargesSearch(ChargeSearch cs)
        {
            var url = ParameterBuilder.ApplyAllParameters(cs, Urls.ChargesSearch);

            var response = Requestor.GetString(url);
            return JsonConvert.DeserializeObject<Charges>(response);
        }

        #endregion

        #region Customers

        public CustomerAdd CustomerAdd(Customer c)
        {
            var url = Urls.CustomerAdd;
            var postData = ParameterBuilder.ApplyAllParameters(c, "");

            var response = Requestor.PostString(url, postData);
            var customerAdd = JsonConvert.DeserializeObject<CustomerAdd>(response);

            return customerAdd;
        }

        public CustomerUpdate CustomerUpate(Customer c)
        {
            var url = Urls.CustomerAdd + "/" + c.Token;
            var postData = ParameterBuilder.ApplyAllParameters(c, "");

            var response = Requestor.PutString(url, postData);
            var result = JsonConvert.DeserializeObject<CustomerUpdate>(response);
            return result;
        }

        public Customers Customers()
        {
            return Customers(null);
        }

        public Customers Customers(int? page)
        {
            var url = Urls.Customers;
            if (page != null)
            {
                url += "?page=" + page.ToString();
            }
            var response = Requestor.GetString(url);


            var result = JsonConvert.DeserializeObject<Customers>(response);
            return result;
        }

        public Customer Customer(string token)
        {
            var url = Urls.Customers + "/" + token;

            var response = Requestor.GetString(url);
            var customer = JsonConvert.DeserializeObject<CustomerAdd>(response);
            return customer.Response;
        }

        #endregion

        #region Refunds

        public RefundResponse Refund(string chargeToken, int amount)
        {
            var url = Urls.Refund;
            var response = Requestor.PostString(url.Replace("{token}", chargeToken), "amount=" + amount.ToString());
            var result = JsonConvert.DeserializeObject<RefundResponse>(response);
            return result;
        }

        public RefundsResponse Refunds(string chargeToken)
        {
            var url = Urls.Refund;
            var response = Requestor.GetString(url.Replace("{token}", chargeToken));
            var result = JsonConvert.DeserializeObject<RefundsResponse>(response);
            return result;
        }

        #endregion
        
        #region Subscriptions

        public Subscription SubscriptionAdd(Subscription subscription)
        {
            var url = Urls.Subscriptions;
            var postData = ParameterBuilder.ApplyAllParameters(subscription, "");

            var response = Requestor.PostString(url, postData);
            var result = JsonConvert.DeserializeObject<SubscriptionAdd>(response);
            return result.Response;
        }

        public Subscription[] Subscriptions()
        {
            var url = Urls.Subscriptions;

            var response = Requestor.GetString(url);
            var result = JsonConvert.DeserializeObject<Subscriptions>(response);
            return result.Response;
        }

        public Subscription Subscription(string subscriptionToken)
        {
            var url = Urls.Subscription;

            var response = Requestor.GetString(url.Replace("{token}", subscriptionToken));
            var result = JsonConvert.DeserializeObject<SubscriptionResponse>(response);
            return result.Response;
        }

        public Subscription SubscriptionUpdate(string subscriptionToken, string cardToken)
        {
            var url = Urls.Subscriptions;

            var response = Requestor.PutString(url.Replace("{token}", subscriptionToken), "card_token=" + cardToken);
            var result = JsonConvert.DeserializeObject<SubscriptionResponse>(response);
            return result.Response;
        }

        public bool SubscriptionDelete(string subscriptionToken)
        {
            var url = Urls.Subscription;

            try
            {
                var response = Requestor.Delete(url.Replace("{token}", subscriptionToken));
                return true;
            }
            catch (Exception) // Don't use try/catch to test success/failure
            {
                return false;
            }
        }

        public Subscription SubscriptionReactivate(string subscriptionToken, bool includeSetupFee)
        {
            var url = Urls.SubscriptionReactivate;

            var response = Requestor.PutString(url.Replace("{token}", subscriptionToken), "include_setup_fee=" + includeSetupFee.ToString());
            var result = JsonConvert.DeserializeObject<SubscriptionResponse>(response);
            return result.Response;
        }

        public Ledger[] SubscriptionLedger(string subscriptionToken)
        {
            var url = Urls.SubscriptionLedger;

            var response = Requestor.GetString(url.Replace("{token}", subscriptionToken));
            var result = JsonConvert.DeserializeObject<SubsriptionLedger>(response);
            return result.Response;
        }

        #endregion

        #region Plans

        public Plan PlanAdd(Plan plan)
        {
            var url = Urls.Plans;
            var postData = ParameterBuilder.ApplyAllParameters(plan, "");

            var response = Requestor.PostString(url, postData);
            var result = JsonConvert.DeserializeObject<PlanAdd>(response);
            return result.Response;
        }

        public Plan[] Plans()
        {
            var url = Urls.Plans;

            var response = Requestor.GetString(url);
            var result = JsonConvert.DeserializeObject<Plans>(response);
            return result.Response;
        }

        public Plan Plan(string planToken)
        {
            var url = Urls.Plan;

            var response = Requestor.GetString(url.Replace("{token}", planToken));
            var result = JsonConvert.DeserializeObject<PlanResponse>(response);
            return result.Response;
        }

        public Subscription[] PlanSubscriptions(string planToken)
        {
            var url = Urls.PlanSubscriptions;

            var response = Requestor.GetString(url.Replace("{token}", planToken));
            var result = JsonConvert.DeserializeObject<Subscriptions>(response);
            return result.Response;
        }

        public bool PlanDelete(string planToken)
        {
            var url = Urls.Plan;

            try
            {
                var response = Requestor.Delete(url.Replace("{token}", planToken));
                return true;
            }
            catch (Exception) // Don't use try/catch to test success/failure
            {
                return false;
            }            
        }

        #endregion
    }
}
