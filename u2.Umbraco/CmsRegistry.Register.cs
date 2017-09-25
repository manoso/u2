//using System;
//using System.Collections.Generic;
//using Cinema.Data.Cms.Helper;
//using Cinema.Data.Model.Cinema;
//using Cinema.Data.Model.Cms;
//using Cinema.Data.Model.Membership;
//using Cinema.Data.Model.Movie;
//using Cinema.Data.Model.Cms.Section;
//using Cinema.Data.Model.Email;
//using Cinema.Data.Model.Order;
//using Cinema.Data.Model.Payment;
//using Cinema.Data.Model.Setting;
//using Cinema.Data.Model.Notification;
//using Cinema.Data.Model.ExternalScripts;
//using Cinema.Data.Model.Ad;
//using u2.Umbraco;

//namespace u2.Core
//{
//    public class Model
//    {
//        public int Id { get; set; }
//        public List<Item> Items { get; set; }
//    }

//    public class Item
//    {
//        public int Id { get; set; }
//        public string Info { get; set; }
//    }
//    public static partial class CmsRegistry
//    {
//        static CmsRegistry()
//        {
//            var map = new Map();
//            map.Register<Model>()
//                .Map("id", x => x.Id)
//                .Map("items", x => x.Items, x => x.Archetype<Item>(map));
//            Register<Home>()
//                .All()
//                .Map("id", x => x.CmsId)
//                ;
//            Register<Movie>()
//                .All()
//                .Map("id", x => x.CmsId)
//                .Map("nodeName", x => x.Name)
//                .Map("urlName", x => x.UrlName)
//                .Map<string, IList<string>>(x => x.VistaId, x => x.CsvToList<string>())
//                .Map<string, IList<string>>(x => x.YouTubeIds, x => x.CsvToList<string>())
//                .Map<string, IList<int>>(x => x.GenreIds, x => x.CsvToList<int>())
//                .Act((c, m) =>
//                {
//                    if (string.IsNullOrWhiteSpace(m.Title)) m.Title = m.Name;
//                })
//                ;
//            Register<SessionAttribute>()
//                .All()
//                .Ignore(x => x.Image)
//                .Map("id", x => x.CmsId)
//                .Map("nodeName", x => x.Name)
//                ;
//            Register<MoviesShowing>()
//                .Map("id", x => x.CmsId)
//                .Map<string, IList<int>>(x => x.MovieIds, x => x.CsvToList<int>())
//                ;
//            Register<CinemaFeature>()
//                .All()
//                .Map("id", x => x.CmsId)
//                .Map("nodeName", x => x.Name)
//                ;
//            Register<CinemaTimeZone>()
//                .Map("id", x => x.CmsId)
//                .Map("nodeName", x => x.Name)
//                .Map(x => x.TimeZone)
//                ;
//            Register<Model.Cinema.Cinema>()
//                .All()
//                .Map("id", x => x.CmsId)
//                .Map<string, IList<int>>(x => x.SessionAttributeIds, x => x.CsvToList<int>())
//                .Map<string, IList<int>>(x => x.CinemaFeatureIds, x => x.CsvToList<int>())
//                ;
//            Register<WeekMovie>()
//                .All()
//                .Map("id", x => x.CmsId)
//                ;
//            Register<MembershipBanner>()
//                .All()
//                .Map("id", x => x.CmsId)
//                .Media(x => x.BackgroundUrl)
//                ;
//            Register<MovieBanner>()
//                .Map("id", x => x.CmsId)
//                ;
//            Register<MovieInfo>()
//                .All()
//                .Map("id", x => x.CmsId)
//                ;
//            Register<MovieSession>()
//                .All()
//                .Map("id", x => x.CmsId)
//                ;
//            Register<MovieReview>()
//                .All()
//                .Map("id", x => x.CmsId)
//                ;
//            Register<ContentBlock>()
//                .All()
//                .Map("id", x => x.CmsId)
//                .Map("nodeName", x => x.Name)
//                ;
//            Register<ContentTab>()
//                .All()
//                .Map("tabName", x => x.Name)
//                .Map<string, IList<int>>("contentIds", x => x.ContentIds, x => x.CsvToList<int>())
//                .Act((content, tab) => 
//                {
//                    tab.ContentIds = tab.ContentIds ?? new List<int>(0);
//                    tab.Contents = tab.Contents ?? new List<Content>(0);
//                })
//                ;
//            Register<ContentTabs>()
//                .All()
//                .Map("id", x => x.CmsId)
//                .Map("nodeName", x => x.Name)
//                .Map<string, IList<ContentTab>>("TabbedContent", x => x.Tabs, x => x.ArcheTypeToList<ContentTab>())
//                .Act((content, tabs) =>
//                {
//                    tabs.Tabs = tabs.Tabs ?? new List<ContentTab>(0);
//                })
//                ;
//            Register<ContentFooter>()
//                .All()
//                .Map("footerName", x => x.FooterName)
//                .Map("internalLinkId", x => x.InternalLinkId)
//                .Map("externalLink", x => x.ExternalLink)
//                .Map("openInNewTab", x => x.OpenInNewTab)
//                .Map("footerColumn", x => x.FooterColumn)
//                ;
//            Register<ContentFooters>()
//                .All()
//                .Map("id", x => x.CmsId)
//                .Map("nodeName", x => x.Name)
//                .Map<string, IList<ContentFooter>>("Footers", x => x.Footers, x => x.ArcheTypeToList<ContentFooter>())
//                .Act((content, footers) =>
//                {
//                    footers.Footers = footers.Footers ?? new List<ContentFooter>(0);                    
//                })
//                ;
//            Register<View>()
//                .Map("id", x => x.CmsId)
//                .Map("nodeName", x => x.Name)
//                .Map<string, IList<int>>(x => x.ContentIds, x => x.CsvToList<int>())
//                ;
//            Register<Page>()
//                .All()
//                .Map("id", x => x.CmsId)
//                .Map("nodeName", x => x.Name)
//                ;
//            Register<ConcessionFilter>()
//                .All()
//                .Map("id", x => x.CmsId)
//                .Map("nodeName", x => x.Name)
//                .Map<string, IList<int>>(x => x.SessionAttributeIds, x => x.CsvToList<int>())
//                .Map<string, IList<int>>(x => x.ExSessionAttributeIds, x => x.CsvToList<int>())
//                ;
//            Register<Classification>()
//                .All()
//                .Map("id", x => x.CmsId)
//                .Map("nodeName", x => x.Code)
//                .Map<string, IList<string>>(x => x.MatchCode, x => x.CsvToList<string>())
//                .Media(x => x.Icon)
//                ;
//            Register<Genre>()
//                .Map("id", x => x.CmsId)
//                .Map("nodeName", x => x.Name)
//                .Map(x => x.Description)
//                ;
//            Register<AdSlide>()
//                .All()
//                .Map("id", x => x.CmsId)
//                .Map("nodeName", x => x.Name)
//                .Map("largeAdId", x => x.LargeAd)
//                .Map("smallAdId", x => x.SmallAd)
//                ;
//            Register<Slide>()
//                .All()
//                .Map("id", x => x.CmsId)
//                .Map("nodeName", x => x.Name)
//                .Media(x => x.ImageLarge)
//                .Media(x => x.ImageSmall)
//                ;
//            Register<MovieCarousel>()
//                .All()
//                .Map("id", x => x.CmsId)
//                .Map("nodeName", x => x.Name)
//                .Map<string, IList<int>>(x => x.SlideIds, x => x.CsvToList<int>())
//                ;
//            Register<Movies>()
//                .Map("id", x => x.CmsId)
//                .Map(x => x.ImageBaseUrl)
//                ;
//            Register<MovieTile>()
//                .All()
//                .Map("id", x => x.CmsId)
//                .Map("nodeName", x => x.Name)
//                ;
//            Register<InfoTile>()
//                .All()
//                .Map("id", x => x.CmsId)
//                .Map("nodeName", x => x.Name)
//                .Media(x => x.Image)
//                ;
//            Register<Ad>()
//                .All()
//                .Map("id", x => x.CmsId)
//                .Map("nodeName", x => x.Name)
//                .Media(x => x.Image)
//                ;
//            Register<AdBundle>()
//                .All()
//                .Map("id", x => x.CmsId)
//                .Map("nodeName", x => x.Name)
//                .Map<string, IList<int>>(x => x.AdIds, x => x.CsvToList<int>())
//                ;
//            Register<AllMoviesAds>()
//                .All()
//                .Map("id", x => x.CmsId)
//                .Map("nodeName", x => x.Name)
//                .Map<string, IList<int>>(x => x.AdBundleIds, x => x.CsvToList<int>())
//                ;
//            Register<AdTile>()
//                .All()
//                .Map("id", x => x.CmsId)
//                .Map("nodeName", x => x.Name)
//                ;
//            Register<MovieGrid>()
//                .All()
//                .Map(x => x.Flippable, true)
//                .Map("id", x => x.CmsId)
//                .Map("nodeName", x => x.Name)
//                .Map<string, IList<int>>(x => x.TileIds, x => x.CsvToList<int>())
//                ;
//            Register<TicketFilter>()
//                .All()
//                .Map<string, IList<string>>(x => x.TicketTypes, x => x.CsvToList<string>())
//                .Map<string, IList<string>>(x => x.RequiredTicketTypes, x => x.CsvToList<string>())
//                .Map<string, IList<int>>(x => x.SessionAttributeIds, x => x.CsvToList<int>())
//                .Map<string, IList<int>>(x => x.MovieClassificationIds, x => x.CsvToList<int>())
//                //.Map<string, IList<int>>(x => x.PartnerIds, x => x.CsvToList<int>())
//                ;
//            Register<TicketSort>()
//                .All()
//                .Map<string, IList<string>>(x => x.TicketTypes, x => x.CsvToList<string>())
//                .Map<string, IList<int>>(x => x.SessionAttributeIds, x => x.CsvToList<int>())
//                .Map<string, IList<int>>(x => x.MovieClassificationIds, x => x.CsvToList<int>())
//                //.Map<string, IList<MembershipTier>>(x => x.MembershipTiers, x => x == "-1" ? null : x.CsvToList<MembershipTier>())
//                ;
//            Register<BookingFee>()
//                .All()
//                .Map("nodeName", x => x.Name)
//                .Map<string, IList<string>>(x => x.TicketTypes, x => x.CsvToList<string>())
//                ;
//            Register<TicketingConfig>()
//                .All()
//                .Map<string, IList<string>>(x => x.RewardsExclusions, x => x.CsvToList<string>())
//                ;
//            Register<CinemaConfig>()
//                .Map<string, IList<int>>(x => x.AllowedCinemaIds, x => x.CsvToList<int>())
//                ; 
//            Register<PaymentMethod>()
//                .All()
//                .Map("id", x => x.CmsId)
//                .Map("nodeName", x => x.Name)
//                .Media(x => x.Icon)
//                ;
//            Register<PaymentGroup>()
//                .Map("id", x => x.CmsId)
//                .Map("nodeName", x => x.Name)
//                .Map<string, IList<int>>(x => x.PaymentMethodIds, x => x.CsvToList<int>())
//                ;
//            Register<DynamicContentRule>()
//                .Map("id", x => x.CmsId)
//                .Map("nodeName", x => x.Name)
//                .Map("parentID", x => x.DynamicContentId)
//                .Map(x => x.Content)
//                .Map(x => x.ScriptContent)
//                .Map(x => x.MemberType)
//                .Map(x => x.TimeOfDay)
//                .Map(x => x.DateFrom)
//                .Map(x => x.DateTo)
//                .Map<string, IList<int>>(x => x.CinemaIds, x => x.CsvToList<int>())
//                .Map<string, IList<int>>(x => x.MovieIds, x => x.CsvToList<int>())
//                .Map<string, IList<int>>(x => x.CinemaFeatureIds, x => x.CsvToList<int>())
//                .Map<string, IList<int>>(x => x.SessionAttributeIds, x => x.CsvToList<int>())
//                .Map<string, IList<string>>(x => x.RecognitionIds, x => x.CsvToList<string>())
//                .Map<string, IList<string>>(x => x.TicketTypes, x => x.CsvToList<string>())
//                .Map<string, IList<int>>(x => x.PaymentMethodIds, x => x.CsvToList<int>())
//                //.Map<string, IList<int>>(x => x.PartnerIds, x => x.CsvToList<int>())
//                .Map<string, WeekDays>(x => x.DaysOfWeek, x => new WeekDays { Days = x.CsvToList<DayOfWeek>() })
//                .Map<string, WeekDays>(x => x.DaysOfWeekSession, x => new WeekDays { Days = x.CsvToList<DayOfWeek>() })
//                .Map(x => x.TimeOfDaySession)
//                .Map(x => x.DateFromSession)
//                .Map(x => x.DateToSession)
//                ;
//            Register<EmailTemplate>()
//                .All()
//                .Map("id", x => x.CmsId)
//                .Map("nodeName", x => x.Name)
//                ;
//            Register<EmailTemplatePdf>()
//                .All()
//                .Map("id", x => x.CmsId)
//                .Map("nodeName", x => x.Name)
//                ;
//            Register<Message>()
//                .All()
//                .Map("id", x => x.CmsId);
//            Register<Script>()
//                .All()
//                .Map("id", x => x.CmsId)
//                .Map("nodeName", x => x.Name);
//            //.AliasTo("CinemaPage")
//            //.Map(x => x.Name)
//            //.Map(x => x.AlternateName)
//            //.Map(x => x.Heading)
//            //.Map(x => x.MenuName)
//            //.Map(x => x.NameAppendix)
//            //.Map(x => x.Telephone)
//            //.Map("telephoneTwo", x => x.Telephone2)
//            //.Map(x => x.Fax)
//            //.Map(x => x.Content)
//            //.Map(x => x.Email)
//            //.Map("smsAllowed", x => x.SMSAllowed)
//            //.Map(x => x.IsAComplex)
//            //.Map(x => x.DisableSessionTimes)
//            //.Map(x => x.CinemaId)
//            //.Map(x => x.IsEnabled)
//            //.Map(x => x.LocationID)
//            //.Map(x => x.OperatorId)
//            //.Map(x => x.CinemaDirectionOverride)
//            //.Map(x => x.GroupBookingsEmail)
//            //.Map(x => x.FeaturesTitle)
//            //.Map("locationBasedAdDynamicContent", x => x.LocationBasedAdNodeId)
//            //.Map<int, string>("cinemaDetailsImage", x => x.CinemaDetailsImageUrl, x => x.ImageIdToUrl())
//            //.Map<string, List<Sponsor>>("sponsor", x => x.Sponsors, x =>
//            //{
//            //    var nodes = x.CsvIdsToNodes();
//            //    return nodes == null
//            //        ? null
//            //        : nodes.Select(y => Solve<Sponsor>(y)).ToList();

