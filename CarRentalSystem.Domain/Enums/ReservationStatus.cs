namespace CarRentalSystem.Domain.Enums;

public enum ReservationStatus
{
    Pending = 1,      // Customer requested
    Confirmed = 2,    // Admin approved
    Active = 3,       // Currently renting
    Completed = 4,    // Returned successfully
    Cancelled = 5     // Cancelled by customer or admin
}