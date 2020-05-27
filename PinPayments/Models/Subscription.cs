using System;

using Newtonsoft.Json;
using PinPayments.Actions;

namespace PinPayments.Models
{
    public class SubscriptionAdd : PinError
    {
        [JsonProperty("response")]
        public Subscription Response { get; set; }
    }

    public class Subscriptions : PinError
    {
        [JsonProperty("response")]
        public Subscription[] Response { get; set; }

        [JsonProperty("count")]
        public long Count { get; set; }

        [JsonProperty("pagination")]
        public Pagination Pagination { get; set; }
    }

    public class SubscriptionResponse : PinError
    {
        public Subscription Response { get; set; }
    }

    public class Subscription
    {
        [JsonProperty("state")]
        public string State { get; set; }

        [JsonProperty("next_billing_date")]
        public DateTimeOffset? NextBillingDate { get; set; }

        [JsonProperty("active_interval_started_at")]
        public DateTimeOffset? ActiveIntervalStartedAt { get; set; }

        [JsonProperty("active_interval_finishes_at")]
        public DateTimeOffset? ActiveIntervalFinishesAt { get; set; }

        [JsonProperty("cancelled_at")]
        public DateTimeOffset? CancelledAt { get; set; }

        [JsonProperty("created_at")]
        public DateTimeOffset? CreatedAt { get; set; }

        [JsonProperty("token")]
        public string Token { get; set; }

        [JsonProperty("plan_token")]
        public string PlanToken { get; set; }

        [JsonProperty("customer_token")]
        public string CustomerToken { get; set; }

        [JsonProperty("card_token")]
        public string CardToken { get; set; }
    }

    public partial class SubsriptionLedger
    {
        [JsonProperty("response")]
        public Ledger[] Response { get; set; }

        [JsonProperty("count")]
        public long Count { get; set; }

        [JsonProperty("pagination")]
        public Pagination Pagination { get; set; }
    }

    public partial class Ledger
    {
        [JsonProperty("created_at")]
        public DateTimeOffset CreatedAt { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("amount")]
        public long Amount { get; set; }

        [JsonProperty("currency")]
        public string Currency { get; set; }

        [JsonProperty("annotation")]
        public string Annotation { get; set; }
    }
}
