public class TransferRecord
{
    public int Id { get; set; }
    public required int AnimalId { get; set; }
    public Animal? animal { get; set; }

    public required DateOnly LastDateAtZoo { get; set; }

    public required string LocationOfTransfer { get; set; }
}