//            //})
//            //.Act((nod, cin, defer) =>
//            //{
//            //    cin.CinemaCmsId = Convert.ToString(nod.Id);
//            //    cin.CMSUrl = nod.Url;
//            //    if (string.IsNullOrWhiteSpace(cin.AlternateName))
//            //        cin.AlternateName = cin.Name;
//            //    if (string.IsNullOrWhiteSpace(cin.MenuName))
//            //        cin.MenuName = cin.Name;
//            //    if (string.IsNullOrWhiteSpace(cin.FeaturesTitle))
//            //        cin.FeaturesTitle = "Features";
//            //    cin.Location = nod.Solve<Address>(defer: defer);
//            //});

//            //    Register<Navigation>()
//            //        .AliasTo("BasePage")
//            //        .Map<string, string>("menuName", x => x.Name)
//            //        .Map("showInNavigation", x => x.ShowInNav)
//            //        .Map("showInFooter", x => x.ShowInFooter)
//            //        .Map("showInFooterBar", x => x.ShowInFooterBar)
//            //        .Map("highlightSection", x => x.HighlightSection)
//            //        .Map("menuView", x => x.MenuView)
//            //        .Act((nod, nav) =>
//            //        {
//            //            nav.ItemId = nod.Id;
//            //            nav.URL = nod.Url;
//            //            nav.FooterColumn = nod.Parent.Name;

