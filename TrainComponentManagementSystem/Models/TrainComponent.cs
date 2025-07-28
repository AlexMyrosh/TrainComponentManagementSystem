namespace TrainComponentManagementSystem.Models
{
    public class TrainComponent
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public string UniqueNumber { get; set; } = null!;

        public bool CanAssignQuantity { get; set; }

        public int? Quantity { get; set; }

        public bool IsDeleted { get; set; }
    }
}
