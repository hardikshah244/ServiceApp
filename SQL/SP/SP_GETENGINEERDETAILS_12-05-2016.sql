USE [ServiceAppDB]
GO
/****** Object:  StoredProcedure [dbo].[GETENGINEERDETAILS]    Script Date: 12-05-2016 21:58:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Hardik Shah>
-- Create date: <31/03/2016>
-- Description:	<GETENGINEERDETAILS>
-- =============================================
ALTER PROCEDURE [dbo].[GETENGINEERDETAILS]
(
	@PServiceTypeID INT,
	@PStatusTypeID INT,
	@PLandmark NVARCHAR(MAX),
	@PRemark NVARCHAR(MAX),
	@PCreatedUserID NVARCHAR(MAX),
	@PPincode INT
)
AS
BEGIN

DECLARE @UserID AS NVARCHAR(200)=''
DECLARE @Email AS NVARCHAR(200)=''
DECLARE @Name AS NVARCHAR(200)=''
DECLARE @ServiceRequestID AS INT=0

		-- SET NOCOUNT ON added to prevent extra result sets from
		-- interfering with SELECT statements.
		SET NOCOUNT ON;

		BEGIN TRY		

			BEGIN TRANSACTION
			
				INSERT INTO ServiceRequest
							   (
								ServiceTypeID,
								StatusTypeID,
								Landmark,
								Remark,
								CreatedDateTime,
								CreatedUserID
							   )
						 VALUES
							   (
								@PServiceTypeID,
								@PStatusTypeID,
								@PLandmark,
								@PRemark,
								GETDATE(),
								@PCreatedUserID
							   );

				SET @ServiceRequestID = SCOPE_IDENTITY();
								

			IF (@ServiceRequestID > 0)
			BEGIN
				-- New Engineer Not Any Request Assign
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
														WHERE StatusTypeID = 2
														)
								AND USERS.Pincode = @PPincode)
				BEGIN

						SELECT TOP 1 @Name=USERS.Name,@Email=USERS.Email,@UserID=USERS.id
						FROM AspNetUsers USERS	
						INNER JOIN AspNetUserRoles USERROLES ON  USERROLES.UserId = USERS.Id
						INNER JOIN AspNetRoles ROLES ON USERROLES.RoleId = ROLES.Id AND ROLES.Name = 'Engineer'
						INNER JOIN EngineerMembership MEMBERSHIP ON MEMBERSHIP.UserId = USERS.Id	
						WHERE USERS.IsActive = 1
						AND ((MEMBERSHIP.StartDate IS NULL AND MEMBERSHIP.EndDate IS NULL)
								OR MEMBERSHIP.EndDate >= GETDATE())
						AND USERS.Id NOT IN (SELECT UpdatedUserID
												FROM ServiceRequest
												WHERE StatusTypeID = 2
												)
						AND USERS.Pincode = @PPincode;

						-- Update service status as 'Raised(1)' to 'Assigned(2)'
						UPDATE ServiceRequest SET UpdatedDateTime = GETDATE(),UpdatedUserID=@UserID,StatusTypeID=2
							WHERE ServiceRequestID = @ServiceRequestID;

						-- Return values
						SELECT @Name AS Name,@Email AS Email,@ServiceRequestID AS ServiceRequestID;
		
					END
					-- Is Any Engineer Free From ServiceRequest
					ELSE IF EXISTS(SELECT USERS.Name,USERS.Email
									FROM AspNetUsers USERS	
									INNER JOIN EngineerMembership MEMBERSHIP ON MEMBERSHIP.UserId = USERS.Id
									INNER JOIN ServiceRequest SR ON SR.UpdatedUserID = USERS.Id	
									WHERE USERS.IsActive = 1
									AND ((MEMBERSHIP.StartDate IS NULL AND MEMBERSHIP.EndDate IS NULL)
											OR MEMBERSHIP.EndDate >= GETDATE())					
									AND StatusTypeID NOT IN (2)
									AND USERS.Pincode = @PPincode)
					BEGIN
						SELECT TOP 1 @Name=USERS.Name,@Email=USERS.Email,@UserID=USERS.id
								FROM AspNetUsers USERS	
								INNER JOIN EngineerMembership MEMBERSHIP ON MEMBERSHIP.UserId = USERS.Id
								INNER JOIN ServiceRequest SR ON SR.UpdatedUserID = USERS.Id	
								WHERE USERS.IsActive = 1
								AND ((MEMBERSHIP.StartDate IS NULL AND MEMBERSHIP.EndDate IS NULL)
										OR MEMBERSHIP.EndDate >= GETDATE())
								AND StatusTypeID NOT IN (2)
								AND USERS.Pincode = @PPincode
								ORDER BY SR.UpdatedDateTime;

						-- Update service status as 'Raised(1)' to 'Assigned(2)'
						UPDATE ServiceRequest SET UpdatedDateTime = GETDATE(),UpdatedUserID=@UserID,StatusTypeID=2
							WHERE ServiceRequestID = @ServiceRequestID;

						-- Return values
						SELECT @Name AS Name,@Email AS Email,@ServiceRequestID AS ServiceRequestID;

					END
					-- Pincode Range(+/- 5) Based Searching 
					ELSE IF EXISTS(SELECT USERS.Name,USERS.Email
									FROM AspNetUsers USERS	
									INNER JOIN AspNetUserRoles USERROLES ON  USERROLES.UserId = USERS.Id
									INNER JOIN AspNetRoles ROLES ON USERROLES.RoleId = ROLES.Id AND ROLES.Name = 'Engineer'
									INNER JOIN EngineerMembership MEMBERSHIP ON MEMBERSHIP.UserId = USERS.Id	
									WHERE USERS.IsActive = 1
									AND ((MEMBERSHIP.StartDate IS NULL AND MEMBERSHIP.EndDate IS NULL)
											OR MEMBERSHIP.EndDate >= GETDATE())
									AND USERS.Id NOT IN (SELECT UpdatedUserID
															FROM ServiceRequest
															WHERE StatusTypeID = 2
															)
									AND USERS.Pincode BETWEEN @PPincode - 5 AND @PPincode + 5
								)
					BEGIN

						SELECT TOP 1 @Name=USERS.Name,@Email=USERS.Email,@UserID=USERS.id
								FROM AspNetUsers USERS	
								INNER JOIN AspNetUserRoles USERROLES ON  USERROLES.UserId = USERS.Id
								INNER JOIN AspNetRoles ROLES ON USERROLES.RoleId = ROLES.Id AND ROLES.Name = 'Engineer'
								INNER JOIN EngineerMembership MEMBERSHIP ON MEMBERSHIP.UserId = USERS.Id	
								WHERE USERS.IsActive = 1
								AND ((MEMBERSHIP.StartDate IS NULL AND MEMBERSHIP.EndDate IS NULL)
										OR MEMBERSHIP.EndDate >= GETDATE())
								AND USERS.Id NOT IN (SELECT UpdatedUserID
														FROM ServiceRequest
														WHERE StatusTypeID = 2
														)
								AND USERS.Pincode BETWEEN @PPincode - 5 AND @PPincode + 5;
		

						-- Update service status as 'Raised(1)' to 'Assigned(2)'
						UPDATE ServiceRequest SET UpdatedDateTime = GETDATE(),UpdatedUserID=@UserID,StatusTypeID=2
							WHERE ServiceRequestID = @ServiceRequestID;

						-- Return values
						SELECT @Name AS Name,@Email AS Email,@ServiceRequestID AS ServiceRequestID;

					END
					ELSE
					BEGIN
						-- Return values
						SELECT @Name AS Name,@Email AS Email,@ServiceRequestID AS ServiceRequestID;
					END

				COMMIT TRANSACTION; 

			END			

		END TRY			
		
		BEGIN CATCH

			IF @@TRANCOUNT > 0
				ROLLBACK TRANSACTION --RollBack in case of Error

			--DECLARE @ErrorNumber INT = ERROR_NUMBER();
			--DECLARE @ErrorLine INT = ERROR_LINE();
			DECLARE @ErrorMessage NVARCHAR(4000) = ERROR_MESSAGE();
			DECLARE @ErrorSeverity INT = ERROR_SEVERITY();
			DECLARE @ErrorState INT = ERROR_STATE();

			RAISERROR(@ErrorMessage, @ErrorSeverity, @ErrorState);

		END CATCH
	
END