//            //            if (String.IsNullOrEmpty(nav.Name)) nav.Name = nod.Name;
//            //            var childNodes = nod.GetChildNodes().Where(c => c.IsNavigation()).ToList();
//            //            if (childNodes.Any())
//            //            {
//            //                nav.Children = childNodes.Select(x => ContentType.For<Navigation>()
//            //                    //.UseDefault
//            //                    .Add<MovieNavigation>("MovieListPage")
//            //                    .Add<Navigation>("GenrePage")
//            //                    .Apply(x)).ToList();
//            //            }
//            //        })
//            //        .Act<int>("menuShortcut", (n, i) => { if (i > 0) n.URL = i.NodeIdToUrl(); });

//            //    Register<MovieNavigation>()
//            //        .AliasTo("MovieListPage")
//            //        .Copy<Navigation>()
//            //        .Map(x => x.ListMode)
//            //        .Map<string, IList<string>>("menuSortList", x => x.MovieSortList, x =>
//            //        {
//            //            var autoComplete = x.XmlToSqlAutoComplete<string>();
//            //            return autoComplete == null 
//            //                ? null 
//            //                : autoComplete.Items.Select(i => i.Value).ToList();
//            //        });

//            //    Register<NavigationMenuItem>()
//            //        .AliasTo("NavigationItem")
//            //        .Copy<Navigation>()
//            //        .Map(x => x.Featured)
//            //        .Map<int, string>("thumbnailImage", x => x.ThumbnailImageUrl, x => x.ImageIdToUrl())
//            //        .Map(x => x.ViewAllText)
//            //        .Act((nod, nav) =>
//            //        {
//            //            nav.ViewAllUrlOverride = nav.URL;
//            //            nav.SubItems = nod.GetChildNodes().Select(x => x.Solve<NavigationMenuItem>()).ToList();
//            //        })
//            //        .Act<int>("viewAllUrlOverride", (n, i) =>
//            //        {
//            //            var url = i.NodeIdToUrl();
//            //            if (url != null)
//            //                n.ViewAllUrlOverride = url;
//            //        });

