using exe2.equipment;

namespace exe2.crud;

public interface IEquipmentManager
{
    void AddEquipment(Equipment equipment);
    IEnumerable<(Equipment Equipment, string Status)> GetAllEquipmentWithStatus();
    IEnumerable<Equipment> GetAvailableEquipment();
    void MarkEquipmentAvailable(string equipmentId);
    void MarkEquipmentUnavailable(string equipmentId);
}
