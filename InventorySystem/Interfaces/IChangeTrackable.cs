namespace InventorySystem.Interfaces;

public interface IChangeTrackable
{
    public bool IsChanged { get; set; }
}