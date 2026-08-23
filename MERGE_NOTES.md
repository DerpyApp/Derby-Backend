# Derby Backend - Merged Version

This folder combines the latest `main` changes with the Club Owner / Court Management work from `Derby-Backend-Updated-v2.zip`.

## Included
- Latest `main` authentication/Identity changes, notification controller, global exception middleware, and related updates.
- Club Owner endpoints and services (#29-36).
- `Club.OwnerId` and `User.OwnedClubs` relationship.
- `CourtBlock` model/repository and owner court blocking endpoints.
- Court-block checks in booking and court availability.
- Dependency injection registrations for owner services and `CourtBlockRepo`.
- `COURTS_MODULE_README.md` from v2.

## Important
The uploaded v2 did not contain a new EF Core migration for `OwnerId` and `CourtBlock`. The merged code therefore keeps the latest existing migrations rather than fabricating a generated migration.

From the solution root, after reviewing the code, generate/apply the migration with:

```bash
dotnet ef migrations add AddCourtOwnershipAndBlocks -p PadelBooking.DAL -s PadelBooking.API
dotnet ef database update -p PadelBooking.DAL -s PadelBooking.API
```

Review the generated migration before applying it to any shared/production database.
