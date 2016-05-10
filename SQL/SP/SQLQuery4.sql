SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Hardik Shah>
-- Create date: <31/03/2016>
-- Description:	<GETENGINEERDETAILS>
-- =============================================
ALTER PROCEDURE GETENGINEERDETAILS
(
	@PPINCODE INT
)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	IF EXISTS (SELECT USERS.Name,USERS.Email
				FROM AspNetUsers USERS	
				INNER JOIN AspNetUserRoles USERROLES ON  USERROLES.UserId = USERS.Id
				INNER JOIN AspNetRoles ROLES ON USERROLES.RoleId = ROLES.Id AND ROLES.Name = 'Engineer'
				INNER JOIN EngineerMembership MEMBERSHIP ON MEMBERSHIP.UserId = USERS.Id	
				WHERE USERS.IsActive = 1
				AND ((MEMBERSHIP.StartDate IS NULL AND MEMBERSHIP.EndDate IS NULL)
						OR MEMBERSHIP.EndDate >= GETDATE())
				AND USERS.Id NOT IN (SELECT UpdatedUserID
										FROM ServiceRequest
										WHERE UpdatedUserID IS NOT NULL
										AND StatusTypeID IN (1,2)
										)
				AND USERS.Pincode = @PPINCODE)
    BEGIN

        SELECT TOP 1 USERS.Name,USERS.Email
		FROM AspNetUsers USERS	
		INNER JOIN AspNetUserRoles USERROLES ON  USERROLES.UserId = USERS.Id
		INNER JOIN AspNetRoles ROLES ON USERROLES.RoleId = ROLES.Id AND ROLES.Name = 'Engineer'
		INNER JOIN EngineerMembership MEMBERSHIP ON MEMBERSHIP.UserId = USERS.Id	
		WHERE USERS.IsActive = 1
		AND ((MEMBERSHIP.StartDate IS NULL AND MEMBERSHIP.EndDate IS NULL)
				OR MEMBERSHIP.EndDate >= GETDATE())
		AND USERS.Id NOT IN (SELECT UpdatedUserID
								FROM ServiceRequest
								WHERE UpdatedUserID IS NOT NULL
								AND StatusTypeID IN (1,2)
								)
		AND USERS.Pincode = @PPINCODE;
		
    END
	ELSE IF EXISTS(SELECT USERS.Name,USERS.Email
					FROM AspNetUsers USERS	
					INNER JOIN EngineerMembership MEMBERSHIP ON MEMBERSHIP.UserId = USERS.Id
					INNER JOIN ServiceRequest SR ON SR.UpdatedUserID = USERS.Id	
					WHERE USERS.IsActive = 1
					AND ((MEMBERSHIP.StartDate IS NULL AND MEMBERSHIP.EndDate IS NULL)
							OR MEMBERSHIP.EndDate >= GETDATE())					
					AND StatusTypeID NOT IN (1,2)
					AND USERS.Pincode = @PPINCODE)
	BEGIN
			SELECT TOP 1 USERS.Name,USERS.Email
				FROM AspNetUsers USERS	
				INNER JOIN EngineerMembership MEMBERSHIP ON MEMBERSHIP.UserId = USERS.Id
				INNER JOIN ServiceRequest SR ON SR.UpdatedUserID = USERS.Id	
				WHERE USERS.IsActive = 1
				AND ((MEMBERSHIP.StartDate IS NULL AND MEMBERSHIP.EndDate IS NULL)
						OR MEMBERSHIP.EndDate >= GETDATE())
				AND StatusTypeID NOT IN (1,2)
				AND USERS.Pincode = @PPINCODE
				ORDER BY SR.UpdatedDateTime;
	END
	ELSE
	BEGIN
		SELECT TOP 1 USERS.Name,USERS.Email
		FROM AspNetUsers USERS	
		INNER JOIN AspNetUserRoles USERROLES ON  USERROLES.UserId = USERS.Id
		INNER JOIN AspNetRoles ROLES ON USERROLES.RoleId = ROLES.Id AND ROLES.Name = 'Engineer'
		INNER JOIN EngineerMembership MEMBERSHIP ON MEMBERSHIP.UserId = USERS.Id	
		WHERE USERS.IsActive = 1
		AND ((MEMBERSHIP.StartDate IS NULL AND MEMBERSHIP.EndDate IS NULL)
				OR MEMBERSHIP.EndDate >= GETDATE())
		AND USERS.Id NOT IN (SELECT UpdatedUserID
								FROM ServiceRequest
								WHERE UpdatedUserID IS NOT NULL
								AND StatusTypeID IN (1,2)
								)
		AND USERS.Pincode BETWEEN @PPINCODE - 5 AND @PPINCODE + 5;
		--SELECT Name='',Email='';
	END
	
END
GO
