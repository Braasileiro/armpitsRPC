namespace ArmpitsRPC.Models
{
    internal class AppsModel
    {
        public required string DiscordApplicationId { get; set; }
        public required string ProcessName { get; set; }
        public required string Name { get; set; }
        public required string? Details { get; set; }
        public required string? State { get; set; }
        public required string? LargeImage { get; set; }
        public required string? LargeText { get; set; }
        public required string? SmallImage { get; set; }
        public required string? SmallText { get; set; }
    }
}