//            //    Register<TicketSort>()
//            //        .Map<string, IList<string>>("ticketTypes", x => x.TicketTypes, x => x.CsvToStringList())
//            //        .Map<string, IList<int>>("sessionAttributes", x => x.SessionAttributes, x => x.CsvToIntList())
//            //        .Map<string, IList<int>>("movieClassifications", x => x.MovieClassifications, x => x.CsvToIntList())
//            //        .Map<string, IList<MembershipTier>>("memberTypes", x => x.MemberTypes, x => x == "-1" ? null : x.CsvToList<MembershipTier>())
//            //        .Map(x => x.DateFrom)
//            //        .Map(x => x.DateTo)
//            //        .Map(x => x.SortOrder);

//            //    Register<Competition>()
//            //        .Map(x => x.StartDate)
//            //        .Map(x => x.EndDate)
//            //        .Map(x => x.DrawnDate)
//            //        .Map("enable", x => x.Enabled)
//            //        .Map(x => x.EnteredSuccess)
//            //        .Map(x => x.IsThirdParty)
//            //        .Map(x => x.RequiresBookingRef)
//            //        .Map(x => x.IsGameOfChance)
//            //        .Map(x => x.Title)
//            //        .Map("thumbnailImage", x => x.ThumbnailImageURL, x => x.XmlToDampUrl())
//            //        .Map(x => x.OpenTo)
//            //        .Map(x => x.PrizeDetails)
//            //        .Map("competitionShortDescription", x => x.ShortDescription)
//            //        .Map("competitionLongDescription", x => x.LongDescription)
//            //        .Map(x => x.Terms)
//            //        .Map(x => x.ShortTerms)
//            //        .Map(x => x.HideFromListPage)
//            //        .Map(x => x.IsSendingEmail)
//            //        .Map(x => x.PromoCode)
//            //        .Map<string, string[]>("relatedMovie", x => x.MovieIds, x =>
//            //        {
//            //            var autoComplete = x.XmlToSqlAutoComplete<string>();
//            //            return autoComplete == null 
//            //                ? null
//            //                : autoComplete.Items.Select(i => i.Value).ToArray();
//            //        })
//            //        .Act((nod, cmp) =>
//            //        {
//            //            cmp.CMSUrl = nod.Url;
//            //            cmp.CMSNodeId = nod.Id;
//            //            var childNodes = nod.GetChildNodes().ToList();
//            //            if (childNodes.Any())
//            //                cmp.Questions = childNodes.Select(x => x.Solve<CompetitionQuestion>()).ToList();
//            //        });

