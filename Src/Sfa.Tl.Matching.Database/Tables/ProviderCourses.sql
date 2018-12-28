CREATE TABLE [dbo].[ProviderCourses]
(
	[Id] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY, 
	[ProviderId] UNIQUEIDENTIFIER NOT NULL, 
	[CourseId] UNIQUEIDENTIFIER NOT NULL, 
	[AddressId] UNIQUEIDENTIFIER NOT NULL, 
	CONSTRAINT [FK_ProviderCourse_Provider] FOREIGN KEY ([ProviderId]) REFERENCES [Provider]([Id]), 
	CONSTRAINT [FK_ProviderCourse_Course] FOREIGN KEY ([CourseId]) REFERENCES [Course]([Id]),
	CONSTRAINT [FK_ProviderCourse_Address] FOREIGN KEY ([AddressId]) REFERENCES [Address]([Id]), 
)
