--TLWP-1352 - Delete pipeline opportunities for specific users


declare @updatedByUser nvarchar(50) = 'Update Script TLWP-1352'

declare @opportunityItemsToRemove table (
  opportunityItemId int not null
)

declare @users table (
  username nvarchar(50) not null
)

insert into @users 
values	('Simon Peek'), 
		('Mark Coulson'), 
		('Nichola Akers'),
		--For testing on local environment
		--,('Dev Surname')
		--For testing on the test environment
		('2 Tmatching')

SELECT 
			o.Id AS OpportunityId,
			e.CompanyName,
			e.AlsoKnownAs AS CompanyNameAka,
			oi.Id AS OpportunityItemId,
			oi.OpportunityType,
			CAST(CASE 
					WHEN oi.IsSaved = 1 and oi.IsCompleted = 0 
					THEN 1 
					ELSE 0 
				END
			  AS BIT) AS PipelineOpportunity,
			oi.JobRole,
			oi.Placements,
			oi.PlacementsKnown,
			oi.Town + ' ' + oi.Postcode AS Workplace,
			oi.OpportunityType,
			pg.HadBadExperience,
			pg.NoSuitableStudent,
			pg.ProvidersTooFarAway,
			p.DisplayName,
			pv.[Name] AS VenueName,
			pv.Postcode,
			o.createdby,
			o.ModifiedOn,
			o.ModifiedBy
		FROM Opportunity AS o 
			INNER JOIN OpportunityItem AS OI ON o.Id = oi.OpportunityId
			LEFT JOIN Employer AS E ON o.EmployerCrmId = e.CrmId
			LEFT JOIN ProvisionGap AS PG on oi.Id = PG.OpportunityItemId
			LEFT JOIN Referral AS R on oi.Id = r.OpportunityItemId
			LEFT JOIN ProviderVenue AS PV on r.ProviderVenueId = pv.Id
			LEFT JOIN Provider AS P on pv.ProviderId = p.Id
			INNER JOIN @users u on o.CreatedBy = u.username
		WHERE
			oi.IsSaved = 1 
			AND oi.IsCompleted = 0
			AND oi.IsDeleted = 0
			order by opportunityitemid

--select * from OpportunityItem
--where ModifiedOn >= convert(date, getutcdate())
--and ModifiedBy = @updatedByUser

insert into @opportunityItemsToRemove
select distinct oi.Id as OpportunityItemId
from Opportunity AS o 
	INNER JOIN OpportunityItem AS OI ON o.Id = oi.OpportunityId
	INNER JOIN @users u on o.CreatedBy = u.username
WHERE
	oi.IsSaved = 1 
	AND oi.IsCompleted = 0
	AND oi.IsDeleted = 0

select count(*) from @opportunityItemsToRemove
select * from @opportunityItemsToRemove

begin transaction

update OpportunityItem
set IsDeleted = 1,
ModifiedOn = getutcdate(),
ModifiedBy = @updatedByUser
where Id in (select OpportunityItemId from @opportunityItemsToRemove)

select * from OpportunityItem
where ModifiedOn >= convert(date, getutcdate())
and ModifiedBy = @updatedByUser

SELECT 
oi.IsSaved,
oi.IsCompleted,
oi.IsDeleted,
			o.Id AS OpportunityId,
			e.CompanyName,
			e.AlsoKnownAs AS CompanyNameAka,
			oi.Id AS OpportunityItemId,
			oi.OpportunityType,
			CAST(CASE 
					WHEN oi.IsSaved = 1 and oi.IsCompleted = 0 
					THEN 1 
					ELSE 0 
				END
			  AS BIT) AS PipelineOpportunity,
			oi.JobRole,
			oi.Placements,
			oi.PlacementsKnown,
			oi.Town + ' ' + oi.Postcode AS Workplace,
			oi.OpportunityType,
			pg.HadBadExperience,
			pg.NoSuitableStudent,
			pg.ProvidersTooFarAway,
			p.DisplayName,
			pv.[Name] AS VenueName,
			pv.Postcode,
			o.createdby,
			oi.ModifiedBy
		FROM @opportunityItemsToRemove removed
			INNER JOIN OpportunityItem AS OI ON removed.opportunityItemId = oi.OpportunityId
			INNER JOIN Opportunity AS o ON o.Id = oi.OpportunityId
			LEFT JOIN Employer AS E ON o.EmployerCrmId = e.CrmId
			LEFT JOIN ProvisionGap AS PG on oi.Id = PG.OpportunityItemId
			LEFT JOIN Referral AS R on oi.Id = r.OpportunityItemId
			LEFT JOIN ProviderVenue AS PV on r.ProviderVenueId = pv.Id
			LEFT JOIN Provider AS P on pv.ProviderId = p.Id
			INNER JOIN @users u on o.CreatedBy = u.username
		WHERE
			oi.IsSaved = 1 
	AND oi.IsCompleted = 0
	AND oi.IsDeleted = 0
			--oi.IsSaved <> 1 
			--or oi.IsCompleted <> 0
			--or oi.IsDeleted <> 0
-- opportunityId = 975
			order by opportunityitemid

			rollback