//            //    Register<CompetitionQuestion>()
//            //        .Map(x => x.Question)
//            //        .Map("answerWordLimit", x => x.AnswerLimit);

//            //    Register<Genre>()
//            //        .All()
//            //        .Ignore(x => x.CmsId)
//            //        .Act((nod, g) =>
//            //        {
//            //            g.CmsId = nod.Id;
//            //        });

//            //    Register<Classification>()
//            //        .Map(x => x.AvailableInSearch)
//            //        .Map("rating", x => x.Description)
//            //        .Map("ratingAdvice", x => x.Advice)
//            //        .Map<int, string>("ratingImage", x => x.ImageURL, x => x.ImageIdToUrl(string.Empty))
//            //        .Act<string>("matchCode", (c, mc) =>
//            //        {
//            //            c.MatchCode = string.IsNullOrWhiteSpace(mc)
//            //                ? new List<string> {c.Code}
//            //                : mc.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries)
//            //                    .Where(x => !string.IsNullOrWhiteSpace(x))
//            //                    .Select(x => x.Trim())
//            //                    .ToList();
//            //        })
//            //        .Act((nod, c) =>
//            //        {
//            //            c.CmsId = nod.Id;
//            //            c.Code = nod.Name;
//            //        });
//            //    Register<HomePageFeature>()
//            //        .All()
//            //        .Ignore(x => x.NodeId)
//            //        .Map<int, string>(x => x.BackgroundImage, x => x.ImageIdToUrl(string.Empty))
//            //        .Map<string, Dictionary<string, string>>("dFPTargeting", x => x.Targeting, x => x.XmlToTextstringArray().TextstringArrayToDictionary())
//            //        .Map(x => x.AdServed)
//            //        .Act<string>("featureUrl", (f, x) =>
//            //        {
//            //            var featurelink = uComponents.DataTypes.UrlPicker.Dto.UrlPickerState.Deserialize(x);
//            //            if (featurelink != null)
//            //                f.FeatureUrl = featurelink.Url;
//            //        })
//            //        .Act((nod, f) =>
//            //        {
//            //            f.NodeId = nod.Id;
//            //        });

