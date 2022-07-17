using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Application.DTOs.Event.PersonalEvent;
using Application.DTOs.Offer;
using Application.Features.Offer.Commands.AddOffer;
using Application.Features.Offer.Commands.DeleteOffer;
using Application.Features.Offer.Commands.EditOffer;
using Application.Features.Offer.Queries.SearchOffers;
using Application.Features.Offer.Queries.SearchUserOffers;
using Domain.Enum;
using IntegrationTest.Handlers;
using MarkopTest;
using MarkopTest.Attributes;
using Microsoft.AspNetCore.Components.Web;
using Newtonsoft.Json.Linq;
using Xunit;
using Xunit.Abstractions;

namespace IntegrationTest.Controller;

[Endpoint("[controller]/[action]")]


public class OfferTests : AppFactory
{
    public OfferTests(ITestOutputHelper outputHelper, HttpClient client = null) : base(outputHelper, client)
    {
    }
    
    [Theory]
    [InstructorHandler]
    [InlineData("description", "1000", "Title", "smiley.png", OfferType.Buy,  HttpStatusCode.OK)]
    [InlineData("description", "1000", "Title", null, OfferType.Buy,  HttpStatusCode.NotAcceptable,
        ErrorType.FileNotFound)]
    [InlineData("description", null, "Title", "smiley.png", OfferType.Buy,  HttpStatusCode.NotAcceptable,
        ErrorType.InvalidInput)]
    public async Task AddOffer(string description, string price, string title, string avatarId, OfferType offerType,
        HttpStatusCode httpStatusCode, ErrorType? errorCode = null)
    {
        var data = new AddOfferCommand
        {
            Description = description,
            Price = price,
            Title = title,
            AvatarId = avatarId,
            OfferType = offerType
        };

        PostJson(data, new FetchOptions
        {
            HttpStatusCode = httpStatusCode,
            ErrorCode = errorCode
        });
    }
    
    [Theory]
    [InstructorHandler]
    [InlineData("SecondOfferId",  HttpStatusCode.OK)]
    [InlineData("WrongOfferId",  HttpStatusCode.NotAcceptable,
        ErrorType.OfferNotFound)]
    public async Task DeleteOffer(string offerId,
        HttpStatusCode httpStatusCode, ErrorType? errorCode = null)
    {
        var data = new DeleteOfferCommand
        {
            OfferId = offerId
        };

        PostJson(data, new FetchOptions
        {
            HttpStatusCode = httpStatusCode,
            ErrorCode = errorCode
        });
    }

    [Endpoint("[controller]/DeleteOffer")]
    [Fact]
    [SecondInstructorHandler]
    public async Task AnotherInstructorCannotDeleteOffer()
    {
        var data = new DeleteOfferCommand
        {
            OfferId = "OfferId"
        };
        PostJson(data, new FetchOptions
        {
            HttpStatusCode = HttpStatusCode.NotAcceptable,
            ErrorCode = ErrorType.Unauthorized
        });
    }
    
    [Theory]
    [InstructorHandler]
    [InlineData("New description", null, null, null, "SecondOfferId", null, HttpStatusCode.OK)]
    [InlineData( null, "2000", null, null, "SecondOfferId", null, HttpStatusCode.OK)]
    [InlineData(null, null, "New Title", null, "SecondOfferId", null, HttpStatusCode.OK)]
    [InlineData(null, null, null, "sad.png", "SecondOfferId", null, HttpStatusCode.OK)]
    [InlineData(null, null, null, null, "SecondOfferId", OfferType.Buy, HttpStatusCode.OK)]
    [InlineData(null, null, null, null, "WrongOfferId", null, HttpStatusCode.NotAcceptable,
        ErrorType.OfferNotFound)]
    [InlineData(null, null, null, "Not.png", "SecondOfferId", null, HttpStatusCode.NotAcceptable,
        ErrorType.FileNotFound)]
    public async Task EditOffer(string description, string price, string title, string avatarId, string offerId,
        OfferType offerType, HttpStatusCode httpStatusCode, ErrorType? errorCode = null)
    {
        var data = new EditOfferCommand
        {
            OfferType = offerType,
            OfferId = offerId,
            Description = description,
            Price = price,
            Title = title,
            AvatarId = avatarId
        };

        PostJson(data, new FetchOptions
        {
            HttpStatusCode = httpStatusCode,
            ErrorCode = errorCode
        });
    }
    
