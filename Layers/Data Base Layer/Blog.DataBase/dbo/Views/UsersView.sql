CREATE View [dbo].[UsersView] as

SELECT us.[IdUser]
      ,us.[UserName]
      ,us.[Email]
      ,us.[EmailConfirmed]
      ,us.[PasswordHash]
      ,us.[SecurityStamp]
      ,us.[PhoneNumber]
      ,us.[PhoneNumberConfirmed]
      ,us.[TwoFactorEnabled]
      ,us.[LockoutEndDateUtc]
      ,us.[LockoutEnabled]
      ,us.[AccessFailedCount]
      ,us.[CreationDate]
      ,us.[LastActivityDate]
      ,us.[Disabled]
	  ,ro.Name as 'RoleName'
  FROM [dbo].[Users] us inner join UserRoles usro on us.IdUser = usro.IdUser inner join Roles ro on ro.IdRole = usro.IdRole