//            //    Register<CommonContent>()
//            //        .Map("sidebarSocialMediaTwitterLink", x => x.TwitterLink)
//            //        .Map("sidebarSocialMediaFBLink", x => x.FBLink)
//            //        .Map("sidebarSocialMediaYouTubeLink", x => x.YoutubeLink)
//            //        .Map("footerAddress", x => x.FooterAddress)
//            //        .Map<int, string>("footerLogoPath", x => x.FooterHoytsLogo, x => x.ImageIdToUrl(string.Empty))
//            //        .Map("noCinemaText", x => x.NoCinemaText);

//            //    Register<GenrePage>()
//            //        .Map(x => x.ShortDescription)
//            //        .Act((nod, gp) =>
//            //        {
//            //            gp.Id = nod.Id;
//            //            gp.Name = nod.Name;
//            //            gp.Url = nod.Url;
//            //        });

//            //    Register<Partner>()
//            //        .All()
//            //        .Ignore(x => x.Id)
//            //        //.Ignore(x => x.AccessToken)
//            //        .Map<string, string>(x => x.BackgroundImage, x => x.XmlToDampUrl(string.Empty))
//            //        .Map<string, IList<string>>(x => x.PartnerTickets, x => x.CsvToStringList().EmptyAsNull())
//            //        .Map("mobileRedirectUrl", x => x.MobileRedirectUrl)
//            //        .Act((nod, p) => p.Id = nod.Id);


