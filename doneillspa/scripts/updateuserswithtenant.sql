
  begin transaction
  insert into dbo.AspNetUserClaims (ClaimType, ClaimValue, UserId)
  select 'Tenant', 1, Id from AspNetUsers

  commit