namespace brandportal_dotnet.Contracts.Games
{
    public class GameGeneralInfoCreateDto
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string? CateId { get; set; }
        public bool? IsActive { get; set; }
        public string PeriodType { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string? FrequencyValue { get; set; }
        public string? FrequencyMonthlyType { get; set; }
        public string Frequency { get; set; }
        public string? DayInMonthly { get; set; }
        public string? DayInWeek { get; set; }
        public string? DayInWeekRepeat { get; set; }
        public string PeriodInDateType { get; set; }
        public string? PeriodInDateStart { get; set; }
        public string? PeriodInDateEnd { get; set; }
        public string? RewardProgramId { get; set; }
        public string? ConfigTurn { get; set; }
    }
}
