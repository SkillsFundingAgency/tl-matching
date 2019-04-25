--BEGIN TRANSACTION

UPDATE [dbo].[Provider]
SET 
[PrimaryContact]    = 'Juliet Holloway',
[PrimaryContactEmail]    = 'industry.placements@windsor-forest.ac.uk' ,
[SecondaryContact]        = 'Kate Webb' ,
[SecondaryContactEmail] = 'kate.webb@windsor-forest.ac.uk' ,
[SecondaryContactPhone] = '01753 793000'
WHERE UkPrn = 10002107


UPDATE [dbo].[Provider]
SET 
[PrimaryContact]    = 'Nikki Smith',
[PrimaryContactEmail]    = 'placements@liv-coll.ac.uk',
[PrimaryContactPhone]    = '01512 523163',
[SecondaryContact]        = 'Tracey Brown',
[SecondaryContactEmail] = 'tracey.brown@liv-coll.ac.uk',
[SecondaryContactPhone] = '01512 524352'
WHERE UkPrn = 10003955


UPDATE [dbo].[Provider]
SET 
[PrimaryContact]    = 'Kirsty Ranford',
[PrimaryContactEmail]    = 'kirsty.ranford@tpc.ac.uk',
[PrimaryContactPhone]    = '02392 344411',
[SecondaryContact]        = 'Frances Mullen',
[SecondaryContactEmail] = 'frances.mullen@tpc.ac.uk',
[SecondaryContactPhone] = '02392 344391'
WHERE UkPrn = 10005158
--ROLLBACK TRANSACTION