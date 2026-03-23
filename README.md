University equipment management system for tracking:
- Current rentals
- Available equipment
- Outstanding balance of each user

It allows tracking camera, projector, and laptop rentals by students and employees alike.

# Coupling
The project addresses coupling in the Equipment and `crud` modules / packages.
Equipment is implemented as an abstract class to share common features like `isAvailable` across all equipment types.
The `crud` module is separated by responsibility instead, keeping each operation focused and independent.

# Structure
The project is divided into CRUD and Domain layers.
Domain types handle business domain concerns: bookkeeping of actual data and performing operations on it.

The CRUD layer couples the interface to the domain: it provides creation, listing, and deletion functions.

This way a clear separation of concerns is achieved.
