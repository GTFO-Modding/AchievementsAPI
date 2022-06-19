namespace AchievementsAPI
{
#pragma warning disable CS0660 // Type defines operator == or operator != but does not override Object.Equals(object o)
#pragma warning disable CS0661 // Type defines operator == or operator != but does not override Object.GetHashCode()
    public struct AchievementTriggerProgress
#pragma warning restore CS0661 // Type defines operator == or operator != but does not override Object.GetHashCode()
#pragma warning restore CS0660 // Type defines operator == or operator != but does not override Object.Equals(object o)
    {
        public int TriggerCount { get; set; }

        public static bool operator ==(AchievementTriggerProgress a, AchievementTriggerProgress b)
        {
            return a.TriggerCount == b.TriggerCount;
        }
        public static bool operator !=(AchievementTriggerProgress a, AchievementTriggerProgress b)
            => !(a == b);
    }
}
