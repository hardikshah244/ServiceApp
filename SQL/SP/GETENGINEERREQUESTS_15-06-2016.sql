USE [ServiceAppDB]
GO
/****** Object:  StoredProcedure [dbo].[GETENGINEERREQUESTS]    Script Date: 15-06-2016 22:32:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Hardik Shah>
-- Create date: <12/05/2016>
-- Description:	<GETENGINEERREQUESTS>
-- =============================================
ALTER PROCEDURE [dbo].[GETENGINEERREQUESTS]
(
	@PUpdatedUserID NVARCHAR(128)
)
AS
BEGIN

		-- SET NOCOUNT ON added to prevent extra result sets from
		-- interfering with SELECT statements.
		SET NOCOUNT ON;

		BEGIN TRY		

			-- Engineer Get Request
					WITH A AS 
					(
						SELECT ServiceRequestNO,MAX(ServiceRequestID_hist) ServiceRequestID_hist,
							ROW_NUMBER() OVER(PARTITION BY ServiceRequestNO ORDER BY ServiceRequestID_hist DESC) AS RowNo 
							FROM ServiceRequest_Hist 
							WHERE UpdatedUserID = @PUpdatedUserID
							GROUP BY ServiceRequestNO,ServiceRequestID_hist
					),
					C AS
					(
						SELECT ServiceRequestNO,MAX(UpdatedDateTime) AS CompletedDateTime			
								FROM ServiceRequest_Hist 
								WHERE UpdatedUserID = @PUpdatedUserID
								AND StatusTypeID = 4
								GROUP BY ServiceRequestNO
					),
					B AS(
					SELECT HIST.ServiceRequestNO AS "SRNO",
							SCM.ServiceCategoryName AS "SRCategory",
							STM.ServiceTypeName AS "SRTYPE",
							USERS.Name AS "Customer Name",
							HIST.CreatedDateTime AS "Created DateTime",
							HIST.UpdatedDateTime AS "Assigned DateTime",
							C.CompletedDateTime AS "Completed DateTime",
							'' AS "Customer Feedback",
							STTM.StatusTypeName AS "Status",
							STTM.StatusTypeID
							FROM A
							INNER JOIN ServiceRequest_Hist HIST ON A.ServiceRequestID_hist = HIST.ServiceRequestID_hist
							INNER JOIN ServiceTypeMaster STM On HIST.ServiceTypeID = STM.ServiceTypeID
							INNER JOIN ServiceCategoryMaster SCM On HIST.ServiceCategoryID = SCM.ServiceCategoryID
							INNER JOIN StatusTypeMaster STTM ON HIST.StatusTypeID = STTM.StatusTypeID
							LEFT JOIN AspNetUsers USERS ON HIST.CreatedUserID = USERS.Id
							LEFT JOIN C ON C.ServiceRequestNO = HIST.ServiceRequestNO
						Where  HIST.UpdatedUserID = @PUpdatedUserID
						AND A.RowNo=1
					)
						SELECT * FROM B;

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
