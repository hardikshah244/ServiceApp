-- Engineer Get Request
WITH A AS (
		SELECT ServiceRequestID,MAX(ServiceRequestID_hist) ServiceRequestID_hist,
		  ROW_NUMBER() OVER(PARTITION BY ServiceRequestID ORDER BY ServiceRequestID_hist DESC) AS RowNo 
		  FROM ServiceRequest_Hist 
		  WHERE UpdatedUserID = '23b1ab6e-197a-4aaf-8581-98bc53a37d2f'
		  GROUP BY ServiceRequestID,ServiceRequestID_hist
  ),
  B AS(
	SELECT HIST.ServiceRequestID,HIST.CreatedDateTime,HIST.Landmark,
			HIST.Remark,STM.ServiceTypeName,STTM.StatusTypeName,USERS.Name,
			HIST.UpdatedDateTime,STTM.StatusTypeID
			FROM A
			INNER JOIN ServiceRequest_Hist HIST ON A.ServiceRequestID_hist = HIST.ServiceRequestID_hist
			INNER JOIN ServiceTypeMaster STM On HIST.ServiceTypeID = STM.ServiceTypeID
			INNER JOIN StatusTypeMaster STTM ON HIST.StatusTypeID = STTM.StatusTypeID
			LEFT JOIN AspNetUsers USERS ON HIST.CreatedUserID = USERS.Id
		Where  HIST.UpdatedUserID = '23b1ab6e-197a-4aaf-8581-98bc53a37d2f'
		AND A.RowNo=1
  )
  SELECT * FROM B;

-- Customer Get Request
SELECT HIST.ServiceRequestID,HIST.CreatedDateTime,HIST.Landmark,
		HIST.Remark,STM.ServiceTypeName,STTM.StatusTypeName,USERS.Name,
		HIST.UpdatedDateTime,STTM.StatusTypeID
		FROM ServiceRequest HIST
		INNER JOIN ServiceTypeMaster STM On HIST.ServiceTypeID = STM.ServiceTypeID
		INNER JOIN StatusTypeMaster STTM ON HIST.StatusTypeID = STTM.StatusTypeID
		LEFT JOIN AspNetUsers USERS ON HIST.UpdatedUserID = USERS.Id
	Where  HIST.CreatedUserID = '05d22bb6-28bd-4073-b261-42c5dfc7fede';