﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using NSubstitute;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Application.Mappers;
using Sfa.Tl.Matching.Application.Mappers.Resolver;
using Sfa.Tl.Matching.Application.Services;
using Sfa.Tl.Matching.Application.UnitTests.Services.Qualification.Builders;
using Sfa.Tl.Matching.Data.Interfaces;
using Sfa.Tl.Matching.Domain.Models;
using Sfa.Tl.Matching.Models.ViewModel;
using Xunit;

namespace Sfa.Tl.Matching.Application.UnitTests.Services.Qualification
{
    public class When_QualificationService_Is_Called_To_UpdateQualification
    {
        private readonly IRepository<QualificationRoutePathMapping> _qualificationRoutePathMappingRepository;
        private readonly IRepository<Domain.Models.Qualification> _qualificationRepository;

        public When_QualificationService_Is_Called_To_UpdateQualification()
        {
            var httpcontextAccesor = Substitute.For<IHttpContextAccessor>();
            httpcontextAccesor.HttpContext.Returns(new DefaultHttpContext
            {
                User = new ClaimsPrincipal(new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.GivenName, "adminUserName")
                }))
            });

            var dateTimeProvider = Substitute.For<IDateTimeProvider>();
            dateTimeProvider.UtcNow().Returns(new DateTime(2019, 1, 1));

            var config = new MapperConfiguration(c =>
            {
                c.AddMaps(typeof(QualificationMapper).Assembly);
                c.ConstructServicesUsing(type =>
                    type.Name.Contains("LoggedInUserEmailResolver") ?
                        new LoggedInUserEmailResolver<SaveQualificationViewModel, Domain.Models.Qualification>(httpcontextAccesor) :
                        type.Name.Contains("LoggedInUserNameResolver") ?
                            (object)new LoggedInUserNameResolver<SaveQualificationViewModel, Domain.Models.Qualification>(httpcontextAccesor) :
                            type.Name.Contains("UtcNowResolver") ?
                                new UtcNowResolver<SaveQualificationViewModel, Domain.Models.Qualification>(dateTimeProvider) :
                                null);
            });
            var mapper = new Mapper(config);

            var learningAimReferenceRepository = Substitute.For<IRepository<LearningAimReference>>();

            _qualificationRepository = Substitute.For<IRepository<Domain.Models.Qualification>>();
            _qualificationRepository
                .GetSingleOrDefault(Arg.Any<Expression<Func<Domain.Models.Qualification, bool>>>())
                .Returns(new ValidQualificationBuilder().Build());

            _qualificationRoutePathMappingRepository = Substitute.For<IRepository<QualificationRoutePathMapping>>();
            _qualificationRoutePathMappingRepository
                .GetMany(Arg.Any<Expression<Func<QualificationRoutePathMapping, bool>>>())
                .Returns(new List<QualificationRoutePathMapping>
                {
                    new QualificationRoutePathMapping
                    {
                        Id = 101,
                        QualificationId = 1,
                        RouteId = 1
                    },
                    new QualificationRoutePathMapping
                    {
                        Id = 102,
                        QualificationId = 1,
                        RouteId = 2
                    }
                }.AsQueryable());

            var qualificationService = new QualificationService(mapper, _qualificationRepository, _qualificationRoutePathMappingRepository, learningAimReferenceRepository);

            //Set up routes 1 and 2 in the database,
            //then 2 and 3 selected in the view model.
            //Should end up with mapping for route 1 deleted
            //and route 3 added

            var viewModel = new SaveQualificationViewModel
            {
                QualificationId = 1,
                Title = "Title",
                ShortTitle = "Modified Short Title",
                Source = "Test",
                Routes = new List<RouteViewModel>
                {
                    new RouteViewModel
                    {
                        Id = 1,
                        Name = "Route 1",
                        IsSelected = false
                    },
                    new RouteViewModel
                    {
                        Id = 2,
                        Name = "Route 2",
                        IsSelected = true
                    },
                    new RouteViewModel
                    {
                        Id = 3,
                        Name = "Route 1",
                        IsSelected = true
                    }
                }
            };

            qualificationService.UpdateQualificationAsync(viewModel).GetAwaiter().GetResult();
        }

        [Fact]
        public void Then_QualificationRepository_Update_Is_Called_Exactly_Once()
        {
            _qualificationRepository
                .Received(1)
                .Update(Arg.Any<Domain.Models.Qualification>());
        }

        [Fact]
        public void Then_QualificationRepository_Update_Is_Called_With_Expected_Values()
        {
            _qualificationRepository
                .Received(1)
                .Update(Arg.Is<Domain.Models.Qualification>(
                    q => q.Id == 1 &&
                            q.Title == "Title" &&
                            q.ShortTitle == "Modified Short Title" &&
                            q.QualificationSearch == "TitleModifiedShortTitle" &&
                            q.ShortTitleSearch == "ModifiedShortTitle" && 
                            q.ModifiedBy == "adminUserName" &&
                            q.ModifiedOn == new DateTime(2019, 1, 1)
                ));
        }

        [Fact]
        public void Then_QualificationRoutePathMappingRepository_GetMany_Is_Called_Exactly_Once()
        {
            _qualificationRoutePathMappingRepository
                .Received(1)
                .GetMany(Arg.Any<Expression<Func<QualificationRoutePathMapping, bool>>>());
        }

        //[Fact]
        //public void Then_QualificationRoutePathMappingRepository_CreateMany_Is_Called_With_Expected_Values()
        //{
        //    _qualificationRoutePathMappingRepository
        //        .Received(1)
        //        .CreateMany(Arg.Is<IList<QualificationRoutePathMapping>>(
        //            qrpm => qrpm.Count == 1 &&
        //                    qrpm.First().QualificationId == 1 &&
        //                    qrpm.First().RouteId == 3 &&
        //                    qrpm.First().Source == "Test" &&
        //                    qrpm.First().CreatedBy == "adminUserName"
        //        ));
        //}

        [Fact]
        public void Then_QualificationRoutePathMappingRepository_Create_Is_Called_With_Expected_Values()
        {
            _qualificationRoutePathMappingRepository
                .Received(1)
                .Create(Arg.Is<QualificationRoutePathMapping>(
                    qrpm => qrpm.QualificationId == 1 &&
                            qrpm.RouteId == 3 &&
                            qrpm.Source == "Test" &&
                            qrpm.CreatedBy == "adminUserName"
                ));
        }

        [Fact]
        public void Then_QualificationRoutePathMappingRepository_DeleteMany_Is_Called_With_Expected_Values()
        {
            _qualificationRoutePathMappingRepository
                .Received(1)
                .DeleteMany(Arg.Is<IList<QualificationRoutePathMapping>>(
                    qrpm => qrpm.Count == 1 &&
                            qrpm.First().Id == 101 &&
                            qrpm.First().QualificationId == 1 &&
                            qrpm.First().RouteId == 1
                ));
        }
    }
}