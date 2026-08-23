# Courts management module — what changed and how to run it

## 1. Create and apply the EF Core migration

The model changed (`Club.OwnerId`, the new `CourtBlock` table), so you need a new migration
before the app will run. From the solution root:

```
dotnet ef migrations add AddCourtOwnershipAndBlocks -p PadelBooking.DAL -s PadelBooking.API
dotnet ef database update -p PadelBooking.DAL -s PadelBooking.API
```

## 2. Backfill existing clubs

`Club.OwnerId` is a required column. If you already have club rows in your dev database from
testing, the migration will fail (or insert 0) until each existing club has a real owner. Either
truncate your test clubs, or run a one-off UPDATE to assign an owner before applying the
migration.

## 3. Two pre-existing bugs fixed as part of this

- `ClubRepo.GetClubByOwnerAsync` was filtering by `c.Id == ownerId` instead of
  `c.OwnerId == ownerId` — it would have returned the wrong club (or nothing) for every owner.
- `IBookingService`/`BookingService` and `IPaymentRepo`/`PaymentRepo` existed in the codebase but
  were never registered in `Program.cs`, so `BookingController` would have thrown a DI resolution
  error (500) on first use. Both are now registered.

## 4. Known gap, not fixed here (out of scope for this module)

`TokenService.GenerateAccessToken` does not put a role claim in the JWT — only `sub` and `jti`.
Every `[Authorize(Roles = "...")]` attribute anywhere in the API is currently a no-op. The new
`OwnerCourtController` uses `[Authorize]` (any logged-in user) and enforces the real permission
check in the service layer instead — the user must be the `OwnerId` on the club/court they're
touching. That's a correct and sufficient check for this module on its own, but the app still
has no way to stop, say, a plain player from hitting an owner endpoint for *their own* accidental
club (if they ever got OwnerId on one) versus a genuinely different user's resource, since there's
no role gate at all yet. Wiring role claims into the JWT is a separate, small piece of work
(add a `ClaimTypes.Role` claim in `TokenService`, sourced from `UserRoles`/`Roles`) — flag it for
your next session if you want it.
