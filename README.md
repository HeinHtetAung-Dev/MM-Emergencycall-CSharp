# mm-emergency-call-csharp

```

dotnet ef dbcontext scaffold "Server=.;Database=MMEmergencyCall;User Id=sa;Password=sasa@123;TrustServerCertificate=True;" Microsoft.EntityFrameworkCore.SqlServer -o AppDbContextModels -c AppDbContext -f

```


Admin

[ ] Admin / User (CRUD) (By Role)
[x] Register
[x] Sign in
[x] States & Regions (CRUD) - need pagination
[ ] Township (CRUD) - Su Sandar Lin
[x] Emergency Services
[ ] Emergency Request (List with Pagination, Status Change (Closed, Rejected))

User

- [x] Register
- [x] Sign in
- [ ] Profile 
- 	[ ] Deactivate
- 	[ ] Delete Account
- [x] Emergency Requests 
- 	Create - Created User Id,
- 	List - Listing with Pagination (Status - Cancel, Open, Closed)
- 	Update - Only Status
- [x] Emergency Services
- 	Create with Pending (Search with Admin Approved)
- 	[Feature] Update with Pending (Temporary Table) 
- [ ] Search 
	Type, Township, Lat, Lon (with Pagination)
	