    [Endpoint("[controller]/EditOffer")]
    [Fact]
    [SecondInstructorHandler]
    public async Task AnotherInstructorCannotEditOffer()
    {
        var data = new EditOfferCommand
        {
            OfferId = "OfferId",
            Price = "2000"
        };
        PostJson(data, new FetchOptions
        {
            HttpStatusCode = HttpStatusCode.NotAcceptable,
            ErrorCode = ErrorType.Unauthorized
        });
    }

    [Theory]
    [InstructorHandler]
    [MemberData(nameof(SearchOfferDataProvider))]
    public async Task SearchOffers(OfferType offerType, string title, string price, string user,
        OfferColumn OfferColumn = OfferColumn.Title, bool orderDirection = false,
        bool testingOrder = false, Func<UserOfferDto, IComparable> getProp = null, int start = 0,
        int step = 25)
    {
        var data = new SearchOffersQuery()
        {
            EndPrice = price,
            OfferType = offerType,
            StartPrice = price,
            Search = title,
            User = user,
            OfferColumn = OfferColumn,
            OrderDirection = orderDirection,
            Start = start,
            Step = step
        };

        var response = PostJson(data, new FetchOptions
        {
            HttpStatusCode = HttpStatusCode.OK
        });

        var searchResult = (SearchOffersViewModel)JObject.Parse(response.GetContent().Result).ToObject(typeof(SearchOffersViewModel));


        if (testingOrder)
        {
            Assert.True(
                searchResult?.Offer?.SequenceEqual(searchResult.Offer.OrderBy(getProp).ToList()));
        }
        else
        {
            Assert.True(searchResult?.Offer.Count == 1);
            Assert.True(searchResult.Offer[0].OfferId == "SearchOfferId");
        }

    }
    public static IEnumerable<object[]> SearchOfferDataProvider()
    {
        yield return new object[] {OfferType.Sell, null, null, null};
        yield return new object[] {null, "SearchTitle", null, null};
        yield return new object[] {null, null, "9999", null};
        yield return new object[] {null, null, null, "SecondInstructorId"};
        yield return new object[]
        {
            null, null, null, null, OfferColumn.Price, true, true,
            (Func<UserOfferDto, IComparable>)(c => c.Price)
        };
        yield return new object[]
        {
            null, null, null, null, OfferColumn.Title, true, true,
            (Func<UserOfferDto, IComparable>)(c => c.Title)
        };
        yield return new object[]
        {
            null, null, null, null, OfferColumn.OfferId, true, true,
            (Func<UserOfferDto, IComparable>)(c => c.OfferId)
        };
        yield return new object[]
        {
            null, null, null, null, OfferColumn.Description, true, true,
            (Func<UserOfferDto, IComparable>)(c => c.Description)
        };
        yield return new object[]
        {
            null, null, null, null, OfferColumn.CreationDate, true, true,
            (Func<UserOfferDto, IComparable>)(c => c.CreatedDate)
        };
        yield return new object[]
        {
            null, null, null, null, OfferColumn.OfferType, true, true,
            (Func<UserOfferDto, IComparable>)(c => c.OfferType)
        };
    }
    
    
    [Fact]
    [InstructorHandler]
    public async Task SearchUserOffers(){
        var data = new SearchUserOffersQuery();
    
        PostJson(data, new FetchOptions
        {
            HttpStatusCode = HttpStatusCode.OK,
        }); 
    }
}