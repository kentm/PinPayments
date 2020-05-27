using System;

using Newtonsoft.Json;
using PinPayments.Actions;

namespace PinPayments.Models
{
    public class PlanAdd : PinError
    {
        public Plan Response { get; set; }
    }

    public class Plans : PinError
    {
        [JsonProperty("response")]
        public Plan[] Response { get; set; }

        [JsonProperty("count")]
        public long Count { get; set; }

        [JsonProperty("pagination")]
        public Pagination Pagination { get; set; }
    }

    public class PlanResponse : PinError
    {
        public Plan Response { get; set; }
    }

    public class Plan
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("amount")]
        public long Amount { get; set; }

        [JsonProperty("currency")]
        public string Currency { get; set; }

        [JsonProperty("setup_amount")]
        public long SetupAmount { get; set; }

        [JsonProperty("trial_amount")]
        public long TrialAmount { get; set; }

        [JsonProperty("interval")]
        public long Interval { get; set; }

        [JsonProperty("interval_unit")]
        public string IntervalUnit { get; set; }

        [JsonProperty("intervals")]
        public long Intervals { get; set; }

        [JsonProperty("trial_interval")]
        public long TrialInterval { get; set; }

        [JsonProperty("trial_interval_unit")]
        public string TrialIntervalUnit { get; set; }

        [JsonProperty("created_at")]
        public DateTimeOffset CreatedAt { get; set; }

        [JsonProperty("token")]
        public string Token { get; set; }

        [JsonProperty("customer_permissions")]
        public string[] CustomerPermissions { get; set; }

        [JsonProperty("subscription_counts")]
        public SubscriptionCounts SubscriptionCounts { get; set; }
    }

    public partial class SubscriptionCounts
    {
        [JsonProperty("trial")]
        public long Trial { get; set; }

        [JsonProperty("active")]
        public long Active { get; set; }

        [JsonProperty("cancelling")]
        public long Cancelling { get; set; }

        [JsonProperty("cancelled")]
        public long Cancelled { get; set; }
    }
}
