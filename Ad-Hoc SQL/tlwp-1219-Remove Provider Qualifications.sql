begin transaction

declare @providerQualificationsToDelete table 
(
id int not null
)

insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10001850 and pv.postcode = 'DL1 1DR' and q.larid = '50067515'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10001850 and pv.postcode = 'DL1 1DR' and q.larid = '50094397'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10001850 and pv.postcode = 'DL1 1DR' and q.larid = '60039309'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10001850 and pv.postcode = 'DL1 1DR' and q.larid = '60082641'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10001850 and pv.postcode = 'DL1 1DR' and q.larid = '60184358'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10001850 and pv.postcode = 'DL1 1DR' and q.larid = '6016041X'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10001850 and pv.postcode = 'DL1 1DR' and q.larid = '6032496X'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10001934 and pv.postcode = 'DH8 5EE' and q.larid = '50078410'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10001934 and pv.postcode = 'DH8 5EE' and q.larid = '50081561'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10001934 and pv.postcode = 'DH8 5EE' and q.larid = '50090513'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10001934 and pv.postcode = 'DH8 5EE' and q.larid = '50090768'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10001934 and pv.postcode = 'DH8 5EE' and q.larid = '50091487'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10001934 and pv.postcode = 'DH8 5EE' and q.larid = '50094397'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10001934 and pv.postcode = 'DH8 5EE' and q.larid = '50099796'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10001934 and pv.postcode = 'DH8 5EE' and q.larid = '60027472'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10001934 and pv.postcode = 'DH8 5EE' and q.larid = '60170712'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10001934 and pv.postcode = 'DH8 5EE' and q.larid = '6006724X'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10001934 and pv.postcode = 'DH8 5EE' and q.larid = '6013964X'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10002061 and pv.postcode = 'LU5 4HG' and q.larid = '60046259'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10002061 and pv.postcode = 'LU5 4HG' and q.larid = '60046399'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10002065 and pv.postcode = 'DH1 1SG' and q.larid = '60144567'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10002065 and pv.postcode = 'DH1 1SG' and q.larid = '60173415'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10002073 and pv.postcode = 'E3 3QP' and q.larid = '60060992'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10002085 and pv.postcode = 'B79 7HL' and q.larid = '50097453'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10002085 and pv.postcode = 'B79 7HL' and q.larid = '60003364'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10002085 and pv.postcode = 'ST1 5HQ' and q.larid = '50097453'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10002085 and pv.postcode = 'ST1 5HQ' and q.larid = '60139559'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10002085 and pv.postcode = 'ST17 4AA' and q.larid = '60139559'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10002111 and pv.postcode = 'SR8 2HU' and q.larid = '50100191'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10002111 and pv.postcode = 'SR8 2RN' and q.larid = '50067655'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10002126 and pv.postcode = 'HU17 0GH' and q.larid = '60125366'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10002126 and pv.postcode = 'HU17 0GH' and q.larid = '60188546'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10002126 and pv.postcode = 'YO16 7JW' and q.larid = '60188546'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10002370 and pv.postcode = 'EX4 3SR' and q.larid = '60160421'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10002599 and pv.postcode = 'LA14 2PJ' and q.larid = '60079952'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10002599 and pv.postcode = 'LA14 2PJ' and q.larid = '60080838'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10002599 and pv.postcode = 'LA14 2PJ' and q.larid = '60157999'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10002599 and pv.postcode = 'LA14 2PJ' and q.larid = '60171984'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10002696 and pv.postcode = 'GL14 2YT' and q.larid = '50067643'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10002696 and pv.postcode = 'GL2 5JQ' and q.larid = '50067643'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10002696 and pv.postcode = 'GL2 5JQ' and q.larid = '50095201'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10002696 and pv.postcode = 'GL51 7SJ' and q.larid = '50095201'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10002743 and pv.postcode = 'NG31 9AP' and q.larid = '50067515'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10002743 and pv.postcode = 'NG31 9AP' and q.larid = '50094397'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10002743 and pv.postcode = 'NG31 9AP' and q.larid = '60039310'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10002743 and pv.postcode = 'NG31 9AP' and q.larid = '60173245'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10002743 and pv.postcode = 'NG31 9AP' and q.larid = '60174377'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10002899 and pv.postcode = 'CM20 3EZ' and q.larid = '50067205'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10002899 and pv.postcode = 'CM20 3EZ' and q.larid = '60107170'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10002899 and pv.postcode = 'CM20 3EZ' and q.larid = '60175850'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10002899 and pv.postcode = 'CM20 3EZ' and q.larid = '60303517'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10002917 and pv.postcode = 'TS24 7NT' and q.larid = '60133958'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10002976 and pv.postcode = 'CV1 3BA' and q.larid = '6015696X'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10002976 and pv.postcode = 'CV21 2PS' and q.larid = '6015696X'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10002976 and pv.postcode = 'CV21 2TP' and q.larid = '6015696X'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10002976 and pv.postcode = 'CV22 6NU' and q.larid = '50087654'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10002976 and pv.postcode = 'CV6 2BT' and q.larid = '6015696X'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10003011 and pv.postcode = 'HP12 3NE' and q.larid = '60171996'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10003011 and pv.postcode = 'RG1 5AN' and q.larid = '60171996'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10003011 and pv.postcode = 'RG4 9RH' and q.larid = '60171996'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10003011 and pv.postcode = 'RG9 1EY' and q.larid = '60171996'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10003011 and pv.postcode = 'RG9 1UH' and q.larid = '60171996'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10003022 and pv.postcode = 'HR1 1LT' and q.larid = '6003807X'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10003023 and pv.postcode = 'HR1 1LS' and q.larid = '50098184'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10003023 and pv.postcode = 'SY11 4QB' and q.larid = '50067643'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10003023 and pv.postcode = 'SY11 4QB' and q.larid = '50088816'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10003023 and pv.postcode = 'SY11 4QB' and q.larid = '50094397'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10003023 and pv.postcode = 'SY11 4QB' and q.larid = '60039310'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10003023 and pv.postcode = 'SY11 4QB' and q.larid = '60102329'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10003023 and pv.postcode = 'SY11 4QB' and q.larid = '60107170'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10003023 and pv.postcode = 'SY11 4QB' and q.larid = '60156971'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10003023 and pv.postcode = 'SY8 1GD' and q.larid = '60039310'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10003023 and pv.postcode = 'SY8 1GD' and q.larid = '60304595'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10003035 and pv.postcode = 'EN10 6AE' and q.larid = '50088087'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10003035 and pv.postcode = 'EN10 6AE' and q.larid = '50090999'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10003035 and pv.postcode = 'EN10 6AE' and q.larid = '60184371'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10003035 and pv.postcode = 'EN10 6AE' and q.larid = '6018436X'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10003146 and pv.postcode = 'M24 6XH' and q.larid = '50067643'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10003146 and pv.postcode = 'M24 6XH' and q.larid = '50094397'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10003146 and pv.postcode = 'M24 6XH' and q.larid = '50100191'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10003146 and pv.postcode = 'M24 6XH' and q.larid = '60067445'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10003146 and pv.postcode = 'OL12 6RY' and q.larid = '60105604'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10003188 and pv.postcode = 'HX3 0BT' and q.larid = '50078422'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10003188 and pv.postcode = 'HX3 0BT' and q.larid = '50078781'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10003188 and pv.postcode = 'HX3 0BT' and q.larid = '50078884'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10003188 and pv.postcode = 'HX3 0BT' and q.larid = '50091475'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10003188 and pv.postcode = 'HX3 0BT' and q.larid = '50091487'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10003188 and pv.postcode = 'HX3 0BT' and q.larid = '50091499'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10003188 and pv.postcode = 'HX3 0BT' and q.larid = '50091505'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10003188 and pv.postcode = 'HX3 0BT' and q.larid = '60038871'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10003188 and pv.postcode = 'HX3 0BT' and q.larid = '60060724'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10003188 and pv.postcode = 'HX3 0BT' and q.larid = '60064456'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10003219 and pv.postcode = 'HU3 4DD' and q.larid = '50057297'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10003219 and pv.postcode = 'HU3 4DD' and q.larid = '60125366'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10003219 and pv.postcode = 'HU3 4DD' and q.larid = '60188546'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10003406 and pv.postcode = 'PO30 5TA' and q.larid = '50082796'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10003406 and pv.postcode = 'PO30 5TA' and q.larid = '50082802'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10003406 and pv.postcode = 'PO30 5TA' and q.larid = '50083211'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10003406 and pv.postcode = 'PO30 5TA' and q.larid = '50083557'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10003406 and pv.postcode = 'PO30 5TA' and q.larid = '50088282'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10003406 and pv.postcode = 'PO30 5TA' and q.larid = '50089766'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10003406 and pv.postcode = 'PO30 5TA' and q.larid = '60038895'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10003406 and pv.postcode = 'PO30 5TA' and q.larid = '60045371'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10003406 and pv.postcode = 'PO30 5TA' and q.larid = '60046053'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10003406 and pv.postcode = 'PO30 5TA' and q.larid = '60125366'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10003406 and pv.postcode = 'PO30 5TA' and q.larid = '6018436X'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10003558 and pv.postcode = 'LA9 5AY' and q.larid = '60334927'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10003867 and pv.postcode = 'LE1 3WA' and q.larid = '50098147'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10003867 and pv.postcode = 'LE1 3WA' and q.larid = '50098184'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10003867 and pv.postcode = 'LE1 3WA' and q.larid = '60025797'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10003867 and pv.postcode = 'LE1 3WA' and q.larid = '60174377'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10003867 and pv.postcode = 'LE1 3WA' and q.larid = '60186161'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10003867 and pv.postcode = 'LE2 7LW' and q.larid = '60303505'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10003899 and pv.postcode = 'E10 6EQ' and q.larid = '60028270'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10003928 and pv.postcode = 'LN2 5HQ' and q.larid = '50090768'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10003928 and pv.postcode = 'LN2 5HQ' and q.larid = '50091499'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10003928 and pv.postcode = 'LN2 5HQ' and q.larid = '60011774'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10003928 and pv.postcode = 'LN2 5HQ' and q.larid = '60094321'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10003928 and pv.postcode = 'LN2 5HQ' and q.larid = '60106025'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10003928 and pv.postcode = 'LN2 5HQ' and q.larid = '60171996'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10003928 and pv.postcode = 'LN2 5HQ' and q.larid = '60303773'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10003928 and pv.postcode = 'LN2 5HQ' and q.larid = '6009915x'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10003928 and pv.postcode = 'NG24 1PB' and q.larid = '50091499'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10003928 and pv.postcode = 'NG24 1PB' and q.larid = '60011774'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10003928 and pv.postcode = 'NG24 1PB' and q.larid = '60054980'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10003928 and pv.postcode = 'NG24 1PB' and q.larid = '60171996'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10004013 and pv.postcode = 'W1F 7LN' and q.larid = '50090768'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10004013 and pv.postcode = 'W1F 7LN' and q.larid = '50093186'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10004013 and pv.postcode = 'W1F 7LN' and q.larid = '50093198'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10004013 and pv.postcode = 'W1F 7LN' and q.larid = '50118201'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10004013 and pv.postcode = 'W1F 7LN' and q.larid = '60051516'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10004013 and pv.postcode = 'W1F 7LN' and q.larid = '5009838X'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10004013 and pv.postcode = 'W1F 7LN' and q.larid = '5010987X'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10004116 and pv.postcode = 'NR31 0ED' and q.larid = '50067461'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10004116 and pv.postcode = 'NR31 0ED' and q.larid = '50088609'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10004116 and pv.postcode = 'NR31 0ED' and q.larid = '50099796'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10004116 and pv.postcode = 'NR31 0ED' and q.larid = '60008805'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10004116 and pv.postcode = 'NR32 2NB' and q.larid = '50088609'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10004116 and pv.postcode = 'NR32 2NB' and q.larid = '50099796'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10004116 and pv.postcode = 'NR32 2NB' and q.larid = '60008805'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10004116 and pv.postcode = 'NR32 2NB' and q.larid = '60038883'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10004144 and pv.postcode = 'SK11 8LF' and q.larid = '50078008'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10004144 and pv.postcode = 'SK11 8LF' and q.larid = '60038883'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10004344 and pv.postcode = 'TS2 1AD' and q.larid = '50071397'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10004344 and pv.postcode = 'TS2 1AD' and q.larid = '50072961'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10004344 and pv.postcode = 'TS2 1AD' and q.larid = '50080970'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10004344 and pv.postcode = 'TS2 1AD' and q.larid = '50091499'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10004344 and pv.postcode = 'TS2 1AD' and q.larid = '50096795'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10004344 and pv.postcode = 'TS2 1AD' and q.larid = '50098147'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10004344 and pv.postcode = 'TS2 1AD' and q.larid = '60038883'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10004344 and pv.postcode = 'TS2 1AD' and q.larid = '60184358'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10004375 and pv.postcode = 'MK3 6DR' and q.larid = '50090392'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10004375 and pv.postcode = 'MK3 6DR' and q.larid = '50098147'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10004375 and pv.postcode = 'MK3 6DR' and q.larid = '60042321'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10004375 and pv.postcode = 'MK6 5LP' and q.larid = '60164281'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10004375 and pv.postcode = 'MK6 5LP' and q.larid = '60174699'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10004375 and pv.postcode = 'MK6 5LP' and q.larid = '60175886'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10004576 and pv.postcode = 'DH1 5ES' and q.larid = '50090999'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10004576 and pv.postcode = 'DH1 5ES' and q.larid = '50094397'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10004576 and pv.postcode = 'DH1 5ES' and q.larid = '60038895'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10004576 and pv.postcode = 'DH1 5ES' and q.larid = '60126292'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10004576 and pv.postcode = 'DH1 5ES' and q.larid = '60161103'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10004576 and pv.postcode = 'DH1 5ES' and q.larid = '60184383'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10004576 and pv.postcode = 'DH1 5ES' and q.larid = '6015696X'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10004579 and pv.postcode = 'SN3 1AH' and q.larid = '50067515'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10004579 and pv.postcode = 'SN3 1AH' and q.larid = '50081962'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10004579 and pv.postcode = 'SN3 1AH' and q.larid = '60169904'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10004596 and pv.postcode = 'RG14 7TD' and q.larid = '50097076'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10004596 and pv.postcode = 'RG14 7TD' and q.larid = '60172071'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10004596 and pv.postcode = 'RG14 7TD' and q.larid = '60172204'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10004596 and pv.postcode = 'RG14 7TD' and q.larid = '60172307'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10004596 and pv.postcode = 'RG14 7TD' and q.larid = '60175242'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10004599 and pv.postcode = 'CA1 1HS' and q.larid = '60190048'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10004599 and pv.postcode = 'CA1 1HS' and q.larid = '60304595'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10004599 and pv.postcode = 'DY10 1AB' and q.larid = '50073576'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10004599 and pv.postcode = 'DY10 1AB' and q.larid = '50098147'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10004599 and pv.postcode = 'DY10 1AB' and q.larid = '60039309'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10004599 and pv.postcode = 'DY10 1AB' and q.larid = '60179922'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10004599 and pv.postcode = 'NE4 7SA' and q.larid = '50067515'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10004599 and pv.postcode = 'NE4 7SA' and q.larid = '50067552'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10004599 and pv.postcode = 'NE4 7SA' and q.larid = '50090768'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10004599 and pv.postcode = 'NE4 7SA' and q.larid = '60039310'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10004599 and pv.postcode = 'NE4 7SA' and q.larid = '60131408'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10004599 and pv.postcode = 'NE4 7SA' and q.larid = '60131421'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10004599 and pv.postcode = 'SE4 1UT' and q.larid = '50065695'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10004609 and pv.postcode = 'E15 4GY' and q.larid = '60167178'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10004609 and pv.postcode = 'E15 4GY' and q.larid = '60167269'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10004686 and pv.postcode = 'KT17 3DS' and q.larid = '60038895'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10004686 and pv.postcode = 'KT17 3DS' and q.larid = '60173488'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10004686 and pv.postcode = 'KT17 3DS' and q.larid = '60175503'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10004686 and pv.postcode = 'KT17 3DS' and q.larid = '6017352X'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10004718 and pv.postcode = 'CV11 6BH' and q.larid = '50063558'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10004718 and pv.postcode = 'CV11 6BH' and q.larid = '50078781'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10004718 and pv.postcode = 'CV11 6BH' and q.larid = '50091256'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10004718 and pv.postcode = 'CV11 6BH' and q.larid = '50091499'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10004718 and pv.postcode = 'CV11 6BH' and q.larid = '50094397'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10004718 and pv.postcode = 'CV11 6BH' and q.larid = '50099796'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10004718 and pv.postcode = 'CV11 6BH' and q.larid = '50101584'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10004718 and pv.postcode = 'CV11 6BH' and q.larid = '60012018'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10004718 and pv.postcode = 'CV11 6BH' and q.larid = '60042229'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10004718 and pv.postcode = 'CV11 6BH' and q.larid = '60042370'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10004718 and pv.postcode = 'CV11 6BH' and q.larid = '60064456'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10004718 and pv.postcode = 'CV11 6BH' and q.larid = '5011430X'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10004718 and pv.postcode = 'CV11 6BH' and q.larid = '6006724X'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10004718 and pv.postcode = 'LE10 1QU' and q.larid = '60064456'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10004718 and pv.postcode = 'LE10 1QU' and q.larid = '60101726'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10004718 and pv.postcode = 'LE10 3DT' and q.larid = '60079137'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10004718 and pv.postcode = 'LE10 3DT' and q.larid = '60080498'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10004718 and pv.postcode = 'LE18 4PH' and q.larid = '50073576'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10004718 and pv.postcode = 'LE18 4PH' and q.larid = '50078410'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10004718 and pv.postcode = 'LE18 4PH' and q.larid = '50091499'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10004718 and pv.postcode = 'LE18 4PH' and q.larid = '60012018'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10004718 and pv.postcode = 'LE18 4PH' and q.larid = '60042229'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10004721 and pv.postcode = 'DA1 2JT' and q.larid = '50067643'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10004721 and pv.postcode = 'DA1 2JT' and q.larid = '50091475'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10004721 and pv.postcode = 'DA1 2JT' and q.larid = '60125366'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10004721 and pv.postcode = 'DA12 2JT' and q.larid = '50078410'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10004721 and pv.postcode = 'DA12 2JT' and q.larid = '50080982'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10004721 and pv.postcode = 'DA12 2JT' and q.larid = '50094397'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10004721 and pv.postcode = 'DA12 2JT' and q.larid = '50098147'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10004721 and pv.postcode = 'DA12 2JT' and q.larid = '60010630'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10004721 and pv.postcode = 'DA12 2JT' and q.larid = '60060049'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10004721 and pv.postcode = 'DA12 2JT' and q.larid = '60086038'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10004721 and pv.postcode = 'DA12 2JT' and q.larid = '60094813'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10004721 and pv.postcode = 'DA12 2JT' and q.larid = '60107170'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10004721 and pv.postcode = 'DA12 2JT' and q.larid = '60125366'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10004721 and pv.postcode = 'DA12 2JT' and q.larid = '60140008'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10004721 and pv.postcode = 'DA12 2JT' and q.larid = '5009824X'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10004721 and pv.postcode = 'DA12 2JT' and q.larid = '6006724X'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10004721 and pv.postcode = 'DA12 2JT' and q.larid = '6018436X'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10004736 and pv.postcode = 'BN1 4FA' and q.larid = '50090513'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10004736 and pv.postcode = 'BN1 4FA' and q.larid = '60028270'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10004736 and pv.postcode = 'BN12 6NU' and q.larid = '50094397'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10004736 and pv.postcode = 'BN14 8HJ' and q.larid = '50090513'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10004736 and pv.postcode = 'BN2 5PB' and q.larid = '50097076'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10004736 and pv.postcode = 'BN2 5PB' and q.larid = '50100191'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10004762 and pv.postcode = 'NE23 6UR' and q.larid = '60324764'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10004762 and pv.postcode = 'NE23 6UR' and q.larid = '6032496X'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10004772 and pv.postcode = 'NR2 2LJ' and q.larid = '50098184'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10004772 and pv.postcode = 'NR2 2LJ' and q.larid = '60008829'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10004772 and pv.postcode = 'NR2 2LJ' and q.larid = '60038883'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10004772 and pv.postcode = 'NR2 2LJ' and q.larid = '60117904'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10004772 and pv.postcode = 'NR2 2LJ' and q.larid = '60139869'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10004772 and pv.postcode = 'NR2 2LJ' and q.larid = '60174626'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10004772 and pv.postcode = 'NR2 2LJ' and q.larid = '6017576X'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10004835 and pv.postcode = 'AL1 3JE' and q.larid = '50091499'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10004835 and pv.postcode = 'AL1 5HL' and q.larid = '60042333'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10004835 and pv.postcode = 'SG1 2JE' and q.larid = '60061121'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10004835 and pv.postcode = 'SG4 0TP' and q.larid = '50071397'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10004835 and pv.postcode = 'WD7 9AB' and q.larid = '60110958'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10004927 and pv.postcode = 'OX16 9QA' and q.larid = '60163148'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10005077 and pv.postcode = 'PE1 4DZ' and q.larid = '60054992'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10005077 and pv.postcode = 'PE1 4DZ' and q.larid = '60145110'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10005077 and pv.postcode = 'PE1 4DZ' and q.larid = '60145134'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10005077 and pv.postcode = 'PE1 4DZ' and q.larid = '60184346'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10005077 and pv.postcode = 'PE1 4DZ' and q.larid = '60184358'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10005128 and pv.postcode = 'PL1 5QG' and q.larid = '50090513'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10005128 and pv.postcode = 'PL1 5QG' and q.larid = '50098147'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10005128 and pv.postcode = 'PL1 5QG' and q.larid = '60058493'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10005128 and pv.postcode = 'PL1 5QG' and q.larid = '60079952'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10005128 and pv.postcode = 'PL1 5QG' and q.larid = '60174973'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10005128 and pv.postcode = 'PL1 5QG' and q.larid = '60303566'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10005128 and pv.postcode = 'PL1 5QG' and q.larid = '6017352X'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10005158 and pv.postcode = 'PO3 6PZ' and q.larid = '50067643'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10005158 and pv.postcode = 'PO3 6PZ' and q.larid = '60017867'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10005158 and pv.postcode = 'PO3 6PZ' and q.larid = '60039310'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10005158 and pv.postcode = 'PO3 6PZ' and q.larid = '60042278'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10005158 and pv.postcode = 'PO3 6PZ' and q.larid = '60061765'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10005158 and pv.postcode = 'PO3 6PZ' and q.larid = '60185867'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10005158 and pv.postcode = 'PO3 6PZ' and q.larid = '6006819X'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10005172 and pv.postcode = 'NR20 3JY' and q.larid = '60008805'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10005172 and pv.postcode = 'NR20 3JY' and q.larid = '6015696X'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10005172 and pv.postcode = 'NR5 8BF' and q.larid = '60008805'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10005172 and pv.postcode = 'NR5 8BF' and q.larid = '6015696X'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10005172 and pv.postcode = 'NR5 8BF' and q.larid = '6017013X'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10005200 and pv.postcode = 'PR2 8UR' and q.larid = '50072961'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10005200 and pv.postcode = 'PR2 8UR' and q.larid = '50078008'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10005200 and pv.postcode = 'PR2 8UR' and q.larid = '50080970'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10005200 and pv.postcode = 'PR2 8UR' and q.larid = '60026571'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10005200 and pv.postcode = 'PR2 8UR' and q.larid = '60028269'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10005200 and pv.postcode = 'PR2 8UR' and q.larid = '60072374'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10005200 and pv.postcode = 'PR2 8UR' and q.larid = '60086038'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10005200 and pv.postcode = 'PR2 8UR' and q.larid = '60086099'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10005200 and pv.postcode = 'PR2 8UR' and q.larid = '60142911'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10005200 and pv.postcode = 'PR2 8UR' and q.larid = '60174377'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10005404 and pv.postcode = 'CW5 6DF' and q.larid = '60001069'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10005469 and pv.postcode = 'TW2 7SJ' and q.larid = '50099796'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10005469 and pv.postcode = 'TW2 7SJ' and q.larid = '60061789'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10005469 and pv.postcode = 'TW2 7SJ' and q.larid = '6006139X'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10005469 and pv.postcode = 'TW2 7SJ' and q.larid = '6006318X'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10005514 and pv.postcode = 'L7 2RN' and q.larid = '60135864'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10005514 and pv.postcode = 'L7 2RN' and q.larid = '60181953'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10005514 and pv.postcode = 'L7 2RN' and q.larid = '60328988'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10005534 and pv.postcode = 'S65 1EG' and q.larid = '50068015'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10005534 and pv.postcode = 'S65 1EG' and q.larid = '50078781'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10005534 and pv.postcode = 'S65 1EG' and q.larid = '60042345'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10005534 and pv.postcode = 'S65 1EG' and q.larid = '60169837'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10005534 and pv.postcode = 'S65 1EG' and q.larid = '60303566'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10005534 and pv.postcode = 'S65 1EG' and q.larid = '60326116'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10005534 and pv.postcode = 'S65 1EG' and q.larid = '60330430'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10005736 and pv.postcode = 'RM17 5TD' and q.larid = '50067461'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10005736 and pv.postcode = 'RM17 5TD' and q.larid = '60038895'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10005736 and pv.postcode = 'RM17 5TD' and q.larid = '60068206'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10005736 and pv.postcode = 'RM17 5TD' and q.larid = '60117904'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10005736 and pv.postcode = 'SS7 1TW' and q.larid = '50067461'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10005736 and pv.postcode = 'SS7 1TW' and q.larid = '50068015'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10005736 and pv.postcode = 'SS7 1TW' and q.larid = '50081652'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10005736 and pv.postcode = 'SS7 1TW' and q.larid = '60139869'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10005736 and pv.postcode = 'SS7 1TW' and q.larid = '60160421'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10005736 and pv.postcode = 'SS7 1TW' and q.larid = '60174377'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10005736 and pv.postcode = 'SS7 1TW' and q.larid = '6016041X'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10005741 and pv.postcode = 'YO8 8AT' and q.larid = '60173427'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10005775 and pv.postcode = 'TS3 8BT' and q.larid = '60326608'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10005775 and pv.postcode = 'TS3 8BT' and q.larid = '5011394X'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10005788 and pv.postcode = 'S2 2RL' and q.larid = '50067643'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10005788 and pv.postcode = 'S2 2RL' and q.larid = '50091013'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10005788 and pv.postcode = 'S2 2RL' and q.larid = '60028269'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10005788 and pv.postcode = 'S2 2RL' and q.larid = '60139870'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10005788 and pv.postcode = 'S2 2RL' and q.larid = '60184358'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10005788 and pv.postcode = 'S2 2RL' and q.larid = '60330430'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10005859 and pv.postcode = 'E17 5AA' and q.larid = '60039310'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10005859 and pv.postcode = 'E17 5AA' and q.larid = '60058493'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10005859 and pv.postcode = 'E17 5AA' and q.larid = '60068152'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10005859 and pv.postcode = 'E17 5AA' and q.larid = '60070468'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10005859 and pv.postcode = 'E17 5AA' and q.larid = '60105768'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10005859 and pv.postcode = 'E17 5AA' and q.larid = '6006819X'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10005972 and pv.postcode = 'CH65 7BF' and q.larid = '50094397'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10005972 and pv.postcode = 'CH65 7BF' and q.larid = '60066830'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10005972 and pv.postcode = 'CH65 7BF' and q.larid = '60107170'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10005972 and pv.postcode = 'CH65 7BF' and q.larid = '60324752'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10005972 and pv.postcode = 'CW2 8AB' and q.larid = '50094397'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10005972 and pv.postcode = 'CW2 8AB' and q.larid = '60066830'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10005972 and pv.postcode = 'CW2 8AB' and q.larid = '60107170'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10005972 and pv.postcode = 'CW2 8AB' and q.larid = '60324752'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10005977 and pv.postcode = 'TQ4 7EJ' and q.larid = '60139274'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10005979 and pv.postcode = 'PO7 8AA' and q.larid = '60160421'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10005979 and pv.postcode = 'PO7 8AA' and q.larid = '60169874'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10005998 and pv.postcode = 'M32 0XH' and q.larid = '50067205'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10005998 and pv.postcode = 'M32 0XH' and q.larid = '50098184'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10005998 and pv.postcode = 'WA14 5PQ' and q.larid = '50094397'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10005998 and pv.postcode = 'WA14 5PQ' and q.larid = '50095432'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10005999 and pv.postcode = 'NE34 6ET' and q.larid = '50100191'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10005999 and pv.postcode = 'NE34 6ET' and q.larid = '60131421'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10005999 and pv.postcode = 'NE34 6ET' and q.larid = '6032496X'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10006005 and pv.postcode = 'DL5 6AT' and q.larid = '60038883'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10006174 and pv.postcode = 'WA10 1PP' and q.larid = '50089663'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10006174 and pv.postcode = 'WA10 1PP' and q.larid = '50098184'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10006174 and pv.postcode = 'WA10 1PP' and q.larid = '50098214'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10006174 and pv.postcode = 'WA10 1PP' and q.larid = '60169825'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10006174 and pv.postcode = 'WA10 1PP' and q.larid = '6030229X'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10006341 and pv.postcode = 'TS10 1EZ' and q.larid = '50095018'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10006341 and pv.postcode = 'TS10 1EZ' and q.larid = '60069958'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10006341 and pv.postcode = 'TS10 1EZ' and q.larid = '60140008'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10006341 and pv.postcode = 'TS17 6FB' and q.larid = '50095018'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10006341 and pv.postcode = 'TS17 6FB' and q.larid = '60069958'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10006341 and pv.postcode = 'TS17 6FB' and q.larid = '60110958'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10006341 and pv.postcode = 'TS17 6FB' and q.larid = '60136765'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10006341 and pv.postcode = 'TS17 6FB' and q.larid = '60140008'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10006378 and pv.postcode = 'BA16 0AB' and q.larid = '50067461'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10006378 and pv.postcode = 'BA16 0AB' and q.larid = '50067643'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10006378 and pv.postcode = 'BA16 0AB' and q.larid = '50082097'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10006378 and pv.postcode = 'BA16 0AB' and q.larid = '50087617'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10006378 and pv.postcode = 'BA16 0AB' and q.larid = '50091499'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10006378 and pv.postcode = 'BA16 0AB' and q.larid = '50095018'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10006378 and pv.postcode = 'BA16 0AB' and q.larid = '60125275'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10006378 and pv.postcode = 'BA16 0AB' and q.larid = '60156971'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10006398 and pv.postcode = 'IP4 1LT' and q.larid = '60067445'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10006398 and pv.postcode = 'IP4 1LT' and q.larid = '60068206'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10006398 and pv.postcode = 'IP4 1LT' and q.larid = '60131408'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10006398 and pv.postcode = 'IP4 1LT' and q.larid = '60304595'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10006438 and pv.postcode = 'SM5 3BB' and q.larid = '6015696X'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10006442 and pv.postcode = 'B4 7PS' and q.larid = '60103772'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10006442 and pv.postcode = 'DY5 1RG' and q.larid = '50027074'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10006442 and pv.postcode = 'DY5 1RG' and q.larid = '60139870'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10006463 and pv.postcode = 'SN2 1DY' and q.larid = '60304595'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10006494 and pv.postcode = 'OL6 6DD' and q.larid = '50090768'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10006494 and pv.postcode = 'OL6 6DD' and q.larid = '50094397'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10006494 and pv.postcode = 'OL6 6DD' and q.larid = '50099796'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10006494 and pv.postcode = 'OL6 6DD' and q.larid = '60051516'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10006494 and pv.postcode = 'OL6 6DD' and q.larid = '60070468'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10006494 and pv.postcode = 'OL6 6DF' and q.larid = '60146254'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10006494 and pv.postcode = 'OL6 6DF' and q.larid = '60174377'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10006494 and pv.postcode = 'OL6 6NX' and q.larid = '50067643'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10006494 and pv.postcode = 'OL6 6NX' and q.larid = '50096898'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10006494 and pv.postcode = 'OL6 6NX' and q.larid = '60039310'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10006494 and pv.postcode = 'OL6 6NX' and q.larid = '5008365X'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10006549 and pv.postcode = 'TF1 2NP' and q.larid = '60175916'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10006549 and pv.postcode = 'TF1 2NP' and q.larid = '6014029X'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10006570 and pv.postcode = 'CT1 3AJ' and q.larid = '50073898'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10006570 and pv.postcode = 'CT1 3AJ' and q.larid = '60169825'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10006570 and pv.postcode = 'CT1 3AJ' and q.larid = '60184358'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10006570 and pv.postcode = 'CT10 1PN' and q.larid = '50073898'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10006570 and pv.postcode = 'CT10 1PN' and q.larid = '50094397'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10006570 and pv.postcode = 'CT16 1DH' and q.larid = '50098147'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10006570 and pv.postcode = 'CT20 2TZ' and q.larid = '60028270'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10006570 and pv.postcode = 'CT20 2TZ' and q.larid = '60184358'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10006770 and pv.postcode = 'OL9 6AA' and q.larid = '60174377'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10006770 and pv.postcode = 'OL9 6AA' and q.larid = '6032496X'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10006813 and pv.postcode = 'E5 8BP' and q.larid = '6016475X'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10006963 and pv.postcode = 'E14 0AF' and q.larid = '60058493'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10006963 and pv.postcode = 'IG1 4HP' and q.larid = '60038895'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10006963 and pv.postcode = 'IG1 4HP' and q.larid = '60058493'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10006963 and pv.postcode = 'IG1 4HP' and q.larid = '60319148'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10006963 and pv.postcode = 'IG1 4HP' and q.larid = '60334927'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10006963 and pv.postcode = 'IG10 3SA' and q.larid = '60334927'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10006963 and pv.postcode = 'RM11 2LL' and q.larid = '10060960'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10007011 and pv.postcode = 'NN3 3RF' and q.larid = '50118031'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10007063 and pv.postcode = 'TR1 3XX' and q.larid = '60048505'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10007063 and pv.postcode = 'TR1 3XX' and q.larid = '60169904'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10007063 and pv.postcode = 'TR1 3XX' and q.larid = '60169977'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10007063 and pv.postcode = 'TR1 3XX' and q.larid = '6030229X'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10007063 and pv.postcode = 'TR18 2SA' and q.larid = '60045322'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10007063 and pv.postcode = 'TR18 2SA' and q.larid = '60169825'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10007063 and pv.postcode = 'TR18 2SA' and q.larid = '60169904'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10007063 and pv.postcode = 'TR18 2SA' and q.larid = '60301910'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10007063 and pv.postcode = 'TR18 2SA' and q.larid = '6030229X'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10007212 and pv.postcode = 'BN1 6WQ' and q.larid = '60171947'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10007315 and pv.postcode = 'WS2 8ES' and q.larid = '50067503'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10007315 and pv.postcode = 'WS2 8ES' and q.larid = '50067643'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10007315 and pv.postcode = 'WS2 8ES' and q.larid = '50068726'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10007315 and pv.postcode = 'WS2 8ES' and q.larid = '50071397'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10007315 and pv.postcode = 'WS2 8ES' and q.larid = '50081652'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10007315 and pv.postcode = 'WS2 8ES' and q.larid = '50089572'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10007315 and pv.postcode = 'WS2 8ES' and q.larid = '50091001'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10007315 and pv.postcode = 'WS2 8ES' and q.larid = '50091499'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10007315 and pv.postcode = 'WS2 8ES' and q.larid = '50100191'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10007315 and pv.postcode = 'WS2 8ES' and q.larid = '60054979'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10007315 and pv.postcode = 'WS2 8ES' and q.larid = '60054992'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10007315 and pv.postcode = 'WS2 8ES' and q.larid = '60131391'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10007315 and pv.postcode = 'WS2 8ES' and q.larid = '5006485X'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10007321 and pv.postcode = 'E17 4JB' and q.larid = '50094397'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10007321 and pv.postcode = 'E17 4JB' and q.larid = '60106025'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10007321 and pv.postcode = 'E17 4JB' and q.larid = '60107170'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10007321 and pv.postcode = 'E17 4JB' and q.larid = '60132309'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10007321 and pv.postcode = 'E17 4JB' and q.larid = '60162661'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10007339 and pv.postcode = 'WA2 8QA' and q.larid = '50098147'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10007339 and pv.postcode = 'WA2 8QA' and q.larid = '60131391'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10007417 and pv.postcode = 'LU2 7BF' and q.larid = '10061472'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10007417 and pv.postcode = 'LU2 7BF' and q.larid = '50090719'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10007417 and pv.postcode = 'LU2 7BF' and q.larid = '50094397'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10007417 and pv.postcode = 'LU2 7BF' and q.larid = '50098147'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10007417 and pv.postcode = 'LU2 7BF' and q.larid = '60038871'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10007417 and pv.postcode = 'LU2 7BF' and q.larid = '60039309'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10007417 and pv.postcode = 'LU2 7BF' and q.larid = '60055005'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10007417 and pv.postcode = 'LU2 7BF' and q.larid = '60164724'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10007417 and pv.postcode = 'LU2 7BF' and q.larid = '60169771'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10007417 and pv.postcode = 'LU2 7BF' and q.larid = '60169904'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10007417 and pv.postcode = 'LU2 7BF' and q.larid = '60171613'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10007417 and pv.postcode = 'WD17 3EZ' and q.larid = '50067643'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10007417 and pv.postcode = 'WD17 3EZ' and q.larid = '50094397'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10007417 and pv.postcode = 'WD17 3EZ' and q.larid = '60028270'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10007417 and pv.postcode = 'WD17 3EZ' and q.larid = '60038871'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10007417 and pv.postcode = 'WD17 3EZ' and q.larid = '60064456'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10007417 and pv.postcode = 'WD17 3EZ' and q.larid = '60106025'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10007417 and pv.postcode = 'WD17 3EZ' and q.larid = '60107170'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10007417 and pv.postcode = 'WD17 3EZ' and q.larid = '60174997'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10007417 and pv.postcode = 'WD17 3EZ' and q.larid = '6015696X'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10007417 and pv.postcode = 'WD4 8LZ' and q.larid = '50081652'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10007417 and pv.postcode = 'WD4 8LZ' and q.larid = '50098147'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10007417 and pv.postcode = 'WD4 8LZ' and q.larid = '60054980'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10007417 and pv.postcode = 'WD4 8LZ' and q.larid = '60055005'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10007417 and pv.postcode = 'WD4 8LZ' and q.larid = '60080498'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10007417 and pv.postcode = 'WD4 8LZ' and q.larid = '60100874'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10007417 and pv.postcode = 'WD4 8LZ' and q.larid = '60105458'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10007417 and pv.postcode = 'WD4 8LZ' and q.larid = '60156405'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10007427 and pv.postcode = 'NG17 5FA' and q.larid = '50078410'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10007427 and pv.postcode = 'NG17 5FA' and q.larid = '50098147'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10007427 and pv.postcode = 'NG17 5FA' and q.larid = '50098184'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10007427 and pv.postcode = 'NG17 7RB' and q.larid = '50071385'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10007427 and pv.postcode = 'NG17 7RB' and q.larid = '60054979'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10007427 and pv.postcode = 'NG17 7RB' and q.larid = '60054980'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10007427 and pv.postcode = 'NG17 7RB' and q.larid = '60054992'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10007427 and pv.postcode = 'NG17 7RB' and q.larid = '60055005'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10007427 and pv.postcode = 'NG17 7RB' and q.larid = '60174183'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10007427 and pv.postcode = 'NG17 7RB' and q.larid = '60303529'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10007427 and pv.postcode = 'NG18 5BH' and q.larid = '60174687'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10007434 and pv.postcode = 'TW7 4HS' and q.larid = '60038871'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10007434 and pv.postcode = 'TW7 4HS' and q.larid = '60163586'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10007434 and pv.postcode = 'TW7 4HS' and q.larid = '60188546'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10007455 and pv.postcode = 'N15 4RU' and q.larid = '50090768'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10007455 and pv.postcode = 'N15 4RU' and q.larid = '60058493'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10007455 and pv.postcode = 'N15 4RU' and q.larid = '60184358'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10007455 and pv.postcode = 'N7 0SP' and q.larid = '50068015'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10007455 and pv.postcode = 'N7 0SP' and q.larid = '60117801'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10007455 and pv.postcode = 'N7 0SP' and q.larid = '60117904'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10007459 and pv.postcode = 'BS22 8ND' and q.larid = '50091499'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10007459 and pv.postcode = 'BS22 8ND' and q.larid = '60093316'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10007459 and pv.postcode = 'BS22 8ND' and q.larid = '60308631'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10007459 and pv.postcode = 'BS23 2AL' and q.larid = '60171984'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10007459 and pv.postcode = 'BS24 8EE' and q.larid = '50067643'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10007459 and pv.postcode = 'BS24 8EE' and q.larid = '60160421'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10007459 and pv.postcode = 'BS24 8EE' and q.larid = '60174225'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10007527 and pv.postcode = 'BA14 0ES' and q.larid = '60172204'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10007527 and pv.postcode = 'SN15 2NY' and q.larid = '60172204'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10007527 and pv.postcode = 'SN15 2NY' and q.larid = '60174389'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10007527 and pv.postcode = 'SN15 3QD' and q.larid = '60175801'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10007527 and pv.postcode = 'SP1 2LW' and q.larid = '60172204'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10007527 and pv.postcode = 'SP1 2LW' and q.larid = '60174389'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10007553 and pv.postcode = 'CH41 1AA' and q.larid = '6010563X'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10007553 and pv.postcode = 'CH41 1AG' and q.larid = '50066717'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10007553 and pv.postcode = 'CH41 1AG' and q.larid = '50067205'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10007553 and pv.postcode = 'CH41 1AG' and q.larid = '50098147'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10007553 and pv.postcode = 'CH41 1AG' and q.larid = '60058493'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10007553 and pv.postcode = 'CH41 1AG' and q.larid = '60165522'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10007553 and pv.postcode = 'CH63 7LH' and q.larid = '50067552'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10007553 and pv.postcode = 'CH63 7LH' and q.larid = '60039309'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10007578 and pv.postcode = 'WV6 0DU' and q.larid = '50096886'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10007578 and pv.postcode = 'WV6 0DU' and q.larid = '50098147'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10007578 and pv.postcode = 'WV6 0DU' and q.larid = '50098184'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10007578 and pv.postcode = 'WV6 0DU' and q.larid = '60020787'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10007578 and pv.postcode = 'WV6 0DU' and q.larid = '60079137'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10007578 and pv.postcode = 'WV6 0DU' and q.larid = '60102330'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10007578 and pv.postcode = 'WV6 0DU' and q.larid = '60184346'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10007673 and pv.postcode = 'HU3 2JZ' and q.larid = '60039310'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10007673 and pv.postcode = 'HU3 2JZ' and q.larid = '60061789'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10007673 and pv.postcode = 'HU3 4AE' and q.larid = '60039310'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10007673 and pv.postcode = 'HU3 4AE' and q.larid = '60061789'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10007851 and pv.postcode = 'SK17 6RY' and q.larid = '50081974'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10007851 and pv.postcode = 'SK17 6RY' and q.larid = '60038895'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10007851 and pv.postcode = 'SK17 6RY' and q.larid = '60042370'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10007851 and pv.postcode = 'SK17 6RY' and q.larid = '60061789'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10007851 and pv.postcode = 'SK17 6RY' and q.larid = '60125366'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10007851 and pv.postcode = 'SK17 6RY' and q.larid = '60184371'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10007851 and pv.postcode = 'SK17 6RY' and q.larid = '60303761'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10007851 and pv.postcode = 'SK17 6RY' and q.larid = '60303773'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10007859 and pv.postcode = 'CV21 1AR' and q.larid = '60028270'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10007859 and pv.postcode = 'CV21 1AR' and q.larid = '60304595'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10007859 and pv.postcode = 'CV32 5JE' and q.larid = '60028270'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10007859 and pv.postcode = 'CV32 5JE' and q.larid = '60143319'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10007859 and pv.postcode = 'CV32 5JE' and q.larid = '60164724'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10007859 and pv.postcode = 'CV32 5JE' and q.larid = '60330661'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10007859 and pv.postcode = 'CV35 9BL' and q.larid = '60174225'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10007859 and pv.postcode = 'CV35 9BL' and q.larid = '60304595'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10007859 and pv.postcode = 'WR11 1LP' and q.larid = '50098147'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10007924 and pv.postcode = 'DY1 4AS' and q.larid = '60038901'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10007924 and pv.postcode = 'DY1 4AS' and q.larid = '60039309'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10007924 and pv.postcode = 'DY1 4AS' and q.larid = '60039310'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10007924 and pv.postcode = 'DY1 4AS' and q.larid = '60064456'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10007924 and pv.postcode = 'DY1 4AS' and q.larid = '60174626'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10007924 and pv.postcode = 'DY1 4AS' and q.larid = '60174973'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10007938 and pv.postcode = 'DN34 5BQ' and q.larid = '50090513'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10007938 and pv.postcode = 'DN34 5BQ' and q.larid = '50090768'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10007938 and pv.postcode = 'DN34 5BQ' and q.larid = '50100191'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10007938 and pv.postcode = 'DN34 5BQ' and q.larid = '60107170'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10007945 and pv.postcode = 'PO6 2SA' and q.larid = '50098147'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10007945 and pv.postcode = 'PO6 2SA' and q.larid = '60038871'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10007945 and pv.postcode = 'PO6 2SA' and q.larid = '60044561'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10007945 and pv.postcode = 'PO6 2SA' and q.larid = '60171613'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10007945 and pv.postcode = 'PO6 2SA' and q.larid = '6011096X'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10007977 and pv.postcode = 'B98 8DW' and q.larid = '60175722'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10007977 and pv.postcode = 'WR1 2JF' and q.larid = '60038871'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10007977 and pv.postcode = 'WR1 2JF' and q.larid = '60175722'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10008007 and pv.postcode = 'SW12 8EN' and q.larid = '50091499'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10009439 and pv.postcode = 'HA7 4BQ' and q.larid = '60125366'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10009439 and pv.postcode = 'HA7 4BQ' and q.larid = '60321301'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10023139 and pv.postcode = 'M1 3HB' and q.larid = '60139869'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10023139 and pv.postcode = 'M1 3HB' and q.larid = '60139870'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10023139 and pv.postcode = 'M11 2WH' and q.larid = '50071397'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10023139 and pv.postcode = 'M11 2WH' and q.larid = '50099371'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10023139 and pv.postcode = 'M11 2WH' and q.larid = '60045346'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10023139 and pv.postcode = 'M11 2WH' and q.larid = '60054992'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10023139 and pv.postcode = 'M11 2WH' and q.larid = '60080504'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10023139 and pv.postcode = 'M11 2WH' and q.larid = '60139869'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10023139 and pv.postcode = 'M11 2WH' and q.larid = '60169977'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10023139 and pv.postcode = 'M11 2WH' and q.larid = '60173245'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10023139 and pv.postcode = 'M11 2WH' and q.larid = '60175916'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10023139 and pv.postcode = 'M12 6BA' and q.larid = '60105021'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10023139 and pv.postcode = 'M23 0DD' and q.larid = '50086212'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10023139 and pv.postcode = 'M23 0DD' and q.larid = '60086609'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10023139 and pv.postcode = 'M23 0DD' and q.larid = '60087067'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10023139 and pv.postcode = 'M23 0DD' and q.larid = '60087602'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10023139 and pv.postcode = 'M23 0DD' and q.larid = '60169825'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10023139 and pv.postcode = 'M23 0DD' and q.larid = '60169849'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10023139 and pv.postcode = 'M9 4AF' and q.larid = '50073576'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10023139 and pv.postcode = 'M9 4AF' and q.larid = '60087067'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10023139 and pv.postcode = 'M9 4AF' and q.larid = '60087614'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10023139 and pv.postcode = 'M9 4AF' and q.larid = '60169825'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10023139 and pv.postcode = 'M9 4AF' and q.larid = '60169874'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10023526 and pv.postcode = 'B79 8AE' and q.larid = '50089766'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10023526 and pv.postcode = 'B79 8AE' and q.larid = '50091475'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10023526 and pv.postcode = 'B79 8AE' and q.larid = '60038871'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10023526 and pv.postcode = 'B79 8AE' and q.larid = '60086609'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10023526 and pv.postcode = 'B79 8AE' and q.larid = '60086658'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10023526 and pv.postcode = 'B79 8AE' and q.larid = '60188558'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10023526 and pv.postcode = 'B79 8AE' and q.larid = '6033292X'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10023526 and pv.postcode = 'ST19 5PH' and q.larid = '60054979'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10023526 and pv.postcode = 'ST19 5PH' and q.larid = '60055005'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10023526 and pv.postcode = 'WS11 1UE' and q.larid = '60188558'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10023526 and pv.postcode = 'WS13 6QG' and q.larid = '60038895'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10028138 and pv.postcode = 'CO15 3JL' and q.larid = '50067552'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10028138 and pv.postcode = 'CO15 3JL' and q.larid = '60115622'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10028138 and pv.postcode = 'CO15 3JL' and q.larid = '60171571'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10031579 and pv.postcode = 'CT2 8QA' and q.larid = '60051516'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10032198 and pv.postcode = 'LS25 1LJ' and q.larid = '60145626'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10032198 and pv.postcode = 'LS25 1LJ' and q.larid = '60169825'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10032198 and pv.postcode = 'LS25 1LJ' and q.larid = '6030229X'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10033255 and pv.postcode = 'TN24 9AL' and q.larid = '50099796'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10033771 and pv.postcode = 'DL14 7JZ' and q.larid = '60171947'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10033771 and pv.postcode = 'DL14 7JZ' and q.larid = '60312075'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10034161 and pv.postcode = 'RG10 8DS' and q.larid = '50096436'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10034161 and pv.postcode = 'RG10 8DS' and q.larid = '60130593'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10034161 and pv.postcode = 'RG10 8DS' and q.larid = '60170943'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10034161 and pv.postcode = 'RG10 8DS' and q.larid = '60171571'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10034913 and pv.postcode = 'WA12 0AQ' and q.larid = '50082310'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10034999 and pv.postcode = 'PE12 7PU' and q.larid = '60086063'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10034999 and pv.postcode = 'PE12 7PU' and q.larid = '60170335'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10034999 and pv.postcode = 'PE12 7PU' and q.larid = '60173245'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10034999 and pv.postcode = 'PE12 7PU' and q.larid = '60174377'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10034999 and pv.postcode = 'PE12 7PU' and q.larid = '60184322'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10036143 and pv.postcode = 'BS34 7AT' and q.larid = '60175230'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10039420 and pv.postcode = 'SE18 4LD' and q.larid = '50067552'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10039420 and pv.postcode = 'SE18 4LD' and q.larid = '50067643'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10039420 and pv.postcode = 'SE18 4LD' and q.larid = '50094397'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10039420 and pv.postcode = 'SE18 4LD' and q.larid = '50095432'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10039420 and pv.postcode = 'SE18 4LD' and q.larid = '60039310'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10039420 and pv.postcode = 'SE18 4LD' and q.larid = '60054980'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10039420 and pv.postcode = 'SE18 4LD' and q.larid = '60055005'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10039420 and pv.postcode = 'SE18 4LD' and q.larid = '60068140'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10039420 and pv.postcode = 'SE18 4LD' and q.larid = '60078650'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10039420 and pv.postcode = 'SE18 4LD' and q.larid = '60086609'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10063846 and pv.postcode = 'WA4 6RD' and q.larid = '50066730'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10063846 and pv.postcode = 'WA4 6RD' and q.larid = '60003212'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10063846 and pv.postcode = 'WA4 6RD' and q.larid = '60038871'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10063846 and pv.postcode = 'WA4 6RD' and q.larid = '60042242'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10063846 and pv.postcode = 'WA4 6RD' and q.larid = '60060724'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10063846 and pv.postcode = 'WA4 6RD' and q.larid = '60145948'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10063846 and pv.postcode = 'WA4 6RD' and q.larid = '60149115'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10063846 and pv.postcode = 'WA4 6RD' and q.larid = '60170608'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10063846 and pv.postcode = 'WA4 6RD' and q.larid = '60175692'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10065145 and pv.postcode = 'OL8 1XU' and q.larid = '60061443'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10065146 and pv.postcode = 'ST15 0BF' and q.larid = '60172265'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10065146 and pv.postcode = 'ST15 0BF' and q.larid = '60174705'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10065146 and pv.postcode = 'ST15 0BF' and q.larid = '60174730'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10065473 and pv.postcode = 'RG21 3HF' and q.larid = '60042278'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10065835 and pv.postcode = 'PO12 4QA' and q.larid = '50095018'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10065835 and pv.postcode = 'PO12 4QA' and q.larid = '60061789'
insert into @providerQualificationsToDelete
select pq.id from provider p 
inner join providervenue pv on pv.providerid = p.id
inner join providerqualification pq on pq.providervenueid = pv.id
inner join qualification q on q.id = pq.qualificationid
where p.ukprn = 10067981 and pv.postcode = 'NR31 7BQ' and q.larid = '60174225'


PRINT ('DELETING...')
delete ProviderQualification where id in (select id from @providerQualificationsToDelete)


--commit
rollback

