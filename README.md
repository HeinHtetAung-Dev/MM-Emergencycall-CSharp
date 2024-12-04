# mm-emergency-call-csharp

```

dotnet ef dbcontext scaffold "Server=.;Database=MMEmergencyCall;User Id=sa;Password=sasa@123;TrustServerCertificate=True;" Microsoft.EntityFrameworkCore.SqlServer -o AppDbContextModels -c AppDbContext -f

mmemergencycall@gmail.com
mmemergencycalldev20
xzrl ajmh aumn ixmw

```


Admin

- [x] Admin / User (CRUD) (By Role)
- [x] Register
- [x] Sign in
- 	[ ] Don't allow delete status
- [x] States & Regions (CRUD) - need pagination
- [x] Township (CRUD) - Su Sandar Lin
- [x] Emergency Services
	- [x] Create Service
	- [x] Get All Services
	- [x] Get Services By Status
	- [x] Change Status
	- [ ] Update Service 
	- [ ] Delete Service
	
- [x] Emergency Request (List with Pagination, Status Change (Closed, Rejected))

User

- [x] Register
- 	[ ] Sent OPT Mail
-	[ ] Confirm OTP -> Active account
- [x] Sign in
- 	[ ] Don't allow delete status
- [x] Profile 
- 	[x] Deactivate
- 	[ ] Delete Account
- [x] Emergency Requests 
- 	Create - Created User Id,
- 	List - Listing with Pagination (Status - Cancel, Open, Closed)
- 	[ ] Update - Only Status ( Need to change )
- [x] Emergency Services
- 	[x] Create with Pending 
-	[x] Search with Approved 
- 	[x] Update with Pending 
- [x] Search 
	Type, Township, Lat, Lon (with Pagination)
- [x] Dashboard
    [ ] Top Ten request per user ( user, email, Township, times, service type, service) (Su Sandar Lin)
    [ ] Top Ten request per Township (Township, times, service type, service)           (Hnin Wut Yi)
    [ ] Total Active Users, Total Active Service Provider, Total Active Service         (Myo Thet)
    [ ] Weekly Monthly Daily Per Request                                                (Hein Htet)
    [ ] Top Ten service per Township                                                    (Hein Htet)
	