//            //    Register<TicketFilter>()
//            //        .Map<int, int>(x => x.MinOrder, x => x < 1 ? -1 : x)
//            //        .Map<int, int>(x => x.MaxAvailable, x => x < 1 ? -1 : x)
//            //        .Map(x => x.RequiredTicketType)
//            //        .Map<string, IList<PartnerCode>>(x => x.Partners, x => x.CsvToList<PartnerCode>())
//            //        .Map<string, IList<string>>(x => x.TicketType, x => x.CsvToStringList().EmptyAsNull())
//            //        .Map<string, IList<string>>(x => x.SessionAttributes, x => x.CsvToStringList().EmptyAsNull())
//            //        .Map<string, IList<string>>(x => x.MovieClassifications, x => x.CsvToStringList().EmptyAsNull());

//            //    Register<DynamicContentSection>()
//            //        .Map("defaultContent", x => x.DefaultContentHTML)
//            //        .Map("defaultScriptContent", x => x.DefaultScript)
//            //        .Map(x => x.Code)
//            //        .Map(x => x.WrapScriptInFunction)
//            //        .Map("disableDefault", x => x.EnableDefaultContent, x => !x)
//            //        .Act((nod, dcs) =>
//            //        {
//            //            dcs.UmbracoNodeId = nod.Id;
//            //            dcs.UmbracoNodeName = nod.Name;
//            //        });


//            //    Register<PaymentMethod>()
//            //        .All()
//            //        .Ignore(x => x.ImageUrl)
//            //        .Map<string, string>("image", x => x.ImageUrl, x => x.XmlToDampUrl(string.Empty));

