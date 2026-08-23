# Owner reservations, reports & notifications — endpoints #37-42

No schema changes here (no new tables/columns), so no new EF Core migration is needed —
everything below is read/aggregate queries over the existing `Bookings`/`Courts`/`Clubs`/
`Notifications` tables, plus one status field update.

## What was added

| # | Route | Controller | What it does |
|---|-------|------------|---------------|
| 37 | `GET /api/owner/reservations?date=&status=` | `OwnerReservationController` | Every reservation across all of the owner's clubs. `date` and `status` are both optional filters. |
| 38 | `PUT /api/owner/reservations/{id}/status` | `OwnerReservationController` | Confirm, complete, or cancel a single reservation. See allowed-transition rules below. |
| 39 | `GET /api/owner/reports/revenue?from=&to=` | `OwnerReportController` | Paid revenue in the range, broken down by club and by day. Defaults to the last 30 days if `from`/`to` are omitted. |
| 40 | `GET /api/owner/reports/bookings?from=&to=` | `OwnerReportController` | Status breakdown (pending/confirmed/completed/cancelled/expired) + cancellation rate + per-court utilization for the range. Same 30-day default. |
| 41 | `GET /api/owner/dashboard` | `OwnerReportController` | Quick summary: club/court counts, today's bookings & revenue, pending reservations, this month's revenue. |
| 42 | `GET /api/owner/notifications` | `OwnerNotificationController` | The owner's own `Notification` rows where `Type == Booking`, newest first — instant alerts for new/changed reservations. |

All four controllers use `[Authorize]` + a service-layer ownership check (same pattern as
`OwnerClubController`/`OwnerCourtController`) — there's still no role claim in the JWT, see the
"known gap" in `COURTS_MODULE_README.md`.

## #38 status transition rules

Owners can only move a reservation forward or cancel it — they can't reopen a finished/cancelled
booking or push a reservation back to `Pending`:

- `Pending` → `Confirmed` — allowed
- `Confirmed` → `Completed` — allowed
- anything not already `Completed`/`Cancelled` → `Cancelled` — allowed
- everything else — rejected with a 400 explaining why

## New repo methods

- `IBookingRepo.GetBookingsByOwnerAsync(ownerId, date?, status?)` — #37
- `IBookingRepo.GetBookingWithCourtAndClubAsync(bookingId)` — #38 response after update
- `IBookingRepo.GetBookingsByOwnerInRangeAsync(ownerId, from, to)` — #39/#40/#41
- `ICourtRepo.GetCourtsByOwnerAsync(ownerId)` — #41 court count
- `INotificationRepo.GetNotificationsByUserIdAndTypeAsync(userId, type)` — #42

## Revenue definition

`RevenueReportDto.TotalRevenue` and the dashboard's `TodayRevenue`/`ThisMonthRevenue` only count
bookings with `PaymentStatus == Paid`. A `Pending`/`Failed`/unpaid booking isn't revenue yet even
if it's `Confirmed`.
