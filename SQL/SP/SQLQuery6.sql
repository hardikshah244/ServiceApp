USE [ServiceAppDB]
GO
/****** Object:  StoredProcedure [dbo].[GETENGINEERINFO]    Script Date: 29-04-2016 12:28:04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Hardik Shah>
-- Create date: <29/04/2016>
-- Description:	<GETENGINEERINFO>
-- =============================================
CREATE PROCEDURE [dbo].[GETENGINEERINFO]
AS
BEGIN

		-- SET NOCOUNT ON added to prevent extra result sets from
		-- interfering with SELECT statements.
		SET NOCOUNT ON;

		BEGIN TRY		

			SELECT USERS.Email,USERS.Name,USERS.PhoneNumber,USERS.City,USERS.State,USERS.Pincode,
					USERS.Latitude,USERS.Longitude,USERS.DeviceID,USERS.IsActive
				FROM AspNetUsers USERS	
				INNER JOIN AspNetUserRoles USERROLES ON USERS.Id = USERROLES.UserId
				INNER JOIN AspNetRoles ROLES ON USERROLES.RoleId = ROLES.Id AND ROLES.Name = 'Engineer';

		END TRY			
		
		BEGIN CATCH			

			--DECLARE @ErrorNumber INT = ERROR_NUMBER();
			--DECLARE @ErrorLine INT = ERROR_LINE();
			DECLARE @ErrorMessage NVARCHAR(4000) = ERROR_MESSAGE();
			DECLARE @ErrorSeverity INT = ERROR_SEVERITY();
			DECLARE @ErrorState INT = ERROR_STATE();

			RAISERROR(@ErrorMessage, @ErrorSeverity, @ErrorState);

		END CATCH
	
END