//            //    Register<HeroBanner>()
//            //        .Map(x => x.Title)
//            //        .Map(x => x.Description)
//            //        .Map(x => x.Layout)
//            //        .Map(x => x.AdServed)
//            //        .Map("numberOfSeconds", x => x.TimePerSlide, x => x == 0 ? 10 : x)
//            //        .Map<int, string>("bannerImage", x => x.BannerImageUrl, x => x.ImageIdToUrl(string.Empty))
//            //        .Map("vistaId", x => x.MovieId, x => x ?? string.Empty)
//            //        .Map<string, Dictionary<string, string>>("dFPTargeting", x => x.Targeting,
//            //            x => x.XmlToTextstringArray().TextstringArrayToDictionary())
//            //        //2 mapping for x.Disabled because "disabled" will override "isEnabled"
//            //        .Map("isEnabled", x => x.Disabled, x => !x)
//            //        .Map(x => x.Disabled)
//            //        .Map<string, IList<PartnerCode>>(x => x.Partners, x => x.CsvToList<PartnerCode>())
//            //        .Act<string>("genreLink", (hb, xml) =>
//            //        {
//            //            var urlPicker = xml.XmlToUrlPicker();
//            //            hb.GenreLink = urlPicker != null ? urlPicker.Url : string.Empty;
//            //            hb.GenreLinkText = urlPicker != null ? urlPicker.Title : string.Empty;
//            //        })
//            //        .Act<string>("buyNowLink", (hb, xml) =>
//            //        {
//            //            var urlPicker = xml.XmlToUrlPicker();
//            //            hb.BuyNowLink = urlPicker != null ? urlPicker.Url : string.Empty;
//            //            hb.BuyNowLinkText = urlPicker != null ? urlPicker.Title : string.Empty;
//            //            hb.BuyNowLinkIsOpen = urlPicker != null && urlPicker.NewWindow;
//            //        })
//            //        .Act<string>("buyNowLinkColour", (hb, x) =>
//            //        {
//            //            hb.BuyNowLinkColour = string.IsNullOrWhiteSpace(hb.BuyNowLink) ? string.Empty : x;
//            //        })
//            //        .Act<string>("moreInfoLink", (hb, xml) =>
//            //        {
//            //            var urlPicker = xml.XmlToUrlPicker();
//            //            hb.MoreInfoLink = urlPicker != null ? urlPicker.Url : string.Empty;
//            //            hb.MoreInfoLinkText = urlPicker != null ? urlPicker.Title : string.Empty;
//            //            hb.MoreInfoLinkIsOpen = urlPicker != null && urlPicker.NewWindow;
//            //        })
//            //        .Act<string>("moreInfoLinkColour", (hb, x) =>
//            //        {
//            //            hb.MoreInfoLinkColour = string.IsNullOrWhiteSpace(hb.MoreInfoLink) ? string.Empty : x;
//            //        })
//            //        .Act<string>("promotionLink", (hb, xml) =>
//            //        {
//            //            var urlPicker = xml.XmlToUrlPicker();
//            //            hb.PromotionLink = urlPicker != null ? urlPicker.Url : string.Empty;
//            //            hb.PromotionLinkText = urlPicker != null ? urlPicker.Title : string.Empty;
//            //            hb.PromotionLinkIsOpen = urlPicker != null && urlPicker.NewWindow;
//            //        })
//            //        .Act<string>("promotionLinkColour", (hb, x) =>
//            //        {
//            //            hb.PromotionLinkColour = string.IsNullOrWhiteSpace(hb.PromotionLink) ? string.Empty : x;
//            //        })
//            //        .Act<string>("promotionBgColour", (hb, x) =>
//            //        {
//            //            hb.PromotionBgColour = string.IsNullOrWhiteSpace(hb.PromotionLink) ? string.Empty : x;
//            //        })
//            //        .Act<string>("promotionDesc", (hb, x) =>
//            //        {
//            //            hb.PromotionDesc = string.IsNullOrWhiteSpace(hb.PromotionLink) ? string.Empty : x;
//            //        })
//            //        .Act<int>("promotionImage", (hb, x) =>
//            //        {
//            //            hb.PromotionImage = string.IsNullOrWhiteSpace(hb.PromotionLink)
//            //                ? string.Empty
//            //                : x.ImageIdToUrl(string.Empty);
//            //        })
//            //        .Act((nod, hb) =>
//            //        {
//            //            if (!string.IsNullOrWhiteSpace(hb.MovieId) || !string.IsNullOrWhiteSpace(hb.Title)) return;

//            //            var link = nod.GetProperty<string>("buyNowLink").XmlToUrlPicker() 
//            //                       ?? nod.GetProperty<string>("moreInfoLink").XmlToUrlPicker();
//            //            var movieNode = link != null ? link.NodeId : null;

//            //            if (movieNode == null) return;

//            //            var movieTitle = movieNode.Value.NodeIdToProperty<string>("title");
//            //            hb.Title = !string.IsNullOrWhiteSpace(movieTitle) ? movieTitle : hb.Title;
//            //        });

//            //    Register<Product>()
//            //        .Map("productSKU", x => x.ProductSku)
//            //        .Map("productName", x => x.Name)
//            //        .Map("productDescription", x => x.Description)
//            //        .Map(x => x.MaxQuantity)
//            //        .Map<int, int>(x => x.MinQuantity, x => x <= 0 ? 1 : x)
//            //        .Map<int, int>("incrementQuantity", x => x.IncQuantity, x => x <= 0 ? 1 : x)
//            //        .Map("defaultPrice", x => x.Price)
//            //        .Act((nod, prd) =>
//            //        {
//            //            prd.ProductId = string.Format("{0}", nod.Id);
//            //            var childNodes = nod.GetDescendantNodesByType("ProductVariant").ToList();
//            //            if (childNodes.Any())
//            //                prd.ProductVariants = childNodes.Select(x => x.Solve<ProductVariant>()).ToList();
//            //        });
//            //    Register<ProductVariant>()
//            //        .Map("productSKU", x => x.ProductSku)
//            //        .Map("variantName", x => x.Name)
//            //        .Map("variantPrice", x => x.Price)
//            //        .Map<string, IList<int>>("promotion", x => x.PromotionId, x => x.CsvToIntList());
//        }
//    }
//}
